using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //reading and writing files
//both needed for printing
using System.Drawing;
using System.Drawing.Printing; 

namespace PrintFromFile
{
    class PrintingExample
    {
        private Font printFont;
        private StreamReader streamToPrint;
        private static string filePath;

        public PrintingExample()
        {
            //call the printing method to start the process
            Printing();
        }

        //PrintPage event is raised each time a page is printed
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //variables to hold information about the page
            float linesPerPage = 0;
            float yPos = 0;
            float leftMargin = ev.MarginBounds.Left;
            float rightMargin = ev.MarginBounds.Right;
            float topMargin = ev.MarginBounds.Top;
            int count = 0;
            string line = null;

            //calculate the number of lines on a page
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

            //go through the file and print each line inside
            while (count < linesPerPage &&
                   ((line = streamToPrint.ReadLine()) != null))
            {
                //yPost gets the information about the margin and font to determine
                //how much space we can use
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));

                //DrawString actually draws the information stored in line to the 
                //page that is being created. 
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                    leftMargin, yPos, new StringFormat());

                //increment counter
                count++;
            }//end of while

            //if more lines exist, print another page
            if (line != null)
            {
                ev.HasMorePages = true;
            }
            else
            {
                ev.HasMorePages = false;
            }


        } //end of PrintPage

        /// <summary>
        /// This method handles the printing of a file on the system
        /// </summary>
        public void Printing()
        {
            try
            {
                //get the file 
                filePath = "test.txt";
                streamToPrint = new StreamReader(filePath);
                try
                {
                    //if you want to set specifics about the font used
                    //you can do that here
                    printFont = new Font("Arial", 16);

                    //create a new instance of PrintDocument
                    PrintDocument pd = new PrintDocument();

                    //assign our PrintPage method to the event that handles printing
                    //in the PrintDocument class object
                    pd.PrintPage += new PrintPageEventHandler(PrintPage);

                    //print the document
                    pd.Print();
                }
                finally
                {
                    //close up the open streamreader
                    streamToPrint.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    } // end of class
}
