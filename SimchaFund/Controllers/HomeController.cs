using data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimchaFund.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Simcha;Integrated Security=true;";
        public IActionResult Contributors()
        {
            dbMngr c = new dbMngr(_connectionString);
            var p = c.GetPeople();
            return View(new ViewModel
            {
                People = p,
                NewPersonTempData = (string)TempData["personCreated"],
                DepositTempData = (string)TempData["Deposit"],
                Total = p.Select(x => x.Balance).ToList().Sum(),
                UpdatePersonTempData = (string)TempData["updatePerson"]
            });
        }
        [HttpPost]
        public IActionResult add(Person p)
        {
            dbMngr c = new dbMngr(_connectionString);
            c.AddPerson(p);
            TempData["personCreated"] = "New Contributor Created!! :)";
            return Redirect("/home/Contributors");
        }
        [HttpPost]
        public IActionResult addSimcha(Simcha s)
        {
            dbMngr c = new dbMngr(_connectionString);
            c.AddSimcha(s);
            TempData["message"] = "New Simcha Created!! :)";
            return Redirect("/home/Simchas");
        }
        public IActionResult Simchas()
        {
            dbMngr c = new dbMngr(_connectionString);
            return View(new ViewModel
            {
                Simchos = c.GetSimchos(),
                NewSimchaTempdata = (string)TempData["message"],
                People = c.GetPeople(),
                UpdateSimchaTempData = (string)TempData["updateSimcha"]
            });
        }
        public IActionResult History(int id)
        {
            dbMngr c = new dbMngr(_connectionString);
            return View(new ViewModel
            {
                NameOfPerson = c.GetPersonById(id),
                History = c.GetPersonHistory(id).OrderByDescending(x => x.Date).ToList()
            });
        }
        public IActionResult contributions(int id)
        {
            dbMngr c = new dbMngr(_connectionString);
            var x = c.GetPeople();
            foreach (var p in x)
            {
                p.Include = c.CheckIfContributed(p.Id, id);
            };
            return View(new ViewModel
            {
                People = x,
                NameOfSimcha = c.GetSimchaById(id)
            });
        }
        [HttpPost]
        public IActionResult update(int SimchaId, List<Person> person)
        {
            dbMngr c = new dbMngr(_connectionString);
            c.DeleteSimchaContributers(SimchaId);
            for (int i = 0; i < person.Count(); i++)
            {
                if (person[i].Include)
                {
                    c.UpdateSimcha(SimchaId, person[i].Balance, person[i].Id);
                }
            }
            TempData["updateSimcha"] = "Simcha updated successfully :)";
            return Redirect("/home/Simchas");
        }
        [HttpPost]
        public IActionResult saveDeposit(int personId, decimal amount, DateTime date)
        {
            dbMngr c = new dbMngr(_connectionString);
            c.AddDeposit(personId, amount, date);
            TempData["Deposit"] = "Deposit successfully recorded :)";
            return Redirect("/home/Contributors");
        }
        [HttpPost]
        public IActionResult Edit(Person p)
        {
            dbMngr c = new dbMngr(_connectionString);
            c.EditPerson(p);
            TempData["updatePerson"] = "person updated successfully :)";
            return Redirect("/home/Contributors");
        }
    }
}
