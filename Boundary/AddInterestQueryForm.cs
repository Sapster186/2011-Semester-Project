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
        private InterestQuery editingQuery = null;
        public AddInterestQueryForm(InterestQuery existing)
        {
            InitializeComponent();
            editingQuery = existing;
        }
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

            // If editing, pre-fill the fields
            if (editingQuery != null)
            {
                this.Text = "Edit Interest Question";
                txtQuestion.Text = editingQuery.QueryText;
                txtWeight.Text = editingQuery.Weight.ToString();
                cmbGroup.SelectedValue = editingQuery.InterestGroupID;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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

            InterestQueryDA da = new InterestQueryDA();

            if (editingQuery == null)
            {
                // Adding new
                InterestQuery q = new InterestQuery();
                q.InterestGroupID = (int)cmbGroup.SelectedValue;
                q.QueryText = txtQuestion.Text.Trim();
                q.Weight = weight;
                q.IsActive = true;
                da.AddQuery(q);
                MessageBox.Show("Question added.", "Success");
            }
            else
            {
                // Editing existing
                editingQuery.InterestGroupID = (int)cmbGroup.SelectedValue;
                editingQuery.QueryText = txtQuestion.Text.Trim();
                editingQuery.Weight = weight;
                da.UpdateQuery(editingQuery);
                MessageBox.Show("Question updated.", "Success");
            }

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
