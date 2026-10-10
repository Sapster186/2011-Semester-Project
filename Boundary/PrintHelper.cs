using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace _2011__Semester_Project.Boundary
{
    // Printing code shared by the Advisory Summary and the report screen.
    public static class PrintHelper
    {
        // Builds a document that prints the text over as many pages as needed
        public static PrintDocument CreateDocument(string text, Font font)
        {
            PrintDocument document = new PrintDocument();
            string remaining = text;   // the text still waiting to be printed

            document.BeginPrint += (sender, e) => remaining = text;   // start from the top each time

            document.PrintPage += (sender, e) =>
            {
                int charsFitted, linesFilled;
                StringFormat format = new StringFormat(StringFormatFlags.LineLimit);

                // Measure how much of the remaining text fits on this page, then draw it
                e.Graphics.MeasureString(remaining, font, e.MarginBounds.Size, format, out charsFitted, out linesFilled);
                e.Graphics.DrawString(remaining.Substring(0, charsFitted), font, Brushes.Black, e.MarginBounds, format);

                remaining = remaining.Substring(charsFitted);
                e.HasMorePages = remaining.Length > 0 && charsFitted > 0;   // true = print another page
            };
            return document;
        }

        // Shows the Windows print dialog and prints. Returns true if it was sent to the printer.
        public static bool Print(string text, Font font)
        {
            PrintDialog dialog = new PrintDialog();
            dialog.Document = CreateDocument(text, font);
            if (dialog.ShowDialog() != DialogResult.OK) return false;
            dialog.Document.Print();
            return true;
        }

        // Creates a PDF using the "Microsoft Print to PDF" printer that comes with Windows 10 and 11.
        // If it is missing, the learner can use "Save as Text File" instead.
        public static bool SaveAsPdf(string text, Font font, string filePath)
        {
            PrintDocument document = CreateDocument(text, font);
            document.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            if (!document.PrinterSettings.IsValid)
            {
                MessageBox.Show("'Microsoft Print to PDF' is not installed on this computer. Please use Save as Text File instead.");
                return false;
            }
            document.PrinterSettings.PrintToFile = true;
            document.PrinterSettings.PrintFileName = filePath;
            document.Print();
            return true;
        }
    }
}
