using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VincereSharp.UnitTests;

namespace VincereSharp.Tests
{
    [TestClass]
    public class GetReferenceData : VincereSharpTestsBase
    {

        [TestMethod]
        public async Task GetIndustries()
        {
            if (string.IsNullOrWhiteSpace(client.IdToken))
                await SetupAuth();

            var industries = await client.GetIndustries();

            Assert.IsNotNull(industries);
        }
    }
}
