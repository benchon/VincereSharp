using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VincereSharp
{
    public class VincereClient
    {
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.'000Z'"
        };

        public string RefresherToken { get; set; }
        public string IdToken { get; set; }

        private string GetBaseUrl(string domain = "id")
        {
            return Config.UseTest ? $"https://{ domain }.vinceredev.com" : $"https://{ domain }.vincere.io";
        }

        public VincereClient(string clientId, string apiKey = "", string domainId = "id", bool useTest = false)
        {
            Config.UseTest = useTest;
            Config.ClientId = clientId;
            Config.ApiKey = apiKey;
            Config.DomainId = domainId;
        }

        public VincereClient(VincereConfig config)
        {
            Config = config;
        }

        private VincereConfig _config;
        public VincereConfig Config
        {
            get => _config ?? (_config = new VincereConfig());
            set => _config = value;
        }

        private HttpClient _client;
        private HttpClient Client
        {
            get
            {
                return _client ?? (_client = new HttpClient
                {
                    BaseAddress = new Uri(GetBaseUrl(Config.DomainId)),
                    DefaultRequestHeaders =
                                {
                                    { "id-token", IdToken },
                                    { "x-api-key", Config.ApiKey }
                                }
                });
            }
        }

        #region "Auth"
        public string GetLoginUrl(string redirectUrl)
        {
            return GetLoginUrl(Config.ClientId, redirectUrl);
        }

        public string GetLoginUrl(string clientId, string redirectUrl, string state = null)
        {
            var url = new StringBuilder("");
            url.Append(GetBaseUrl())
                .Append("/oauth2/authorize?client_id=")
                .Append(clientId)
                .Append("&redirect_uri=")
                .Append(redirectUrl)
                .Append("&response_type=code");

            if (!string.IsNullOrWhiteSpace(state))
            {
                url.Append("&state=")
                    .Append(state);
            }
            return url.ToString();
        }

        public async Task<TokenResponse> GetAuthCode(string AuthCode)
        {
            var client = new HttpClient { BaseAddress = new Uri(GetBaseUrl()) };
            var values = new Dictionary<string, string>()
            {
                {"client_id", Config.ClientId},
                {"code", AuthCode},
                {"grant_type", "authorization_code"}
            };
            var content = new FormUrlEncodedContent(values);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("/oauth2/token", UriKind.Relative),
                Method = HttpMethod.Post,
                Content = content,
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
            this.SetTokenResponse(tokenResponse);
            return tokenResponse;
        }

        public async Task<TokenResponse> GetRefreshToken(string refreshToken)
        {
            var client = new HttpClient { BaseAddress = new Uri(GetBaseUrl()) };
            var values = new Dictionary<string, string>()
            {
                {"client_id", Config.ClientId},
                {"refresh_token", refreshToken},
                {"grant_type", "refresh_token"}
            };
            var content = new FormUrlEncodedContent(values);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("/oauth2/token", UriKind.Relative),
                Method = HttpMethod.Post,
                Content = content,
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
            this.SetTokenResponse(tokenResponse);
            return tokenResponse;

        }

        private void SetTokenResponse(TokenResponse tokenResponse)
        {
            this.IdToken = tokenResponse.IdToken;
            this.RefresherToken = tokenResponse.RefreshToken;
        }
        #endregion

        #region "Contact"

        public async Task<IEnumerable<ContactSearchResultItem>> GetContactsAsync(string searchText = "")
        {
            var searchUrl =
                $"/api/v2/contact/search/fl=id,name,job_title,email,company,phone,note;sort=created_date asc";

            if (!string.IsNullOrWhiteSpace(searchText))
                searchUrl += $"?q=text:{searchText}";
            try
            {
                var json = await Client.GetStringAsync(searchUrl);
                var response = await Task.Run(() => JsonConvert.DeserializeObject<ContactSearchResult>(json));
                return response.Result.Items;
            }
            catch (HttpRequestException hrex)
            {
                if (string.IsNullOrWhiteSpace(this.RefresherToken)) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.GetContactsAsync(searchText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Contact> GetContactAsync(int id, int retry = 2)
        {
            try
            {
                var json = await Client.GetStringAsync($"/api/v2/contact/{id}");
                var contact = JsonConvert.DeserializeObject<Contact>(json);
                return contact;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                if (retry <= 0) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.GetContactAsync(id, --retry);
            }
        }

        public async Task<int> AddContactAsync(Contact item)
        {
            if (item == null)
                throw new NullReferenceException("Contact is null");

            if (string.IsNullOrWhiteSpace(IdToken))
                throw new NullReferenceException("IdToken is null");

            var serializedItem = JsonConvert.SerializeObject(item,
                                                            Formatting.None,
                                                            jsonSerializerSettings);

            try
            {
                var response = await Client.PostAsync("/api/v2/contact",
                    new StringContent(serializedItem,
                        Encoding.UTF8,
                        "application/json"));
                var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ContactCreatedResponse>(json).id;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.AddContactAsync(item);
            }
        }

        public async Task<bool> UpdateContactAsync(Contact item, int id)
        {
            if (id <= 0)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item,
                                                             Formatting.None,
                                                             jsonSerializerSettings);
            try
            {
                var response = await Client.PutAsync($"/api/v2/contact/{id}",
                    new StringContent(serializedItem,
                        Encoding.UTF8,
                        "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.UpdateContactAsync(item, id);
            }

        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            if (id <= 0)
                return false;

            var response = await Client.DeleteAsync($"api/v2/contact/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<DeleteResponse>(json);

            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}
