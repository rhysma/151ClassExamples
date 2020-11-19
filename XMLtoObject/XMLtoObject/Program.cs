using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLtoObject
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new instance of our XML Serializer class
            XMLSerializer ser = new XMLSerializer();

            //variable to hold the path of the file we want to work with
            string path = string.Empty;

            //variables to hold the input and output 
            string xmlInputData = string.Empty;
            string xmlOutputData = string.Empty;

            //--------------------------------------//
            // EXAMPLE 1 - only works with customer object
            //path assumes XML file in the bin directory
            path = Directory.GetCurrentDirectory() + @"\CustomerFile1.xml";

            //read in the xml data 
            xmlInputData = File.ReadAllText(path);

            //use the XML serializer to walk through the XML structure and assign
            //each node to customer class properties
            Customer customer = ser.Deserialize<Customer>(xmlInputData);

            //reverse the process - take the customer object and turn it into XML structure
            xmlOutputData = ser.Serialize<Customer>(customer);

            Console.WriteLine(xmlInputData); //write out the structure
            Console.WriteLine(" ");
            Console.WriteLine(customer); //write out the class object
            Console.WriteLine(" ");

            //--------------------------------------//
            // EXAMPLE 2 - works with multiple classes
            path = Directory.GetCurrentDirectory() + @"\CustomerFile2.xml";
            xmlInputData = File.ReadAllText(path);

            //use the XML serializer to walk through the XML structure and assign
            //each node to customer class properties
            Customer customer2 = ser.Deserialize<Customer>(xmlInputData);

            //reverse the process - take the customer object and turn it into XML structure
            xmlOutputData = ser.Serialize<Customer>(customer2);

            Console.WriteLine(xmlInputData); //write out the structure
            Console.WriteLine(" ");
            Console.WriteLine(customer2); //write out the class object
        }
    }
}
