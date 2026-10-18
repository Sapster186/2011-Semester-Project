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
    public partial class LearnerProfileForm : Form
    {
        // all forms validate fields in 2 different ways
        // manual validate fields function and on the database field
        // all forms also ensure information is filled out before displaying buttons
        public LearnerProfileForm()
        {
            InitializeComponent();

            btnNext.Enabled = false;
        }

        private void LearnerProfileForm_Load(object sender, EventArgs e)
        {
            LearnerController.StartNewSession();

            // Limits the length of the tabs to 100 characters as that is the length of the database rows.
            txtFullName.MaxLength = 100;
            txtSchoolName.MaxLength = 100;
            txtContact.MaxLength = 100;

            cmbGrade.DataSource = LearnerController.GetGrades();
            cmbGrade.DisplayMember = "gradeName";
            cmbGrade.ValueMember = "gradeLevelID";
            cmbGrade.SelectedIndex = -1;

            cmbProvince.Items.Clear();
            cmbProvince.Items.AddRange(new string[] {
        "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo",
        "Mpumalanga", "North West", "Northern Cape", "Western Cape" });
          
        }
        
        // necessary checks if any fields are empty or null
        private void ValidateRequiredFields(object sender, EventArgs e)
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(txtFullName.Text);
            bool isSchoolValid = !string.IsNullOrWhiteSpace(txtSchoolName.Text);
            bool isGradeValid = cmbGrade.SelectedIndex != -1;
            bool isProvinceValid = cmbProvince.SelectedIndex != -1;

            btnNext.Enabled = isNameValid && isSchoolValid && isGradeValid && isProvinceValid;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (cmbGrade.SelectedValue == null)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }
            int gradeId = Convert.ToInt32(cmbGrade.SelectedValue);

            string err = LearnerController.CaptureProfile(txtFullName.Text, txtSchoolName.Text,
                gradeId, cmbProvince.Text, txtContact.Text); // controller validates all fields
            if (err != null)
            {
                MessageBox.Show(err, "Check the profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SubjectMarkEntry subjectForm = new SubjectMarkEntry();
            subjectForm.MdiParent = this.MdiParent;
            subjectForm.Show();
            this.Close();   // closes to prevent duplicate forms

        }
    }
}