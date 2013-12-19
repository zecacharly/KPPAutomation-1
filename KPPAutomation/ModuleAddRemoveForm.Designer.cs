namespace KPPAutomation {
    partial class ModuleAddRemoveForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleAddRemoveForm));
            this.@__ListModules = new BrightIdeasSoftware.ObjectListView();
            this.@__ModuleName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__ModuleType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__btAddModule = new System.Windows.Forms.Button();
            this.@__btRemoveModule = new System.Windows.Forms.Button();
            this.@__comboModuleTypes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.@__ListModules)).BeginInit();
            this.SuspendLayout();
            // 
            // __ListModules
            // 
            resources.ApplyResources(this.@__ListModules, "__ListModules");
            this.@__ListModules.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.@__ListModules.AllColumns.Add(this.@__ModuleName);
            this.@__ListModules.AllColumns.Add(this.@__ModuleType);
            this.@__ListModules.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__ListModules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.@__ModuleName,
            this.@__ModuleType});
            this.@__ListModules.FullRowSelect = true;
            this.@__ListModules.GridLines = true;
            this.@__ListModules.HideSelection = false;
            this.@__ListModules.IsSimpleDropSink = true;
            this.@__ListModules.LabelEdit = true;
            this.@__ListModules.MultiSelect = false;
            this.@__ListModules.Name = "__ListModules";
            this.@__ListModules.OverlayText.Text = resources.GetString("resource.Text");
            this.@__ListModules.ShowCommandMenuOnRightClick = true;
            this.@__ListModules.ShowGroups = false;
            this.@__ListModules.ShowItemToolTips = true;
            this.@__ListModules.ShowSortIndicators = false;
            this.@__ListModules.SortGroupItemsByPrimaryColumn = false;
            this.@__ListModules.UseCompatibleStateImageBehavior = false;
            this.@__ListModules.UseTranslucentSelection = true;
            this.@__ListModules.View = System.Windows.Forms.View.Details;
            this.@__ListModules.SelectedIndexChanged += new System.EventHandler(this.@__ListModules_SelectedIndexChanged);
            // 
            // __ModuleName
            // 
            this.@__ModuleName.AspectName = "ModuleName";
            resources.ApplyResources(this.@__ModuleName, "__ModuleName");
            // 
            // __ModuleType
            // 
            this.@__ModuleType.AspectName = "ModuleType";
            resources.ApplyResources(this.@__ModuleType, "__ModuleType");
            // 
            // __btAddModule
            // 
            resources.ApplyResources(this.@__btAddModule, "__btAddModule");
            this.@__btAddModule.Name = "__btAddModule";
            this.@__btAddModule.UseVisualStyleBackColor = true;
            this.@__btAddModule.Click += new System.EventHandler(this.@__btAddModule_Click);
            // 
            // __btRemoveModule
            // 
            resources.ApplyResources(this.@__btRemoveModule, "__btRemoveModule");
            this.@__btRemoveModule.Name = "__btRemoveModule";
            this.@__btRemoveModule.UseVisualStyleBackColor = true;
            this.@__btRemoveModule.Click += new System.EventHandler(this.@__btRemoveModule_Click);
            // 
            // __comboModuleTypes
            // 
            resources.ApplyResources(this.@__comboModuleTypes, "__comboModuleTypes");
            this.@__comboModuleTypes.DisplayMember = "Name";
            this.@__comboModuleTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__comboModuleTypes.FormattingEnabled = true;
            this.@__comboModuleTypes.Name = "__comboModuleTypes";
            this.@__comboModuleTypes.SelectedIndexChanged += new System.EventHandler(this.@__comboModuleTypes_SelectedIndexChanged);
            // 
            // ModuleAddRemoveForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.@__comboModuleTypes);
            this.Controls.Add(this.@__btRemoveModule);
            this.Controls.Add(this.@__btAddModule);
            this.Controls.Add(this.@__ListModules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ModuleAddRemoveForm";
            this.Load += new System.EventHandler(this.ModuleAddRemoveForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.@__ListModules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public BrightIdeasSoftware.ObjectListView __ListModules;
        public BrightIdeasSoftware.OLVColumn __ModuleName;
        private System.Windows.Forms.Button __btAddModule;
        private System.Windows.Forms.Button __btRemoveModule;
        private BrightIdeasSoftware.OLVColumn __ModuleType;
        public System.Windows.Forms.ComboBox __comboModuleTypes;
    }
}