using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintFromWindowsForm
{
    class Person
    {
        public Person(string first, string last, string address)
        {
            FirstName = first;
            LastName = last;
            Address = address;
        }

        public Person()
        {

        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string FormatPrint()
        {
            return FirstName + " " + LastName + "\n" + Address;
        }
    }
}
