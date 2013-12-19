namespace VisionModule {
    partial class ProjectOptionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectOptionsForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.@__btNewProj = new System.Windows.Forms.Button();
            this.@__btduplicate = new System.Windows.Forms.Button();
            this.@__listprojects = new BrightIdeasSoftware.ObjectListView();
            this.olvProjName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvProjID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvLoadOnStart = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__btLoadProj = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.@__contextInspection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__tooladdInspection = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolremoveInspection = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolInspSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__listprojects)).BeginInit();
            this.@__contextInspection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.@__btNewProj, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.@__btduplicate, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.@__listprojects, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.@__btLoadProj, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // __btNewProj
            // 
            resources.ApplyResources(this.@__btNewProj, "__btNewProj");
            this.@__btNewProj.Name = "__btNewProj";
            this.tableLayoutPanel1.SetRowSpan(this.@__btNewProj, 2);
            this.@__btNewProj.UseVisualStyleBackColor = true;
            this.@__btNewProj.Click += new System.EventHandler(this.@__btNewProj_Click);
            // 
            // __btduplicate
            // 
            resources.ApplyResources(this.@__btduplicate, "__btduplicate");
            this.@__btduplicate.Name = "__btduplicate";
            this.tableLayoutPanel1.SetRowSpan(this.@__btduplicate, 2);
            this.@__btduplicate.UseVisualStyleBackColor = true;
            this.@__btduplicate.Click += new System.EventHandler(this.@__btduplicate_Click);
            // 
            // __listprojects
            // 
            resources.ApplyResources(this.@__listprojects, "__listprojects");
            this.@__listprojects.AllColumns.Add(this.olvProjName);
            this.@__listprojects.AllColumns.Add(this.olvProjID);
            this.@__listprojects.AllColumns.Add(this.olvLoadOnStart);
            this.@__listprojects.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__listprojects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvProjName,
            this.olvProjID,
            this.olvLoadOnStart});
            this.tableLayoutPanel1.SetColumnSpan(this.@__listprojects, 5);
            this.@__listprojects.Cursor = System.Windows.Forms.Cursors.Default;
            this.@__listprojects.FullRowSelect = true;
            this.@__listprojects.HideSelection = false;
            this.@__listprojects.MultiSelect = false;
            this.@__listprojects.Name = "__listprojects";
            this.@__listprojects.OverlayText.Text = resources.GetString("resource.Text");
            this.@__listprojects.ShowFilterMenuOnRightClick = false;
            this.@__listprojects.ShowGroups = false;
            this.@__listprojects.ShowItemToolTips = true;
            this.@__listprojects.UseCompatibleStateImageBehavior = false;
            this.@__listprojects.UseTranslucentSelection = true;
            this.@__listprojects.View = System.Windows.Forms.View.Details;
            this.@__listprojects.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.@__listprojects_CellEditFinishing);
            this.@__listprojects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.@__listprojects_KeyDown);
            // 
            // olvProjName
            // 
            this.olvProjName.AspectName = "Name";
            resources.ApplyResources(this.olvProjName, "olvProjName");
            // 
            // olvProjID
            // 
            this.olvProjID.AspectName = "ProjectID";
            resources.ApplyResources(this.olvProjID, "olvProjID");
            // 
            // olvLoadOnStart
            // 
            this.olvLoadOnStart.AspectName = "Loadonstart";
            resources.ApplyResources(this.olvLoadOnStart, "olvLoadOnStart");
            // 
            // __btLoadProj
            // 
            resources.ApplyResources(this.@__btLoadProj, "__btLoadProj");
            this.@__btLoadProj.Name = "__btLoadProj";
            this.tableLayoutPanel1.SetRowSpan(this.@__btLoadProj, 2);
            this.@__btLoadProj.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.tableLayoutPanel1.SetColumnSpan(this.button1, 5);
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.tableLayoutPanel1.SetColumnSpan(this.button2, 6);
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // __contextInspection
            // 
            resources.ApplyResources(this.@__contextInspection, "__contextInspection");
            this.@__contextInspection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__tooladdInspection,
            this.@__toolremoveInspection,
            this.@__toolInspSettings});
            this.@__contextInspection.Name = "__contextAddInsp";
            // 
            // __tooladdInspection
            // 
            resources.ApplyResources(this.@__tooladdInspection, "__tooladdInspection");
            this.@__tooladdInspection.Image = global::VisionModule.Properties.Resources.Actions_list_add_icon;
            this.@__tooladdInspection.Name = "__tooladdInspection";
            // 
            // __toolremoveInspection
            // 
            resources.ApplyResources(this.@__toolremoveInspection, "__toolremoveInspection");
            this.@__toolremoveInspection.Image = global::VisionModule.Properties.Resources.Actions_remove_icon;
            this.@__toolremoveInspection.Name = "__toolremoveInspection";
            // 
            // __toolInspSettings
            // 
            resources.ApplyResources(this.@__toolInspSettings, "__toolInspSettings");
            this.@__toolInspSettings.Image = global::VisionModule.Properties.Resources.Settings_icon2;
            this.@__toolInspSettings.Name = "__toolInspSettings";
            // 
            // ProjectOptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProjectOptionsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectOptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.ProjectOptionsForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__listprojects)).EndInit();
            this.@__contextInspection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public BrightIdeasSoftware.ObjectListView __listprojects;
        private BrightIdeasSoftware.OLVColumn olvProjName;
        public System.Windows.Forms.Button __btLoadProj;
        private BrightIdeasSoftware.OLVColumn olvProjID;
        public BrightIdeasSoftware.OLVColumn olvLoadOnStart;
        public System.Windows.Forms.ContextMenuStrip __contextInspection;
        public System.Windows.Forms.ToolStripMenuItem __tooladdInspection;
        public System.Windows.Forms.ToolStripMenuItem __toolremoveInspection;
        public System.Windows.Forms.ToolStripMenuItem __toolInspSettings;
        public System.Windows.Forms.Button __btduplicate;
        public System.Windows.Forms.Button __btNewProj;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}