namespace _2011__Semester_Project.Boundary
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLearner = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLearnerProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInterestQuestionnaire = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdvisor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCareerAdvisorDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdminDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMaintainReferenceData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReportSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.checkEligibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuLearner,
            this.mnuAdvisor,
            this.mnuAdmin,
            this.mnuReports,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1047, 30);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogout,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(46, 26);
            this.mnuFile.Text = "File";
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(224, 26);
            this.mnuLogout.Text = "Logout";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(224, 26);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuLearner
            // 
            this.mnuLearner.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLearnerProfile,
            this.mnuInterestQuestionnaire,
            this.checkEligibilityToolStripMenuItem});
            this.mnuLearner.Name = "mnuLearner";
            this.mnuLearner.Size = new System.Drawing.Size(72, 26);
            this.mnuLearner.Text = "Learner";
            // 
            // mnuLearnerProfile
            // 
            this.mnuLearnerProfile.Name = "mnuLearnerProfile";
            this.mnuLearnerProfile.Size = new System.Drawing.Size(233, 26);
            this.mnuLearnerProfile.Text = "New Learner Profile";
            this.mnuLearnerProfile.Click += new System.EventHandler(this.mnuLearnerProfile_Click);
            // 
            // mnuInterestQuestionnaire
            // 
            this.mnuInterestQuestionnaire.Name = "mnuInterestQuestionnaire";
            this.mnuInterestQuestionnaire.Size = new System.Drawing.Size(233, 26);
            this.mnuInterestQuestionnaire.Text = "Interest Questonnaire";
            this.mnuInterestQuestionnaire.Click += new System.EventHandler(this.mnuInterestQuestionnaire_Click);
            // 
            // mnuAdvisor
            // 
            this.mnuAdvisor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCareerAdvisorDashboard});
            this.mnuAdvisor.Name = "mnuAdvisor";
            this.mnuAdvisor.Size = new System.Drawing.Size(73, 26);
            this.mnuAdvisor.Text = "Advisor";
            // 
            // mnuCareerAdvisorDashboard
            // 
            this.mnuCareerAdvisorDashboard.Name = "mnuCareerAdvisorDashboard";
            this.mnuCareerAdvisorDashboard.Size = new System.Drawing.Size(266, 26);
            this.mnuCareerAdvisorDashboard.Text = "Career Advisor Dashboard";
            this.mnuCareerAdvisorDashboard.Click += new System.EventHandler(this.mnuCareerAdvisorDashboard_Click);
            // 
            // mnuAdmin
            // 
            this.mnuAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAdminDashboard,
            this.mnuMaintainReferenceData});
            this.mnuAdmin.Name = "mnuAdmin";
            this.mnuAdmin.Size = new System.Drawing.Size(67, 26);
            this.mnuAdmin.Text = "Admin";
            // 
            // mnuAdminDashboard
            // 
            this.mnuAdminDashboard.Name = "mnuAdminDashboard";
            this.mnuAdminDashboard.Size = new System.Drawing.Size(256, 26);
            this.mnuAdminDashboard.Text = "Admin Dashboard";
            this.mnuAdminDashboard.Click += new System.EventHandler(this.mnuAdminDashboard_Click);
            // 
            // mnuMaintainReferenceData
            // 
            this.mnuMaintainReferenceData.Name = "mnuMaintainReferenceData";
            this.mnuMaintainReferenceData.Size = new System.Drawing.Size(256, 26);
            this.mnuMaintainReferenceData.Text = "Maintain Reference Data";
            this.mnuMaintainReferenceData.Click += new System.EventHandler(this.mnuMaintainReferenceData_Click);
            // 
            // mnuReports
            // 
            this.mnuReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReportSelection});
            this.mnuReports.Name = "mnuReports";
            this.mnuReports.Size = new System.Drawing.Size(74, 26);
            this.mnuReports.Text = "Reports";
            // 
            // mnuReportSelection
            // 
            this.mnuReportSelection.Name = "mnuReportSelection";
            this.mnuReportSelection.Size = new System.Drawing.Size(224, 26);
            this.mnuReportSelection.Text = "Report Selection";
            this.mnuReportSelection.Click += new System.EventHandler(this.mnuReportSelection_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(55, 26);
            this.mnuHelp.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(224, 26);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // checkEligibilityToolStripMenuItem
            // 
            this.checkEligibilityToolStripMenuItem.Name = "checkEligibilityToolStripMenuItem";
            this.checkEligibilityToolStripMenuItem.Size = new System.Drawing.Size(233, 26);
            this.checkEligibilityToolStripMenuItem.Text = "Check Eligibility";
            this.checkEligibilityToolStripMenuItem.Click += new System.EventHandler(this.checkEligibilityToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 596);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Future Path";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuLearner;
        private System.Windows.Forms.ToolStripMenuItem mnuAdvisor;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuLearnerProfile;
        private System.Windows.Forms.ToolStripMenuItem mnuInterestQuestionnaire;
        private System.Windows.Forms.ToolStripMenuItem mnuCareerAdvisorDashboard;
        private System.Windows.Forms.ToolStripMenuItem mnuAdmin;
        private System.Windows.Forms.ToolStripMenuItem mnuAdminDashboard;
        private System.Windows.Forms.ToolStripMenuItem mnuMaintainReferenceData;
        private System.Windows.Forms.ToolStripMenuItem mnuReports;
        private System.Windows.Forms.ToolStripMenuItem mnuReportSelection;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem checkEligibilityToolStripMenuItem;
    }
}