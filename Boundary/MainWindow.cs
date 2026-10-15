using System;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class MainWindow : Form
    {
        public MainWindow() { InitializeComponent(); }

        private bool HasRole(string role)
        {
            if (LoginController.CurrentUserRole == role) return true;
            MessageBox.Show("Access denied for your current role.", "Access denied");
            return false;
        }

        private void OpenChild(Form form)
        {
            // Reuse a screen already open in this session.
            foreach (Form child in MdiChildren)
            {
                if (child.GetType() == form.GetType())
                {
                    form.Dispose();
                    child.Show();
                    child.Activate();
                    return;
                }
            }
            form.MdiParent = this;
            form.Show();
        }

        private void mnuLogout_Click(object sender, EventArgs e) { Close(); }
        private void mnuExit_Click(object sender, EventArgs e) { Application.Exit(); }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            new LoginController().Logout();
            base.OnFormClosed(e);
        }

        private void mnuLearnerProfile_Click(object sender, EventArgs e)
        {
            if (HasRole("Learner")) OpenChild(new LearnerProfileForm());
        }

        private void mnuInterestQuestionnaire_Click(object sender, EventArgs e)
        {
            if (!HasRole("Learner")) return;
            MessageBox.Show("Start with Learner Profile, then enter subjects and marks before answering the questionnaire.");
        }

        private void mnuCareerAdvisorDashboard_Click(object sender, EventArgs e)
        {
            if (HasRole("Career Advisor"))
                OpenChild(new CareerAdvisorDashboardForm(LoginController.CurrentUserRole));
        }

        private void mnuAdminDashboard_Click(object sender, EventArgs e)
        {
            if (HasRole("Administrator")) OpenChild(new AdminDashboardForm());
        }

        private void mnuMaintainReferenceData_Click(object sender, EventArgs e)
        {
            // Maintenance is entered through the Administrator Dashboard.
            mnuAdminDashboard_Click(sender, e);
        }

        private void mnuReportSelection_Click(object sender, EventArgs e)
        {
            if (LoginController.CurrentUserRole == "Career Advisor" || LoginController.CurrentUserRole == "Administrator")
                OpenChild(new ReportSelectionForm(LoginController.CurrentUserRole));
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            string role = LoginController.CurrentUserRole;
            mnuLearner.Visible = role == "Learner";
            mnuAdvisor.Visible = role == "Career Advisor";
            mnuAdmin.Visible = role == "Administrator";
            // Reports are opened from the appropriate dashboard.
            mnuReports.Visible = false;
            mnuMaintainReferenceData.Visible = false;
            if (role != "Learner" && role != "Career Advisor" && role != "Administrator") Close();
        }

        private void checkEligibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HasRole("Learner")) return;
            int learnerID = LearnerController.CurrentLearnerID;
            if (learnerID <= 0)
            {
                MessageBox.Show("Please complete your learner profile, subjects and interest questionnaire first.");
                return;
            }
            OpenChild(new FacultyDegreeSelectionForm(learnerID));
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Future Path Student Advising System\nINF2011S Project 2026", "About");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
    }
}
