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
    public partial class pgAssemblyInfo : TrueSoftware.Framework.Wizard.WizardLastPage
    {
        public pgAssemblyInfo()
        {
            InitializeComponent();
            Load += new EventHandler(pgAssemblyInfo_Load);
        }

        void pgAssemblyInfo_Load(object sender, EventArgs e)
        {
            picLogo.Image = Helper.PGLogo;
            lblPageTitle.Text = "Assembly Information";
            lblSubTitle.Text = "Please provide metadata for the generated assemblies";
        }

        public override void EnteringThisPage()
        {
            chkSNK_CheckedChanged(null, null);
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgProjectOptions));
            return true;
        }

        private void chkSNK_CheckedChanged(object sender, EventArgs e)
        {
            selectKeyFile.Visible = chkSNK.Checked;
        }
    }
}
