using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExample
{
    class Program
    {
        //this example demonstrates how to use LINQ to filter a list of objects based on specific properties or by type
        static void Main(string[] args)
        {
            //a list to hold all of the objects
            List<Employee> employeeList = new List<Employee>();

            //create full time employee objects
            FullTimeEmployee emp1 = new FullTimeEmployee("Sarah", "Gregson", "0001", 43, "555-55-5555", "Sales", "123 Somewhere street, Portland OR", "2", 35000, true, 7);
            FullTimeEmployee emp2 = new FullTimeEmployee("George", "Harrison", "0002", 51, "555-55-5555", "Marketing", "345 Somewhere street, Portland OR", "4", 45000, true, 8);
            FullTimeEmployee emp3 = new FullTimeEmployee("Mark", "Johnson", "0003", 40, "555-55-5555", "Sales", "678 Somewhere street, Portland OR", "2", 40000, true, 9);

            Manager emp4 = new Manager("Bob", "Vance", "0004", 52, "555-55-5555", "Department Manager", "555 Main Blvd, Portland OR", "6", 60000, "123K", "Anna Summers", true, "G1122");
            Manager emp5 = new Manager("Clara", "Marshall", "0005", 30, "555-55-5555", "Department Manager", "125 Main Blvd, Portland OR", "6", 50000, "124K", "Anna Summers", true, "G1130");
            Manager emp6 = new Manager("Dana", "Clark", "0006", 65, "555-55-5555", "Department Manager", "900 Main Blvd, Portland OR", "6", 70000, "125K", "Anna Summers", true, "G1220");

            //add all of the employee objects to a list
            employeeList.Add(emp1);
            employeeList.Add(emp2);
            employeeList.Add(emp3);
            employeeList.Add(emp4);
            employeeList.Add(emp5);
            employeeList.Add(emp6);

            //display the list
            Console.WriteLine("All Employees");
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine(emp);
            }
            Console.WriteLine("");

            //ask the user which type of employees they want to see
            Console.WriteLine("Choose from the following options to filter the list.");
            Console.WriteLine("1 for Full Time Employees");
            Console.WriteLine("2 for Managers");
            int response = Convert.ToInt32(Console.ReadLine());

            //filter the list using LINQ by class type based on the response using a Where clause
            //also puts them in alphabetical order using an OrderBy
            IEnumerable<Employee> filteredList = employeeList;
            switch(response)
            {
                case 1:
                    filteredList = employeeList.Where(x => x is FullTimeEmployee).OrderBy(x => x.LastName);
                    break;
                case 2:
                    filteredList = employeeList.Where(x => x is Manager).OrderBy(x => x.LastName);
                    break;

            }

            //print out the contents of the filtered list
            foreach (Employee emp in filteredList)
            {
                Console.WriteLine(emp);
            }


            Console.ReadKey();


        }
    }
}
