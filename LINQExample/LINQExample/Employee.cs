using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExample
{
    class Employee
    {
        private string empID;
        private string fname;
        private string lname;
        private string address;
        private int age;
        private string dept;
        private string ssn;
        private string access;

        /// <summary>
        /// The employee class is the base class for other derived employee objects. It contains all of the base properties for an employee
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="address"></param>
        /// <param name="age"></param>
        /// <param name="dept"></param>
        /// <param name="ssn"></param>
        /// <param name="access"></param>
        public Employee(string empId, string fname, string lname, string address, int age, string dept, string ssn, string access)
        {
            EmpID = empId;
            FirstName = fname;
            LastName = lname;
            Address = address;
            Age = age;
            Department = dept;
            SSN = ssn;
            AccessLevel = access;
        }

        public string EmpID
        {
            get { return empID; }
            set { empID = value; }
        }

        public string FirstName
        {
            get { return fname; }
            set { fname = value; }
        }

        public string LastName
        {
            get { return lname; }
            set { lname = value; }
        }


        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Department
        {
            get { return dept; }
            set { dept = value; }
        }

        public string SSN
        {
            get { return ssn; }
            set { ssn = value; }
        }

        public string AccessLevel
        {
            get { return access; }
            set { access = value; }
        }

        public override string ToString()
        {
            return LastName + ", " + FirstName;
        }

    }

}
