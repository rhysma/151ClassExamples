using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions; //needed for regex classes

namespace RegexExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //an example program of how to use regular expressions (regex) to validate data

            //get some info from the user
            Console.WriteLine("Please give a string without numbers");
            string noNums = Console.ReadLine();

            //use regex to see if there are any numbers in the string
            string anyNumbersPattern = "\\d";
            Regex reg = new Regex(anyNumbersPattern);

            while(reg.IsMatch(noNums))
            {
                //the user gave numbers in the string
                Console.WriteLine("You put numbers in your string!");
                Console.WriteLine("Please give a string without numbers");
                noNums = Console.ReadLine();

            }

            //user cleared this check
            Console.WriteLine("Thanks, this is a good string");


            //get some info from the user
            Console.WriteLine("Please give me some numbers");
            string onlyNums = Console.ReadLine();

            //use regex to see if there are any letters in the string
            string anyLettersPattern = "[a-zA-Z]";
            Regex reg2 = new Regex(anyLettersPattern);

            while(reg2.IsMatch(onlyNums))
            {
                //the user gave letters in the string
                Console.WriteLine("You put letters in your string!");
                Console.WriteLine("Please give me some numbers");
                onlyNums = Console.ReadLine();

            }

            //user cleared this check
            Console.WriteLine("Thanks, this is a good string");

            Console.ReadKey();

        }
    }
}
