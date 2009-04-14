/*-------------------------------------------------------------------------
 * TableListView.Designer.cs
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
namespace PGORMWizard.Controls
{
    partial class TableListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableListView));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Table.png");
            this.imageList.Images.SetKeyName(1, "View.png");
            this.imageList.Images.SetKeyName(2, "Function.png");
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "";
            this.columnHeader.Width = 250;
            // 
            // TableListView
            // 
            this.CheckBoxes = true;
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FullRowSelect = true;
            this.GridLines = true;
            this.MultiSelect = false;
            this.SmallImageList = this.imageList;
            this.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.View = System.Windows.Forms.View.Details;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        protected System.Windows.Forms.ColumnHeader columnHeader;
    }
}