using System;
using System.Drawing;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Boundary
{
    public partial class ReferenceDataMaintenanceForm : Form
    {
        private readonly AdminController controller = new AdminController();
        private string DataType { get { return cmbDataType.Text; } }

        public ReferenceDataMaintenanceForm()
        {
            InitializeComponent();
            dgvRecords.ReadOnly = true;
            dgvRecords.AllowUserToAddRows = false;
            dgvRecords.AllowUserToDeleteRows = false;
            dgvRecords.MultiSelect = false;
            dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ReferenceDataMaintenanceForm_Load(object sender, EventArgs e)
        {
            if (!CheckAccess()) { Close(); return; }
            cmbDataType.Items.Clear();
            cmbDataType.Items.AddRange(new object[] { "Degree Programme", "Degree Requirements", "Funding Option", "Interest Questionnaire" });
            cmbDataType.Width = 240;
            cmbDataType.SelectedIndex = 0;
        }

        private bool CheckAccess()
        {
            string error = controller.CheckAdminAccess();
            if (error == null) return true;
            ShowStatus(error, true);
            btnAdd.Enabled = btnEdit.Enabled = btnDeactivate.Enabled = false;
            return false;
        }

        private void ShowStatus(string text, bool error)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = error ? Color.Red : Color.DarkGreen;
        }

        private bool LoadGrid()
        {
            dgvRecords.DataSource = null;
            if (!CheckAccess()) return false;
            bool editable = DataType == "Funding Option" || DataType == "Interest Questionnaire";
            btnAdd.Enabled = btnEdit.Enabled = btnDeactivate.Enabled = editable;
            try
            {
                if (DataType == "Degree Programme")
                {
                    dgvRecords.DataSource = controller.GetDegreeProgrammes();
                    ShowStatus("View only: degree save/deactivate methods are not available yet.", true);
                }
                else if (DataType == "Degree Requirements")
                {
                    dgvRecords.DataSource = controller.GetDegreeRequirements();
                    ShowStatus("View only: requirement save methods are not available yet.", true);
                }
                else if (DataType == "Funding Option")
                {
                    dgvRecords.DataSource = controller.GetFundingOptions();
                    ShowStatus("Showing active funding with a future closing date.", false);
                }
                else
                {
                    dgvRecords.DataSource = controller.GetInterestQueries();
                    ShowStatus("Showing active and inactive questions.", false);
                }
                dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                return true;
            }
            catch (Exception ex)
            {
                btnAdd.Enabled = btnEdit.Enabled = btnDeactivate.Enabled = false;
                ShowStatus("Could not load records.", true);
                MessageBox.Show(ex.Message, "Could not load records");
                return false;
            }
        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e) { LoadGrid(); }
        private void btnRefresh_Click(object sender, EventArgs e) { LoadGrid(); }
        private void btnAdd_Click(object sender, EventArgs e) { EditRecord(false); }
        private void btnEdit_Click(object sender, EventArgs e) { EditRecord(true); }

        private void EditRecord(bool edit)
        {
            if (!CheckAccess()) return;
            object selected = dgvRecords.CurrentRow == null ? null : dgvRecords.CurrentRow.DataBoundItem;
            if (edit && selected == null) { MessageBox.Show("Please select a row first."); return; }
            bool saved = false;
            if (DataType == "Interest Questionnaire")
            {
                using (AddInterestQueryForm dialog = new AddInterestQueryForm(edit ? (InterestQuery)selected : null))
                    saved = dialog.ShowDialog(this) == DialogResult.OK;
            }
            else if (DataType == "Funding Option")
            {
                try { saved = EditFundingOption(edit ? (FundOption)selected : null); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Could not open funding editor"); }
            }
            if (saved)
            {
                MessageBox.Show(edit ? "Record updated." : "Record added.", "Saved");
                LoadGrid();
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (!CheckAccess()) return;
            object selected = dgvRecords.CurrentRow == null ? null : dgvRecords.CurrentRow.DataBoundItem;
            if (selected == null) { MessageBox.Show("Please select a row first."); return; }
            if (!(selected is InterestQuery) && !(selected is FundOption)) return;
            if (MessageBox.Show("Are you sure you want to deactivate this item?", "Confirm Deactivate",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            string error = selected is InterestQuery
                ? controller.DeactivateInterestQuery(((InterestQuery)selected).QueryID)
                : controller.DeactivateFundingOption(((FundOption)selected).FundID);
            if (error != null) { MessageBox.Show(error, "Could not deactivate"); return; }
            MessageBox.Show("Record deactivated.", "Saved");
            LoadGrid();
        }

        // Small funding dialog built here so no changes to Member D's forms are needed.
        private bool EditFundingOption(FundOption existing)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = existing == null ? "Add Funding Option" : "Edit Funding Option";
                dialog.ClientSize = new Size(580, 410);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = dialog.MinimizeBox = false;

                string[] labels = { "Funding name", "Eligibility", "Documents needed", "Contact details" };
                string[] values = existing == null ? new string[4] : new string[] {
                    existing.FundName, existing.EligibleSummary, existing.NecessaryDocs, existing.ContactInformation };
                TextBox[] inputs = new TextBox[4];
                for (int i = 0; i < inputs.Length; i++)
                {
                    dialog.Controls.Add(new Label { Text = labels[i], Left = 15, Top = 23 + i * 45, Width = 140 });
                    inputs[i] = new TextBox { Left = 165, Top = 20 + i * 45, Width = 390,
                        MaxLength = i == 0 ? 100 : 200, Text = values[i] ?? "" };
                    dialog.Controls.Add(inputs[i]);
                }
                ComboBox type = new ComboBox { Left = 165, Top = 200, Width = 390,
                    DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "fundType", ValueMember = "fundType" };
                type.DataSource = controller.GetFundingTypes();
                if (existing != null) type.SelectedValue = existing.FundType;
                dialog.Controls.Add(new Label { Text = "Funding type", Left = 15, Top = 203, Width = 140 });
                dialog.Controls.Add(type);

                DateTimePicker closing = new DateTimePicker { Left = 165, Top = 245, Width = 390,
                    Format = DateTimePickerFormat.Short, MinDate = new DateTime(1753, 1, 1) };
                closing.Value = existing == null ? DateTime.Today.AddMonths(1) : existing.CloseDate;
                dialog.Controls.Add(new Label { Text = "Closing date", Left = 15, Top = 248, Width = 140 });
                dialog.Controls.Add(closing);
                CheckBox active = new CheckBox { Text = "Active", Left = 165, Top = 285, Checked = existing == null || existing.IsActive };
                dialog.Controls.Add(active);
                Button save = new Button { Text = "Save", Left = 365, Top = 345, Width = 90 };
                Button cancel = new Button { Text = "Cancel", Left = 465, Top = 345, Width = 90, DialogResult = DialogResult.Cancel };
                dialog.Controls.Add(save);
                dialog.Controls.Add(cancel);
                dialog.AcceptButton = save;
                dialog.CancelButton = cancel;
                save.Click += delegate
                {
                    FundOption option = new FundOption {
                        FundID = existing == null ? 0 : existing.FundID,
                        FundName = inputs[0].Text, EligibleSummary = inputs[1].Text,
                        NecessaryDocs = inputs[2].Text, ContactInformation = inputs[3].Text,
                        FundType = type.SelectedValue == null ? "" : type.SelectedValue.ToString(),
                        CloseDate = closing.Value.Date.AddDays(1).AddSeconds(-1), IsActive = active.Checked };
                    string error = controller.SaveFundingOption(option);
                    if (error != null) { MessageBox.Show(error, "Check funding details"); return; }
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                };
                return dialog.ShowDialog(this) == DialogResult.OK;
            }
        }
    }
}
