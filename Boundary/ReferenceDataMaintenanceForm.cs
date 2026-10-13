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

            LoadGrid();
        }

        // Loads the grid based on what data type is selected in the combo.
        private void LoadGrid()
        {
            string dataType = cmbDataType.SelectedItem == null ? "" : cmbDataType.SelectedItem.ToString();

            if (dataType == "Interest Questionnaire")
            {
                InterestQueryDA da = new InterestQueryDA();
                dgvRecords.DataSource = null;
                dgvRecords.DataSource = da.GetAllQueries();
            }
            else if (dataType == "Funding Option")
            {
                FundOptionDA da = new FundOptionDA();
                dgvRecords.DataSource = null;
                dgvRecords.DataSource = da.GetActiveFundOptions(); // from Member D's DA
            }
            // Degree Programme and Degree Requirements tabs will be added once their DA classes exist

            // Auto-size columns so they fit the data
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
            lblStatus.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGrid();
            lblStatus.Text = "Refreshed.";
            lblStatus.ForeColor = System.Drawing.Color.Green;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string dataType = cmbDataType.SelectedItem == null ? "" : cmbDataType.SelectedItem.ToString();

            if (dataType == "Interest Questionnaire")
            {
                AddInterestQueryForm dialog = new AddInterestQueryForm();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadGrid();
                    lblStatus.Text = "Question added.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                MessageBox.Show("Add is not yet wired for: " + dataType);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            // 1. Check the user actually selected a row
            if (dgvRecords.CurrentRow == null)
            {
                MessageBox.Show("Please select a row first.", "Nothing selected");
                return;
            }

            string dataType = cmbDataType.SelectedItem == null ? "" : cmbDataType.SelectedItem.ToString();

            // 2. Ask for confirmation
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to deactivate this item? It will no longer appear in learner-facing screens.",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // 3. Deactivate based on the data type
                if (dataType == "Interest Questionnaire")
                {
                    InterestQuery selected = (InterestQuery)dgvRecords.CurrentRow.DataBoundItem;
                    InterestQueryDA da = new InterestQueryDA();
                    da.DeactivateQuery(selected.QueryID);
                }
                else if (dataType == "Funding Option")
                {
                    // Member D's DA has DeactivateFundOption — call it here
                    // FundOption selected = (FundOption)dgvRecords.CurrentRow.DataBoundItem;
                    // FundOptionDA da = new FundOptionDA();
                    // da.DeactivateFundOption(selected.FundID);
                    MessageBox.Show("Deactivate for Funding Option coming soon.");
                    return;
                }
                else
                {
                    MessageBox.Show("Deactivate not yet wired for: " + dataType);
                    return;
                }

                // 4. Refresh + confirm
                LoadGrid();
                lblStatus.Text = "Deactivated.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRecords.CurrentRow == null)
            {
                MessageBox.Show("Please select a row first.");
                return;
            }

            string dataType = cmbDataType.SelectedItem == null ? "" : cmbDataType.SelectedItem.ToString();

            if (dataType == "Interest Questionnaire")
            {
                InterestQuery selected = (InterestQuery)dgvRecords.CurrentRow.DataBoundItem;
                AddInterestQueryForm dialog = new AddInterestQueryForm(selected);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadGrid();
                    lblStatus.Text = "Question updated.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                MessageBox.Show("Edit not yet wired for: " + dataType);
            }
        }
    }
}
