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
using System.Reflection;
using TrueSoftware.Framework.Wizard;
using PGORM;

namespace PGORMWizard
{
    public partial class MainForm : Form
    {
        protected BuilderWizard wizEngine;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] asmInfo = Assembly.GetExecutingAssembly().FullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string version = asmInfo[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            this.Text = "PGORM Wizard v" + version;
            wizEngine = new BuilderWizard(this, CancelWizard, ProcessWizard);
            wizEngine.StartWizard();
        }

        bool ProcessWizard()
        {
            bool result = true;
            wizEngine.pgProgress.EnableCancelButton = false;
            wizEngine.SetupProgressBar(9);
            wizEngine.pgormBuilder.OnBuildStep += new PGORM.BuilderEventHandler(pgormBuilder_OnBuildStep);
            wizEngine.pgormBuilder.SendMessage(this,BuilderMessageType.Major, "Preparing...");
#if !DEBUG
            try
            {
#endif
                wizEngine.pgormBuilder.Build(wizEngine.projectFile, wizEngine.databaseSchema);
                SaveProjectFile();
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Major, "Done.");
#if !DEBUG
            }
            catch (Exception ex)
            {
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Major, "Build faild. Please check the details...");
                if (ex is PGORMLoaggerException)
                    wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, "{0}", (ex as PGORMLoaggerException).ToString());
                else
                    wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, ex.Message);
                wizEngine.pgormBuilder.SendMessage(this, BuilderMessageType.Minor, ex.StackTrace);

                ExceptionForm exForm = new ExceptionForm(ex);

                exForm.ShowDialog();
                result = false;
            }
            finally
            {
                wizEngine.pgProgress.EnableCancelButton = true;
                wizEngine.pgormBuilder.OnBuildStep -= new PGORM.BuilderEventHandler(pgormBuilder_OnBuildStep);
            }
#endif

                // animate progress to end
            for (int a = 0; a != 5; a++)
                wizEngine.ReportProgress("Finished.");

            if (result)
                MessageBox.Show(wizEngine.HostForm, "Project done.", wizEngine.HostForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            return result;
        }

        void SaveProjectFile()
        {
            if(wizEngine.Parameters.ContainsKey(ParameterName.save_project_file))
                ProjectFile.SaveProject(wizEngine.projectFile, wizEngine.Parameters[ParameterName.save_project_file].ToString()); 
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