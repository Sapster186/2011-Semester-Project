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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();

            // Basic validation check
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {

                lblErrorMsg.Text = "Invalid Username or Password";
                lblErrorMsg.Visible = true;
            }
            else
            {
                // Hide error message on successful input
                lblErrorMsg.Visible = false;

                // TODO: Call your Controller layer here to authenticate against the database
                MessageBox.Show($"Successfully authenticated as {role}.", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Example of how you will route to the next form:
                // DashboardForm dashboard = new DashboardForm(role);
                // dashboard.Show();
                // this.Hide();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }
    }
}