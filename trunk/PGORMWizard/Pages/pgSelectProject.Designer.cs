namespace PGORMWizard.Pages
{
    partial class pgSelectProject
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.optNewProject = new System.Windows.Forms.RadioButton();
            this.optExisting = new System.Windows.Forms.RadioButton();
            this.pnlSelect = new System.Windows.Forms.Panel();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainGroupBox.SuspendLayout();
            this.panelTextHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.pnlSelect);
            this.MainGroupBox.Controls.Add(this.optExisting);
            this.MainGroupBox.Controls.Add(this.optNewProject);
            // 
            // optNewProject
            // 
            this.optNewProject.AutoSize = true;
            this.optNewProject.Checked = true;
            this.optNewProject.Location = new System.Drawing.Point(138, 54);
            this.optNewProject.Name = "optNewProject";
            this.optNewProject.Size = new System.Drawing.Size(168, 17);
            this.optNewProject.TabIndex = 0;
            this.optNewProject.TabStop = true;
            this.optNewProject.Text = "New code generation project.";
            this.optNewProject.UseVisualStyleBackColor = true;
            // 
            // optExisting
            // 
            this.optExisting.AutoSize = true;
            this.optExisting.Location = new System.Drawing.Point(138, 86);
            this.optExisting.Name = "optExisting";
            this.optExisting.Size = new System.Drawing.Size(99, 17);
            this.optExisting.TabIndex = 1;
            this.optExisting.Text = "Existing project";
            this.optExisting.UseVisualStyleBackColor = true;
            // 
            // pnlSelect
            // 
            this.pnlSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelect.Controls.Add(this.label1);
            this.pnlSelect.Controls.Add(this.txtFile);
            this.pnlSelect.Controls.Add(this.btnSelectFile);
            this.pnlSelect.Location = new System.Drawing.Point(12, 121);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Size = new System.Drawing.Size(471, 68);
            this.pnlSelect.TabIndex = 2;
            this.pnlSelect.Visible = false;
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(12, 26);
            this.txtFile.Multiline = true;
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(356, 21);
            this.txtFile.TabIndex = 1;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(374, 24);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(92, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "&Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Project file:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "PGORM file|*.pgorm";
            this.openFileDialog.Title = "Select a PGORM project";
            // 
            // pgSelectProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "pgSelectProject";
            this.MainGroupBox.ResumeLayout(false);
            this.MainGroupBox.PerformLayout();
            this.panelTextHolder.ResumeLayout(false);
            this.panelTextHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlSelect.ResumeLayout(false);
            this.pnlSelect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton optNewProject;
        private System.Windows.Forms.RadioButton optExisting;
        private System.Windows.Forms.Panel pnlSelect;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
