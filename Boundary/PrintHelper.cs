using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace _2011__Semester_Project.Boundary
{
    // PrintHelper — shared printing code for the Advisory Summary and the
    // Report Selection screen.
    //
    // Why a helper class: both screens need to print and export text, and the
    // exact same multi-page logic applies to both. Keeping it in one place
    // avoids duplicating the pagination code.
    //
    // PDF export uses the "Microsoft Print to PDF" printer that ships with
    // Windows 10 and 11, so no external PDF library is required.
    public static class PrintHelper
    {
        // Builds a PrintDocument that will paginate the given text across as
        // many pages as needed.
        //
        // How the pagination works:
        //   - BeginPrint  resets "remaining" to the full text (so re-printing
        //                 starts from page 1 again).
        //   - PrintPage   measures how much fits on the current page, draws it,
        //                 trims it off "remaining", and asks for another page
        //                 if any text is left.
        public static PrintDocument CreateDocument(string text, Font font)
        {
            PrintDocument document = new PrintDocument();
            string remaining = text;   // text still waiting to be printed

            document.BeginPrint += (sender, e) => remaining = text;   // reset on each print

            document.PrintPage += (sender, e) =>
            {
                int charsFitted, linesFilled;
                StringFormat format = new StringFormat(StringFormatFlags.LineLimit);

                // Measure what fits on this page, then draw exactly that much.
                e.Graphics.MeasureString(remaining, font, e.MarginBounds.Size, format, out charsFitted, out linesFilled);
                e.Graphics.DrawString(remaining.Substring(0, charsFitted), font, Brushes.Black, e.MarginBounds, format);

                remaining = remaining.Substring(charsFitted);
                // More pages only if there is text left AND this page actually
                // made progress (guards against an infinite loop if charsFitted = 0).
                e.HasMorePages = remaining.Length > 0 && charsFitted > 0;
            };
            return document;
        }

        // Shows the Windows print dialog and prints if the user accepts.
        // Returns true only if the document was actually sent to the printer —
        // the caller uses this to decide whether to log the export.
        public static bool Print(string text, Font font)
        {
            PrintDialog dialog = new PrintDialog();
            dialog.Document = CreateDocument(text, font);
            if (dialog.ShowDialog() != DialogResult.OK) return false;
            dialog.Document.Print();
            return true;
        }

        // Creates a PDF via the built-in "Microsoft Print to PDF" printer.
        // If it isn't installed, tells the user to use Save as Text File instead
        // and returns false so the caller does not log an export.
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