using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SqlDelete
{
    public partial class Form1 : Form
    {
        List<Employee> employees;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            employees = new List<Employee>();

            employees = DataAdapter.Get();

            foreach (var emp in employees)
            {
                lstOutput.Items.Add(emp);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Employee myEmp = lstOutput.SelectedItem as Employee;
            string message = DataAdapter.Delete(myEmp.EmpID);

            lstOutput.Items.Remove(myEmp);
        }
    }
}
