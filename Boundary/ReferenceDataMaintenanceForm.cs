using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Boundary
{
    public partial class ReferenceDataMaintenanceForm : Form
    {
        public ReferenceDataMaintenanceForm()
        {
            InitializeComponent();
        }

        private void ReferenceDataMaintenanceForm_Load(object sender, EventArgs e)
        {
            cmbDataType.Items.Add("Degree Programme");
            cmbDataType.Items.Add("Degree Requirements");
            cmbDataType.Items.Add("Funding Option");
            cmbDataType.Items.Add("Interest Questionnaire");
            cmbDataType.SelectedIndex = 0;
        }
    }
}
