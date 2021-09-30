using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExample
{
    class Manager : Employee
    {
        private string location;
        private string assistant;
        private bool hasVacation;
        private string parkingSpace;
        private decimal salary;

        public Manager(string fname, string lname, string empID, int age, string ssn, string dept, string address, string level, decimal salary, string location, string assistant, bool vacation, string parking)
            : base(empID, fname, lname, address, age, dept, ssn, level)
        {
            Location = location;
            Assistant = assistant;
            HasVacation = vacation;
            ParkingSpace = parking;
            Salary = salary;
        }

        public string Location { get; set; }
        public string Assistant { get; set; }
        public bool HasVacation { get; set; }
        public string ParkingSpace { get; set; }
        public decimal Salary { get; set; }

    }
}
