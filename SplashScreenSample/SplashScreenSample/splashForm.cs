using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplashScreenSample
{
    public partial class splashForm : Form
    {
        int counter;
        public delegate void MakeCloseButtonVisibleDelegate();
        public event MakeCloseButtonVisibleDelegate MCBVEvent;

        public splashForm()
        {
            InitializeComponent();
            //add the event handler in the constructor
            MCBVEvent += MakeCloseButtonVisible;

            //start the first timer
            timer1.Start();
        }

        /// <summary>
        /// This method allows us to show the close button in case the animation timer fails to close the form as designed
        /// </summary>
        public void MakeCloseButtonVisible()
        {
            if (InvokeRequired)
                Invoke(MCBVEvent);
            else
                CloseButton.Visible = true;
        }

        /// <summary>
        /// This method fires on every 'tick' of the second timer and controls the animation of the form as it closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05d;
            this.Width -= 30;
            this.Left += 15;
            this.Height -= 18;
            this.Top += 9;


            //when we get to a specific level of opacity, stop the timer and close the form
            if (this.Opacity <= 0.000001d)
            {
                timer2.Stop();
                this.Close();
            }
        }

        /// <summary>
        /// this method fires every time the first timer 'ticks" and changes the opacity of the form as it loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05d;
            if (this.Opacity >= 1.0d)
            {
                timer1.Stop();
                timer3.Start();
            }
        }

        /// <summary>
        /// This method fires every time the third timer 'ticks' and controls the amount of time on the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            counter += 100; //how fast to you want this to go? 
            if (counter > 4000) //this number controls how long the splash screen sits and waits before animating out
            {
                timer2.Start();
                timer3.Stop();
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            //they want to close the form so start the second timer
            timer2.Start();
        }
    }
}
