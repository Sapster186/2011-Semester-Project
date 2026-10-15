using System;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Boundary
{
    public partial class AddInterestQueryForm : Form
    {
        private readonly InterestQuery editingQuery;
        private readonly AdminController controller = new AdminController();

        public AddInterestQueryForm() : this(null) { }
        public AddInterestQueryForm(InterestQuery existing)
        {
            InitializeComponent();
            editingQuery = existing;
            cmbGroup.DropDownStyle = ComboBoxStyle.DropDownList;
            txtQuestion.MaxLength = 200;
        }

        private void AddInterestQueryForm_Load(object sender, EventArgs e)
        {
            try
            {
                cmbGroup.DisplayMember = "GroupName";
                cmbGroup.ValueMember = "InterestGroupID";
                cmbGroup.DataSource = controller.GetInterestGroups();
                if (editingQuery != null)
                {
                    Text = "Edit Interest Question";
                    txtQuestion.Text = editingQuery.QueryText;
                    txtWeight.Text = editingQuery.Weight.ToString();
                    cmbGroup.SelectedValue = editingQuery.InterestGroupID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not load interest groups");
                Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            float weight;
            if (cmbGroup.SelectedValue == null)
            {
                lblError.Text = "Please select an interest group.";
                return;
            }
            if (!float.TryParse(txtWeight.Text, out weight))
            {
                lblError.Text = "Weight must be a number (e.g. 1.50).";
                return;
            }
            // A separate object keeps Cancel and failed saves from changing the grid.
            InterestQuery question = new InterestQuery(
                editingQuery == null ? 0 : editingQuery.QueryID,
                (int)cmbGroup.SelectedValue, txtQuestion.Text, weight,
                editingQuery == null || editingQuery.IsActive);
            string error = controller.SaveInterestQuery(question);
            if (error != null) { lblError.Text = error; return; }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
