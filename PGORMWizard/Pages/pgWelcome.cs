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
    public partial class pgWelcome : TrueSoftware.Framework.Wizard.WizardWelcomePage
    {
        public pgWelcome()
        {
            InitializeComponent();
            this.checkDontShow.Visible = false;
            picLogo.Image = Helper.PGLogo;
            picLogo.Padding = new Padding(5);
            lblHeaderTitle.Text = global::PGORMWizard.Properties.Resources.WizardCaption;
            txtDescrption.Text = global::PGORMWizard.Properties.Resources.About;
        }

        protected override bool ValidatePage()
        {
            SetNextPage(typeof(pgSelectProject));
            return true;
        }
    }
}
