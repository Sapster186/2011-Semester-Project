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
            this.titleLbl.Location = new System.Drawing.Point(295, 78);
            this.titleLbl.Name = "titleLbl";
            this.titleLbl.Size = new System.Drawing.Size(151, 16);
            this.titleLbl.TabIndex = 0;
            this.titleLbl.Text = "Alternative Programmes";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(322, 365);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // altListView
            // 
            this.altListView.HideSelection = false;
            this.altListView.Location = new System.Drawing.Point(88, 123);
            this.altListView.Name = "altListView";
            this.altListView.Size = new System.Drawing.Size(578, 219);
            this.altListView.TabIndex = 3;
            this.altListView.UseCompatibleStateImageBehavior = false;
            // 
            // AlternativeProgramsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.altListView);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.titleLbl);
            this.Name = "AlternativeProgramsForm";
            this.Text = "AlternativeProgramsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLbl;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ListView altListView;
    }
}