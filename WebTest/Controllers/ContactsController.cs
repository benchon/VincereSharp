using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VincereSharp;
using WebTest.Services;

namespace WebTest.Controllers
{
    public class ContactsController : Controller
    {
        private VincereClient VincereClient
        {
            get => VincereSessionHelper.Client;
            set => VincereSessionHelper.Client = value;
        }

        public ContactsController(IOptions<VincereConfig> vincereConfig)
        {
            if (VincereClient == null)
                VincereClient = new VincereClient(vincereConfig.Value);
        }

        // GET: Contacts
        public async Task<ActionResult> Index([FromQuery] string searchText = "")
        {
            try
            {
                var model = await VincereClient.GetContactsAsync(searchText);
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
                throw;
            }
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await VincereClient.GetContactAsync(id);
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
                contact.RegistrationDate = DateTime.Now;
                var result = await VincereClient.AddContactAsync(contact);
                TempData["Message"] = "Contact Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await VincereClient.GetContactAsync(id);
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
                var result = await VincereClient.UpdateContactAsync(contact, id);
                TempData["Message"] = "Contact Updated";
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
                var model = await VincereClient.GetContactAsync(id);
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
                var response = await VincereClient.DeleteContactAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
                return View();

            }
        }
    }
}