using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class Order_Detail
    {
		[XmlElement(ElementName = "OrderID")]
		public string OrderID { get; set; }
		[XmlElement(ElementName = "ProductID")]
		public string ProductID { get; set; }
		[XmlElement(ElementName = "UnitPrice")]
		public string UnitPrice { get; set; }
		[XmlElement(ElementName = "Quantity")]
		public string Quantity { get; set; }
		[XmlElement(ElementName = "Discount")]
		public string Discount { get; set; }
		[XmlElement(ElementName = "Product")]
		public Product Product { get; set; }
	}
}
