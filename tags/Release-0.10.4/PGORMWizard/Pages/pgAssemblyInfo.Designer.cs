/*-------------------------------------------------------------------------
 * pgAssemblyInfo.Designer.cs
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
    partial class pgAssemblyInfo
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtConfiguration = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTrademark = new System.Windows.Forms.TextBox();
            this.txtCulture = new System.Windows.Forms.TextBox();
            this.txtGuid = new System.Windows.Forms.TextBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtFileVersion = new System.Windows.Forms.TextBox();
            this.selectKeyFile = new TrueSoftware.Framework.Wizard.SelectFileControl();
            this.chkSNK = new System.Windows.Forms.CheckBox();
            this.MainGroupBox.SuspendLayout();
            this.panelTextHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.chkSNK);
            this.MainGroupBox.Controls.Add(this.selectKeyFile);
            this.MainGroupBox.Controls.Add(this.tableLayoutPanel);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.txtTitle, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.txtDescription, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.txtConfiguration, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.txtCompany, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.txtProduct, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.txtCopyright, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.label7, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.label8, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.label9, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.label10, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.label11, 3, 4);
            this.tableLayoutPanel.Controls.Add(this.txtTrademark, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.txtCulture, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.txtGuid, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.txtVersion, 4, 3);
            this.tableLayoutPanel.Controls.Add(this.txtFileVersion, 4, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(490, 165);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Location = new System.Drawing.Point(81, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(144, 21);
            this.txtTitle.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 27);
            this.label3.TabIndex = 3;
            this.label3.Text = "Configuration";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 27);
            this.label4.TabIndex = 4;
            this.label4.Text = "Company";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 27);
            this.label5.TabIndex = 5;
            this.label5.Text = "Product";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 27);
            this.label6.TabIndex = 6;
            this.label6.Text = "Copyright";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(81, 30);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(144, 21);
            this.txtDescription.TabIndex = 7;
            // 
            // txtConfiguration
            // 
            this.txtConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfiguration.Location = new System.Drawing.Point(81, 57);
            this.txtConfiguration.Name = "txtConfiguration";
            this.txtConfiguration.Size = new System.Drawing.Size(144, 21);
            this.txtConfiguration.TabIndex = 8;
            // 
            // txtCompany
            // 
            this.txtCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCompany.Location = new System.Drawing.Point(81, 84);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(144, 21);
            this.txtCompany.TabIndex = 9;
            // 
            // txtProduct
            // 
            this.txtProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProduct.Location = new System.Drawing.Point(81, 111);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(144, 21);
            this.txtProduct.TabIndex = 10;
            // 
            // txtCopyright
            // 
            this.txtCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCopyright.Location = new System.Drawing.Point(81, 138);
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(144, 21);
            this.txtCopyright.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(231, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 27);
            this.label7.TabIndex = 12;
            this.label7.Text = "Trademark";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(231, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 27);
            this.label8.TabIndex = 13;
            this.label8.Text = "Culture";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(231, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 27);
            this.label9.TabIndex = 14;
            this.label9.Text = "Guid";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(231, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 27);
            this.label10.TabIndex = 15;
            this.label10.Text = "Version";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(231, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 27);
            this.label11.TabIndex = 16;
            this.label11.Text = "FileVersion";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTrademark
            // 
            this.txtTrademark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrademark.Location = new System.Drawing.Point(295, 3);
            this.txtTrademark.Name = "txtTrademark";
            this.txtTrademark.Size = new System.Drawing.Size(192, 21);
            this.txtTrademark.TabIndex = 17;
            // 
            // txtCulture
            // 
            this.txtCulture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCulture.Location = new System.Drawing.Point(295, 30);
            this.txtCulture.Name = "txtCulture";
            this.txtCulture.Size = new System.Drawing.Size(192, 21);
            this.txtCulture.TabIndex = 18;
            // 
            // txtGuid
            // 
            this.txtGuid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuid.Location = new System.Drawing.Point(295, 57);
            this.txtGuid.Name = "txtGuid";
            this.txtGuid.ReadOnly = true;
            this.txtGuid.Size = new System.Drawing.Size(192, 21);
            this.txtGuid.TabIndex = 19;
            // 
            // txtVersion
            // 
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.Location = new System.Drawing.Point(295, 84);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(192, 21);
            this.txtVersion.TabIndex = 20;
            // 
            // txtFileVersion
            // 
            this.txtFileVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileVersion.Location = new System.Drawing.Point(295, 111);
            this.txtFileVersion.Name = "txtFileVersion";
            this.txtFileVersion.Size = new System.Drawing.Size(192, 21);
            this.txtFileVersion.TabIndex = 21;
            // 
            // selectKeyFile
            // 
            this.selectKeyFile.Caption = "Strong name key file:";
            this.selectKeyFile.Filter = "";
            this.selectKeyFile.InitialValue = "";
            this.selectKeyFile.Location = new System.Drawing.Point(6, 211);
            this.selectKeyFile.Name = "selectKeyFile";
            this.selectKeyFile.SelectedValue = "";
            this.selectKeyFile.SelectionMode = TrueSoftware.Framework.Wizard.SelectFileMode.SelectFile;
            this.selectKeyFile.Size = new System.Drawing.Size(483, 48);
            this.selectKeyFile.TabIndex = 1;
            this.selectKeyFile.ValueLocked = false;
            // 
            // chkSNK
            // 
            this.chkSNK.AutoSize = true;
            this.chkSNK.Location = new System.Drawing.Point(9, 188);
            this.chkSNK.Name = "chkSNK";
            this.chkSNK.Size = new System.Drawing.Size(169, 17);
            this.chkSNK.TabIndex = 2;
            this.chkSNK.Text = "Sign the generated assembly.";
            this.chkSNK.UseVisualStyleBackColor = true;
            this.chkSNK.CheckedChanged += new System.EventHandler(this.chkSNK_CheckedChanged);
            // 
            // pgAssemblyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "pgAssemblyInfo";
            this.MainGroupBox.ResumeLayout(false);
            this.MainGroupBox.PerformLayout();
            this.panelTextHolder.ResumeLayout(false);
            this.panelTextHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtConfiguration;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTrademark;
        private System.Windows.Forms.TextBox txtCulture;
        private System.Windows.Forms.TextBox txtGuid;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.TextBox txtFileVersion;
        private System.Windows.Forms.CheckBox chkSNK;
        private TrueSoftware.Framework.Wizard.SelectFileControl selectKeyFile;
    }
}