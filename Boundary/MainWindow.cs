using _2011_Semester_Project.Controller;
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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            LoginController controller = new LoginController();
            controller.Logout();
            Application.Restart();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuLearnerProfile_Click(object sender, EventArgs e)
        {
            LearnerProfileForm form = new LearnerProfileForm();
            form.MdiParent = this;
            form.Show();
        }

        private void mnuInterestQuestionnaire_Click(object sender, EventArgs e)
        {
            InterestQuestionnaireForm form = new InterestQuestionnaireForm();
            form.MdiParent = this;
            form.Show();
        }

        private void mnuCareerAdvisorDashboard_Click(object sender, EventArgs e)
        {
            CareerAdvisorDashboardForm form = new CareerAdvisorDashboardForm(LoginController.CurrentUserRole);
            form.MdiParent = this;
            form.Show();
        }

        private void mnuAdminDashboard_Click(object sender, EventArgs e)
        {
            AdminDashboardForm form = new AdminDashboardForm();
            form.MdiParent = this;
            form.Show();
        }

        private void mnuMaintainReferenceData_Click(object sender, EventArgs e)
        {
            // ReferenceDataMaintenanceForm form = new ReferenceDataMaintenanceForm();
            // form.MdiParent = this;
            // form.Show();
        }

        private void mnuReportSelection_Click(object sender, EventArgs e)
        {
            ReportSelectionForm form = new ReportSelectionForm(LoginController.CurrentUserRole);
            form.MdiParent = this;
            form.Show();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Future Path Student Advising System\nINF2011S Project 2026", "About");
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            string role = LoginController.CurrentUserRole;

            // Hide everything by default
            mnuLearner.Visible = false;
            mnuAdvisor.Visible = false;
            mnuAdmin.Visible = false;
            mnuReports.Visible = false;

            // Show only what this role should see
            if (role == "Learner")
            {
                mnuLearner.Visible = true;
            }
            else if (role == "Career Advisor")
            {
                mnuAdvisor.Visible = true;
                mnuReports.Visible = true;
            }
            else if (role == "Administrator")
            {
                mnuAdmin.Visible = true;
                mnuReports.Visible = true;
            }
        }
    }
}
