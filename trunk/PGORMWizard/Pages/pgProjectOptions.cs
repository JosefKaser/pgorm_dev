/*-------------------------------------------------------------------------
 * pgProjectOptions.cs
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

namespace PGORMWizard.Pages
{
    public partial class pgProjectOptions : TrueSoftware.Framework.Wizard.WizardDefaultPage
    {
        public pgProjectOptions()
        {
            InitializeComponent();
            Load += new EventHandler(pgProjectOptions_Load);
        }

        void pgProjectOptions_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Project Options";
            lblSubTitle.Text = "Please provide project options.";
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgAssemblyInfo));
            if (
                txtRootNS.Text.Trim() != "" &&
                folderCompilerOutput.SelectedValue.Trim() != ""
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void LeavingThisPage()
        {
            ((BuilderWizard)wizardEngine).projectFile.RootNamespace 
                = txtRootNS.Text;

            ((BuilderWizard)wizardEngine).projectFile.CompilerOutputFolder =
            folderCompilerOutput.InitialValue;

            if (savePrpjectFile.SelectedValue != "")
                wizardEngine.Parameters[ParameterName.save_project_file] = savePrpjectFile.SelectedValue;
        }

        protected override void InitializePage()
        {
            txtRootNS.Text = ((BuilderWizard)wizardEngine).projectFile.RootNamespace;
            folderCompilerOutput.InitialValue = ((BuilderWizard)wizardEngine).projectFile.CompilerOutputFolder;
        }
    }
}