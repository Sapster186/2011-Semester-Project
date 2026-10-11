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
    public partial class SubjectMarkEntry : Form
    {
        // List to track duplicate subjects behind the scenes
        private List<string> addedSubjects = new List<string>();

        public SubjectMarkEntry()
        {
            InitializeComponent();

            // Disables the buttons by default when the form opens
            btnAddSubject.Enabled = false;
            btnNext.Enabled = false;
        }

       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        
        private void database1DataSetBindingSource_CurrentChanged(object sender, EventArgs e)
        {
        }

        private void SubjectMarkEntry_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;

            
            cmbSubject.Items.Clear();
            cmbSubject.Items.AddRange(new string[] {
                "Accounting", "English Home Language", "Information Technology",
                "Life Sciences", "Mathematics", "Mathematical Literacy", "Physical Sciences"
            });
        }

        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void txtMark_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        // Handles the logic of has the user typed in valid information.
        private void ValidateInput()
        {
            lblError.Visible = false;
            btnAddSubject.Enabled = false;

            // if nothing is entered then it does not check for validation
            if (cmbSubject.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtMark.Text))
                return;

            string selectedSubject = cmbSubject.SelectedItem.ToString();

            // checks for duplicates
            if (addedSubjects.Contains(selectedSubject))
            {
                lblError.Text = "This subject has already been added.";
                lblError.Visible = true;
                return;
            }

            // range check
            if (!float.TryParse(txtMark.Text, out float mark) || mark < 0 || mark > 100)
            {
                lblError.Text = "Mark must be between 0 and 100.";
                lblError.Visible = true;
                return;
            }

            // Enables buttons if true
            btnAddSubject.Enabled = true;
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string subject = cmbSubject.SelectedItem.ToString();
            string mark = txtMark.Text;

            
            addedSubjects.Add(subject);
            dgvSubjects.Rows.Add(subject, mark);

            // Reset inputs 
            cmbSubject.SelectedIndex = -1;
            txtMark.Clear();
            txtMark.Focus();

            
            btnNext.Enabled = true;
        }

        // Links to next! form
        private void btnNext_Click(object sender, EventArgs e)
        {
            InterestQuestionnaireForm nextForm = new InterestQuestionnaireForm();
            nextForm.Show();
            this.Hide();
        }
    }
}