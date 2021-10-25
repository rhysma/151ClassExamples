using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplashScreenClassExample
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
        }

        //Visual styling changes made to this form's properties to make it 
        //look better as a splash screen
        //1. Change the formborderstyle to none
        //2. Change the start position to centerScreen so it shows up in the center of the screen
        //3. Add a picture box control that contains the splash image you want to use. Fill the whole
        //form with the picture box


        //to make sure your application uses this form as the first form that shows up
        //you have to go into the program.cs file and make sure the form that is set to load 
        //is SplashForm under Application.Run

        //Use timer class
        Timer tmr;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method determines the amount of time the splashscreen will be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashForm_Shown(object sender, EventArgs e)
        {
            //instantiate the timer
            tmr = new Timer();

            //set time interval 3 sec
            tmr.Interval = 3000;

            //starts the timer
            tmr.Start();

            //tick the timer
            tmr.Tick += tmr_Tick;
        }

        /// <summary>
        /// This method ticks for the timer and keeps track of how long it has been
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmr_Tick(object sender, EventArgs e)
        {

            //after 3 sec stop the timer
            tmr.Stop();

            //display mainform
            //this transition will be instantaneous and abrupt
            MainForm mf = new MainForm();
            mf.Show();

            //hide this form
            this.Hide();

        }
    }
}
