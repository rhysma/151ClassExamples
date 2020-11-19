using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class Order
    {
		[XmlElement(ElementName = "OrderID")]
		public string OrderID { get; set; }
		[XmlElement(ElementName = "CustomerID")]
		public string CustomerID { get; set; }
		[XmlElement(ElementName = "EmployeeID")]
		public string EmployeeID { get; set; }
		[XmlElement(ElementName = "OrderDate")]
		public string OrderDate { get; set; }
		[XmlElement(ElementName = "RequiredDate")]
		public string RequiredDate { get; set; }
		[XmlElement(ElementName = "ShippedDate")]
		public string ShippedDate { get; set; }
		[XmlElement(ElementName = "ShipVia")]
		public string ShipVia { get; set; }
		[XmlElement(ElementName = "Freight")]
		public string Freight { get; set; }
		[XmlElement(ElementName = "ShipName")]
		public string ShipName { get; set; }
		[XmlElement(ElementName = "ShipAddress")]
		public string ShipAddress { get; set; }
		[XmlElement(ElementName = "ShipCity")]
		public string ShipCity { get; set; }
		[XmlElement(ElementName = "ShipPostalCode")]
		public string ShipPostalCode { get; set; }
		[XmlElement(ElementName = "ShipCountry")]
		public string ShipCountry { get; set; }
		[XmlElement(ElementName = "Order_Details")]
		public OrderDetails Order_Details { get; set; }

		public override string ToString()
		{
			return "Order: " + OrderID + " " + OrderDate;
		}
	}
}
