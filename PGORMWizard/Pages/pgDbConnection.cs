using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace PGORMWizard.Pages
{
    public partial class pgDbConnection : TrueSoftware.Framework.Wizard.WizardDefaultPage
    {
        bool IsValid = false;
        public pgDbConnection()
        {
            InitializeComponent();
            Load += new EventHandler(pgDbConnection_Load);
            txtDatabase.TextChanged += new EventHandler(Text_TextChanged);
            txtOptions.TextChanged += new EventHandler(Text_TextChanged);
            txtPort.TextChanged += new EventHandler(Text_TextChanged);
            txtServer.TextChanged += new EventHandler(Text_TextChanged);
            txtUsername.TextChanged += new EventHandler(Text_TextChanged);
            txtPassword.TextChanged += new EventHandler(Text_TextChanged);
        }

        void Text_TextChanged(object sender, EventArgs e)
        {
            IsValid = false;
        }

        void pgDbConnection_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "PostgreSQL Database Server";
            lblSubTitle.Text = "Please provide connection information";
        }

        protected override bool ValidatePage()
        {
            txtResult.Text =
                string.Format("host={0}; database={1}; user={2}; password={3}; port={4}; {5}",
                txtServer.Text,
                txtDatabase.Text,
                txtUsername.Text,
                txtPassword.Text,
                txtPort.Text,
                txtOptions.Text
                );
            SetNextPage(typeof(pgSelectDbObjects));
            return IsValid;
        }

        private void btnTestDb_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection();
            Cursor = Cursors.WaitCursor;
            try
            {
                conn.ConnectionString = txtResult.Text;
                conn.Open();
                conn.Close();
                IsValid = true;
                wizardEngine.Parameters[ParameterName.db_connection_string] = txtResult.Text;
                Cursor = Cursors.Default;
                MessageBox.Show(wizardEngine.HostForm, "Connection succeed.\nClick Next to continue.", wizardEngine.HostForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(wizardEngine.HostForm, ex.Message, wizardEngine.HostForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsValid = false;
                wizardEngine.Parameters[ParameterName.db_connection_string] = txtResult.Text;
                Cursor = Cursors.Default;
            }
        }
    }
}
