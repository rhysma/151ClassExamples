using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ShirtService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MyShirtService : IShirtService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public decimal ShirtPrices(string size)
        {
            decimal price = 0.0m; 

            switch(size)
            {
                case "Small":
                    price = 10.00m;
                    break;

                case "Medium":
                    price = 15.00m;
                    break;

                case "Large":
                    price = 20.00m;
                    break;
            }

            return price;
        }

    }
}
