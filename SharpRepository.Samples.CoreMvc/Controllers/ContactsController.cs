using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpRepository.Repository;
using SharpRepository.CoreMvc.Models;

namespace SharpRepository.CoreMvc.Controllers
{
    public class ContactsController : Controller
    {
        protected IRepository<Contact, string> repository;

        public ContactsController(IRepository<Contact, string> repository)
        {
            this.repository = repository;
        }

        // GET: Contacts
        public ActionResult Index()
        {
            var contacts = repository.GetAll();

            return View(contacts);
        }

        // GET: Contacts/Details/5
        public ActionResult Details(string id)
        {
            var contact = repository.Get(id, "Emails");

            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            repository.Add(contact);

            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(string id)
        {
            var contact = repository.Get(id);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Contact contact)
        {
            try
            {
                repository.Update(contact);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contact);
            }
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(string id)
        {
            var contact = repository.Get(id);

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                repository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}