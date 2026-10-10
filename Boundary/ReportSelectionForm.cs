using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class ReportSelectionForm : Form
    {
        private ReportController reportController = new ReportController();
        private string role;           // the logged-in role, passed in by the dashboard
        private int currentReport = 0; // 2 or 3 once a report has been generated

        private Label lblTitle;
        private ComboBox cmbReportType;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Button btnGenerate;
        private TextBox txtHeader;
        private DataGridView dgvResults;
        private Label lblFooter;
        private Button btnSaveText;
        private Button btnSavePdf;
        private Button btnPrint;

        // Only the Career Advisor Dashboard and the Administrator Dashboard open this form:
        // new ReportSelectionForm(role).Show();
        public ReportSelectionForm(string role)
        {
            InitializeComponent();
            this.role = role;
            SetupControls();

            // Access control (spec test case 14): other roles are refused and the form closes
            this.Load += (s, e) =>
            {
                if (!reportController.HasStaffAccess(this.role))
                {
                    MessageBox.Show("Access denied: reports are only available to Career Advisors and Administrators.", "Access Denied");
                    this.Close();
                }
            };
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Report Generation";
            this.Width = 960;
            this.Height = 760;

            lblTitle = new Label();
            lblTitle.Text = "Report Selection and Generation";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            cmbReportType = new ComboBox();
            cmbReportType.Location = new Point(20, 60);
            cmbReportType.Width = 400;
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Items.Add("Report 2: Degree Interest and Eligibility");
            cmbReportType.Items.Add("Report 3: Learners Not Currently Eligible (Exception)");
            cmbReportType.SelectedIndex = 0;
            // The date pickers only apply to Report 2, so hide them for Report 3
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
            lblStartDate.Location = new Point(20, 105);
            lblStartDate.AutoSize = true;

            dtpStartDate = new DateTimePicker();   // format check: only valid dates can be chosen
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(100, 100);
            dtpStartDate.Width = 130;
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, 1, 1);   // 1 January this year

            lblEndDate = new Label();
            lblEndDate.Text = "End Date:";
            lblEndDate.Location = new Point(260, 105);
            lblEndDate.AutoSize = true;

            dtpEndDate = new DateTimePicker();
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(330, 100);
            dtpEndDate.Width = 130;
            dtpEndDate.Value = DateTime.Today;

            btnGenerate = new Button();
            btnGenerate.Text = "Generate Report";
            btnGenerate.Width = 130;
            btnGenerate.Location = new Point(20, 140);
            btnGenerate.Click += BtnGenerate_Click;

            // Shows the report title, totals and percentages above the table
            txtHeader = new TextBox();
            txtHeader.Multiline = true;
            txtHeader.ReadOnly = true;
            txtHeader.Location = new Point(20, 180);
            txtHeader.Width = 900;
            txtHeader.Height = 190;
            txtHeader.Font = new Font("Consolas", 9);

            dgvResults = new DataGridView();
            dgvResults.Location = new Point(20, 380);
            dgvResults.Width = 900;
            dgvResults.Height = 240;
            dgvResults.ReadOnly = true;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvResults.DataBindingComplete += DgvResults_DataBindingComplete;   // used for the colours

            lblFooter = new Label();
            lblFooter.Location = new Point(20, 628);
            lblFooter.AutoSize = true;
            lblFooter.ForeColor = Color.DimGray;

            btnSaveText = new Button();
            btnSaveText.Text = "Save as Text";
            btnSaveText.Width = 110;
            btnSaveText.Location = new Point(20, 665);
            btnSaveText.Click += BtnSaveText_Click;

            btnSavePdf = new Button();
            btnSavePdf.Text = "Save as PDF";
            btnSavePdf.Width = 110;
            btnSavePdf.Location = new Point(145, 665);
            btnSavePdf.Click += BtnSavePdf_Click;

            btnPrint = new Button();
            btnPrint.Text = "Print";
            btnPrint.Location = new Point(270, 665);
            btnPrint.Click += BtnPrint_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(cmbReportType);
            this.Controls.Add(lblStartDate);
            this.Controls.Add(dtpStartDate);
            this.Controls.Add(lblEndDate);
            this.Controls.Add(dtpEndDate);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(txtHeader);
            this.Controls.Add(dgvResults);
            this.Controls.Add(lblFooter);
            this.Controls.Add(btnSaveText);
            this.Controls.Add(btnSavePdf);
            this.Controls.Add(btnPrint);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string header;
                if (cmbReportType.SelectedIndex == 0)
                {
                    // Validation: the end date must not be before the start date
                    if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)
                    {
                        MessageBox.Show("End Date must not be before Start Date.");
                        return;
                    }
                    currentReport = 2;
                    dgvResults.DataSource = reportController.GetDegreeInterestReport(dtpStartDate.Value, dtpEndDate.Value, role, out header);
                }
                else
                {
                    currentReport = 3;
                    dgvResults.DataSource = reportController.GetExceptionReport(role, out header);
                }
                txtHeader.Text = header;
                lblFooter.Text = reportController.GetReportFooter(currentReport);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Access Denied");   // the controller refused too
            }
        }

        // Formatting: group, subtotal and total rows are bold on grey; Report 3 learner rows are red
        private void DgvResults_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dgvResults.Columns.Contains("RowType")) return;

            dgvResults.Columns["RowType"].Visible = false;   // helper column, not shown to the user
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                string type = Convert.ToString(row.Cells["RowType"].Value);
                if (type == "Data")
                {
                    if (currentReport == 3) row.DefaultCellStyle.ForeColor = Color.Red;   // exceptions in red
                }
                else
                {
                    row.DefaultCellStyle.Font = new Font(dgvResults.Font, FontStyle.Bold);
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        // Turns the header, grid and footer into plain text for saving and printing
        private string BuildReportText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(txtHeader.Text);

            foreach (DataGridViewColumn col in dgvResults.Columns)
                if (col.Visible) sb.Append(col.HeaderText + " | ");
            sb.AppendLine();

            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                foreach (DataGridViewColumn col in dgvResults.Columns)
                    if (col.Visible) sb.Append(Convert.ToString(row.Cells[col.Index].Value) + " | ");
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.AppendLine(lblFooter.Text);
            return sb.ToString();
        }

        // True when a report has been generated; otherwise tells the user to generate one
        private bool HasReport()
        {
            if (currentReport == 0)
            {
                MessageBox.Show("Please generate a report first.");
                return false;
            }
            return true;
        }

        private string DefaultName()
        {
            return currentReport == 2 ? "Report2_DegreeInterest" : "Report3_ExceptionReport";
        }

        private void BtnSaveText_Click(object sender, EventArgs e)
        {
            if (!HasReport()) return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file (*.txt)|*.txt";
            dialog.FileName = DefaultName() + ".txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, BuildReportText());
                MessageBox.Show("Report saved to:\n" + dialog.FileName + "\n\nDate of export: " + DateTime.Now.ToString("dd/MM/yyyy"), "Saved");
            }
        }

        private void BtnSavePdf_Click(object sender, EventArgs e)
        {
            if (!HasReport()) return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF file (*.pdf)|*.pdf";
            dialog.FileName = DefaultName() + ".pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (PrintHelper.SaveAsPdf(BuildReportText(), new Font("Consolas", 9), dialog.FileName))
                    MessageBox.Show("Report saved to:\n" + dialog.FileName + "\n\nDate of export: " + DateTime.Now.ToString("dd/MM/yyyy"), "Saved");
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!HasReport()) return;
            PrintHelper.Print(BuildReportText(), new Font("Consolas", 9));
        }
    }
}