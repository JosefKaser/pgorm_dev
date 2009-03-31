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
    public partial class ViewListView : Controls.TableListView
    {
        public ViewListView()
        {
            InitializeComponent();
            columnHeader.Text = "View name";
        }

        public void LoadViews(PGORM.DatabaseSchema dbschema)
        {
            Items.Clear();
            foreach (Table item in dbschema.Tables)
                if (item.IsView)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = item.TableName;
                    lvi.Tag = item.TableName;
                    lvi.ImageIndex = 1;
                    Items.Add(lvi);
                }
        }
    }
}
