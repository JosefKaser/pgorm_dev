using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrueSoftware.Framework.Wizard;

namespace PGORMWizard
{
    public partial class MainForm : Form
    {
        protected WizardEngine wizEngine;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            wizEngine = new BuilderWizard(this, CancelWizard, ProcessWizard);
            wizEngine.StartWizard();
        }

        bool ProcessWizard()
        {
            return true;
        }

        bool CancelWizard()
        {
            return true;
        }
    }
}
