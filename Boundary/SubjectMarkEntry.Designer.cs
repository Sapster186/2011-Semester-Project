namespace _2011__Semester_Project.Boundary
{
    partial class SubjectMarkEntry
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.dgvSubjects = new System.Windows.Forms.DataGridView();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.database1DataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.database1DataSet = new _2011__Semester_Project.Database1DataSet();
            this.txtSchoolName = new System.Windows.Forms.Label();
            this.txtMark = new System.Windows.Forms.TextBox();
            this.btnAddSubject = new System.Windows.Forms.Button();
            this.cmbSubject = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.database1DataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.database1DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(383, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mark:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(12, 46);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(67, 20);
            this.lblSubject.TabIndex = 1;
            this.lblSubject.Text = "Subject:";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(240, 328);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(256, 20);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "Error in your input, please try again!";
            this.lblError.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(615, 393);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(99, 32);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // dgvSubjects
            // 
            this.dgvSubjects.AutoGenerateColumns = false;
            this.dgvSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Subject,
            this.Mark});
            this.dgvSubjects.DataSource = this.database1DataSetBindingSource;
            this.dgvSubjects.Location = new System.Drawing.Point(12, 72);
            this.dgvSubjects.Name = "dgvSubjects";
            this.dgvSubjects.RowHeadersWidth = 62;
            this.dgvSubjects.RowTemplate.Height = 28;
            this.dgvSubjects.Size = new System.Drawing.Size(776, 224);
            this.dgvSubjects.TabIndex = 7;
            this.dgvSubjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Subject
            // 
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 8;
            this.Subject.Name = "Subject";
            this.Subject.Width = 150;
            // 
            // Mark
            // 
            this.Mark.HeaderText = "Mark";
            this.Mark.MinimumWidth = 8;
            this.Mark.Name = "Mark";
            this.Mark.Width = 150;
            // 
            // database1DataSetBindingSource
            // 
            this.database1DataSetBindingSource.DataSource = this.database1DataSet;
            this.database1DataSetBindingSource.Position = 0;
            this.database1DataSetBindingSource.CurrentChanged += new System.EventHandler(this.database1DataSetBindingSource_CurrentChanged);
            // 
            // database1DataSet
            // 
            this.database1DataSet.DataSetName = "Database1DataSet";
            this.database1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txtSchoolName
            // 
            this.txtSchoolName.AutoSize = true;
            this.txtSchoolName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSchoolName.Location = new System.Drawing.Point(363, 9);
            this.txtSchoolName.Name = "txtSchoolName";
            this.txtSchoolName.Size = new System.Drawing.Size(207, 20);
            this.txtSchoolName.TabIndex = 8;
            this.txtSchoolName.Text = "Mark and Subjects Table";
            // 
            // txtMark
            // 
            this.txtMark.Location = new System.Drawing.Point(528, 40);
            this.txtMark.MaxLength = 3;
            this.txtMark.Name = "txtMark";
            this.txtMark.Size = new System.Drawing.Size(100, 26);
            this.txtMark.TabIndex = 10;
            // 
            // btnAddSubject
            // 
            this.btnAddSubject.Enabled = false;
            this.btnAddSubject.Location = new System.Drawing.Point(45, 393);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(125, 32);
            this.btnAddSubject.TabIndex = 11;
            this.btnAddSubject.Text = "Add Subject";
            this.btnAddSubject.UseVisualStyleBackColor = true;
            // 
            // cmbSubject
            // 
            this.cmbSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubject.FormattingEnabled = true;
            this.cmbSubject.Location = new System.Drawing.Point(157, 38);
            this.cmbSubject.Name = "cmbSubject";
            this.cmbSubject.Size = new System.Drawing.Size(121, 28);
            this.cmbSubject.TabIndex = 12;
            this.cmbSubject.SelectedIndexChanged += new System.EventHandler(this.cmbSubject_SelectedIndexChanged);
            // 
            // SubjectMarkEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cmbSubject);
            this.Controls.Add(this.btnAddSubject);
            this.Controls.Add(this.txtMark);
            this.Controls.Add(this.txtSchoolName);
            this.Controls.Add(this.dgvSubjects);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblSubject);
            this.Controls.Add(this.label1);
            this.Name = "SubjectMarkEntry";
            this.Text = "SubjectMarkEntry";
            this.Load += new System.EventHandler(this.SubjectMarkEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.database1DataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.database1DataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridView dgvSubjects;
        private System.Windows.Forms.BindingSource database1DataSetBindingSource;
        private Database1DataSet database1DataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mark;
        private System.Windows.Forms.Label txtSchoolName;
        private System.Windows.Forms.TextBox txtMark;
        private System.Windows.Forms.Button btnAddSubject;
        private System.Windows.Forms.ComboBox cmbSubject;
    }
}