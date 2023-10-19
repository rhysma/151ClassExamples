
namespace LINQExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Example - Deferred execution
            // a string array is a sequence that implements IEnumerable<string>
            string[] names = new[] { "Michael", "Pam", "Jim", "Dwight",
                "Angela", "Kevin", "Toby", "Creed" };

            Console.WriteLine("Deferred execution");

            // Question: Which names end with an M?
            // (written using a LINQ extension method)
            var query1 = names.Where(name => name.EndsWith("m"));

            // Question: Which names end with an M?
            // (written using LINQ query comprehension syntax)
            var query2 = from name in names where name.EndsWith("m") select name;

            // Answer returned as an array of strings containing Pam and Jim
            string[] result1 = query1.ToArray();

            // Answer returned as a list of strings containing Pam and Jim
            List<string> result2 = query2.ToList();

            // Answer returned as we enumerate over the results
            foreach (string name in query1)
            {
                Console.WriteLine(name); // outputs Pam
                names[2] = "Jimmy"; // change Jim to Jimmy
                                    // on the second iteration Jimmy does not end with an M
            }

            #endregion

            #region Example - Using Methods
            //var query = names.Where(new Func<string, bool>(NameLongerThanFour));
            //foreach (string item in query)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region Example - Sorting
            //var query = names
            //    .Where(name => name.Length > 4)
            //    .OrderBy(name => name.Length);

            //foreach (string item in query)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region Example - Using Types
            //IOrderedEnumerable<string> query = names
            //    .Where(name => name.Length > 4)
            //    .OrderBy(name => name.Length)
            //    .ThenBy(name => name);

            //foreach (string item in query)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region Example - Filtering By Type

            //Console. WriteLine("Filtering by type");

            //List<Exception> exceptions = new()
            //{
            //    new ArgumentException(),
            //    new SystemException(),
            //    new IndexOutOfRangeException(),
            //    new InvalidOperationException(),
            //    new NullReferenceException(),
            //    new InvalidCastException(),
            //    new OverflowException(),
            //    new DivideByZeroException(),
            //    new ApplicationException()
            //};

            //IEnumerable<ArithmeticException> arithmeticExceptionsQuery = exceptions.OfType<ArithmeticException>();

            //foreach (ArithmeticException exception in arithmeticExceptionsQuery)
            //{
            //    Console.WriteLine(exception);
            //}

            #endregion

            #region Example - Sets and Bags

            //string[] cohort1 = new[]
            //    { "Rachel", "Gareth", "Jonathan", "George" };

            //string[] cohort2 = new[]
            //    { "Jack", "Stephen", "Daniel", "Jack", "Jared" };
            //string[] cohort3 = new[]
            //    { "Declan", "Jack", "Jack", "Jasmine", "Conor" };

            //Output(cohort1, "Cohort 1");
            //Output(cohort2, "Cohort 2");
            //Output(cohort3, "Cohort 3");
            //Output(cohort2.Distinct(), "cohort2.Distinct()");
            //Output(cohort2.DistinctBy(name => name.Substring(0, 2)),
            //"cohort2.DistinctBy(name => name.Substring(0, 2)):");
            //Output(cohort2.Union(cohort3), "cohort2.Union(cohort3)");
            //Output(cohort2.Concat(cohort3), "cohort2.Concat(cohort3)");
            //Output(cohort2.Intersect(cohort3), "cohort2.Intersect(cohort3)");
            //Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");
            //Output(cohort1.Zip(cohort2, (c1, c2) => $"{c1} matched with {c2}"),
            //"cohort1.Zip(cohort2)");

            #endregion

            #region Example - Projection

            //explicity define the object type
            //Person knownTypeObject = new()
            //{
            //    Name = "Boris Johnson",
            //    DateOfBirth = new(year: 1964, month: 6, day: 19)
            //};

            ////Allow the compiler to infer the type with projection
            //var anonymouslyTypedObject = new
            //{
            //    Name = "Boris Johnson",
            //    DateOfBirth = new DateTime(year: 1964, month: 6, day: 19)
            //};

            #endregion

            #region Example - Syntactic Sugar

            ////query using lambda expressions
            //var query = names
            //    .Where(name => name.Length > 4)
            //    .OrderBy(name => name.Length)
            //    .ThenBy(name => name);

            ////query using comprehension syntax
            //var query3 = from name in names
            //            where name.Length > 4
            //            orderby name.Length, name
            //            select name;

            ////using Skip - cannot be written using only the query comprehension syntax
            //var skipquery = names
            //    .Where(name => name.Length > 4)
            //    .Skip(80)
            //    .Take(10);

            ////using Take - can wrap query comprehension syntax in parentheses and then switch to using extension methods
            //var takequery = (from name in names
            //             where name.Length > 4
            //             select name)
            //            .Skip(80)
            //            .Take(10);
            #endregion
        }

        static bool NameLongerThanFour(string name)
        {
            return name.Length > 4;
        }

        static void Output(IEnumerable<string> cohort, string description = "")
        {
            if (!string.IsNullOrEmpty(description))
            {
                Console.WriteLine(description);
            }
            Console.Write(" ");
            Console.WriteLine(string.Join(", ", cohort.ToArray()));
            Console.WriteLine();
        }
    }

    public class Person
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}