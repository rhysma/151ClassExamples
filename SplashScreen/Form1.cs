using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace splash
{
    public partial class Form1 : Form
    {
        private frmSplash myForm;

        public Form1()
        {
            InitializeComponent();
            ParameterizedThreadStart pts =
                new ParameterizedThreadStart(DoSplash);
            Thread t = new Thread(pts);
            double time = 0.05d;
            t.Start(time);
        }

        private void DoSplash(object o)
        {
            double value = (double)o;
            myForm = new frmSplash();
            myForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
                System.Diagnostics.Debug.WriteLine(i.ToString());
            myForm.MakeCloseButtonVisible();

            //sounds
            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer("tada.wav");

            myPlayer.Play();
        }
    }
}
