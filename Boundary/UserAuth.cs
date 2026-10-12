using _2011_Semester_Project.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2011__Semester_Project.Boundary
{
    public partial class UserAuth : Form
    {
        public UserAuth()
        {
            InitializeComponent();
        }

        private void UserAuth_Load(object sender, EventArgs e)
        {
            // Hide the error message by default when the form loads
            lblErrorMsg.Visible = false;

            // Populate the Role dropdown if not already set in the Designer properties
            if (cmbRole.Items.Count == 0)
            {
                cmbRole.Items.Add("Learner");
                cmbRole.Items.Add("Career Advisor");
                cmbRole.Items.Add("Administrator");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. Read the inputs from the form.
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem == null ? "" : cmbRole.SelectedItem.ToString();

            // 2. Basic form-level validation first.
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                lblErrorMsg.Text = "Please fill in all fields.";
                lblErrorMsg.Visible = true;
                return;
            }

            // 3. Ask the LoginController to verify.
            LoginController controller = new LoginController();
            bool success = controller.Login(username, password, role);

            // 4. If wrong, show the error and stop.
            if (!success)
            {
                lblErrorMsg.Text = "Invalid username or password.";
                lblErrorMsg.Visible = true;
                return;
            }

            // 5. Success — open the MainWindow and hide this form.
            lblErrorMsg.Visible = false;
            MainWindow main = new MainWindow();  
            main.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }
    }
}