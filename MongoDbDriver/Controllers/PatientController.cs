using System.Collections.Generic;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using MongoDbDriver.Models;
using MongoDB.Bson;
using SharpRepository.Repository;
using Patient = MongoDbDriver.Models.Patient;

namespace MongoDbDriver.Controllers
{
    public class PatientController : Controller
    {
        private IRepository<Patient, string> _repository;
        public PatientController(IRepository<Models.Patient, string> repository)
        {
            _repository = repository;
        }

        public IActionResult Patient()
        {
            return View();
        }

        public ActionResult Create(Models.PatientCreateModel patient)
        {
            var newPatient = new Patient()
            {
                Name = new List<HumanName>() {new HumanName() {Given = new[] {patient.Name}, Family = patient.Name}}
            };
            _repository.Add(newPatient);

            return RedirectToAction(nameof(Patient));
        }
    }
}