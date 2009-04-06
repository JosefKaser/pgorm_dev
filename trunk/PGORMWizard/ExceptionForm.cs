/*-------------------------------------------------------------------------
 * ExceptionForm.cs
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PGORM;

namespace PGORMWizard
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm()
        {
            InitializeComponent();
        }

        public ExceptionForm(string text, Exception e)
            : this(e)
        {
            txtContent.ForeColor = Color.Red;
            txtContent.Text = text + "\r\n" + txtContent.Text;
        }

        public ExceptionForm(string text, Exception e,Color color)
            : this(e)
        {
            txtContent.ForeColor = color;
            txtContent.Text = text + "\r\n" + txtContent.Text;
        }

        public ExceptionForm(Exception e)
            : this()
        {
            txtContent.ForeColor = Color.Red;
            if (e != null)
                txtContent.Text = e.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}