using SharpRepository.Repository;
using SharpRepository.Samples.MVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharpRepository.Samples.MVC5.Controllers
{
    public class ContactsController : Controller
    {
        IRepository<Contact, int> repository;

        public ContactsController(IRepository<Contact, int> repository)
        {
            this.repository = repository;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }

            base.Dispose(disposing);
        }

        // GET: Contacts
        public ActionResult Index()
        {
            var contacs = repository.GetAll();
            return View(contacs);
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
        public ActionResult Create(Contact contact)
        {
            repository.Add(contact);
            return RedirectToAction("Index");
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            var contact = repository.Get(id);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            repository.Update(contact);
            return RedirectToAction("Index");
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int id)
        {
            var contact = repository.Get(id);
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
