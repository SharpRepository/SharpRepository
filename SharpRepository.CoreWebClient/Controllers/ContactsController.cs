using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpRepository.Repository;
using SharpRepository.CoreWebClient.Models;

namespace SharpRepository.CoreWebClient.Controllers
{
    public class ContactsController : Controller
    {
        protected IRepository<Contact, int> repository;

        public ContactsController(IRepository<Contact, int> repository)
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
        public ActionResult Details(int id)
        {
            var contact = repository.Get(id);

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
            try
            {
                repository.Add(contact);

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
            var contact = repository.Get(id);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contact contact)
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
        public ActionResult Delete(int id)
        {
            var contact = repository.Get(id);

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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