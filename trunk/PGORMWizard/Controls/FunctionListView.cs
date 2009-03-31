/*-------------------------------------------------------------------------
 * FunctionListView.cs
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
    public partial class FunctionListView : Controls.TableListView
    {
        public FunctionListView()
        {
            InitializeComponent();
            columnHeader.Text = "Function name";
            Columns.Add("Return type");
            Columns.Add("Paramaters");
            Columns[0].Width = 150;
            Columns[1].Width = 150;
            Columns[2].Width = 150;
        }

        public void LoadFunctions(PGORM.DatabaseSchema dbschema)
        {
            Items.Clear();
            foreach (Function item in dbschema.StoredFunctions)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.FunctionName;
                lvi.Tag = item.FunctionName;
                lvi.ImageIndex = 2;
                lvi.SubItems.Add(item.DB_ReturnType);
                lvi.SubItems.Add(item.ToStringParamTypes());
                Items.Add(lvi);
            }
        }
    }
}