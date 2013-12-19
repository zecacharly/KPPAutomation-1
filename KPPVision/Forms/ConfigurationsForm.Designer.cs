namespace VisionModule {
    partial class ConfigurationsForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationsForm));
            this.@__contextServers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__tooladdServer = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolremoveServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.@__btExit = new System.Windows.Forms.Button();
            this.@__btSave = new System.Windows.Forms.Button();
            this.@__tabConf = new System.Windows.Forms.TabControl();
            this.@__tabProjectsConf = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.@__textFiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.@__btCreateEmpty = new System.Windows.Forms.Button();
            this.@__EditEmptyFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.@__listfiles = new System.Windows.Forms.ListBox();
            this.@__btRemove = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.@__listdirs = new System.Windows.Forms.ListBox();
            this.@__btAddDir = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.@__serverconflist = new BrightIdeasSoftware.ObjectListView();
            this.@__olvname = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__port = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__state = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__contextServers.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.@__tabConf.SuspendLayout();
            this.@__tabProjectsConf.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__serverconflist)).BeginInit();
            this.SuspendLayout();
            // 
            // __contextServers
            // 
            resources.ApplyResources(this.@__contextServers, "__contextServers");
            this.@__contextServers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__tooladdServer,
            this.@__toolremoveServer});
            this.@__contextServers.Name = "__contextAddInsp";
            // 
            // __tooladdServer
            // 
            resources.ApplyResources(this.@__tooladdServer, "__tooladdServer");
            this.@__tooladdServer.Image = global::VisionModule.Properties.Resources.Actions_list_add_icon;
            this.@__tooladdServer.Name = "__tooladdServer";
            this.@__tooladdServer.Click += new System.EventHandler(this.@__tooladdServer_Click);
            // 
            // __toolremoveServer
            // 
            resources.ApplyResources(this.@__toolremoveServer, "__toolremoveServer");
            this.@__toolremoveServer.Image = global::VisionModule.Properties.Resources.Actions_remove_icon;
            this.@__toolremoveServer.Name = "__toolremoveServer";
            this.@__toolremoveServer.Click += new System.EventHandler(this.@__toolremoveServer_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.@__btExit, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.@__btSave, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.@__tabConf, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // __btExit
            // 
            resources.ApplyResources(this.@__btExit, "__btExit");
            this.@__btExit.Name = "__btExit";
            this.@__btExit.UseVisualStyleBackColor = true;
            this.@__btExit.Click += new System.EventHandler(this.@__btExit_Click_1);
            // 
            // __btSave
            // 
            resources.ApplyResources(this.@__btSave, "__btSave");
            this.@__btSave.Name = "__btSave";
            this.@__btSave.UseVisualStyleBackColor = true;
            this.@__btSave.Click += new System.EventHandler(this.@__btSave_Click);
            // 
            // __tabConf
            // 
            resources.ApplyResources(this.@__tabConf, "__tabConf");
            this.tableLayoutPanel1.SetColumnSpan(this.@__tabConf, 2);
            this.@__tabConf.Controls.Add(this.@__tabProjectsConf);
            this.@__tabConf.Controls.Add(this.tabPage2);
            this.@__tabConf.Name = "__tabConf";
            this.@__tabConf.SelectedIndex = 0;
            // 
            // __tabProjectsConf
            // 
            resources.ApplyResources(this.@__tabProjectsConf, "__tabProjectsConf");
            this.@__tabProjectsConf.Controls.Add(this.groupBox4);
            this.@__tabProjectsConf.Name = "__tabProjectsConf";
            this.@__tabProjectsConf.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.@__textFiles);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.@__btCreateEmpty);
            this.groupBox4.Controls.Add(this.@__EditEmptyFile);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.@__listfiles);
            this.groupBox4.Controls.Add(this.@__btRemove);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.@__listdirs);
            this.groupBox4.Controls.Add(this.@__btAddDir);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // __textFiles
            // 
            resources.ApplyResources(this.@__textFiles, "__textFiles");
            this.@__textFiles.Name = "__textFiles";
            this.@__textFiles.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // __btCreateEmpty
            // 
            resources.ApplyResources(this.@__btCreateEmpty, "__btCreateEmpty");
            this.@__btCreateEmpty.Name = "__btCreateEmpty";
            this.@__btCreateEmpty.UseVisualStyleBackColor = true;
            this.@__btCreateEmpty.Click += new System.EventHandler(this.@__btCreateEmpty_Click_1);
            // 
            // __EditEmptyFile
            // 
            resources.ApplyResources(this.@__EditEmptyFile, "__EditEmptyFile");
            this.@__EditEmptyFile.Name = "__EditEmptyFile";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // __listfiles
            // 
            resources.ApplyResources(this.@__listfiles, "__listfiles");
            this.@__listfiles.FormattingEnabled = true;
            this.@__listfiles.Name = "__listfiles";
            // 
            // __btRemove
            // 
            resources.ApplyResources(this.@__btRemove, "__btRemove");
            this.@__btRemove.Name = "__btRemove";
            this.@__btRemove.UseVisualStyleBackColor = true;
            this.@__btRemove.Click += new System.EventHandler(this.@__btRemove_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // __listdirs
            // 
            resources.ApplyResources(this.@__listdirs, "__listdirs");
            this.@__listdirs.FormattingEnabled = true;
            this.@__listdirs.Name = "__listdirs";
            this.@__listdirs.SelectedIndexChanged += new System.EventHandler(this.@__listdirs_SelectedIndexChanged);
            // 
            // __btAddDir
            // 
            resources.ApplyResources(this.@__btAddDir, "__btAddDir");
            this.@__btAddDir.Name = "__btAddDir";
            this.@__btAddDir.UseVisualStyleBackColor = true;
            this.@__btAddDir.Click += new System.EventHandler(this.@__btAddDir_Click);
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.tableLayoutPanel2.SetColumnSpan(this.groupBox1, 3);
            this.groupBox1.Controls.Add(this.@__serverconflist);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel2.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.TabStop = false;
            // 
            // __serverconflist
            // 
            resources.ApplyResources(this.@__serverconflist, "__serverconflist");
            this.@__serverconflist.AllColumns.Add(this.@__olvname);
            this.@__serverconflist.AllColumns.Add(this.@__port);
            this.@__serverconflist.AllColumns.Add(this.@__state);
            this.@__serverconflist.AllColumns.Add(this.olvColumn9);
            this.@__serverconflist.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__serverconflist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.@__olvname,
            this.@__port,
            this.@__state,
            this.olvColumn9});
            this.@__serverconflist.ContextMenuStrip = this.@__contextServers;
            this.@__serverconflist.FullRowSelect = true;
            this.@__serverconflist.GridLines = true;
            this.@__serverconflist.HideSelection = false;
            this.@__serverconflist.IsSimpleDropSink = true;
            this.@__serverconflist.LabelEdit = true;
            this.@__serverconflist.MultiSelect = false;
            this.@__serverconflist.Name = "__serverconflist";
            this.@__serverconflist.OverlayText.Text = resources.GetString("resource.Text");
            this.@__serverconflist.ShowCommandMenuOnRightClick = true;
            this.@__serverconflist.ShowGroups = false;
            this.@__serverconflist.ShowItemToolTips = true;
            this.@__serverconflist.ShowSortIndicators = false;
            this.@__serverconflist.SortGroupItemsByPrimaryColumn = false;
            this.@__serverconflist.UseCompatibleStateImageBehavior = false;
            this.@__serverconflist.UseTranslucentSelection = true;
            this.@__serverconflist.View = System.Windows.Forms.View.Details;
            // 
            // __olvname
            // 
            this.@__olvname.AspectName = "ID";
            resources.ApplyResources(this.@__olvname, "__olvname");
            // 
            // __port
            // 
            this.@__port.AspectName = "Port";
            resources.ApplyResources(this.@__port, "__port");
            // 
            // __state
            // 
            this.@__state.AspectName = "State";
            resources.ApplyResources(this.@__state, "__state");
            this.@__state.IsEditable = false;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "Enabled";
            resources.ApplyResources(this.olvColumn9, "olvColumn9");
            // 
            // ConfigurationsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConfigurationsForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationsForm_FormClosing);
            this.Load += new System.EventHandler(this.@__ConfigurationsForm_Load);
            this.@__contextServers.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.@__tabConf.ResumeLayout(false);
            this.@__tabProjectsConf.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__serverconflist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ContextMenuStrip __contextServers;
        public System.Windows.Forms.ToolStripMenuItem __tooladdServer;
        public System.Windows.Forms.ToolStripMenuItem __toolremoveServer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button __btExit;
        public System.Windows.Forms.Button __btSave;
        private System.Windows.Forms.TabControl __tabConf;
        private System.Windows.Forms.TabPage __tabProjectsConf;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        public BrightIdeasSoftware.ObjectListView __serverconflist;
        public BrightIdeasSoftware.OLVColumn __olvname;
        public BrightIdeasSoftware.OLVColumn __port;
        public BrightIdeasSoftware.OLVColumn __state;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button __btRemove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox __listdirs;
        private System.Windows.Forms.Button __btAddDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox __listfiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox __textFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button __btCreateEmpty;
        private System.Windows.Forms.TextBox __EditEmptyFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
    }
}