using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintingExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                //create an instance of our printer class
                Printing printer = new Printing();
                //set the font
                printer.PrinterFont = new Font("Verdana", 15);
                //set the text to print propery
                printer.TextToPrint = documentBox.Text;
                //issue the print command
                //(make it so)
                printer.Print();
            }
        }
    }
}
