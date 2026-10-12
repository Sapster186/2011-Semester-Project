using System;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

    namespace _2011__Semester_Project.Controller
{
    // Handles admin-only actions. Refuses anything if the logged-in
    // user is not an Administrator. Called by the maintenance forms.
    public class AdminController
    {
        // Checks whether the current logged-in user is an Administrator.
        // This is the double-check required by test case 10 —
        // even if the form lies, the controller refuses.
        public bool IsAdministrator()
        {
            return LoginController.CurrentUserRole == "Administrator";
        }

        // Called by any admin maintenance action before it runs.
        // Returns null if allowed, or an error message if refused.
        public string CheckAdminAccess()
        {
            if (!IsAdministrator())
            {
                return "Access denied. Only Administrators may maintain reference data.";
            }
            return null;
        }

        // ---- Interest Questionnaire ----

        public string AddInterestQuery(int groupID, string questionText, float weight)
        {
            string check = CheckAdminAccess();
            if (check != null) return check;

            if (string.IsNullOrWhiteSpace(questionText)) return "Question text is required.";
            if (weight <= 0) return "Weight must be greater than 0.";

            InterestQuery q = new InterestQuery();
            q.InterestGroupID = groupID;
            q.QueryText = questionText.Trim();
            q.Weight = weight;
            q.IsActive = true;

            try
            {
                InterestQueryDA da = new InterestQueryDA();
                da.AddQuery(q);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not save: " + ex.Message;
            }
        }

        public string DeactivateInterestQuery(int queryID)
        {
            string check = CheckAdminAccess();
            if (check != null) return check;

            try
            {
                InterestQueryDA da = new InterestQueryDA();
                da.DeactivateQuery(queryID);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not deactivate: " + ex.Message;
            }
        }

        // ---- Degree Programmes (wired later, once DA exists) ----

        // TODO: Add methods once Member B pushes DegreeProgrammeDA and DegreeRequirementsDA

        // ---- Funding Options (wired once we confirm Member D's DA method names) ----

        // TODO: Add methods once we confirm FundOptionDA method names
    }
}
