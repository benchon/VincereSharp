using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VincereSharp;
using WebTest.Services;

namespace WebTest.Controllers
{
    public class VincereControllerBase : Controller
    {
        protected VincereClient VincereClient
        {
            get => VincereSessionHelper.Client;
            set => VincereSessionHelper.Client = value;
        }

        public VincereControllerBase(IOptions<VincereConfig> vincereConfig)
        {
            if (VincereClient == null)
                VincereClient = new VincereClient(vincereConfig.Value);
        }
    }
}