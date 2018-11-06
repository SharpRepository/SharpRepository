using Microsoft.AspNetCore.Mvc;
using SharpRepository.CoreMvc.Models;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Samples.CoreMvc.CustomRepositories;
using SharpRepository.Samples.CoreMvc.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.CoreMvc.Controllers
{
    public class Emails2Controller : Controller
    {
        protected IRepository<Email, int> repository;
        protected IRepository<Contact, string> repositoryContacts;

        public Emails2Controller(IRepository<Email, int> repository, IRepository<Contact, string> contactRepository)
        {
            this.repository = repository;
            this.repositoryContacts = contactRepository;
        }

        // GET: Mails
        public ActionResult Index()
        {
            var mails = repository.GetMails().ToArray();
            //var mails = repositoryContacts.GetAll(r => r.Emails, new PagingOptions<Contact, string>(1, 2, x => x.Name));

            return View(mails);
        }
        
        public ActionResult Create(string id)
        {
            ViewBag.ContactId = id;
            var email = new Email();
            return View(email);
        }

        [HttpPost]
        public ActionResult Create(Email email)
        {
            string contactId = this.Request.Form["ContactId"];
            var contact = repositoryContacts.Get(contactId);

            if (contact.Emails == null)
            {
                contact.Emails = new List<Email>();
            }

            contact.Emails.Add(email);
            repositoryContacts.Update(contact);

            return RedirectToAction("Index");
        }
    }
}