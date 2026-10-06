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
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Boundary
{
    public partial class FundingInformationForm : Form
    {
        private ReportController reportController = new ReportController();

        // All controls declared here, built in code below instead of the Designer
        private Label lblTitle;
        private Label lblTypeFilter;
        private ComboBox cmbTypeFilter;
        private Button btnSearch;
        private DataGridView dgvFunding;
        private Button btnBack;

        public FundingInformationForm()
        {
            InitializeComponent();
            SetupControls();
            LoadFunding(null); // show everything on first open
        }

        private void SetupControls()
        {
            this.Text = "Future Path - Funding Information";
            this.Width = 700;
            this.Height = 500;

            lblTitle = new Label();
            lblTitle.Text = "Funding Opportunities";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.AutoSize = true;

            lblTypeFilter = new Label();
            lblTypeFilter.Text = "Funding Type:";
            lblTypeFilter.Location = new System.Drawing.Point(20, 60);
            lblTypeFilter.AutoSize = true;

            cmbTypeFilter = new ComboBox();
            cmbTypeFilter.Location = new System.Drawing.Point(130, 57);
            cmbTypeFilter.Width = 180;
            cmbTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList; // stops free typing - must pick from list
            cmbTypeFilter.Items.Add("All");
            cmbTypeFilter.Items.Add("Scholarship");
            cmbTypeFilter.Items.Add("Bursary");
            cmbTypeFilter.Items.Add("Financial Aid");
            cmbTypeFilter.Items.Add("External Funding");
            cmbTypeFilter.SelectedIndex = 0;

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new System.Drawing.Point(330, 56);
            btnSearch.Click += BtnSearch_Click; // runs BtnSearch_Click when clicked

            dgvFunding = new DataGridView();
            dgvFunding.Location = new System.Drawing.Point(20, 100);
            dgvFunding.Width = 640;
            dgvFunding.Height = 300;
            dgvFunding.ReadOnly = true;
            dgvFunding.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFunding.AllowUserToAddRows = false;

            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Location = new System.Drawing.Point(20, 420);
            btnBack.Click += (s, e) => this.Close();

            // Add every control onto the visible form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTypeFilter);
            this.Controls.Add(cmbTypeFilter);
            this.Controls.Add(btnSearch);
            this.Controls.Add(dgvFunding);
            this.Controls.Add(btnBack);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string selectedType = cmbTypeFilter.SelectedItem.ToString();
            LoadFunding(selectedType == "All" ? null : selectedType);
        }

        // Loads funding into the grid, optionally filtered by type
        private void LoadFunding(string fundType)
        {
            var results = reportController.GetFilteredFunding(null, null, fundType);

            dgvFunding.Rows.Clear();
            dgvFunding.Columns.Clear();
            dgvFunding.Columns.Add("Name", "Funding Name");
            dgvFunding.Columns.Add("Type", "Type");
            dgvFunding.Columns.Add("Eligibility", "Eligibility");
            dgvFunding.Columns.Add("Closes", "Closing Date");
            dgvFunding.Columns.Add("Verify", "Status");

            foreach (FundOption option in results)
            {
                dgvFunding.Rows.Add(
                    option.FundName, option.FundType, option.EligibleSummary,
                    option.CloseDate.ToString("dd/MM/yyyy"),
                    "Sample information - verify with institution"
                );
            }
        }
    }
}
