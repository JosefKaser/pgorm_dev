namespace PGORMWizard.Pages
{
    partial class pgSelectDbObjects
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
            this.timerStarter = new System.Windows.Forms.Timer(this.components);
            this.pnlSubCommand = new System.Windows.Forms.Panel();
            this.chkFunction = new System.Windows.Forms.CheckBox();
            this.chkViews = new System.Windows.Forms.CheckBox();
            this.chkTables = new System.Windows.Forms.CheckBox();
            this.tabObjects = new System.Windows.Forms.TabControl();
            this.tabTables = new System.Windows.Forms.TabPage();
            this.tableList = new PGORMWizard.Controls.TableListView();
            this.tabViews = new System.Windows.Forms.TabPage();
            this.viewList = new PGORMWizard.Controls.ViewListView();
            this.tabFunctions = new System.Windows.Forms.TabPage();
            this.functionList = new PGORMWizard.Controls.FunctionListView();
            this.MainGroupBox.SuspendLayout();
            this.panelTextHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlSubCommand.SuspendLayout();
            this.tabObjects.SuspendLayout();
            this.tabTables.SuspendLayout();
            this.tabViews.SuspendLayout();
            this.tabFunctions.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.tabObjects);
            this.MainGroupBox.Controls.Add(this.pnlSubCommand);
            // 
            // timerStarter
            // 
            this.timerStarter.Interval = 1000;
            // 
            // pnlSubCommand
            // 
            this.pnlSubCommand.Controls.Add(this.chkFunction);
            this.pnlSubCommand.Controls.Add(this.chkViews);
            this.pnlSubCommand.Controls.Add(this.chkTables);
            this.pnlSubCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSubCommand.Location = new System.Drawing.Point(3, 214);
            this.pnlSubCommand.Name = "pnlSubCommand";
            this.pnlSubCommand.Size = new System.Drawing.Size(490, 60);
            this.pnlSubCommand.TabIndex = 0;
            // 
            // chkFunction
            // 
            this.chkFunction.AutoSize = true;
            this.chkFunction.Location = new System.Drawing.Point(111, 7);
            this.chkFunction.Name = "chkFunction";
            this.chkFunction.Size = new System.Drawing.Size(110, 17);
            this.chkFunction.TabIndex = 2;
            this.chkFunction.Text = "Select all function";
            this.chkFunction.UseVisualStyleBackColor = true;
            this.chkFunction.CheckedChanged += new System.EventHandler(this.chkFunction_CheckedChanged);
            // 
            // chkViews
            // 
            this.chkViews.AutoSize = true;
            this.chkViews.Location = new System.Drawing.Point(4, 30);
            this.chkViews.Name = "chkViews";
            this.chkViews.Size = new System.Drawing.Size(98, 17);
            this.chkViews.TabIndex = 1;
            this.chkViews.Text = "Select all views";
            this.chkViews.UseVisualStyleBackColor = true;
            this.chkViews.CheckedChanged += new System.EventHandler(this.chkViews_CheckedChanged);
            // 
            // chkTables
            // 
            this.chkTables.AutoSize = true;
            this.chkTables.Location = new System.Drawing.Point(4, 6);
            this.chkTables.Name = "chkTables";
            this.chkTables.Size = new System.Drawing.Size(100, 17);
            this.chkTables.TabIndex = 0;
            this.chkTables.Text = "Select all tables";
            this.chkTables.UseVisualStyleBackColor = true;
            this.chkTables.CheckedChanged += new System.EventHandler(this.chkTables_CheckedChanged);
            // 
            // tabObjects
            // 
            this.tabObjects.Controls.Add(this.tabTables);
            this.tabObjects.Controls.Add(this.tabViews);
            this.tabObjects.Controls.Add(this.tabFunctions);
            this.tabObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabObjects.Location = new System.Drawing.Point(3, 17);
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.SelectedIndex = 0;
            this.tabObjects.Size = new System.Drawing.Size(490, 197);
            this.tabObjects.TabIndex = 2;
            // 
            // tabTables
            // 
            this.tabTables.Controls.Add(this.tableList);
            this.tabTables.Location = new System.Drawing.Point(4, 22);
            this.tabTables.Name = "tabTables";
            this.tabTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabTables.Size = new System.Drawing.Size(482, 171);
            this.tabTables.TabIndex = 0;
            this.tabTables.Text = "Tables";
            this.tabTables.UseVisualStyleBackColor = true;
            // 
            // tableList
            // 
            this.tableList.CheckBoxes = true;
            this.tableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableList.FullRowSelect = true;
            this.tableList.GridLines = true;
            this.tableList.Location = new System.Drawing.Point(3, 3);
            this.tableList.MultiSelect = false;
            this.tableList.Name = "tableList";
            this.tableList.Size = new System.Drawing.Size(476, 165);
            this.tableList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.tableList.TabIndex = 0;
            this.tableList.UseCompatibleStateImageBehavior = false;
            this.tableList.View = System.Windows.Forms.View.Details;
            // 
            // tabViews
            // 
            this.tabViews.Controls.Add(this.viewList);
            this.tabViews.Location = new System.Drawing.Point(4, 22);
            this.tabViews.Name = "tabViews";
            this.tabViews.Padding = new System.Windows.Forms.Padding(3);
            this.tabViews.Size = new System.Drawing.Size(482, 171);
            this.tabViews.TabIndex = 1;
            this.tabViews.Text = "Views";
            this.tabViews.UseVisualStyleBackColor = true;
            // 
            // viewList
            // 
            this.viewList.CheckBoxes = true;
            this.viewList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewList.FullRowSelect = true;
            this.viewList.GridLines = true;
            this.viewList.Location = new System.Drawing.Point(3, 3);
            this.viewList.MultiSelect = false;
            this.viewList.Name = "viewList";
            this.viewList.Size = new System.Drawing.Size(476, 165);
            this.viewList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.viewList.TabIndex = 0;
            this.viewList.UseCompatibleStateImageBehavior = false;
            this.viewList.View = System.Windows.Forms.View.Details;
            // 
            // tabFunctions
            // 
            this.tabFunctions.Controls.Add(this.functionList);
            this.tabFunctions.Location = new System.Drawing.Point(4, 22);
            this.tabFunctions.Name = "tabFunctions";
            this.tabFunctions.Size = new System.Drawing.Size(482, 171);
            this.tabFunctions.TabIndex = 2;
            this.tabFunctions.Text = "Functions";
            this.tabFunctions.UseVisualStyleBackColor = true;
            // 
            // functionList
            // 
            this.functionList.CheckBoxes = true;
            this.functionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionList.FullRowSelect = true;
            this.functionList.GridLines = true;
            this.functionList.Location = new System.Drawing.Point(0, 0);
            this.functionList.MultiSelect = false;
            this.functionList.Name = "functionList";
            this.functionList.Size = new System.Drawing.Size(482, 171);
            this.functionList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.functionList.TabIndex = 0;
            this.functionList.UseCompatibleStateImageBehavior = false;
            this.functionList.View = System.Windows.Forms.View.Details;
            // 
            // pgSelectDbObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "pgSelectDbObjects";
            this.MainGroupBox.ResumeLayout(false);
            this.panelTextHolder.ResumeLayout(false);
            this.panelTextHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlSubCommand.ResumeLayout(false);
            this.pnlSubCommand.PerformLayout();
            this.tabObjects.ResumeLayout(false);
            this.tabTables.ResumeLayout(false);
            this.tabViews.ResumeLayout(false);
            this.tabFunctions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerStarter;
        private System.Windows.Forms.Panel pnlSubCommand;
        private System.Windows.Forms.CheckBox chkFunction;
        private System.Windows.Forms.CheckBox chkViews;
        private System.Windows.Forms.CheckBox chkTables;
        private System.Windows.Forms.TabControl tabObjects;
        private System.Windows.Forms.TabPage tabTables;
        private System.Windows.Forms.TabPage tabViews;
        private System.Windows.Forms.TabPage tabFunctions;
        private PGORMWizard.Controls.TableListView tableList;
        private PGORMWizard.Controls.ViewListView viewList;
        private PGORMWizard.Controls.FunctionListView functionList;

    }
}
