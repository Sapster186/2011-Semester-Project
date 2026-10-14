using System;

namespace _2011__Semester_Project.Boundary
{
    partial class FacultyDegreeSelectionForm
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
            this.facultyLbl = new System.Windows.Forms.Label();
            this.degreeLbl = new System.Windows.Forms.Label();
            this.degreeComboBox = new System.Windows.Forms.ComboBox();
            this.facultyComboBox = new System.Windows.Forms.ComboBox();
            this.checkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // facultyLbl
            // 
            this.facultyLbl.AutoSize = true;
            this.facultyLbl.Location = new System.Drawing.Point(152, 62);
            this.facultyLbl.Name = "facultyLbl";
            this.facultyLbl.Size = new System.Drawing.Size(50, 16);
            this.facultyLbl.TabIndex = 0;
            this.facultyLbl.Text = "Faculty";
            // 
            // degreeLbl
            // 
            this.degreeLbl.AutoSize = true;
            this.degreeLbl.Location = new System.Drawing.Point(152, 102);
            this.degreeLbl.Name = "degreeLbl";
            this.degreeLbl.Size = new System.Drawing.Size(53, 16);
            this.degreeLbl.TabIndex = 1;
            this.degreeLbl.Text = "Degree";
            // 
            // degreeComboBox
            // 
            this.degreeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.degreeComboBox.FormattingEnabled = true;
            this.degreeComboBox.Location = new System.Drawing.Point(336, 102);
            this.degreeComboBox.Name = "degreeComboBox";
            this.degreeComboBox.Size = new System.Drawing.Size(121, 24);
            this.degreeComboBox.TabIndex = 2;
            this.degreeComboBox.SelectedIndexChanged += new System.EventHandler(this.degreeComboBox_SelectedIndexChanged);
            // 
            // facultyComboBox
            // 
            this.facultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.facultyComboBox.FormattingEnabled = true;
            this.facultyComboBox.Location = new System.Drawing.Point(336, 62);
            this.facultyComboBox.Name = "facultyComboBox";
            this.facultyComboBox.Size = new System.Drawing.Size(121, 24);
            this.facultyComboBox.TabIndex = 3;
            this.facultyComboBox.SelectedIndexChanged += new System.EventHandler(this.facultyComboBox_SelectedIndexChanged);
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(196, 261);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 4;
            this.checkButton.Text = "Check";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(366, 261);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // FacultyDegreeSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.facultyComboBox);
            this.Controls.Add(this.degreeComboBox);
            this.Controls.Add(this.degreeLbl);
            this.Controls.Add(this.facultyLbl);
            this.Name = "FacultyDegreeSelectionForm";
            this.Text = "FacultyDegreeSelectionForm";
            this.Load += new System.EventHandler(this.FacultyDegreeSelectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     

        #endregion

        private System.Windows.Forms.Label facultyLbl;
        private System.Windows.Forms.Label degreeLbl;
        private System.Windows.Forms.ComboBox degreeComboBox;
        private System.Windows.Forms.ComboBox facultyComboBox;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button cancelButton;
    }
}