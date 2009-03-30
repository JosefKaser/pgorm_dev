using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PGORM;

namespace PGORMWizard
{
    class BuilderWizard : TrueSoftware.Framework.Wizard.WizardEngine
    {

        public Builder pgormBuilder;
        public ProjectFile projectFile = null;

        public BuilderWizard(Form hostForm, CancelFunction cancelFunction, ProcessFunction processFunction)
            : base(hostForm, cancelFunction, processFunction)
        {
            pgormBuilder = new Builder();

            AddPage(new Pages.pageWelcome());
            AddPage(new Pages.pgSelectProject());
            AddPage(new Pages.pgDbConnection());
        }
    }
}
