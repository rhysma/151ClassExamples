using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace JSON_Example
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //this is a sample application to show how to use Newtonsoft's JSON package to read in and write out JSON object data

            //create employee objects
            FullTimeEmployee emp1 = new FullTimeEmployee("Sarah", "Gregson", "0001", 43, "555-55-5555", "Sales", "123 Somewhere street, Portland OR", "2", 35000, true, 7);
            FullTimeEmployee emp2 = new FullTimeEmployee("George", "Harrison", "0002", 51, "555-55-5555", "Marketing", "345 Somewhere street, Portland OR", "4", 45000, true, 8);
            FullTimeEmployee emp3 = new FullTimeEmployee("Mark", "Johnson", "0003", 40, "555-55-5555", "Sales", "678 Somewhere street, Portland OR", "2", 40000, true, 9);

            //add all of the employee objects to a list
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(emp1);
            employeeList.Add(emp2);
            employeeList.Add(emp3);

            //save the objects to a JSON File
            SaveJSON(employeeList);

            //load the objects from a JSON File
            List<Employee> readEmployees = LoadJSON();

            //loop through the returned list and display
            foreach(Employee emp in readEmployees)
            {
                Console.WriteLine(emp);
            }


            Console.ReadKey();
        }

        /// <summary>
        /// This method loads in a JSON File form a fixed location (same file location as the program's EXE)
        /// The method uses the JSON library to deserialize the object and put it into a list which is returned
        /// </summary>
        public static List<Employee> LoadJSON()
        {
            try
            {
                using (StreamReader r = new StreamReader("employees.json"))
                {
                    string json = r.ReadToEnd();
                    List<Employee> items = JsonConvert.DeserializeObject<List<Employee>>(json);
                    return items;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public static void SaveJSON(List<Employee> list)
        {
            try
            {
                //set up the file we want to save to
                string filename = "employees.json";

                //serialize the list to JSON data
                string json = JsonConvert.SerializeObject(list);

                //write it to the file
                File.WriteAllText(filename, json);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
        }
    }
}
