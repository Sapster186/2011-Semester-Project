using System;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class AdminDashboardForm : Form
    {
        public AdminDashboardForm() { InitializeComponent(); }

        private bool CheckAccess()
        {
            string error = new AdminController().CheckAdminAccess();
            if (error == null) return true;
            MessageBox.Show(error, "Access denied");
            return false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!CheckAccess()) Close();
        }

        private void btnMaintainReferenceData_Click(object sender, EventArgs e)
        {
            if (!CheckAccess()) return;
            foreach (Form child in MdiParent == null ? new Form[0] : MdiParent.MdiChildren)
            {
                if (child is ReferenceDataMaintenanceForm) { child.Activate(); return; }
            }
            ReferenceDataMaintenanceForm form = new ReferenceDataMaintenanceForm();
            form.MdiParent = MdiParent;
            form.Show();
        }

        private void btnViewReports_Click(object sender, EventArgs e)
        {
            if (!CheckAccess()) return;
            ReportSelectionForm form = new ReportSelectionForm(LoginController.CurrentUserRole);
            form.MdiParent = MdiParent;
            form.Show();
        }

        private void btnClose_Click(object sender, EventArgs e) { Close(); }
    }
}
