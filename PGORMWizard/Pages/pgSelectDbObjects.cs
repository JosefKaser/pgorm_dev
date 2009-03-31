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
    public partial class pgSelectDbObjects : TrueSoftware.Framework.Wizard.WizardDefaultPage
    {
        Controls.SchemaLoader schemaLoader;
        string current_conn_str = "";
        int num_selected = 0;

        public pgSelectDbObjects()
        {
            InitializeComponent();
            Load += new EventHandler(pgSelectDbObjects_Load);
            SetContentState(false);
            tableList.ItemChecked += new ItemCheckedEventHandler(Object_ItemChecked);
            viewList.ItemChecked += new ItemCheckedEventHandler(Object_ItemChecked);
            functionList.ItemChecked += new ItemCheckedEventHandler(Object_ItemChecked);
        }

        void Object_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            num_selected += (e.Item.Checked ? 1 : -1);
            if (num_selected < 0)
                num_selected = 0;
        }

        public override void EnteringThisPage()
        {
            ReloadSchema();
        }

        public override void LeavingThisPage()
        {
            (wizardEngine as BuilderWizard).projectFile.Tables.Clear();
            (wizardEngine as BuilderWizard).projectFile.Views.Clear();
            (wizardEngine as BuilderWizard).projectFile.Functions.Clear();
            (wizardEngine as BuilderWizard).projectFile.Tables.AddRange(tableList.GetSelectedNames());
            (wizardEngine as BuilderWizard).projectFile.Views.AddRange(viewList.GetSelectedNames());
            (wizardEngine as BuilderWizard).projectFile.Functions.AddRange(functionList.GetSelectedNames());
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgProjectOptions));
            if (num_selected > 0)
            {
                return true;
            }
            else
                return false;
        }

        void pgSelectDbObjects_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Database Objects";
            lblSubTitle.Text = "Please select database object to generate code from.";
        }

        void ReloadSchema()
        {
            if (current_conn_str != wizardEngine.Parameters[ParameterName.db_connection_string].ToString())
            {
                current_conn_str = wizardEngine.Parameters[ParameterName.db_connection_string].ToString();
                Cursor = Cursors.WaitCursor;
                schemaLoader = new PGORMWizard.Controls.SchemaLoader();
                schemaLoader.progressBar.Minimum = 0;
                schemaLoader.progressBar.Maximum = 7;
                schemaLoader.progressBar.Step = 1;
                MainGroupBox.Controls.Add(schemaLoader);
                SetContentState(false);
                Application.DoEvents();
                (wizardEngine as BuilderWizard).databaseSchema = schemaLoader.LoadSchema((wizardEngine as BuilderWizard));
                schemaLoader.Visible = false;
                tableList.LoadTables((wizardEngine as BuilderWizard).databaseSchema);
                viewList.LoadViews((wizardEngine as BuilderWizard).databaseSchema);
                functionList.LoadFunctions((wizardEngine as BuilderWizard).databaseSchema);
                (wizardEngine as BuilderWizard).pgormBuilder.SendMessage(null,BuilderMessageType.Major,"Preparing controls. Please wait...");
                SetContentState(true);
                Cursor = Cursors.Default;
            }
        }

        protected override void InitializePage()
        {
            ReloadSchema();
        }

        void SetContentState(bool state)
        {
            tabObjects.Visible = state;
            pnlSubCommand.Visible = state;
            if (schemaLoader != null)
                schemaLoader.Visible = !state;
        }

        private void chkTables_CheckedChanged(object sender, EventArgs e)
        {
            tableList.SetItemsState(chkTables.Checked);
            tabObjects.SelectedTab = tabTables;
        }

        private void chkViews_CheckedChanged(object sender, EventArgs e)
        {
            viewList.SetItemsState(chkViews.Checked);
            tabObjects.SelectedTab = tabViews;

        }

        private void chkFunction_CheckedChanged(object sender, EventArgs e)
        {
            functionList.SetItemsState(chkFunction.Checked);
            tabObjects.SelectedTab = tabFunctions;

        }
    }
}
