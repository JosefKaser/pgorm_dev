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
    public partial class pgSelectProject : TrueSoftware.Framework.Wizard.WizardDefaultPage
    {
        public pgSelectProject()
        {
            InitializeComponent();
            Load += new EventHandler(pgSelectProject_Load);
            optExisting.CheckedChanged += new EventHandler(Option_CheckedChanged);
            optNewProject.CheckedChanged += new EventHandler(Option_CheckedChanged);
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgDbConnection));

            if (optNewProject.Checked)
            {
                wizardEngine.Parameters[ParameterName.project_type] = "n"; // for new
                return true;
            }
            else if(optExisting.Checked && txtFile.Text != "")
            {
                wizardEngine.Parameters[ParameterName.project_type] = "e"; // for existing
                wizardEngine.Parameters[ParameterName.selected_project] = txtFile.Text;
                return true;
            }
            return false;
        }

        void Option_CheckedChanged(object sender, EventArgs e)
        {
            pnlSelect.Visible = (sender == optExisting);
        }

        void pgSelectProject_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Project";
            lblSubTitle.Text = "Please select a project type";
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Trim() != "")
            {
                txtFile.Text = openFileDialog.FileName;
            }
        }
    }
}
