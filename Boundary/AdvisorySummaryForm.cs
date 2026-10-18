using System;
using System.Drawing;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    // AdvisorySummaryForm — Report 1 (the learner's Advisory Summary).
    //
    // Built in code (no designer). Two modes:
    //   - requireExport = true  → the learner view. Save/export/print is mandatory
    //                             before the form can close.
    //   - requireExport = false → the advisor's read-only view (used by
    //                             ReadOnlyAdvisorySummaryForm).
    //
    // Member B opens it with just the session ID:
    //     new AdvisorySummaryForm(sessionID).Show();
    public partial class AdvisorySummaryForm : Form
    {
        private ReportController reportController = new ReportController();
        private int sessionID;
        private bool requireExport;         // true for the learner, false for the advisor's read-only view
        private bool actionDone = false;    // set to true once save/export/print succeeds

        private TextBox txtSummary;
        private Label lblFooter;
        private Button btnSaveText;
        private Button btnSavePdf;
        private Button btnPrint;
        private Button btnClose;

        // Constructor: builds the layout, loads the summary text from the controller,
        // and wires the FormClosing guard.
        public AdvisorySummaryForm(int sessionID, bool requireExport = true)
        {
            InitializeComponent();
            this.sessionID = sessionID;
            this.requireExport = requireExport;
            SetupControls();

            // Controller builds the whole summary string; the form only displays it.
            txtSummary.Text = reportController.BuildAdvisorySummaryText(sessionID);
            // Footer is separate so it's always visible outside the scrolling text.
            lblFooter.Text = reportController.GetFooterText();
            this.FormClosing += AdvisorySummaryForm_FormClosing;
        }

        // Builds all controls in code. The layout follows the screen standard
        // (heading at top, content centre, buttons at bottom).
        private void SetupControls()
        {
            this.Text = "Future Path - Advisory Summary";
            this.Width = 720;
            this.Height = 680;

            txtSummary = new TextBox();
            txtSummary.Multiline = true;
            txtSummary.ReadOnly = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.Font = new Font("Consolas", 9);   // monospace so columns line up
            txtSummary.Location = new Point(20, 20);
            txtSummary.Width = 660;
            txtSummary.Height = 430;

            lblFooter = new Label();
            lblFooter.AutoSize = false;
            lblFooter.Location = new Point(20, 460);
            lblFooter.Width = 660;
            lblFooter.Height = 80;
            lblFooter.ForeColor = Color.DimGray;

            btnSaveText = new Button();
            btnSaveText.Text = "Save as Text File";
            btnSaveText.Width = 130;
            btnSaveText.Location = new Point(20, 560);
            btnSaveText.Click += BtnSaveText_Click;

            btnSavePdf = new Button();
            btnSavePdf.Text = "Save as PDF";
            btnSavePdf.Width = 110;
            btnSavePdf.Location = new Point(165, 560);
            btnSavePdf.Click += BtnSavePdf_Click;

            btnPrint = new Button();
            btnPrint.Text = "Print";
            btnPrint.Location = new Point(290, 560);
            btnPrint.Click += BtnPrint_Click;

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new Point(410, 560);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(txtSummary);
            this.Controls.Add(lblFooter);
            this.Controls.Add(btnSaveText);
            this.Controls.Add(btnSavePdf);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);
        }

        // Summary + footer: what gets saved/exported/printed.
        private string FullText()
        {
            return txtSummary.Text + Environment.NewLine + Environment.NewLine + lblFooter.Text;
        }

        // Called after any successful save/export/print. Unlocks closing and
        // shows the export date (so the user knows the action took effect).
        private void FinishAction(string message)
        {
            actionDone = true;
            MessageBox.Show(message + "\n\nDate of export: " + DateTime.Now.ToString("dd/MM/yyyy"), "Done");
        }

        // Save as Text File: writes the file and logs an ExportReport row (TXT).
        private void BtnSaveText_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file (*.txt)|*.txt";
            dialog.FileName = "AdvisorySummary_" + sessionID + ".txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                reportController.SaveSummaryAsText(sessionID, FullText(), dialog.FileName);   // also logs ExportReport
                FinishAction("Summary saved to:\n" + dialog.FileName);
            }
        }

        // Save as PDF: uses Windows "Microsoft Print to PDF", then logs an ExportReport row (PDF).
        private void BtnSavePdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF file (*.pdf)|*.pdf";
            dialog.FileName = "AdvisorySummary_" + sessionID + ".pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (PrintHelper.SaveAsPdf(FullText(), txtSummary.Font, dialog.FileName))
                {
                    reportController.LogExport(sessionID, FullText(), "PDF");
                    FinishAction("Summary saved to:\n" + dialog.FileName);
                }
            }
        }

        // Print: sends to the default printer, then logs an ExportReport row (PRINT).
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (PrintHelper.Print(FullText(), txtSummary.Font))
            {
                reportController.LogExport(sessionID, FullText(), "PRINT");
                FinishAction("Summary sent to the printer.");
            }
        }

        // Spec rule: the learner's session cannot close (even with the X button)
        // until the summary is saved, exported or printed. The advisor's
        // read-only view has requireExport = false, so this check is skipped.
        private void AdvisorySummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (requireExport && !actionDone)
            {
                MessageBox.Show("Please save, export or print your summary before closing.", "Summary not saved");
                e.Cancel = true;   // keeps the window open
            }
        }
    }
}