/*-------------------------------------------------------------------------
 * MainForm.cs
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrueSoftware.Framework.Wizard;
using PGORM;

namespace PGORMWizard
{
    public partial class MainForm : Form
    {
        protected BuilderWizard wizEngine;
        protected PGORMLogger wizLogger;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            wizEngine = new BuilderWizard(this, CancelWizard, ProcessWizard);
            wizLogger = new PGORMLogger(wizEngine.pgormBuilder);
            wizEngine.StartWizard();
        }

        bool ProcessWizard()
        {
            bool result = true;
            wizEngine.SetupProgressBar(2);
            wizEngine.pgormBuilder.OnBuildStep += new PGORM.BuilderEventHandler(pgormBuilder_OnBuildStep);
            wizEngine.pgormBuilder.SendMessage(this,BuilderMessageType.Major, "Preparing...");
            try
            {
                wizEngine.pgormBuilder.Build(wizEngine.projectFile, wizEngine.databaseSchema);
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Major, "Done.");
            }
            catch (Exception ex)
            {
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Major, "Build faild. Please check the details...");
                if(ex is PGORMLoaggerException)
                    wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, (ex as PGORMLoaggerException).ToString());
                else
                    wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, ex.Message);
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, ex.StackTrace);
                result = false;
            }
            finally
            {
                wizEngine.pgormBuilder.OnBuildStep -= new PGORM.BuilderEventHandler(pgormBuilder_OnBuildStep);
            }
            return result;
        }

        void pgormBuilder_OnBuildStep(object sender, PGORM.BuilderEventArgs e)
        {
            if (e.MessageType == BuilderMessageType.Major ||
                e.MessageType == BuilderMessageType.Error)
                wizEngine.ReportProgress(e.Message);
            wizEngine.pgProgress.WriteLine(e.Message);
        }

        bool CancelWizard()
        {
            return true;
        }
    }
}