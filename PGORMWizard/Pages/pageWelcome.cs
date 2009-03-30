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
    public partial class pageWelcome : TrueSoftware.Framework.Wizard.WizardWelcomePage
    {
        public pageWelcome()
        {
            InitializeComponent();
            this.checkDontShow.Visible = false;
            picLogo.Image = Helper.PGLogo;
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
