namespace _2011__Semester_Project.Boundary
{
    // ReadOnlyAdvisorySummaryForm — the Career Advisor's version of the summary.
    //
    // Reuses the learner's AdvisorySummaryForm, but passes requireExport = false
    // so the advisor can close it without saving, exporting or printing.
    // Everything else (layout, content, buttons) is inherited from the base class.
    //
    // Opened from the Career Advisor Dashboard when the advisor clicks
    // "View Advisory Summary" for a selected session:
    //     new ReadOnlyAdvisorySummaryForm(sessionID).Show();
    public class ReadOnlyAdvisorySummaryForm : AdvisorySummaryForm
    {
        public ReadOnlyAdvisorySummaryForm(int sessionID) : base(sessionID, false)
        {
            this.Text = "Future Path - Advisory Summary (Read-Only)";
        }
    }
}