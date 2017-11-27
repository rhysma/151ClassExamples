using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace splash
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
            MCBVEvent += MakeCloseButtonVisible;
            timer1.Start();
        }

        int counter;
        public delegate void MakeCloseButtonVisibleDelegate();
        public event MakeCloseButtonVisibleDelegate MCBVEvent;

        public void MakeCloseButtonVisible()
        {
            if (InvokeRequired)
                Invoke(MCBVEvent);
            else
                btnClose.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05d;
            this.Width -= 30;
            this.Left += 15;
            this.Height -= 18;
            this.Top += 9;

            if (this.Opacity <= 0.000001d)
            {
                timer2.Stop();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05d;
            if (this.Opacity >= 1.0d)
            {
                timer1.Stop();
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            counter += 100;
            if (counter > 4000)
            {
                timer2.Start();
                timer3.Stop();
            }

        }
    }
}
