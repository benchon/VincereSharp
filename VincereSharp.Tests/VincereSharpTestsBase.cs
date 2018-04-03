using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VincereSharp.UnitTests
{
    public class VincereSharpTestsBase
    {
        public static VincereClient client;

        protected async Task SetupAuth()
        {
            var tokenResponse = await client.GetRefreshToken();
        }

        public VincereSharpTestsBase()
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
    }
}