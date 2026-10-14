using System;
using System.Collections.Generic;
using System.Data;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Controller
{
    public class AdminController
    {
        public bool IsAdministrator()
        {
            return LoginController.CurrentUserRole == "Administrator" &&
                !string.IsNullOrWhiteSpace(LoginController.CurrentUsername);
        }

        public string CheckAdminAccess()
        {
            return IsAdministrator() ? null : "Access denied. Only Administrators may maintain reference data.";
        }

        private void RequireAdministrator()
        {
            string error = CheckAdminAccess();
            if (error != null) throw new UnauthorizedAccessException(error);
        }

        // Reads are protected too. Forms never call DataAccess directly.
        public List<DegreeProgramme> GetDegreeProgrammes()
        {
            RequireAdministrator();
            return new DegreeProgrammeDA().GetAllActiveDegrees();
        }

        public List<DegreeRequirements> GetDegreeRequirements()
        {
            RequireAdministrator();
            return new DegreeRequirementsDA().GetAllRequirements();
        }

        public List<FundOption> GetFundingOptions()
        {
            RequireAdministrator();
            return new FundOptionDA().GetActiveFundOptions();
        }

        public DataTable GetFundingTypes()
        {
            RequireAdministrator();
            return new ReportDA().GetFundTypes();
        }

        public List<InterestQuery> GetInterestQueries()
        {
            RequireAdministrator();
            return new InterestQueryDA().GetAllQueries();
        }

        public List<InterestGroup> GetInterestGroups()
        {
            RequireAdministrator();
            return new InterestGroupDA().GetAllGroups();
        }

        public string AddInterestQuery(int groupID, string questionText, float weight)
        {
            return SaveInterestQuery(new InterestQuery(0, groupID, questionText, weight, true));
        }

        public string SaveInterestQuery(InterestQuery question)
        {
            string error = CheckAdminAccess();
            if (error != null) return error;
            if (question == null) return "Question is required.";
            if (question.QueryID < 0) return "Invalid question ID.";
            if (question.InterestGroupID <= 0) return "Please select an interest group.";
            if (string.IsNullOrWhiteSpace(question.QueryText) || question.QueryText.Trim().Length > 200)
                return "Question text is required and must be at most 200 characters.";
            if (float.IsNaN(question.Weight) || float.IsInfinity(question.Weight) || question.Weight <= 0)
                return "Weight must be a finite number greater than 0.";

            try
            {
                if (new InterestGroupDA().GetGroupByID(question.InterestGroupID) == null)
                    return "The selected interest group no longer exists.";
                question.QueryText = question.QueryText.Trim();
                InterestQueryDA da = new InterestQueryDA();
                if (question.QueryID == 0) da.AddQuery(question);
                else da.UpdateQuery(question);
                return null;
            }
            catch (Exception ex) { return "Could not save the question: " + ex.Message; }
        }

        public string DeactivateInterestQuery(int queryID)
        {
            string error = CheckAdminAccess();
            if (error != null) return error;
            if (queryID <= 0) return "Please select a question.";
            try
            {
                new InterestQueryDA().DeactivateQuery(queryID);
                return null;
            }
            catch (Exception ex) { return "Could not deactivate the question: " + ex.Message; }
        }

        public string SaveFundingOption(FundOption option)
        {
            string error = CheckAdminAccess();
            if (error != null) return error;
            if (option == null) return "Funding option is required.";
            if (option.FundID < 0) return "Invalid funding ID.";
            if (string.IsNullOrWhiteSpace(option.FundName) || option.FundName.Trim().Length > 100)
                return "Funding name is required and must be at most 100 characters.";
            if (string.IsNullOrWhiteSpace(option.FundType) || option.FundType.Length > 50)
                return "Please select a funding type.";
            if ((option.EligibleSummary ?? "").Length > 200 || (option.NecessaryDocs ?? "").Length > 200 ||
                (option.ContactInformation ?? "").Length > 200)
                return "Eligibility, documents and contact details must each be at most 200 characters.";
            if (option.CloseDate < new DateTime(1753, 1, 1)) return "Please select a valid closing date.";

            try
            {
                bool validType = false;
                foreach (DataRow row in GetFundingTypes().Rows)
                    if (row["fundType"].ToString() == option.FundType) validType = true;
                if (!validType) return "Please select an existing funding type.";

                option.FundName = option.FundName.Trim();
                option.EligibleSummary = option.EligibleSummary ?? "";
                option.NecessaryDocs = option.NecessaryDocs ?? "";
                option.ContactInformation = option.ContactInformation ?? "";
                FundOptionDA da = new FundOptionDA();
                if (option.FundID == 0) da.AddFundOption(option);
                else da.UpdateFundOption(option);
                return null;
            }
            catch (Exception ex) { return "Could not save the funding option: " + ex.Message; }
        }

        public string DeactivateFundingOption(int fundID)
        {
            string error = CheckAdminAccess();
            if (error != null) return error;
            if (fundID <= 0) return "Please select a funding option.";
            try
            {
                new FundOptionDA().DeactivateFundOption(fundID);
                return null;
            }
            catch (Exception ex) { return "Could not deactivate the funding option: " + ex.Message; }
        }
    }
}
