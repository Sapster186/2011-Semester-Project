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
    public partial class AlternativeProgramsForm : Form
    {
        private AdviceController adviceController = new AdviceController();
        private LearnerSubjectDA subjectDA = new LearnerSubjectDA();
        private LearnerDA learnerDA = new LearnerDA();
        private DegreeProgrammeDA degreeDA = new DegreeProgrammeDA();
        private DegreeRequirementsDA reqDA = new DegreeRequirementsDA();

        private int learnerID;
        private int sessionID;
        private int degreeID;

        public AlternativeProgramsForm(int learnerID, int sessionID, int degreeID)
        {
            InitializeComponent();
            this.learnerID = learnerID;
            this.sessionID = sessionID;
            this.degreeID = degreeID;
        }

        private void AlternativeProgramsForm_Load(object sender, EventArgs e)
        {
            altListView.View = View.Details;
            altListView.Columns.Insert(0, "Degree", 300, HorizontalAlignment.Left);
            altListView.Columns.Insert(1, "Score", 100, HorizontalAlignment.Center);
            altListView.Columns.Insert(2, "Reason", 400, HorizontalAlignment.Left);

            try
            {
                Learner learner = learnerDA.GetById(learnerID);
                List<LearnerSubject> subjects = subjectDA.GetByLearner(learnerID);
                List<DegreeProgramme> allDegrees = degreeDA.GetAllActiveDegrees();
                List<DegreeRequirements> allReqs = reqDA.GetAllRequirements();

                List<RecommendationResult> recs = adviceController.GenerateRecommendations(
                    learner, subjects, allDegrees, allReqs);

                foreach (RecommendationResult r in recs)
                {
                    // Skip the degree the user originally applied for
                    if (r.degreeID == this.degreeID) continue;

                    DegreeProgramme d = degreeDA.GetDegreeByID(r.degreeID);
                    if (d == null) continue;

                    ListViewItem item = new ListViewItem(d.degreeName);
                    item.SubItems.Add(Math.Round(r.overallScore, 1).ToString());
                    item.SubItems.Add(string.IsNullOrEmpty(r.gapObservation) ? "-" : r.gapObservation);
                    altListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // We added this back in so the Designer file doesn't crash!
        private void AlternativeProgramsForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}