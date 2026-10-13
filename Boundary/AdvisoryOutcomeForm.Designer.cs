namespace _2011__Semester_Project.Boundary
{
    partial class AdvisoryOutcomeForm
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
            this.outcomeLbl = new System.Windows.Forms.Label();
            this.scoreLbl = new System.Windows.Forms.Label();
            this.gapLbl = new System.Windows.Forms.Label();
            this.alternativesButton = new System.Windows.Forms.Button();
            this.fundingButton = new System.Windows.Forms.Button();
            this.summaryButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outcomeLbl
            // 
            this.outcomeLbl.AutoSize = true;
            this.outcomeLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outcomeLbl.Location = new System.Drawing.Point(151, 92);
            this.outcomeLbl.Name = "outcomeLbl";
            this.outcomeLbl.Size = new System.Drawing.Size(85, 29);
            this.outcomeLbl.TabIndex = 0;
            this.outcomeLbl.Text = "label1";
            // 
            // scoreLbl
            // 
            this.scoreLbl.AutoSize = true;
            this.scoreLbl.Location = new System.Drawing.Point(151, 162);
            this.scoreLbl.Name = "scoreLbl";
            this.scoreLbl.Size = new System.Drawing.Size(44, 16);
            this.scoreLbl.TabIndex = 1;
            this.scoreLbl.Text = "label2";
            // 
            // gapLbl
            // 
            this.gapLbl.AutoSize = true;
            this.gapLbl.Location = new System.Drawing.Point(151, 234);
            this.gapLbl.Name = "gapLbl";
            this.gapLbl.Size = new System.Drawing.Size(44, 16);
            this.gapLbl.TabIndex = 2;
            this.gapLbl.Text = "label3";
            // 
            // alternativesButton
            // 
            this.alternativesButton.Location = new System.Drawing.Point(160, 344);
            this.alternativesButton.Name = "alternativesButton";
            this.alternativesButton.Size = new System.Drawing.Size(92, 35);
            this.alternativesButton.TabIndex = 3;
            this.alternativesButton.Text = "Alternatives";
            this.alternativesButton.UseVisualStyleBackColor = true;
            this.alternativesButton.Click += new System.EventHandler(this.alternativesButton_Click);
            // 
            // fundingButton
            // 
            this.fundingButton.Location = new System.Drawing.Point(321, 344);
            this.fundingButton.Name = "fundingButton";
            this.fundingButton.Size = new System.Drawing.Size(83, 35);
            this.fundingButton.TabIndex = 4;
            this.fundingButton.Text = "Funding";
            this.fundingButton.UseVisualStyleBackColor = true;
            this.fundingButton.Click += new System.EventHandler(this.fundingButton_Click);
            // 
            // summaryButton
            // 
            this.summaryButton.Location = new System.Drawing.Point(478, 344);
            this.summaryButton.Name = "summaryButton";
            this.summaryButton.Size = new System.Drawing.Size(89, 35);
            this.summaryButton.TabIndex = 5;
            this.summaryButton.Text = "Summary";
            this.summaryButton.UseVisualStyleBackColor = true;
            this.summaryButton.Click += new System.EventHandler(this.summaryButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(644, 344);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(78, 35);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // AdvisoryOutcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.summaryButton);
            this.Controls.Add(this.fundingButton);
            this.Controls.Add(this.alternativesButton);
            this.Controls.Add(this.gapLbl);
            this.Controls.Add(this.scoreLbl);
            this.Controls.Add(this.outcomeLbl);
            this.Name = "AdvisoryOutcomeForm";
            this.Text = "AdvisoryOutcomeForm";
            this.Load += new System.EventHandler(this.AdvisoryOutcomeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label outcomeLbl;
        private System.Windows.Forms.Label scoreLbl;
        private System.Windows.Forms.Label gapLbl;
        private System.Windows.Forms.Button alternativesButton;
        private System.Windows.Forms.Button fundingButton;
        private System.Windows.Forms.Button summaryButton;
        private System.Windows.Forms.Button closeButton;
    }
}