/*-------------------------------------------------------------------------
 * SchemaLoader.cs
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

namespace PGORMWizard.Controls
{
    public partial class SchemaLoader : UserControl
    {
        public SchemaLoader()
        {
            InitializeComponent();
            this.Visible = false;
        }

        public DatabaseSchema LoadSchema(BuilderWizard wizard) 
        {
            Dock = DockStyle.Fill;
            wizard.pgormBuilder.OnBuildStep += new BuilderEventHandler(pgormBuilder_OnBuildStep);
            DatabaseSchema dbSchema = new DatabaseSchema(wizard.pgormBuilder);
            dbSchema.ReadSchema(wizard.Parameters[ParameterName.db_connection_string].ToString());
            wizard.pgormBuilder.OnBuildStep -= new BuilderEventHandler(pgormBuilder_OnBuildStep);
            return dbSchema;
        }

        void pgormBuilder_OnBuildStep(object sender, BuilderEventArgs e)
        {
            Application.DoEvents();
            lblText.Text = e.Message;
            progressBar.PerformStep();
            Application.DoEvents();
        }
    }
}