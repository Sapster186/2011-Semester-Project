using System;
using System.Drawing;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class CareerAdvisorDashboardForm : Form
    {
        private ReportController reportController = new ReportController();
        private string role;   // the logged-in role, passed in by the login/main window

        private Label lblTitle;
        private TextBox txtSearchName;
        private Button btnSearch;
        private DataGridView dgvLearners;
        private Button btnViewSummary;
        private Button btnReports;
        private Button btnLogout;

        // Opened like this: new CareerAdvisorDashboardForm(role).Show();  (Member C passes the logged-in role)
        public CareerAdvisorDashboardForm(string role)
        {
            InitializeComponent();
            this.role = role;
            SetupControls();

            // Access control: if the role is not allowed, warn and close as soon as the form loads
            this.Load += (s, e) =>
            {
                if (!reportController.HasStaffAccess(this.role))
                {
                    MessageBox.Show("Access denied: only Career Advisors and Administrators can open this screen.", "Access Denied");
                    this.Close();
                }
            };

            if (reportController.HasStaffAccess(role)) LoadLearners(null);   // show every session on open
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Career Advisor Dashboard";
            this.Width = 900;
            this.Height = 560;

            lblTitle = new Label();
            lblTitle.Text = "Career Advisor Dashboard";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            txtSearchName = new TextBox();
            txtSearchName.Location = new Point(20, 60);
            txtSearchName.Width = 220;

            btnSearch = new Button();
            btnSearch.Text = "Search by Name";
            btnSearch.Width = 120;
            btnSearch.Location = new Point(250, 58);
            btnSearch.Click += (s, e) => LoadLearners(txtSearchName.Text);

            dgvLearners = new DataGridView();
            dgvLearners.Location = new Point(20, 100);
            dgvLearners.Width = 840;
            dgvLearners.Height = 310;
            dgvLearners.ReadOnly = true;          // advisors can look but not change anything
            dgvLearners.AllowUserToAddRows = false;
            dgvLearners.MultiSelect = false;
            dgvLearners.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLearners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLearners.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvLearners.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            btnViewSummary = new Button();
            btnViewSummary.Text = "View Advisory Summary";
            btnViewSummary.Width = 160;
            btnViewSummary.Location = new Point(20, 430);
            btnViewSummary.Click += BtnViewSummary_Click;

            btnReports = new Button();
            btnReports.Text = "Generate Reports";
            btnReports.Width = 130;
            btnReports.Location = new Point(200, 430);
            btnReports.Click += (s, e) => new ReportSelectionForm(role).Show();   // the way into the reports

            btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Location = new Point(760, 430);
            btnLogout.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtSearchName);
            this.Controls.Add(btnSearch);
            this.Controls.Add(dgvLearners);
            this.Controls.Add(btnViewSummary);
            this.Controls.Add(btnReports);
            this.Controls.Add(btnLogout);
        }

        // Fills the grid with learner sessions (with outcome and gap), optionally filtered by name
        private void LoadLearners(string nameFilter)
        {
            try
            {
                dgvLearners.DataSource = reportController.SearchSessions(nameFilter, role);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Access Denied");
            }
        }

        // Opens the read-only summary of the selected row
        private void BtnViewSummary_Click(object sender, EventArgs e)
        {
            if (dgvLearners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a learner session first.");
                return;
            }
            int sessionID = Convert.ToInt32(dgvLearners.SelectedRows[0].Cells["SessionID"].Value);
            new ReadOnlyAdvisorySummaryForm(sessionID).Show();
        }
    }
}