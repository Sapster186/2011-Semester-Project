using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    // FundingInformationForm — funding lookup filtered by faculty, degree and type.
    //
    // Built entirely in code (no Designer).
    //
    // Opened two ways:
    //   1. Directly from a menu / dashboard: new FundingInformationForm()
    //      - shows all funding, the user picks filters manually.
    //   2. From the AdvisoryOutcomeForm (Member B):
    //        new FundingInformationForm(facultyID, degreeID)
    //       -opens pre-filtered to the learner's chosen faculty and degree.
    //
    // Spec rules enforced by the query behind this form (test case 8):
    //   - isActive = 1 (inactive funds hidden)
    //   - closeDate is null or in the future (expired funds hidden)
    //   - every row is flagged as "sample information"
    public partial class FundingInformationForm : Form
    {
        private ReportController reportController = new ReportController();

        // Controls are built in code below instead of the Designer
        private Label lblTitle;
        private Label lblFaculty;
        private ComboBox cmbFaculty;
        private Label lblDegree;
        private ComboBox cmbDegree;
        private Label lblType;
        private ComboBox cmbType;
        private Button btnSearch;
        private Label lblCount;
        private DataGridView dgvFunding;
        private Button btnBack;

        // Constructor: builds layout, loads the three dropdowns, then applies the
        // pre-selected filters if any were supplied.
        public FundingInformationForm(int? facultyID = null, int? degreeID = null)
        {
            InitializeComponent();
            SetupControls();

            // Load the faculty list first, then apply the pre-filter, then
            // load degrees for that faculty and apply the degree pre-filter.
            LoadFaculties();
            if (facultyID.HasValue) cmbFaculty.SelectedValue = facultyID.Value;
            LoadDegrees();
            if (degreeID.HasValue) cmbDegree.SelectedValue = degreeID.Value;
            LoadFundTypes();

            // Attached AFTER the initial load so building the lists doesn't fire it.
            // When the user changes the faculty, we rebuild the degree list.
            cmbFaculty.SelectedIndexChanged += (s, e) => LoadDegrees();

            Search();   // show matching funding straight away
        }

        // Builds all controls in code, in the screen-standard layout
        // (heading at top, filters in the middle, grid and buttons at the bottom).
        private void SetupControls()
        {
            this.Text = "Future Path - Funding Information";
            this.Width = 1000;
            this.Height = 580;

            lblTitle = new Label();
            lblTitle.Text = "Funding Opportunities";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            lblFaculty = new Label();
            lblFaculty.Text = "Faculty:";
            lblFaculty.Location = new Point(20, 65);
            lblFaculty.AutoSize = true;

            cmbFaculty = new ComboBox();
            cmbFaculty.Location = new Point(100, 62);
            cmbFaculty.Width = 220;
            cmbFaculty.DropDownStyle = ComboBoxStyle.DropDownList;   // lookup list: user must pick from it

            lblDegree = new Label();
            lblDegree.Text = "Degree:";
            lblDegree.Location = new Point(350, 65);
            lblDegree.AutoSize = true;

            cmbDegree = new ComboBox();
            cmbDegree.Location = new Point(410, 62);
            cmbDegree.Width = 280;
            cmbDegree.DropDownStyle = ComboBoxStyle.DropDownList;

            lblType = new Label();
            lblType.Text = "Funding Type:";
            lblType.Location = new Point(20, 105);
            lblType.AutoSize = true;

            cmbType = new ComboBox();
            cmbType.Location = new Point(120, 102);
            cmbType.Width = 200;
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(350, 101);
            btnSearch.Click += (s, e) => Search();

            lblCount = new Label();
            lblCount.Location = new Point(460, 106);
            lblCount.AutoSize = true;

            dgvFunding = new DataGridView();
            dgvFunding.Location = new Point(20, 145);
            dgvFunding.Width = 940;
            dgvFunding.Height = 330;
            dgvFunding.ReadOnly = true;
            dgvFunding.AllowUserToAddRows = false;
            dgvFunding.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFunding.DefaultCellStyle.WrapMode = DataGridViewTriState.True;     // long text wraps
            dgvFunding.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;  // rows grow to fit

            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Location = new Point(20, 495);
            btnBack.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblFaculty);
            this.Controls.Add(cmbFaculty);
            this.Controls.Add(lblDegree);
            this.Controls.Add(cmbDegree);
            this.Controls.Add(lblType);
            this.Controls.Add(cmbType);
            this.Controls.Add(btnSearch);
            this.Controls.Add(lblCount);
            this.Controls.Add(dgvFunding);
            this.Controls.Add(btnBack);
        }

        // Faculty dropdown: "All" (ID 0) at the top, then every faculty.
        // The "All" row is added to the DataTable before binding.
        private void LoadFaculties()
        {
            DataTable faculties = reportController.GetFaculties();
            DataRow allRow = faculties.NewRow();
            allRow["facultyID"] = 0;
            allRow["facultyName"] = "All";
            faculties.Rows.InsertAt(allRow, 0);

            cmbFaculty.DisplayMember = "facultyName";
            cmbFaculty.ValueMember = "facultyID";
            cmbFaculty.DataSource = faculties;
        }

        // Degree dropdown: only active degrees of the chosen faculty (spec rule).
        // Re-called whenever the faculty changes. "All" (ID 0) is added at the top.
        private void LoadDegrees()
        {
            DataTable degrees = reportController.GetDegrees(SelectedID(cmbFaculty));
            DataRow allRow = degrees.NewRow();
            allRow["degreeID"] = 0;
            allRow["degreeName"] = "All";
            degrees.Rows.InsertAt(allRow, 0);

            cmbDegree.DisplayMember = "degreeName";
            cmbDegree.ValueMember = "degreeID";
            cmbDegree.DataSource = degrees;
        }

        // Type dropdown: "All" plus the funding types that exist in the database.
        // Loaded once (the type list doesn't depend on the other filters).
        private void LoadFundTypes()
        {
            cmbType.Items.Clear();
            cmbType.Items.Add("All");
            foreach (DataRow r in reportController.GetFundTypes().Rows)
                cmbType.Items.Add(r["fundType"].ToString());
            cmbType.SelectedIndex = 0;
        }

        // Translates the combo box selection into a filter value:
        // "All" (ID 0) means "no filter" → return null.
        private int? SelectedID(ComboBox box)
        {
            if (box.SelectedValue == null) return null;
            int id = Convert.ToInt32(box.SelectedValue);
            return id == 0 ? (int?)null : id;
        }

        // Runs the funding lookup with the current filter values and fills the grid.
        // Any of the three filters can be "no filter" (null).
        private void Search()
        {
            string type = cmbType.SelectedItem.ToString();
            DataTable results = reportController.GetFundingOptions(
                SelectedID(cmbFaculty), SelectedID(cmbDegree), type == "All" ? null : type);

            dgvFunding.DataSource = results;
            if (dgvFunding.Columns.Contains("Closing Date"))
                dgvFunding.Columns["Closing Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            lblCount.Text = "Showing " + results.Rows.Count + " option(s)";
        }
    }
}