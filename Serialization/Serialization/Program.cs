using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Person myPerson = new Person();
            myPerson.Age = 43;
            myPerson.Name = "Mike Johnson";
            myPerson.Employer = "OTC";

            //call the serializeNow method
            //to package this Person obj up and
            //store it in the DB
            Serializer.SerializeNow(myPerson);

            //call the DB Get method to get all the records
            DataAdapter.Get();

            Console.ReadKey();
        }
    }
}
