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
    public partial class AdvisoryOutcomeForm : Form
    {
        private AdviceController adviceController = new AdviceController();
        private LearnerSubjectDA subjectDA = new LearnerSubjectDA();
        private LearnerDA learnerDA = new LearnerDA();
        private DegreeProgrammeDA degreeDA = new DegreeProgrammeDA();
        private DegreeRequirementsDA reqDA = new DegreeRequirementsDA();


        private int learnerID;
        private int degreeID;
        private int sessionID = 0;
        private int facultyID = 0;
        public AdvisoryOutcomeForm(int learnerID, int degreeID)
        {
            InitializeComponent();
            this.learnerID = learnerID;
            this.degreeID = degreeID;
        }

        private void AdvisoryOutcomeForm_Load(object sender, EventArgs e)
        {
            try
            {
                Learner learner = learnerDA.GetById(learnerID);
                List<LearnerSubject> subjects = subjectDA.GetByLearner(learnerID);
                DegreeProgramme degree = degreeDA.GetDegreeByID(degreeID);
                List<DegreeRequirements> reqs = reqDA.GetRequirementsByDegree(degreeID);

                if (degree == null)
                {
                    MessageBox.Show("Degree not found.");
                    return;
                }

                facultyID = degree.facultyID;

                string gap;
                double subjectScore, markScore, interestScore, facultyScore;
                AdviceSession session = adviceController.CheckEligibilityWithGap(
                    learner, subjects, degree, reqs,
                    out gap, out subjectScore, out markScore, out interestScore, out facultyScore);

                sessionID = adviceController.SaveSession(session);
                if (session.eligibleResult == AdviceController.STRONG_MATCH
                    || session.eligibleResult == AdviceController.POSSIBLE_MATCH)
                {
                    RecommendationResult chosen = new RecommendationResult();
                    chosen.sessionID = sessionID;
                    chosen.degreeID = degreeID;
                    chosen.subjectMatchScore = subjectScore;
                    chosen.markMatchScore = markScore;
                    chosen.interestMatchScore = interestScore;
                    chosen.desiredFacultyScore = facultyScore;
                    chosen.overallScore = session.overallScore;
                    chosen.isAlternative = false;
                    chosen.gapObservation = gap;
                    adviceController.SaveRecommendations(new List<RecommendationResult> { chosen });
                }
                else
                {
                    // save the chosen degree too, so the gap text is on record (Report 3 needs this)
                    RecommendationResult chosen = new RecommendationResult();
                    chosen.sessionID = sessionID;
                    chosen.degreeID = degreeID;
                    chosen.subjectMatchScore = subjectScore;
                    chosen.markMatchScore = markScore;
                    chosen.interestMatchScore = interestScore;
                    chosen.desiredFacultyScore = facultyScore;
                    chosen.overallScore = session.overallScore;
                    chosen.isAlternative = false;
                    chosen.gapObservation = gap;
                    adviceController.SaveRecommendations(new List<RecommendationResult> { chosen });

                    // then find and save alternatives
                    List<DegreeProgramme> allDegrees = degreeDA.GetAllActiveDegrees();
                    List<DegreeRequirements> allReqs = reqDA.GetAllRequirements();

                    List<RecommendationResult> recs = adviceController.GenerateRecommendations(
                        learner, subjects, allDegrees, allReqs);

                    foreach (RecommendationResult r in recs)
                    {
                        r.sessionID = sessionID;
                        r.isAlternative = true;
                        adviceController.SaveRecommendations(new List<RecommendationResult> { r });
                    }
                }

                outcomeLbl.Text = session.eligibleResult;
                gapLbl.Text = string.IsNullOrEmpty(gap)
                    ? "You meet the requirements for this degree."
                    : gap;
                scoreLbl.Text = "Overall score: " + Math.Round(session.overallScore) + "%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void alternativesButton_Click(object sender, EventArgs e)
        {
            AlternativeProgramsForm alt = new AlternativeProgramsForm(learnerID, sessionID);
            alt.Show();
        }

        private void fundingButton_Click(object sender, EventArgs e)
        {
            // Member D's form
            FundingInformationForm funding = new FundingInformationForm(facultyID, degreeID);
            funding.Show();
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            // Member D's form
            AdvisorySummaryForm summary = new AdvisorySummaryForm(sessionID);
            summary.Show();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
