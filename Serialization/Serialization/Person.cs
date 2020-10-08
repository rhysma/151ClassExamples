using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    [Serializable]
    class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Employer { get; set; }

        public override string ToString()
        {
            return Name + " is " + Age + " years old";
        }

    }
}
