/*-------------------------------------------------------------------------
 * TableListView.cs
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
    public partial class TableListView : ListView
    {
        public TableListView()
        {
            InitializeComponent();
            columnHeader.Text = "Table name";
        }

        public void LoadTables(DatabaseSchema dbschema)
        {
            Items.Clear();
            foreach (Table item in dbschema.Tables)
                if (!item.IsView)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = item.TableName;
                    lvi.Tag = item.TableName;
                    lvi.ImageIndex = 0;
                    Items.Add(lvi);
                }
        }

        public void SetItemsState(bool state)
        {
            foreach (ListViewItem item in Items)
                item.Checked = state;
        }

        public List<string> GetSelectedNames()
        {
            List<string> result = new List<string>();
            foreach (ListViewItem item in Items)
                if (item.Checked)
                    result.Add(item.Text);
            return result;
        }

        public int GetSelectedCount()
        {
            int a = 0;
            foreach (ListViewItem item in Items)
                if (item.Checked)
                    a++;
            return a;
        }
    }
}