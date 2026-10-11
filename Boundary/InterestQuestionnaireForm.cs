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
            // Populate using all questions
            dgvQuestions.Rows.Add(1, "I enjoy solving problems using logic and technology.", null);
            dgvQuestions.Rows.Add(2, "I am interested in how software and computer systems work.", null);
            dgvQuestions.Rows.Add(3, "I enjoy working with money, business and financial decisions.", null);
            dgvQuestions.Rows.Add(4, "I like leading projects and organising people or resources.", null);
            dgvQuestions.Rows.Add(5, "I am interested in the human body, health and medicine.", null);
            dgvQuestions.Rows.Add(6, "I enjoy studying living things and how they function.", null);
            dgvQuestions.Rows.Add(7, "I enjoy building, designing or understanding physical structures.", null);
            dgvQuestions.Rows.Add(8, "I am curious about how the physical world works.", null);
            dgvQuestions.Rows.Add(9, "I enjoy reading, writing and discussing ideas.", null);
            dgvQuestions.Rows.Add(10, "I am interested in people, society and how they behave.", null);
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
            // declare dictionary to store user answers
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

            btnNext.Enabled = false;   // stops a second click saving again
            MessageBox.Show("Learner Profile successfully created and advice session initiated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InterestQuestionnaireForm_Load_1(object sender, EventArgs e)
        {
            dgvQuestions.DataSource = null;   

            List<InterestQuery> questions = LearnerController.GetQuestions();
            foreach (InterestQuery q in questions)
            {
                int i = dgvQuestions.Rows.Add(q.QueryText, null);
                dgvQuestions.Rows[i].Tag = q.QueryID;  
            }
        }
    }
}