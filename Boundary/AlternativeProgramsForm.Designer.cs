namespace _2011__Semester_Project.Boundary
{
    partial class AlternativeProgramsForm
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
            this.titleLbl = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.altListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // titleLbl
            // 
            this.titleLbl.AutoSize = true;
            this.titleLbl.Location = new System.Drawing.Point(332, 98);
            this.titleLbl.Name = "titleLbl";
            this.titleLbl.Size = new System.Drawing.Size(178, 20);
            this.titleLbl.TabIndex = 0;
            this.titleLbl.Text = "Alternative Programmes";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(362, 456);
            this.closeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(84, 29);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // altListView
            // 
            this.altListView.HideSelection = false;
            this.altListView.Location = new System.Drawing.Point(99, 154);
            this.altListView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.altListView.Name = "altListView";
            this.altListView.Size = new System.Drawing.Size(650, 273);
            this.altListView.TabIndex = 3;
            this.altListView.UseCompatibleStateImageBehavior = false;
            // 
            // AlternativeProgramsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.altListView);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.titleLbl);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AlternativeProgramsForm";
            this.Text = "AlternativeProgramsForm";
            this.Load += new System.EventHandler(this.AlternativeProgramsForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLbl;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ListView altListView;
    }
}