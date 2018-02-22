using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VincereSharp;

namespace WebTest.Controllers
{
    public class ContactsController : Controller
    {
        public VincereConfig VincereConfig { get; }
        public ContactsController(IOptions<VincereConfig> vincereConfig)
        {
            VincereConfig = vincereConfig.Value;
        }

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            var token = HttpContext.Session.GetString("VincereToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                Response.Redirect("~/");
            }

            var client = new VincereClient(VincereConfig.ClientId, VincereConfig.ApiKey)
            {
                Token = token
            };

            var model = await client.GetContactsAsync();
            return View(model);
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}