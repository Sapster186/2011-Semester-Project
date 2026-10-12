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
    public partial class AddInterestQueryForm : Form
    {
        public AddInterestQueryForm()
        {
            InitializeComponent();
        }

        private void AddInterestQueryForm_Load(object sender, EventArgs e)
        {
            InterestGroupDA groupDA = new InterestGroupDA();
            cmbGroup.DataSource = groupDA.GetAllGroups();
            cmbGroup.DisplayMember = "GroupName";
            cmbGroup.ValueMember = "InterestGroupID";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                lblError.Text = "Question text is required.";
                return;
            }
            if (cmbGroup.SelectedValue == null)
            {
                lblError.Text = "Please select an interest group.";
                return;
            }

            float weight;
            if (!float.TryParse(txtWeight.Text.Trim(), out weight))
            {
                lblError.Text = "Weight must be a number (e.g. 1.50).";
                return;
            }
            if (weight <= 0)
            {
                lblError.Text = "Weight must be greater than 0.";
                return;
            }

            // 2. Build the entity
            InterestQuery q = new InterestQuery();
            q.InterestGroupID = (int)cmbGroup.SelectedValue;
            q.QueryText = txtQuestion.Text.Trim();
            q.Weight = weight;
            q.IsActive = true;

            // 3. Save it
            InterestQueryDA da = new InterestQueryDA();
            da.AddQuery(q);

            // 4. Confirm and close
            MessageBox.Show("Question added successfully.", "Success");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
