using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using _2011__Semester_Project.DataAccess;

namespace _2011__Semester_Project.Boundary
{
    public partial class CareerAdvisorDashboardForm : Form
    {
        private Label lblTitle;
        private TextBox txtSearchName;
        private Button btnSearch;
        private DataGridView dgvLearners;
        private Button btnViewSummary;
        private Button btnReports;
        private Button btnLogout;

        public CareerAdvisorDashboardForm()
        {
            InitializeComponent();
            SetupControls();
            LoadLearners(null);
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Career Advisor Dashboard";
            this.Width = 750;
            this.Height = 550;

            lblTitle = new Label();
            lblTitle.Text = "Career Advisor Dashboard";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.AutoSize = true;

            txtSearchName = new TextBox();
            txtSearchName.Location = new System.Drawing.Point(20, 60);
            txtSearchName.Width = 200;

            btnSearch = new Button();
            btnSearch.Text = "Search by Name";
            btnSearch.Location = new System.Drawing.Point(230, 58);
            btnSearch.Click += (s, e) => LoadLearners(txtSearchName.Text);

            dgvLearners = new DataGridView();
            dgvLearners.Location = new System.Drawing.Point(20, 100);
            dgvLearners.Width = 690;
            dgvLearners.Height = 300;
            dgvLearners.ReadOnly = true;
            dgvLearners.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLearners.AllowUserToAddRows = false;
            dgvLearners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnViewSummary = new Button();
            btnViewSummary.Text = "View Advisory Summary";
            btnViewSummary.Location = new System.Drawing.Point(20, 420);
            btnViewSummary.Click += BtnViewSummary_Click;

            btnReports = new Button();
            btnReports.Text = "Generate Reports";
            btnReports.Location = new System.Drawing.Point(200, 420);
            btnReports.Click += (s, e) => { new ReportSelectionForm().Show(); };

            btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Location = new System.Drawing.Point(600, 420);
            btnLogout.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtSearchName);
            this.Controls.Add(btnSearch);
            this.Controls.Add(dgvLearners);
            this.Controls.Add(btnViewSummary);
            this.Controls.Add(btnReports);
            this.Controls.Add(btnLogout);
        }

        // Loads learner sessions, optionally filtered by name.
        // NOTE: queries directly for now - can switch to a LearnerDA method later if one exists.
        private void LoadLearners(string nameFilter)
        {
            dgvLearners.Rows.Clear();
            dgvLearners.Columns.Clear();
            dgvLearners.Columns.Add("SessionID", "Session ID");
            dgvLearners.Columns.Add("Name", "Learner");
            dgvLearners.Columns.Add("Grade", "Grade");
            dgvLearners.Columns.Add("Degree", "Selected Degree");
            dgvLearners.Columns.Add("Outcome", "Advisory Outcome");

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT s.sessionID, l.name, g.gradeName, d.degreeName, s.eligibleResult
                    FROM AdviceSession s
                    JOIN Learner l ON s.learnerID = l.learnerID
                    JOIN GradeLevel g ON l.gradeLevelID = g.gradeLevelID
                    JOIN DegreeProgramme d ON s.degreeID = d.degreeID
                    WHERE (@nameFilter IS NULL OR l.name LIKE '%' + @nameFilter + '%')
                    ORDER BY s.dateOfSession DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nameFilter", (object)nameFilter ?? DBNull.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvLearners.Rows.Add(reader["sessionID"], reader["name"], reader["gradeName"],
                        reader["degreeName"], reader["eligibleResult"]);
                }
            }
        }

        private void BtnViewSummary_Click(object sender, EventArgs e)
        {
            if (dgvLearners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a learner session first.");
                return;
            }
            int sessionID = Convert.ToInt32(dgvLearners.SelectedRows[0].Cells["SessionID"].Value);
            new AdvisorySummaryForm(sessionID).Show();
        }
    }
}
