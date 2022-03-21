using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data;

namespace SimchaFund.Models
{
    public class ViewModel
    {
        public List<Person> People { get; set; }
        public List<Simcha> Simchos { get; set; }
        public string NewSimchaTempdata { get; set; }
        public string NewPersonTempData { get; set; }
        public string DepositTempData { get; set; }
        public string UpdateSimchaTempData { get; set; }
        public string UpdatePersonTempData { get; set; }
        public List<History> History { get; set; }
        public Simcha NameOfSimcha { get; set; }
        public Person NameOfPerson { get; set; }
        public decimal Total { get; set; }
    }
   
}
