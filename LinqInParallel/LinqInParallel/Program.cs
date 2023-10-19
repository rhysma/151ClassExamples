using System;
using static System.Console;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace LinqInParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var names = new string[] { "Michael", "Pam", "Jim", "Dwight", "Angela", "Kevin", "Toby", "Creed" };
            var query = names.Where(new Func<string, bool>(NameLongerThanFour));
            //var query = names.Where(NameLongerThanFour);
            //var query = names.Where(name => name.Length > 4);

            foreach (string item in query)
            {
                WriteLine(item);
            }

            var watch = new Stopwatch();
            Write("Press ENTER to start: ");
            ReadLine();
            watch.Start();
            IEnumerable<int> numbers = Enumerable.Range(1, 1_000_000);
            //var squares = numbers.Select(number => number * number).ToArray();
            var squares = numbers.AsParallel().Select(number => number * number).ToArray();
            watch.Stop();
            WriteLine("{0:#,##0} elapsed milliseconds.",
            arg0: watch.ElapsedMilliseconds);
        }

        static bool NameLongerThanFour(string name)
        {
            return name.Length > 4;
        }
    }
}