using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml; //needed to work with XML classes

namespace SimpleXML
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new XML document from a hard-coded string (could also be a file you read in)
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" +
                        "<title>Pride And Prejudice</title>" +
                        "</book>");

            //set up a XML element root from the structure we read in
            XmlElement root = doc.DocumentElement;

            // Check to see if the element has a genre attribute
            if (root.HasAttribute("genre"))
            {
                //find the node that has the information we want
                String genre = root.GetAttribute("genre");

                //print
                Console.WriteLine(genre);
            }


        }
    }
}
