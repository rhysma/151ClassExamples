using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; //necessary for setting up the thread the other form runs on

namespace SplashScreenSample
{
    public partial class Form1 : Form
    {

        //create an instance of the splash screen form so we can control it 
        private splashForm myForm;

        public Form1()
        {
            InitializeComponent();

            //in the constructor of the main form, create a seperate thread so the main
            //form will not lock up while the splash screen is running.
            ParameterizedThreadStart pts =
                new ParameterizedThreadStart(DoSplash);
            Thread t = new Thread(pts);
            double time = 0.05d;
            t.Start(time);
        }

        /// <summary>
        /// This method instatiates the splash screen form and shows it on the screen
        /// </summary>
        /// <param name="o"></param>
        private void DoSplash(object o)
        {
            double value = (double)o;
            myForm = new splashForm();
            myForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this is a counter that writes to the debugger in case we are having trouble with the form closing correctly
            for (int i = 0; i < 10000; i++)
                System.Diagnostics.Debug.WriteLine(i.ToString());

            //raise the event handler for the close button
            myForm.MakeCloseButtonVisible();

            //set up the sound player for the wav file
            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer("vista.wav");

            //play the sound
            myPlayer.Play();
        }
    }
}
