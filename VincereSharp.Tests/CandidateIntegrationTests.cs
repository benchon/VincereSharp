using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VincereSharp.UnitTests
{
    [TestClass]
    public class CandidateIntegrationTests
    {
        public static VincereClient client;

        public CandidateIntegrationTests()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            const string webTestLoc = "../../../../WebTest";
            var settingsFileLoc = $"{ webTestLoc }/RefreshToken.txt";

            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath($"{ webTestLoc }/appsettings.json"))
                .Build();

            if (!File.Exists(settingsFileLoc))
            {
                throw new FileNotFoundException("Run the web project to login first so that RefreshToken.txt is created");
            }

            var clientId = config["Vincere:ClientId"];
            var apiKey = config["Vincere:ApiKey"];
            var domainId = config["Vincere:DomainId"];
            var refreshToken = File.ReadAllText(settingsFileLoc);

            client = new VincereClient(clientId, apiKey, domainId)
            {
                RefresherToken = refreshToken
            };
        }

        private async Task SetupAuth()
        {
            var tokenResponse = await client.GetRefreshToken();
        }

        [TestMethod]
        public async Task SearchCandidates()
        {
            // todo - perhaps this should be moved into the vincere client when the idtoken is null
            if (string.IsNullOrWhiteSpace(client.IdToken))
                await SetupAuth();

            var candidates = await client.SearchCandidatesAsync();

            Assert.IsNotNull(candidates);
        }

        [TestMethod]
        public async Task Create_Update_Delete_Candidates()
        {
            var candidateSources = await client.GetCandidateSources();

            var candidate = new Candidate()
            {
                FirstName = "Test",
                LastName = "Please Delete",
                Email = "test@test.com",
                CandidateSourceId = int.Parse(candidateSources[0].Value),
                RegistrationDate = DateTime.Now
            };

            Assert.IsTrue(candidate.IsValid());
                
            var id = await client.AddCandidateAsync(candidate);

            Assert.IsTrue(id>0);

            candidate.FirstName = "Updated Firstname";

            var result = await client.UpdateCandidateAsync(candidate, id);

            Assert.IsTrue(result);

            var deleteResult = await client.DeleteCandidateAsync(id, "Testing");

            Assert.IsTrue(deleteResult);
        }

        [TestMethod]
        public async Task DeleteCandidate()
        {
            var id = 1000;
            var deleteResult = await client.DeleteCandidateAsync(id, "Testing");

            Assert.IsTrue(deleteResult);

        }
    }
}

