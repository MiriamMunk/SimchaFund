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
        public IActionResult Index()
        {
            Class1 c = new Class1(_connectionString);
            return View(new ViewModel
            {
                People = c.getPeople(),
                NewPersonTempData = (string)TempData["personCreated"],
                DepositTempData = (string)TempData["Deposit"],
                Total = c.getPeople().Select(x => x.Deposit).ToList().Sum(),
                UpdatePersonTempData = (string)TempData["updatePerson"]
            });
        }
        public IActionResult add(Person p)
        {
            Class1 c = new Class1(_connectionString);
            c.addPerson(p);
            TempData["personCreated"] = "New Contributor Created!! :)";
            return Redirect("/home/index");
        }
        public IActionResult addSimcha(Simcha s)
        {
            Class1 c = new Class1(_connectionString);
            c.addSimcha(s);
            TempData["message"] = "New Simcha Created!! :)";
            return Redirect("/home/privacy");
        }
        public IActionResult Privacy()
        {
            Class1 c = new Class1(_connectionString);
            return View(new ViewModel
            {
                Simchos = c.getSimchos(),
                NewSimchaTempdata = (string)TempData["message"],
                People = c.getPeople(),
                UpdateSimchaTempData = (string)TempData["updateSimcha"]
            });
        }
        public IActionResult History(int id)
        {
            Class1 c = new Class1(_connectionString);
            return View(new ViewModel
            {
                NameOfPerson = c.getPersonById(id),
                History = c.getPersonHistory(id).OrderByDescending(x => x.Date).ToList()
            });
        }
        public IActionResult contributions(int id)
        {
            Class1 c = new Class1(_connectionString);
            return View(new ViewModel
            {
                People =  c.getPeople(),
                NameOfSimcha = c.getSimchaById(id)
            });
        }
        public IActionResult update(int SimchaId, List<Person> person)
        {
            Class1 c = new Class1(_connectionString);
            for (int i = 0; i < person.Count(); i++)
            {
                if (person[i].AlwaysInclude)
                {
                    c.UpdateSimcha(SimchaId, person[i].Deposit, person[i].Id);
                }
            }
            TempData["updateSimcha"] = "Simcha updated successfully :)";
            return Redirect("/home/privacy");
        }
        public IActionResult saveDeposit(int personId, decimal amount, DateTime date)
        {
            Class1 c = new Class1(_connectionString);
            c.addDeposit(personId, amount, date);
            TempData["Deposit"] = "Deposit successfully recorded :)";
            return Redirect("/home/index");
        }
        public IActionResult Edit(Person p)
        {
            Class1 c = new Class1(_connectionString);
            c.EditPerson(p);
            TempData["updatePerson"] = "person updated successfully :)";
            return Redirect("/home/index");
        }
    }
}
