using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private VincereClient client
        {
            get
            {
                var accessToken = HttpContext.Session.GetString("AccessToken");
                var idToken = HttpContext.Session.GetString("IdToken");

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    Response.Redirect("/");
                }

                var client = new VincereClient(VincereConfig.ClientId, VincereConfig.ApiKey, VincereConfig.DomainId)
                {
                    AccessToken = accessToken,
                    IdToken = idToken
                };

                return client;
            }
        }

        public ContactsController(IOptions<VincereConfig> vincereConfig)
        {
            VincereConfig = vincereConfig.Value;
        }

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            try
            {
                var model = await client.GetContactsAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index),"Contacts");
            }
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await client.GetContactAsync(id);
            return View(model);
        }

        // GET: Contacts/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm]Contact contact)
        {
            try
            {
                // TODO: Add insert logic here
                var result = await client.AddContactAsync(contact);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await client.GetContactAsync(id);
            return View(model);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [FromForm]Contact contact)
        {
            try
            {
                // TODO: Add update logic here
                var result = await client.UpdateContactAsync(contact, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = await client.GetContactAsync(id);
                return View(model);
            }
            catch (HttpRequestException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await client.DeleteContactAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}