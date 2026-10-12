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
    public partial class InterestQuestionnaireForm : Form
    {
        public InterestQuestionnaireForm()
        {
            InitializeComponent();

            
            btnNext.Enabled = false;
        }

        private void InterestQuestionnaireForm_Load(object sender, EventArgs e)
        {
            dgvQuestions.DataSource = null;
            foreach (InterestQuery q in LearnerController.GetQuestions())
            {
                int i = dgvQuestions.Rows.Add(q.QueryText, null);
                dgvQuestions.Rows[i].Tag = q.QueryID;
            }
        }

       
        private void dgvQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ValidateAnswers();
        }

        private void ValidateAnswers()
        {
            bool allAnswered = true;

            foreach (DataGridViewRow row in dgvQuestions.Rows)
            {
                // Skips the empty row
                if (row.IsNewRow) continue;

               
                // checks if all columns are filled.
                if (row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()))
                {
                    allAnswered = false;
                    break;
                }
            }

            btnNext.Enabled = allAnswered;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> answers = new Dictionary<int, string>();
            foreach (DataGridViewRow row in dgvQuestions.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells[1].Value == null)
                {
                    MessageBox.Show("Please answer every question.");
                    return;
                }
                answers.Add((int)row.Tag, row.Cells[1].Value.ToString());
            }

            string err = LearnerController.SaveSession(answers);
            if (err != null)
            {
                MessageBox.Show(err, "Could not save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnNext.Enabled = false;
            MessageBox.Show("Learner Profile successfully created and advice session initiated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        
    }
}