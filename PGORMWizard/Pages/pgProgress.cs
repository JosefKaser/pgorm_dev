/*-------------------------------------------------------------------------
 * pgProgress.cs
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
    public partial class pgProgress : TrueSoftware.Framework.Wizard.WizardProgressPage
    {
        public pgProgress()
        {
            InitializeComponent();
            Load += new EventHandler(pgProgress_Load);
        }

        void pgProgress_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Project build progress.";
            lblSubTitle.Text = "Please wait while the project is being built.";
        }

        public void WriteLine(string message)
        {
            txtReport.Text += message + "\r\n";
            txtReport.Select(txtReport.Text.Length, 0);
            txtReport.ScrollToCaret();
            Application.DoEvents();
        }

        private void cmdCommand_Click(object sender, EventArgs e)
        {
            if (!wizardEngine.IsProcessing)
            {
                wizardEngine.CancelWizardProcess();
            }
        }
    }
}