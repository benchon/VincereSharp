using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VincereSharp
{
    public class VincereClient
    {
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

        #region "Http"

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

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.'000Z'"
        };

        private string CreateSerializedItem(Object obj)
        {
            return JsonConvert.SerializeObject(obj,
                                            Formatting.None,
                                            jsonSerializerSettings);
        }

        private StringContent BuildRequestContent(object item)
        {
            return BuildRequestContent(CreateSerializedItem(item));
        }

        private StringContent BuildRequestContent(string serializedItem)
        {
            return new StringContent(serializedItem,
                Encoding.UTF8,
                "application/json");
        }

        #endregion

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


        public async Task<TokenResponse> GetRefreshToken()
        {
            return await GetRefreshToken(this.RefresherToken);
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

        private async Task CheckAuthToken()
        {
            if (string.IsNullOrWhiteSpace(IdToken))
            {
                if (string.IsNullOrWhiteSpace(RefresherToken))
                    throw new AuthenticationException("Initial login required to generate refresh token");

                await this.GetRefreshToken();
            }
        }

        #endregion

        #region "Candidates"

        public async Task<IEnumerable<CandidateSearchResultItem>> SearchCandidatesAsync(string searchText = "")
        {
            await CheckAuthToken();

            var searchUrl =
                $"/api/v2/candidate/search/fl=id,name,current_job_title,email,phone;sort=created_date asc";

            if (!string.IsNullOrWhiteSpace(searchText))
                searchUrl += $"?q=text:{searchText}";
            try
            {
                var json = await Client.GetStringAsync(searchUrl);
                var response = await Task.Run(() => JsonConvert.DeserializeObject<SearchResult<CandidateSearchResultItem>>(json));
                return response.Result.Items;
            }
            catch (HttpRequestException hrex)
            {
                if (string.IsNullOrWhiteSpace(this.RefresherToken)) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.SearchCandidatesAsync(searchText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Candidate> GetCandidateAsync(int id, int retry = 2)
        {
            await CheckAuthToken();

            try
            {
                var json = await Client.GetStringAsync($"/api/v2/candidate/{id}");
                var Candidate = JsonConvert.DeserializeObject<Candidate>(json);
                return Candidate;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                if (retry <= 0) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.GetCandidateAsync(id, --retry);
            }
        }

        public async Task<int> AddCandidateAsync(Candidate item)
        {
            if (item == null)
                throw new NullReferenceException("Candidate is null");

            await CheckAuthToken();

            try
            {
                var response = await Client.PostAsync("/api/v2/candidate", BuildRequestContent(item));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ObjectCreatedResponse>(json).id;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetRefreshToken();
                    return await AddCandidateAsync(item);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return 0;
        }

        public async Task<bool> UpdateCandidateAsync(Candidate item, int id)
        {
            await CheckAuthToken();

            if (id <= 0)
                return false;

            try
            {
                var response = await Client.PutAsync($"/api/v2/candidate/{id}", BuildRequestContent(item));

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.UpdateCandidateAsync(item, id);
            }

        }

        public async Task<bool> DeleteCandidateAsync(int id, string reason)
        {
            await CheckAuthToken();

            if (id <= 0)
                return false;

            var request = new HttpRequestMessage
            {
                Content = BuildRequestContent(new CandidateDeleteReason(reason)),
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"api/v2/candidate/{id}", UriKind.Relative)
            };

            var response = await Client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<DeleteResponse>(json);

            return response.IsSuccessStatusCode;
        }

        public async Task<int> LinkCandidateIndustries(int Id, int[] IndustryIds)
        {
            await CheckAuthToken();

            try
            {
                var response = await Client.PutAsync($"/api/v2/candidate/{ Id }/industries", BuildRequestContent(IndustryIds));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ObjectCreatedResponse>(json).id;
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return 0;
        }
        #endregion

        #region "Contact"

        public async Task<IEnumerable<ContactSearchResultItem>> SearchContactsAsync(string searchText = "")
        {
            var searchUrl =
                $"/api/v2/contact/search/fl=id,name,job_title,email,company,phone,note;sort=created_date asc";

            if (!string.IsNullOrWhiteSpace(searchText))
                searchUrl += $"?q=text:{searchText}";
            try
            {
                var json = await Client.GetStringAsync(searchUrl);
                var response = await Task.Run(() => JsonConvert.DeserializeObject<SearchResult<ContactSearchResultItem>>(json));
                return response.Result.Items;
            }
            catch (HttpRequestException hrex)
            {
                if (string.IsNullOrWhiteSpace(this.RefresherToken)) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.SearchContactsAsync(searchText);
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

            await CheckAuthToken();

            try
            {
                var response = await Client.PostAsync("/api/v2/contact", BuildRequestContent(item));
                var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ObjectCreatedResponse>(json).id;
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

            try
            {
                var response = await Client.PutAsync($"/api/v2/contact/{id}", BuildRequestContent(item));

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

        #region "Companies"

        public async Task<IEnumerable<CompanySearchResultItem>> SearchCompaniesAsync(string searchText = "", string companyName = "")
        {
            var searchUrl =
                $"/api/v2/company/search/fl=id,name;sort=created_date asc";

            Dictionary<string, string> searchVariables = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(searchText))
                searchVariables.Add("text", searchText);

            if (!string.IsNullOrWhiteSpace(companyName))
                searchVariables.Add("name", companyName);

            if (searchVariables.Count>0)
            {
                searchUrl += $"?q=";
           
                int count = 1;               
                foreach (var keyValuePair in searchVariables)
                {
                    searchUrl += $"{keyValuePair.Key}:{keyValuePair.Value}";
                    if (count < searchVariables.Count)
                    {
                        searchUrl += "&";
                    }

                    count++;
                }
            }
            
            try
            {
                var json = await Client.GetStringAsync(searchUrl);
                var response = JsonConvert.DeserializeObject<SearchResult<CompanySearchResultItem>>(json);
                return response.Result.Items;
            }
            catch (HttpRequestException hrex)
            {
                Console.WriteLine(hrex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Company> GetCompanyAsync(int id, int retry = 2)
        {
            try
            {
                var json = await Client.GetStringAsync($"/api/v2/company/{id}");
                var company = JsonConvert.DeserializeObject<Company>(json);
                return company;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                if (retry <= 0) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.GetCompanyAsync(id, --retry);
            }
        }

        public async Task<int> AddCompanyAsync(Company item)
        {
            if (item == null)
                throw new NullReferenceException("Company is null");

            await CheckAuthToken();

            try
            {
                var response = await Client.PostAsync("/api/v2/company", BuildRequestContent(item));
                var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ObjectCreatedResponse>(json).id;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.AddCompanyAsync(item);
            }
        }

        public async Task<bool> UpdateCompanyAsync(Contact item, int id)
        {
            if (id <= 0)
                return false;

            try
            {
                var response = await Client.PutAsync($"/api/v2/company/{id}", BuildRequestContent(item));

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.UpdateCompanyAsync(item, id);
            }

        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            if (id <= 0)
                return false;

            var response = await Client.DeleteAsync($"api/v2/company/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<DeleteResponse>(json);

            return response.IsSuccessStatusCode;
        }

        #endregion

        #region "Jobs"

        public async Task<IEnumerable<JobSearchResultItem>> SearchJobsAsync(string searchText = "")
        {
            var searchUrl =
                $"/api/v2/job/search/fl=id,name,job_title,email,company,phone,note;sort=created_date asc";

            if (!string.IsNullOrWhiteSpace(searchText))
                searchUrl += $"?q=text:{searchText}";
            try
            {
                var json = await Client.GetStringAsync(searchUrl);
                var response = await Task.Run(() => JsonConvert.DeserializeObject<SearchResult<JobSearchResultItem>>(json));
                return response.Result.Items;
            }
            catch (HttpRequestException hrex)
            {
                if (string.IsNullOrWhiteSpace(this.RefresherToken)) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.SearchJobsAsync(searchText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Job> GetJobAsync(int id, int retry = 2)
        {
            try
            {
                var json = await Client.GetStringAsync($"/api/v2/job/{id}");
                var job = JsonConvert.DeserializeObject<Job>(json);
                return job;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                if (retry <= 0) throw;

                await this.GetRefreshToken(this.RefresherToken);
                return await this.GetJobAsync(id, --retry);
            }
        }

        public async Task<int> AddJobAsync(Job item)
        {
            if (item == null)
                throw new NullReferenceException("Job is null");

            await CheckAuthToken();

            try
            {
                var response = await Client.PostAsync("/api/v2/job", BuildRequestContent(item));
                var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ObjectCreatedResponse>(json).id;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.AddJobAsync(item);
            }
        }

        public async Task<bool> UpdateJobAsync(Job item, int id)
        {
            if (id <= 0)
                return false;

            try
            {
                var response = await Client.PutAsync($"/api/v2/job/{id}", BuildRequestContent(item));

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                await this.GetRefreshToken(this.RefresherToken);
                return await this.UpdateJobAsync(item, id);
            }

        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            if (id <= 0)
                return false;

            var response = await Client.DeleteAsync($"api/v2/job/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<DeleteResponse>(json);

            return response.IsSuccessStatusCode;
        }


        #endregion

        #region  "Applications"


        #endregion

        #region "Files"

        public async Task<int> DocumentUploadAsync(int id, string url, string fileName, int documentTypeId, bool isOriginalCV = false)
        {
            await CheckAuthToken();

            var docRequest = new DocumentUploadRequest()
            {
                Url = url,
                OriginalCv = isOriginalCV,
                FileName = fileName,
                DocumentTypeId = documentTypeId
            };

            try
            {
                var response = await Client.PostAsync($"/api/v2/candidate/{ id }/file", BuildRequestContent(docRequest));
                var json = await response.Content.ReadAsStringAsync();
                var responseObj = await Task.Run(() => JsonConvert.DeserializeObject<DocumentUploadResponse>(json));
                return responseObj.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Candidate> ParseResumeAsync(string resumeUrl)
        {
            await CheckAuthToken();

            var item = new ResumeParseRequest()
            {
                Url = resumeUrl
            };

            try
            {
                var RequestContent = BuildRequestContent(item);
                var response = await Client.PostAsync("/api/v2/resume/parse", RequestContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Candidate>(json);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetRefreshToken();
                    return await ParseResumeAsync(resumeUrl);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return null;
        }


        #endregion

        #region "Activities"


        #endregion

        #region "User"


        #endregion

        #region "Search"


        #endregion

        #region "Reports

        #endregion

        #region "References"

        public async Task<ReferenceResponse[]> GetCandidateSources()
        {
            await CheckAuthToken();

            var response = await Client.GetAsync("/api/v2/candidatesources");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        public async Task<ReferenceResponse[]> GetDocumentTypes()
        {
            await CheckAuthToken();

            var response = await Client.GetAsync("/api/v2/def/candidate/documenttypes");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        public async Task<ReferenceResponse[]> GetFunctionalExpertises()
        {
            await CheckAuthToken();

            var response = await Client.GetAsync("/api/v2/functionalexpertises");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        public async Task<ReferenceResponse[]> GetSubFunctionalExpertises(int id)
        {
            await CheckAuthToken();

            var response = await Client.GetAsync($"/api/v2/functionalexpertise/{ id }/subfunctionalexpertises");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        public async Task<ReferenceResponse[]> GetSubFunctionalExpertises()
        {
            await CheckAuthToken();

            var response = await Client.GetAsync($"/api/v2/subfunctionalexpertises");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        public async Task<ReferenceResponse[]> GetIndustries()
        {
            await CheckAuthToken();

            var response = await Client.GetAsync("/api/v2/industries");
            var json = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<ReferenceResponse[]>(json);
            return respObj;
        }

        #endregion

    }
}
