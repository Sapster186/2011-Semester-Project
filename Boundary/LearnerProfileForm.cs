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
            
            cmbGrade.Items.Clear();
            cmbGrade.Items.AddRange(new string[] { "Grade 11", "Grade 12" });

            
            cmbProvince.Items.Clear();
            cmbProvince.Items.AddRange(new string[] {
                "Eastern Cape", "Free State", "Gauteng",
                "KwaZulu-Natal", "Limpopo", "Mpumalanga",
                "North West", "Northern Cape", "Western Cape"
            });
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
            // Contact details are optional, so a blank value is fine (anonymous session).
            string err = LearnerController.CaptureProfile(
                txtFullName.Text,
                txtSchoolName.Text,
                (int)cmbGrade.SelectedValue,
                cmbProvince.Text,
                txtContact.Text);

            if (err != null)
            {
                MessageBox.Show(err, "Check the profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SubjectMarkEntry subjectForm = new SubjectMarkEntry();
            subjectForm.Show();
            this.Hide();
        }
    }
}