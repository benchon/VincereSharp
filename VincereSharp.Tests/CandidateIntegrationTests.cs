using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VincereSharp.UnitTests
{
    [TestClass]
    public class CandidateIntegrationTests : VincereSharpTestsBase
    {
        public CandidateIntegrationTests() : base()
        {
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

