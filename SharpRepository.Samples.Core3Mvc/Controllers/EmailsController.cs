using Microsoft.AspNetCore.Mvc;
using SharpRepository.Samples.Core3Mvc.Models;
using SharpRepository.Repository;
using SharpRepository.Samples.Core3Mvc.CustomRepositories;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.Samples.Core3Mvc.Controllers
{
    public class EmailsController : Controller
    {
        protected EmailRepository repository;
        protected IRepository<Contact, string> repositoryContacts;

        public EmailsController(EmailRepository repository, IRepository<Contact, string> contactRepository)
        {
            this.repository = repository;
            this.repositoryContacts = contactRepository;
        }

        // GET: Mails
        public ActionResult Index()
        {
            var mails = repository.GetMails().ToArray();

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