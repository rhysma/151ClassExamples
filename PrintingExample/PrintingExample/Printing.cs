using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

namespace PrintingExample
{
    class Printing : PrintDocument
    {
        private Font font;

        public Font PrinterFont
        {
            //property to hold the font the user wants to use
            get { return font; }
            set { font = value; }
        }
        private string text;

        public string TextToPrint
        {
            //property that holds the text to be printed
            get { return text; }
            set { text = value; }
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            //run base code
            base.OnBeginPrint(e);

            //check to see if the user provided a font
            //if not, use a default
            if (font == null)
            {
                font = new Font("Times New Roman", 12);
            }
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            //run base code
            base.OnPrintPage(e);

            //declare variables
            int printHeight;
            int printWidth;
            int leftMargin;
            int rightMargin;

            //set the print area and margins
            printHeight = base.DefaultPageSettings.PaperSize.Height -
                base.DefaultPageSettings.Margins.Top -
                base.DefaultPageSettings.Margins.Bottom;

            printWidth = base.DefaultPageSettings.PaperSize.Width -
                base.DefaultPageSettings.Margins.Left -
                base.DefaultPageSettings.Margins.Right;

            leftMargin = base.DefaultPageSettings.Margins.Left;
            rightMargin = base.DefaultPageSettings.Margins.Right;

        }
    }
}
