using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class AdvisorySummaryForm : Form
    {
        private ReportController reportController = new ReportController();
        private int sessionID; // which advisory session this summary is for

        private TextBox txtSummary;
        private Label lblFooter;
        private Button btnExport;
        private Button btnClose;

        public AdvisorySummaryForm(int sessionID)
        {
            InitializeComponent();
            this.sessionID = sessionID;
            SetupControls();
            LoadSummary();
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Advisory Summary";
            this.Width = 650;
            this.Height = 550;

            txtSummary = new TextBox();
            txtSummary.Multiline = true;
            txtSummary.ReadOnly = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.Font = new System.Drawing.Font("Consolas", 9);
            txtSummary.Location = new System.Drawing.Point(20, 20);
            txtSummary.Width = 590;
            txtSummary.Height = 400;

            lblFooter = new Label();
            lblFooter.Text = "You must Export before closing this window.";
            lblFooter.ForeColor = System.Drawing.Color.DarkRed;
            lblFooter.Location = new System.Drawing.Point(20, 430);
            lblFooter.AutoSize = true;

            btnExport = new Button();
            btnExport.Text = "Export Summary";
            btnExport.Location = new System.Drawing.Point(20, 460);
            btnExport.Click += BtnExport_Click;

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new System.Drawing.Point(160, 460);
            btnClose.Enabled = false; // stays off until export happens - this enforces "mandatory export"
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(txtSummary);
            this.Controls.Add(lblFooter);
            this.Controls.Add(btnExport);
            this.Controls.Add(btnClose);
        }

        private void LoadSummary()
        {
            txtSummary.Text = reportController.BuildAdvisorySummaryText(sessionID);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            string path = reportController.ExportSummary(sessionID, txtSummary.Text);
            MessageBox.Show("Summary exported to:\n" + path, "Export Successful");
            btnClose.Enabled = true; // now the learner is allowed to close
        }
    }
}
