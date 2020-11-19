using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class Product
    {
		[XmlElement(ElementName = "ProductID")]
		public string ProductID { get; set; }
		[XmlElement(ElementName = "ProductName")]
		public string ProductName { get; set; }
		[XmlElement(ElementName = "SupplierID")]
		public string SupplierID { get; set; }
		[XmlElement(ElementName = "CategoryID")]
		public string CategoryID { get; set; }
		[XmlElement(ElementName = "QuantityPerUnit")]
		public string QuantityPerUnit { get; set; }
		[XmlElement(ElementName = "UnitPrice")]
		public string UnitPrice { get; set; }
		[XmlElement(ElementName = "UnitsInStock")]
		public string UnitsInStock { get; set; }
		[XmlElement(ElementName = "UnitsOnOrder")]
		public string UnitsOnOrder { get; set; }
		[XmlElement(ElementName = "ReorderLevel")]
		public string ReorderLevel { get; set; }
		[XmlElement(ElementName = "Discontinued")]
		public string Discontinued { get; set; }
	}
}
