using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using VincereSharp;
using WebTest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        public VincereConfig VincereConfig { get; }
        public HomeController(IOptions<VincereConfig> vincereConfig)
        {
            VincereConfig = vincereConfig.Value;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "code")] string code)
        {

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                Response.Redirect("/Contacts");
            }

            var vClient = new VincereClient(VincereConfig.ClientId, VincereConfig.ApiKey);
            if (!string.IsNullOrWhiteSpace(code))
            {
                var tokenResponse = await vClient.GetAuthCode(code);
                HttpContext.Session.SetString("AccessToken", tokenResponse.AccessToken);
                HttpContext.Session.SetString("RefresherToken", tokenResponse.RefreshToken);
                HttpContext.Session.SetString("IdToken", tokenResponse.IdToken);
                Response.Redirect("/Contacts");
            }

            var returnUrl = VincereConfig.RedirectUrl;
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
                returnUrl = HttpContext.Request.GetDisplayUrl();
            }
            ViewData["LoginUrl"] = vClient.GetLoginUrl(returnUrl);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
