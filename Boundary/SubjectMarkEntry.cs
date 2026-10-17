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
            dgvSubjects.DataSource = null;
            dgvSubjects.AllowUserToAddRows = false;

            cmbSubject.DataSource = LearnerController.GetSubjects();
            cmbSubject.DisplayMember = "subjectName";
            cmbSubject.ValueMember = "subjectID";
            cmbSubject.SelectedIndex = -1;

        }

        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void txtMark_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        // Handles the validation logic
        private void ValidateInput()
        {
            lblError.Visible = false;
            btnAddSubject.Enabled = false;

            // if nothing is entered then it does not check for validation
            if (cmbSubject.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtMark.Text))
                return;

            Subject picked = cmbSubject.SelectedItem as Subject;
            if (picked == null) return;
            string selectedSubject = picked.subjectName;


            // checks for duplicates
            if (addedSubjects.Contains(selectedSubject))
            {
                lblError.Text = "This subject has already been added.";
                lblError.Visible = true;
                return;
            }

            // ensures the range is between 0 and 100
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
            Subject chosen = cmbSubject.SelectedItem as Subject;
            if (chosen == null) return;

            string mark = txtMark.Text;

            string err = LearnerController.AddSubjectMark(chosen.subjectID, mark);
            if (err != null)
            {
                lblError.Text = err;
                lblError.Visible = true;
                return;
            }

            addedSubjects.Add(chosen.subjectName);
            dgvSubjects.Rows.Add(chosen.subjectName, mark);

            cmbSubject.SelectedIndex = -1;
            txtMark.Clear();
            txtMark.Focus();
            btnNext.Enabled = true;
        }

        // Links to next! form
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (LearnerController.SubjectCount == 0)
            {
                MessageBox.Show("Add at least one subject and mark first.");
                return;
            }
            InterestQuestionnaireForm nextForm = new InterestQuestionnaireForm();
            nextForm.MdiParent = this.MdiParent;
            nextForm.Show();
            this.Close();   // Closes form so user starts on a new form

        }
    }
}