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
    public partial class AdminDashboardForm : Form
    {
        public AdminDashboardForm()
        {
            InitializeComponent();
        }

        private void btnMaintainReferenceData_Click(object sender, EventArgs e)
        {
            // TODO: uncomment once ReferenceDataMaintenanceForm is built (tomorrow)
            // ReferenceDataMaintenanceForm form = new ReferenceDataMaintenanceForm();
            // form.MdiParent = this.MdiParent;
            // form.Show();
        }

        private void btnViewReports_Click(object sender, EventArgs e)
        {
            ReportSelectionForm form = new ReportSelectionForm(LoginController.CurrentUserRole);
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
