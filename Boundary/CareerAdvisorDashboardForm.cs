using System;
using System.Drawing;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    // CareerAdvisorDashboardForm — the Career Advisor's landing screen.
    //
    // Shows one row per AdviceSession (learner, grade, degree, outcome, gap)
    // and provides the way into:
    //   - ReadOnlyAdvisorySummaryForm (read-only view of a session)
    //   - ReportSelectionForm (Reports 2 and 3)
    //
    // Access: role-based. Only "Career Advisor" and "Administrator" may open it.
    // The role is passed in by Member C's login/main window:
    //     new CareerAdvisorDashboardForm(role).Show();
    // The form checks it on load and closes itself if the role is not allowed.
    // The controller checks again inside SearchSessions as a second line of defence.
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

        // Constructor: builds layout, wires the access-check on load,
        // then shows every session if the role is allowed.
        public CareerAdvisorDashboardForm(string role)
        {
            InitializeComponent();
            this.role = role;
            SetupControls();

            // Access control: the load event fires once the form is visible.
            // If the role is not allowed, we show a warning and close the form.
            this.Load += (s, e) =>
            {
                if (!reportController.HasStaffAccess(this.role))
                {
                    MessageBox.Show("Access denied: only Career Advisors and Administrators can open this screen.", "Access Denied");
                    this.Close();
                }
            };

            if (reportController.HasStaffAccess(role)) LoadLearners(null);   // null = show everyone
        }

        // Builds all controls in code (no Designer). Standard layout:
        // title on top, search bar below, grid in the middle, action buttons at the bottom.
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
            // Passes the role on to ReportSelectionForm so the RBAC check continues there.
            btnReports.Click += (s, e) => new ReportSelectionForm(role).Show();

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

        // Fills the grid with learner sessions (with outcome and gap).
        // Passing null for nameFilter shows every session.
        // The controller throws UnauthorizedAccessException if the role is not
        // allowed — caught here so the form shows a friendly message.
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

        // Opens the read-only advisory summary for the selected row.
        // Requires a row to be selected first; the SessionID column carries
        // the ID passed to the ReadOnlyAdvisorySummaryForm.
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