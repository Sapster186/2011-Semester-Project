using _2011__Semester_Project.Controller;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2011__Semester_Project.Boundary
{
    public partial class FacultyDegreeSelectionForm : Form
    {

        private FacultyDA facultyDA = new FacultyDA();
        private DegreeProgrammeDA degreeDA = new DegreeProgrammeDA();
        private AdviceController adviceController = new AdviceController();
        private int learnerID;
        public FacultyDegreeSelectionForm(int learnerID)
        {
            InitializeComponent();
            this.learnerID = learnerID;
        }

        private void FacultyDegreeSelectionForm_Load(object sender, EventArgs e)
        {
            // load faculties
            List<Faculty> faculties = facultyDA.GetAllFaculties();
            facultyComboBox.DataSource = faculties;
            facultyComboBox.DisplayMember = "facultyName";
            facultyComboBox.ValueMember = "facultyID";
        }

        private void facultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (facultyComboBox.SelectedValue == null) return;

            int facultyID;
            if (!int.TryParse(facultyComboBox.SelectedValue.ToString(), out facultyID)) return;

            // load active degrees for that faculty
            List<DegreeProgramme> degrees = degreeDA.GetDegreesByFaculty(facultyID);
            degreeComboBox.DataSource = degrees;
            degreeComboBox.DisplayMember = "degreeName";
            degreeComboBox.ValueMember = "degreeID";
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            if (degreeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a degree.");
                return;
            }

            int degreeID = int.Parse(degreeComboBox.SelectedValue.ToString());

            AdvisoryOutcomeForm outcome = new AdvisoryOutcomeForm(learnerID, degreeID);
            outcome.Show();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void degreeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // nothing to do here
        }
    }
}