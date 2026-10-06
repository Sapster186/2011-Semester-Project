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
    public partial class ReportSelectionForm : Form
    {
        private ReportController reportController = new ReportController();

        private Label lblTitle;
        private ComboBox cmbReportType;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Button btnGenerate;
        private DataGridView dgvResults;

        public ReportSelectionForm()
        {
            // TODO: add role check once LoginController exists - only Advisor/Administrator may open this form
            InitializeComponent();
            SetupControls();
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Report Generation";
            this.Width = 800;
            this.Height = 550;

            lblTitle = new Label();
            lblTitle.Text = "Report Generation";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.AutoSize = true;

            cmbReportType = new ComboBox();
            cmbReportType.Location = new System.Drawing.Point(20, 60);
            cmbReportType.Width = 350;
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Items.Add("Degree Interest and Eligibility Report");
            cmbReportType.Items.Add("Learners Not Currently Eligible Exception Report");
            cmbReportType.SelectedIndex = 0;
            // Only show date pickers for Report 2, since Report 3 has no date filter
            cmbReportType.SelectedIndexChanged += (s, e) =>
            {
                bool isReport2 = cmbReportType.SelectedIndex == 0;
                lblStartDate.Visible = isReport2;
                dtpStartDate.Visible = isReport2;
                lblEndDate.Visible = isReport2;
                dtpEndDate.Visible = isReport2;
            };

            lblStartDate = new Label();
            lblStartDate.Text = "Start Date:";
            lblStartDate.Location = new System.Drawing.Point(20, 100);
            lblStartDate.AutoSize = true;

            dtpStartDate = new DateTimePicker();
            dtpStartDate.Location = new System.Drawing.Point(100, 97);
            dtpStartDate.Value = new DateTime(2026, 1, 1);

            lblEndDate = new Label();
            lblEndDate.Text = "End Date:";
            lblEndDate.Location = new System.Drawing.Point(300, 100);
            lblEndDate.AutoSize = true;

            dtpEndDate = new DateTimePicker();
            dtpEndDate.Location = new System.Drawing.Point(370, 97);
            dtpEndDate.Value = DateTime.Now;

            btnGenerate = new Button();
            btnGenerate.Text = "Generate Report";
            btnGenerate.Location = new System.Drawing.Point(20, 140);
            btnGenerate.Click += BtnGenerate_Click;

            dgvResults = new DataGridView();
            dgvResults.Location = new System.Drawing.Point(20, 180);
            dgvResults.Width = 740;
            dgvResults.Height = 320;
            dgvResults.ReadOnly = true;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.CellFormatting += DgvResults_CellFormatting; // for red highlighting

            this.Controls.Add(lblTitle);
            this.Controls.Add(cmbReportType);
            this.Controls.Add(lblStartDate);
            this.Controls.Add(dtpStartDate);
            this.Controls.Add(lblEndDate);
            this.Controls.Add(dtpEndDate);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(dgvResults);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == 0)
                dgvResults.DataSource = reportController.GetDegreeInterestEligibilityReport(dtpStartDate.Value, dtpEndDate.Value);
            else
                dgvResults.DataSource = reportController.GetExceptionReport();
        }

        // Highlights the outcome column in red for the Exception Report, per your spec
        private void DgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (cmbReportType.SelectedIndex == 1 && dgvResults.Columns.Count > 0 &&
                dgvResults.Columns[e.ColumnIndex].Name == "AdvisoryOutcome")
            {
                e.CellStyle.ForeColor = System.Drawing.Color.Red;
                e.CellStyle.Font = new System.Drawing.Font(dgvResults.Font, System.Drawing.FontStyle.Bold);
            }
        }
    }
}
