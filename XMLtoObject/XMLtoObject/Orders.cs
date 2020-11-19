using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //needed to ID XML Elements

namespace XMLtoObject
{
    public class Orders
    {
        [XmlElement(ElementName = "Order")]
        public Order Order { get; set; }

        public override string ToString()
        {
            return Order.ToString();
        }
    }
}
