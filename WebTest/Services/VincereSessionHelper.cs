using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VincereSharp;

namespace WebTest.Services
{
    public static class VincereSessionHelper
    {
        public static void SetClient(this Controller controller, VincereClient value)
        {
                var str = JsonConvert.SerializeObject(value);
                controller.HttpContext.Session.SetString("VincereClient", str);
        }

        public static VincereClient GetClient(this Controller controller, VincereConfig config)
        {
            VincereClient obj;
            if (controller.HttpContext.Session.Keys.Contains("VincereClient"))
            {
                var str = controller.HttpContext.Session.GetString("VincereClient");
                obj = JsonConvert.DeserializeObject<VincereClient>(str);
                obj.Config = config;
            }
            else
            {
                obj = new VincereClient(config);
            }
            return obj;
        }

        private static VincereClient _client;

        public static VincereClient Client
        {
            get { return _client; }
            set => _client = value;
        }
    }
}
