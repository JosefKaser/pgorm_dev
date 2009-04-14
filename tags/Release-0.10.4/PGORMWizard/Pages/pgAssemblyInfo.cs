/*-------------------------------------------------------------------------
 * pgAssemblyInfo.cs
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PGORM;

namespace PGORMWizard.Pages
{
    public partial class pgAssemblyInfo : TrueSoftware.Framework.Wizard.WizardLastPage
    {
        public pgAssemblyInfo()
        {
            InitializeComponent();
            Load += new EventHandler(pgAssemblyInfo_Load);
        }

        void pgAssemblyInfo_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Assembly Information";
            lblSubTitle.Text = "Please provide metadata for the generated assemblies";
        }

        public override void EnteringThisPage()
        {
            chkSNK_CheckedChanged(null, null);
            AssemblyInfo info = (wizardEngine as BuilderWizard).projectFile.AssemblyInfo;
            txtTitle.Text = info.Title;
            txtDescription.Text = info.Description;
            txtConfiguration.Text = info.Configuration;
            txtCompany.Text = info.Company;
            txtProduct.Text = info.Product;
            txtCopyright.Text = info.Copyright;
            txtTrademark.Text = info.Trademark;
            txtCulture.Text = info.Culture;
            txtGuid.Text = info.Guid;
            txtVersion.Text = info.Version;
            txtFileVersion.Text = info.FileVersion;
        }

        public override void LeavingThisPage()
        {
            AssemblyInfo info = (wizardEngine as BuilderWizard).projectFile.AssemblyInfo;
            info.Title = txtTitle.Text;
            info.Description = txtDescription.Text;
            info.Configuration = txtConfiguration.Text;
            info.Company = txtCompany.Text;
            info.Product = txtProduct.Text;
            info.Copyright = txtCopyright.Text;
            info.Trademark = txtTrademark.Text;
            info.Culture = txtCulture.Text;
            info.Guid = txtGuid.Text;
            info.Version = txtVersion.Text;
            info.FileVersion = txtFileVersion.Text;
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgProjectOptions));
            return true;
        }

        private void chkSNK_CheckedChanged(object sender, EventArgs e)
        {
            selectKeyFile.Visible = chkSNK.Checked;
        }
    }
}