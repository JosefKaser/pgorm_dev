namespace PGORMWizard.Pages
{
    partial class pgProjectOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRootNS = new System.Windows.Forms.TextBox();
            this.folderCompilerOutput = new TrueSoftware.Framework.Wizard.SelectFileControl();
            this.chkSaveSources = new System.Windows.Forms.CheckBox();
            this.MainGroupBox.SuspendLayout();
            this.panelTextHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.chkSaveSources);
            this.MainGroupBox.Controls.Add(this.folderCompilerOutput);
            this.MainGroupBox.Controls.Add(this.txtRootNS);
            this.MainGroupBox.Controls.Add(this.label1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Root Namespace:";
            // 
            // txtRootNS
            // 
            this.txtRootNS.Location = new System.Drawing.Point(15, 38);
            this.txtRootNS.Name = "txtRootNS";
            this.txtRootNS.Size = new System.Drawing.Size(291, 21);
            this.txtRootNS.TabIndex = 1;
            // 
            // folderCompilerOutput
            // 
            this.folderCompilerOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.folderCompilerOutput.Caption = "Compiler Output Folder:";
            this.folderCompilerOutput.Filter = "";
            this.folderCompilerOutput.InitialValue = "";
            this.folderCompilerOutput.Location = new System.Drawing.Point(12, 65);
            this.folderCompilerOutput.Name = "folderCompilerOutput";
            this.folderCompilerOutput.Padding = new System.Windows.Forms.Padding(2);
            this.folderCompilerOutput.SelectedValue = "";
            this.folderCompilerOutput.SelectionMode = TrueSoftware.Framework.Wizard.SelectFileMode.SelectFolder;
            this.folderCompilerOutput.Size = new System.Drawing.Size(329, 48);
            this.folderCompilerOutput.TabIndex = 2;
            this.folderCompilerOutput.ValueLocked = true;
            // 
            // chkSaveSources
            // 
            this.chkSaveSources.AutoSize = true;
            this.chkSaveSources.Location = new System.Drawing.Point(15, 119);
            this.chkSaveSources.Name = "chkSaveSources";
            this.chkSaveSources.Size = new System.Drawing.Size(148, 17);
            this.chkSaveSources.TabIndex = 3;
            this.chkSaveSources.Text = "&Save generated sources?";
            this.chkSaveSources.UseVisualStyleBackColor = true;
            // 
            // pgProjectOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "pgProjectOptions";
            this.MainGroupBox.ResumeLayout(false);
            this.MainGroupBox.PerformLayout();
            this.panelTextHolder.ResumeLayout(false);
            this.panelTextHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRootNS;
        private TrueSoftware.Framework.Wizard.SelectFileControl folderCompilerOutput;
        private System.Windows.Forms.CheckBox chkSaveSources;

    }
}
