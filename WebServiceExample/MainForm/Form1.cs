using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainForm.ServiceReference1;

namespace MainForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            //get the data from the combo box
            string size = SizeBox.SelectedItem.ToString();

            //setup an instance to the web service
            ShirtServiceClient myClient = new ShirtServiceClient();

            //access the method we want to use on the service
            decimal price = myClient.ShirtPrices(size);

            //display the decimal result
            ResponseLabel.Text = string.Format("Price: {0:C}", price);
        }
    }
}
