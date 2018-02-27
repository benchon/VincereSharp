using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace VincereSharp
{
    public class VincereClient
    {
        private string _clientId;
        private string _apiKey;
        private string _domainId;

        private bool _useTest = false;
        private bool _isLoggedin = false;

        public string AccessToken { get; set; }
        public string RefresherToken { get; set; }
        public string IdToken { get; set; }

        private string GetBaseUrl(string domain = "id")
        {
            return _useTest ? $"https://{ domain }.vinceredev.com" : $"https://{ domain }.vincere.io";
        }

        public VincereClient(string clientId, string apiKey = "", string domainId = "id", bool useTest = false)
        {
            _useTest = useTest;
            _clientId = clientId;
            _apiKey = apiKey;
            _domainId = domainId;
        }

        public VincereClient(VincereConfig config, bool useTest = false)
        {
            _useTest = useTest;
            _clientId = config.ClientId;
            _apiKey = config.ApiKey;
            _domainId = config.DomainId;
        }

        private HttpClient _client;
        private HttpClient Client
        {
            get
            {
                return _client ?? (_client = new HttpClient
                {
                    BaseAddress = new Uri(GetBaseUrl(_domainId)),
                    DefaultRequestHeaders =
                                {
                                    { "id-token", IdToken },
                                    { "x-api-key", _apiKey }
                                }
                });
            }
        }

        #region "Auth"
        public string GetLoginUrl(string redirectUrl)
        {
            return GetLoginUrl(_clientId, redirectUrl);
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

        public async Task<TokenResponse> GetAuthCode(string code)
        {
            var client = new HttpClient { BaseAddress = new Uri(GetBaseUrl()) };
            var values = new Dictionary<string, string>()
            {
                {"client_id", _clientId},
                {"code", code},
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
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        public async Task<TokenResponse> GetRefreshToken(string refreshToken)
        {
            var client = new HttpClient { BaseAddress = new Uri(GetBaseUrl()) };
            var values = new Dictionary<string, string>()
            {
                {"client_id", _clientId},
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
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }
        #endregion

        #region "Contact"

        public async Task<IEnumerable<Contact>> GetContactsAsync(bool forceRefresh = false)
        {
            var contacts = new List<Contact>();
            var json = await Client.GetStringAsync($"/api/v2/contact/search/fl=id,current_location;sort=created_date asc");
            var items = await Task.Run(() => JsonConvert.DeserializeObject<ContactSearchResult>(json));
            foreach (var c in items.Result.Items)
            {
                try
                {
                    var newContact = await GetContactAsync(c.Id);
                    contacts.Add(newContact);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return contacts;
        }

        public async Task<Contact> GetContactAsync(int id)
        {
            // https://www.vincere.io/api/v2/contact 
            var json = await Client.GetStringAsync($"/api/v2/contact/{id}");
            return await Task.Run(() => JsonConvert.DeserializeObject<Contact>(json));
        }

        public async Task<bool> AddContactAsync(Contact item)
        {
            if (item == null)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await Client.PostAsync($"/api/v2/contact",
                new StringContent(serializedItem,
                    Encoding.UTF8,
                    "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateContactAsync(Contact item, int id)
        {
            if (id <=0)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await Client.PutAsync(new Uri($"api/v2/contact/{id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            if (id <= 0)
                return false;

            var response = await Client.DeleteAsync($"api/v2/contact/{id}");

            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}
