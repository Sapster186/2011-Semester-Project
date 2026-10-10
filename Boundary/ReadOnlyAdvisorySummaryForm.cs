namespace _2011__Semester_Project.Boundary
{
    // The Career Advisor's version of the summary: the same screen,
    // but it can be closed without exporting (false = export not required)
    public class ReadOnlyAdvisorySummaryForm : AdvisorySummaryForm
    {
        public ReadOnlyAdvisorySummaryForm(int sessionID) : base(sessionID, false)
        {
            this.Text = "Future Path - Advisory Summary (Read-Only)";
        }
    }
}