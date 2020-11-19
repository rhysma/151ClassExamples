using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class Customer
    {
		[XmlElement(ElementName = "CustomerID")]
		public string CustomerID { get; set; }
		[XmlElement(ElementName = "CompanyName")]
		public string CompanyName { get; set; }
		[XmlElement(ElementName = "ContactName")]
		public string ContactName { get; set; }
		[XmlElement(ElementName = "ContactTitle")]
		public string ContactTitle { get; set; }
		[XmlElement(ElementName = "Address")]
		public string Address { get; set; }
		[XmlElement(ElementName = "City")]
		public string City { get; set; }
		[XmlElement(ElementName = "PostalCode")]
		public string PostalCode { get; set; }
		[XmlElement(ElementName = "Country")]
		public string Country { get; set; }
		[XmlElement(ElementName = "Phone")]
		public string Phone { get; set; }
		[XmlElement(ElementName = "Fax")]
		public string Fax { get; set; }
		[XmlElement(ElementName = "Orders")]
		public Orders Orders { get; set; }

		public override string ToString()
        {
			return CustomerID + "\n" + CompanyName + " " + ContactName + " " +
				ContactTitle + "\n" + Address + " " + City + " " + PostalCode + " " +
				Country + "\n" + Phone + " " + Fax + "\n" +
				Orders;

		}
    }
}
