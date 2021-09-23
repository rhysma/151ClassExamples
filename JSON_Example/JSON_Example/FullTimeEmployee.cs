using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Example
{
    class FullTimeEmployee : Employee
    {
        private decimal salary;
        private int vacationDays;
        private bool benefits;

        /// <summary>
        /// Full Time Employee is a dervied class of the base Employee object
        /// It contains all of the specific properties for a full time employee
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="empID"></param>
        /// <param name="age"></param>
        /// <param name="ssn"></param>
        /// <param name="dept"></param>
        /// <param name="address"></param>
        /// <param name="level"></param>
        /// <param name="salary"></param>
        /// <param name="benefits"></param>
        /// <param name="vacation"></param>
        public FullTimeEmployee(string fname, string lname, string empID, int age, string ssn, string dept, string address, string level, decimal salary, bool benefits, int vacation)
            : base(empID, fname, lname, address, age, dept, ssn, level)
        {
            Salary = salary;
            HasBenefits = benefits;
            VacationDays = vacation;
        }

        public decimal Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public bool HasBenefits
        {
            get { return benefits; }
            set { benefits = value; }
        }

        public int VacationDays
        {
            get { return vacationDays; }
            set { vacationDays = value; }
        }

    }
}
