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
using System.IO;


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
            var vClient = new VincereClient(VincereConfig.ClientId, VincereConfig.ApiKey, VincereConfig.DomainId);

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrWhiteSpace(accessToken)) // Already have access token
            {
                Response.Redirect("/Contacts");
            }
            else if (!string.IsNullOrWhiteSpace(code)) // have the code to get an access token
            {
                var tokenResponse = await vClient.GetAuthCode(code);
                HandleAuthResponse(tokenResponse);
            }
            else if (System.IO.File.Exists("./RefreshToken.txt")) // have a Refresh token to get an access token
            {
                var refreshToken = System.IO.File.ReadAllText("./RefreshToken.txt").Trim();
                var tokenResponse = await vClient.GetRefreshToken(refreshToken);
                HandleAuthResponse(tokenResponse);
            }
            else
            {
                // no previous auth so we need a login url
                var returnUrl = VincereConfig.RedirectUrl;
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
                    returnUrl = HttpContext.Request.GetDisplayUrl();
                }
                ViewData["LoginUrl"] = vClient.GetLoginUrl(returnUrl);
            }

            return View();
        }

        private void HandleAuthResponse(TokenResponse tokenResponse)
        {
            HttpContext.Session.SetString("AccessToken", tokenResponse.AccessToken);
            HttpContext.Session.SetString("IdToken", tokenResponse.IdToken);
            if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
            {
                HttpContext.Session.SetString("RefresherToken", tokenResponse.RefreshToken);
                System.IO.File.WriteAllText("./RefreshToken.txt", tokenResponse.RefreshToken);
            }
            Response.Redirect("/Contacts");
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
