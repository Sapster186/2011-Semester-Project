using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Controller
{
    // Holds one learner's session across the three forms and saves it all
    // ONLY SAVES AT THE END. If anything fails part-way, the half-saved learner is removed.
    // 2nd layer of data validation, checks for null values or attributes null values to empty data if theyre null the session would be deleted
    public static class LearnerController
    {
        private static readonly LookupDA lookupDA = new LookupDA();
        private static readonly LearnerDA learnerDA = new LearnerDA();
        private static readonly LearnerSubjectDA subjectDA = new LearnerSubjectDA();
        private static readonly LearnerInterestAnswerDA answerDA = new LearnerInterestAnswerDA();

        // ---- session state (filled form by form) ----
        private static Learner pendingLearner;
        private static List<LearnerSubject> pendingSubjects = new List<LearnerSubject>();
        public static int CurrentLearnerID { get; private set; }   

        // ---- lookups for the forms ----
        public static List<GradeLevel> GetGrades() { return lookupDA.GetGrades(); }
        public static List<Subject> GetSubjects() { return lookupDA.GetSubjects(); }
        public static List<InterestQuery> GetQuestions() { return lookupDA.GetActiveQuestions(); }

        public static void StartNewSession()
        {
            pendingLearner = null;
            pendingSubjects = new List<LearnerSubject>();
            CurrentLearnerID = 0;
        }

        // Form 1. Returns null if OK, else the error message to show.
        public static string CaptureProfile(string name, string school, int gradeLevelID, string province, string contact)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Full name is required.";
            if (string.IsNullOrWhiteSpace(school)) return "School name is required.";
            if (gradeLevelID <= 0) return "Please select a grade.";
            if (string.IsNullOrWhiteSpace(province)) return "Please select a province.";

            pendingSubjects = new List<LearnerSubject>();   // a new profile starts with no subjects
            pendingLearner = new Learner();
            pendingLearner.name = name.Trim();
            pendingLearner.schoolName = school.Trim();
            pendingLearner.gradeLevelID = gradeLevelID;
            pendingLearner.province = province;
            pendingLearner.contactDetails = string.IsNullOrWhiteSpace(contact) ? null : contact.Trim(); // accepts null values
            pendingLearner.dateCreated = DateTime.Now;
            return null;
        }

        // Form 2, per row. Returns null if OK, else error message.
        public static string AddSubjectMark(int subjectID, string markText)
        {
            double mark;

            if (subjectID <= 0) return "Please select a subject.";
            if (!double.TryParse(markText, out mark)) return "Mark must be a number.";
            if (mark < 0 || mark > 100) return "Mark must be between 0 and 100.";
            foreach (LearnerSubject s in pendingSubjects)
            {
                if (s.subjectID == subjectID) return "This subject has already been added.";
            }

            LearnerSubject ls = new LearnerSubject();
            ls.subjectID = subjectID;
            ls.mark = mark;
            pendingSubjects.Add(ls);
            return null;
        }

        public static void RemoveSubjectMark(int subjectID)
        {
            pendingSubjects.RemoveAll(s => s.subjectID == subjectID);
        }

        public static int SubjectCount { get { return pendingSubjects.Count; } }

        // Form 3. answers: queryID -> answer text ("1" to "5"). Saves the whole session.
        // Returns null if OK, else error message.
        public static string SaveSession(Dictionary<int, string> answers)
        {
            if (pendingLearner == null) return "No learner profile captured.";
            if (pendingSubjects.Count == 0) return "At least one subject and mark is required.";
            if (answers == null || answers.Count == 0) return "Please answer the interest questions.";

            // checks for validations
            HashSet<int> seen = new HashSet<int>();
            foreach (LearnerSubject s in pendingSubjects)
            {
                if (s.mark < 0 || s.mark > 100) return "A mark is outside 0 to 100.";
                if (!seen.Add(s.subjectID)) return "A subject was added twice.";
            }
            foreach (KeyValuePair<int, string> a in answers)
            {
                if (string.IsNullOrWhiteSpace(a.Value)) return "Every question must be answered.";
            }

            int id = 0;
            try
            {
                id = learnerDA.Insert(pendingLearner);

                foreach (LearnerSubject s in pendingSubjects)
                {
                    s.learnerID = id;
                    subjectDA.Insert(s);
                }

                foreach (KeyValuePair<int, string> a in answers)
                {
                    LearnerInterestAnswer ia = new LearnerInterestAnswer();
                    ia.learnerID = id;
                    ia.queryID = a.Key;
                    ia.answer = a.Value.Trim();
                    answerDA.Insert(ia);
                }

                CurrentLearnerID = id;
                pendingLearner = null;   // stops a double click saving the same learner twice
                pendingSubjects = new List<LearnerSubject>();   // the next learner must not inherit these subjects
                return null;
            }
            catch (SqlException ex)
            {
                // undo anything that was saved so we never keep half a learner
                if (id > 0)
                {
                    try
                    {
                        answerDA.DeleteByLearner(id);
                        subjectDA.DeleteByLearner(id);
                        learnerDA.Delete(id);
                    }
                    catch (SqlException) { }
                }
                return "Could not save to the database: " + ex.Message;
            }
        }
    }
}