using System;
using System.Windows.Forms;
using _2011__Semester_Project.Controller;

namespace _2011__Semester_Project.Boundary
{
    public partial class UserAuth : Form
    {
        public UserAuth()
        {
            InitializeComponent();
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.SelectedIndexChanged += UpdateLoginInputs;
            txtUsername.TextChanged += UpdateLoginInputs;
            txtPassword.TextChanged += UpdateLoginInputs;
            btnExit.Click += btnExit_Click;
            AcceptButton = btnLogin;
        }

        private void UserAuth_Load(object sender, EventArgs e)
        {
            lblErrorMsg.Visible = false;
            UpdateLoginInputs(sender, e);
        }

        private void UpdateLoginInputs(object sender, EventArgs e)
        {
            bool learner = cmbRole.Text == "Learner";
            txtUsername.Enabled = !learner;
            txtPassword.Enabled = !learner;
            btnLogin.Text = learner ? "Continue" : "Login";
            btnLogin.AutoSize = true;
            btnLogin.Enabled = cmbRole.SelectedIndex >= 0 && (learner ||
                (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text)));
            lblErrorMsg.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string role = cmbRole.SelectedItem == null ? "" : cmbRole.SelectedItem.ToString();
            if (role == "")
            {
                ShowError("Please select a role.");
                return;
            }
            try
            {
                // Do not trim passwords: spaces may be part of a valid password.
                if (!new LoginController().Login(txtUsername.Text, txtPassword.Text, role))
                {
                    ShowError("Invalid username or password.");
                    return;
                }
                MainWindow main = new MainWindow();
                main.FormClosed += MainWindowClosed;
                main.Show();
                Hide();
            }
            catch (Exception)
            {
                new LoginController().Logout();
                ShowError("Could not connect to the database. Please contact the administrator.");
            }
        }

        private void MainWindowClosed(object sender, FormClosedEventArgs e)
        {
            if (IsDisposed || Disposing) return;
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            Show();
            Activate();
        }

        private void ShowError(string message)
        {
            lblErrorMsg.Text = message;
            lblErrorMsg.Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
