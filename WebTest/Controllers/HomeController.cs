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
using WebTest.Services;


namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        private VincereClient VincereClient
        {
            get => VincereSessionHelper.Client;
            set => VincereSessionHelper.Client = value;
        }

        public HomeController(IOptions<VincereConfig> vincereConfig)
        {
            if(VincereClient==null)
                VincereClient = new VincereClient(vincereConfig.Value);
        }

        public async Task<IActionResult> Index([FromQuery(Name = "code")] string code)
        {
            if (!string.IsNullOrWhiteSpace(VincereClient.IdToken)) // Already have access token
            {
                return RedirectToAction("Index", "Contacts");
            }
            if (!string.IsNullOrWhiteSpace(code)) // have the code to get an access token
            {
                var tokenResponse = await VincereClient.GetAuthCode(code);
                //HandleAuthResponse(tokenResponse);
                return RedirectToAction("Index", "Contacts");
            }
            else if (System.IO.File.Exists("./RefreshToken.txt") || !string.IsNullOrWhiteSpace(VincereClient.RefresherToken)) // have a Refresh token to get an access token
            {
                var refreshToken = VincereClient.RefresherToken ?? System.IO.File.ReadAllText("./RefreshToken.txt").Trim();
                var tokenResponse = await VincereClient.GetRefreshToken(refreshToken);
                //(tokenResponse);
                return RedirectToAction("Index", "Contacts");
            }
            else
            {
                // no previous auth so we need a login url
                var returnUrl = VincereClient.Config.RedirectUrl;
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    //returnUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
                    returnUrl = HttpContext.Request.GetDisplayUrl();
                }
                ViewData["LoginUrl"] = VincereClient.GetLoginUrl(returnUrl);
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
