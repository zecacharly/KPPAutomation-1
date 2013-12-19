namespace VisionModule {
    partial class OutputResultConfForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputResultConfForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.@__comboRequest = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.@__comboInsp = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.@__comboRois = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.@__RoiProcList = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumnFuncName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.@__proplist = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__RoiProcList)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__proplist)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.@__comboRequest);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // __comboRequest
            // 
            resources.ApplyResources(this.@__comboRequest, "__comboRequest");
            this.@__comboRequest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__comboRequest.FormattingEnabled = true;
            this.@__comboRequest.Name = "__comboRequest";
            this.@__comboRequest.SelectedValueChanged += new System.EventHandler(this.@__comboRequest_SelectedValueChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.@__comboInsp);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // __comboInsp
            // 
            resources.ApplyResources(this.@__comboInsp, "__comboInsp");
            this.@__comboInsp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__comboInsp.FormattingEnabled = true;
            this.@__comboInsp.Name = "__comboInsp";
            this.@__comboInsp.SelectedIndexChanged += new System.EventHandler(this.@__comboInsp_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.@__comboRois);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // __comboRois
            // 
            resources.ApplyResources(this.@__comboRois, "__comboRois");
            this.@__comboRois.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__comboRois.FormattingEnabled = true;
            this.@__comboRois.Name = "__comboRois";
            this.@__comboRois.SelectedIndexChanged += new System.EventHandler(this.@__comboRois_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.@__RoiProcList);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // __RoiProcList
            // 
            resources.ApplyResources(this.@__RoiProcList, "__RoiProcList");
            this.@__RoiProcList.AllColumns.Add(this.olvColumnFuncName);
            this.@__RoiProcList.AllColumns.Add(this.olvColumn2);
            this.@__RoiProcList.AllowDrop = true;
            this.@__RoiProcList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFuncName,
            this.olvColumn2});
            this.@__RoiProcList.FullRowSelect = true;
            this.@__RoiProcList.IsSimpleDropSink = true;
            this.@__RoiProcList.MultiSelect = false;
            this.@__RoiProcList.Name = "__RoiProcList";
            this.@__RoiProcList.OverlayText.Text = resources.GetString("resource.Text");
            this.@__RoiProcList.ShowFilterMenuOnRightClick = false;
            this.@__RoiProcList.ShowGroups = false;
            this.@__RoiProcList.ShowSortIndicators = false;
            this.@__RoiProcList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.@__RoiProcList.TintSortColumn = true;
            this.@__RoiProcList.UseCompatibleStateImageBehavior = false;
            this.@__RoiProcList.UseTranslucentSelection = true;
            this.@__RoiProcList.View = System.Windows.Forms.View.Details;
            this.@__RoiProcList.VirtualMode = true;
            this.@__RoiProcList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.@__RoiProcList_ItemSelectionChanged);
            // 
            // olvColumnFuncName
            // 
            this.olvColumnFuncName.AspectName = "FunctionName";
            resources.ApplyResources(this.olvColumnFuncName, "olvColumnFuncName");
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "FunctionType";
            resources.ApplyResources(this.olvColumn2, "olvColumn2");
            this.olvColumn2.IsEditable = false;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.@__proplist);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // __proplist
            // 
            resources.ApplyResources(this.@__proplist, "__proplist");
            this.@__proplist.AllColumns.Add(this.olvColumn1);
            this.@__proplist.AllColumns.Add(this.olvColumn3);
            this.@__proplist.AllowDrop = true;
            this.@__proplist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn3});
            this.@__proplist.FullRowSelect = true;
            this.@__proplist.IsSimpleDropSink = true;
            this.@__proplist.MultiSelect = false;
            this.@__proplist.Name = "__proplist";
            this.@__proplist.OverlayText.Text = resources.GetString("resource.Text1");
            this.@__proplist.ShowFilterMenuOnRightClick = false;
            this.@__proplist.ShowGroups = false;
            this.@__proplist.ShowSortIndicators = false;
            this.@__proplist.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.@__proplist.TintSortColumn = true;
            this.@__proplist.UseCompatibleStateImageBehavior = false;
            this.@__proplist.UseTranslucentSelection = true;
            this.@__proplist.View = System.Windows.Forms.View.Details;
            this.@__proplist.VirtualMode = true;
            this.@__proplist.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.@__proplist_ItemSelectionChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            resources.ApplyResources(this.olvColumn1, "olvColumn1");
            // 
            // olvColumn3
            // 
            resources.ApplyResources(this.olvColumn3, "olvColumn3");
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // propertyGrid1
            // 
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // OutputResultConfForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "OutputResultConfForm";
            this.Load += new System.EventHandler(this.OutputResultConfForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__RoiProcList)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__proplist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox __comboRequest;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox __comboInsp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox __comboRois;
        private System.Windows.Forms.GroupBox groupBox4;
        public BrightIdeasSoftware.FastObjectListView __RoiProcList;
        public BrightIdeasSoftware.OLVColumn olvColumnFuncName;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.GroupBox groupBox5;
        public BrightIdeasSoftware.FastObjectListView __proplist;
        public BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}