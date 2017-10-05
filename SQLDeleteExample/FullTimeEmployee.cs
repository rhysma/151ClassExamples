using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlDelete
{
    class FullTimeEmployee : Employee
    {
        private decimal salary;
        private int vacationDays;
        private bool benefits;

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

        public override string ToString()
        {
            return LastName + ", " + FirstName;
        }
    }
}
