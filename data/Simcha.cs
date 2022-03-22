using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
        public bool AlwaysInclude { get; set; }
        public bool Include { get; set; }

    }
    public class Simcha
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int NumOfContributers { get; set; }
    }
    public class History
    {
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
