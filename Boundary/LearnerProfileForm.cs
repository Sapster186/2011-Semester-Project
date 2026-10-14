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
        public LearnerProfileForm()
        {
            InitializeComponent();

            btnNext.Enabled = false;
        }

        private void LearnerProfileForm_Load(object sender, EventArgs e)
        {
            LearnerController.StartNewSession();

            cmbGrade.DataSource = LearnerController.GetGrades();
            cmbGrade.DisplayMember = "gradeName";
            cmbGrade.ValueMember = "gradeLevelID";
            cmbGrade.SelectedIndex = -1;

            cmbProvince.Items.Clear();
            cmbProvince.Items.AddRange(new string[] {
        "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo",
        "Mpumalanga", "North West", "Northern Cape", "Western Cape" });
        }


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
                gradeId, cmbProvince.Text, txtContactDetails.Text);
            if (err != null)
            {
                MessageBox.Show(err, "Check the profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SubjectMarkEntry subjectForm = new SubjectMarkEntry();
            subjectForm.MdiParent = this.MdiParent;
            subjectForm.Show();
            this.Hide();

        }
    }
}