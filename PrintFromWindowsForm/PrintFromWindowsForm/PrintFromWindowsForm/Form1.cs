using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace PrintFromWindowsForm
{
    public partial class PrintExample : Form
    {
        public PrintExample()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            //gather the information from the textboxes
            string firstname = FirstNameBox.Text;
            string lastname = LastNameBox.Text;
            string address = AddressBox.Text;

            //create a new person instance
            Person myPerson = new Person(firstname, lastname, address);

            //print the person information 
            Printing.PrintNow(myPerson);

        }

        private void PrintExample_Load(object sender, EventArgs e)
        {
            //check on form load for a printer
            if (PrinterSettings.InstalledPrinters.Count <= 0)
            {
                MessageBox.Show("Printer not found!");
                return;
            }
        }
    }
}
