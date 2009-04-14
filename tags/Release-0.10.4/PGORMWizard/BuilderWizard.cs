/*-------------------------------------------------------------------------
 * BuilderWizard.cs
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
using PGORM;

namespace PGORMWizard
{
    public class BuilderWizard : TrueSoftware.Framework.Wizard.WizardEngine
    {

        public Builder pgormBuilder;
        public ProjectFile projectFile = null;
        public DatabaseSchema databaseSchema;
        public Pages.pgProgress pgProgress;

        public BuilderWizard(Form hostForm, CancelFunction cancelFunction, ProcessFunction processFunction)
            : base(hostForm, cancelFunction, processFunction)
        {
            pgormBuilder = new Builder();

            projectFile = new ProjectFile();

            string[] current_asm_info = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string current_asm_version = current_asm_info[1].Split(new char[] { '=' })[1];

            projectFile.AssemblyInfo.Description = "This assembly is created by PGORM Wizard v" + current_asm_version;
            projectFile.Tables = new List<string>();
            projectFile.Views = new List<string>();
            projectFile.Functions = new List<string>();

            AddPage(new Pages.pgWelcome());
            AddPage(new Pages.pgSelectProject());
            AddPage(new Pages.pgDbConnection());
            AddPage(new Pages.pgSelectDbObjects());
            AddPage(new Pages.pgAssemblyInfo());
            AddPage(new Pages.pgProjectOptions());
            pgProgress = new PGORMWizard.Pages.pgProgress();
            AddPage(pgProgress);
        }
    }
}