using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class OrderDetails
    {
        [XmlElement(ElementName = "Order_Detail")]
        public List<Order_Detail> Order_Detail { get; set; }
    }
}
