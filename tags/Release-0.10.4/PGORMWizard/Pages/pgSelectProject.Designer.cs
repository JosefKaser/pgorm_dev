/*-------------------------------------------------------------------------
 * pgSelectProject.Designer.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectProject = new TrueSoftware.Framework.Wizard.SelectFileControl();
            this.MainGroupBox.SuspendLayout();
            this.panelTextHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.selectProject);
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
            // openFileDialog
            // 
            this.openFileDialog.Filter = "PGORM file|*.pgorm";
            this.openFileDialog.Title = "Select a PGORM project";
            // 
            // selectProject
            // 
            this.selectProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectProject.Caption = "Name:";
            this.selectProject.Filter = "PGORM Files|*.pgorm";
            this.selectProject.InitialValue = "";
            this.selectProject.Location = new System.Drawing.Point(12, 109);
            this.selectProject.Name = "selectProject";
            this.selectProject.Padding = new System.Windows.Forms.Padding(2);
            this.selectProject.SelectedValue = "";
            this.selectProject.SelectionMode = TrueSoftware.Framework.Wizard.SelectFileMode.SelectFile;
            this.selectProject.Size = new System.Drawing.Size(478, 48);
            this.selectProject.TabIndex = 2;
            this.selectProject.ValueLocked = true;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton optNewProject;
        private System.Windows.Forms.RadioButton optExisting;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private TrueSoftware.Framework.Wizard.SelectFileControl selectProject;
    }
}