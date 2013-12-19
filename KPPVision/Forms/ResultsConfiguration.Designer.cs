namespace VisionModule {
    partial class ResultsConfiguration {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsConfiguration));
            this.@__listinspResults = new BrightIdeasSoftware.ObjectListView();
            this.@__IDName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__btAddResult = new System.Windows.Forms.Button();
            this.@__listInputs = new BrightIdeasSoftware.ObjectListView();
            this.@__Parameter = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__Input = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__Value = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this._DefaultValue = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__btRemove = new System.Windows.Forms.Button();
            this.@__BtRemoveInput = new System.Windows.Forms.Button();
            this.@__BtAddInput = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.@__listinspResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__listInputs)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // __listinspResults
            // 
            resources.ApplyResources(this.@__listinspResults, "__listinspResults");
            this.@__listinspResults.AllColumns.Add(this.@__IDName);
            this.@__listinspResults.AllColumns.Add(this.olvColumn1);
            this.@__listinspResults.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__listinspResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.@__IDName,
            this.olvColumn1});
            this.@__listinspResults.FullRowSelect = true;
            this.@__listinspResults.GridLines = true;
            this.@__listinspResults.HideSelection = false;
            this.@__listinspResults.LabelEdit = true;
            this.@__listinspResults.MultiSelect = false;
            this.@__listinspResults.Name = "__listinspResults";
            this.@__listinspResults.OverlayText.Text = resources.GetString("resource.Text");
            this.@__listinspResults.ShowGroups = false;
            this.@__listinspResults.ShowItemToolTips = true;
            this.@__listinspResults.ShowSortIndicators = false;
            this.@__listinspResults.SortGroupItemsByPrimaryColumn = false;
            this.@__listinspResults.UseCompatibleStateImageBehavior = false;
            this.@__listinspResults.UseTranslucentSelection = true;
            this.@__listinspResults.View = System.Windows.Forms.View.Details;
            // 
            // __IDName
            // 
            this.@__IDName.AspectName = "ID";
            resources.ApplyResources(this.@__IDName, "__IDName");
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "DecimalSeparator";
            resources.ApplyResources(this.olvColumn1, "olvColumn1");
            // 
            // __btAddResult
            // 
            resources.ApplyResources(this.@__btAddResult, "__btAddResult");
            this.@__btAddResult.Name = "__btAddResult";
            this.@__btAddResult.UseVisualStyleBackColor = true;
            this.@__btAddResult.EnabledChanged += new System.EventHandler(this.Bt_enabledChanged);
            this.@__btAddResult.Click += new System.EventHandler(this.@__btAddResult_Click);
            // 
            // __listInputs
            // 
            resources.ApplyResources(this.@__listInputs, "__listInputs");
            this.@__listInputs.AllColumns.Add(this.@__Parameter);
            this.@__listInputs.AllColumns.Add(this.@__Input);
            this.@__listInputs.AllColumns.Add(this.@__Value);
            this.@__listInputs.AllColumns.Add(this._DefaultValue);
            this.@__listInputs.AllowDrop = true;
            this.@__listInputs.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__listInputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.@__Parameter,
            this.@__Input,
            this.@__Value,
            this._DefaultValue});
            this.@__listInputs.FullRowSelect = true;
            this.@__listInputs.GridLines = true;
            this.@__listInputs.HideSelection = false;
            this.@__listInputs.IsSimpleDropSink = true;
            this.@__listInputs.LabelEdit = true;
            this.@__listInputs.MultiSelect = false;
            this.@__listInputs.Name = "__listInputs";
            this.@__listInputs.OverlayText.Text = resources.GetString("resource.Text1");
            this.@__listInputs.ShowGroups = false;
            this.@__listInputs.ShowItemToolTips = true;
            this.@__listInputs.ShowSortIndicators = false;
            this.@__listInputs.SortGroupItemsByPrimaryColumn = false;
            this.@__listInputs.UseCompatibleStateImageBehavior = false;
            this.@__listInputs.UseTranslucentSelection = true;
            this.@__listInputs.View = System.Windows.Forms.View.Details;
            this.@__listInputs.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.@__listInputs_CanDrop);
            this.@__listInputs.SelectedIndexChanged += new System.EventHandler(this.@__listInputs_SelectedIndexChanged);
            this.@__listInputs.DragDrop += new System.Windows.Forms.DragEventHandler(this.@__listInputs_DragDrop);
            this.@__listInputs.Enter += new System.EventHandler(this.@__listInputs_Enter);
            // 
            // __Parameter
            // 
            this.@__Parameter.AspectName = "Parameter";
            resources.ApplyResources(this.@__Parameter, "__Parameter");
            // 
            // __Input
            // 
            this.@__Input.AspectName = "Input";
            resources.ApplyResources(this.@__Input, "__Input");
            // 
            // __Value
            // 
            this.@__Value.AspectName = "InputValue";
            resources.ApplyResources(this.@__Value, "__Value");
            // 
            // _DefaultValue
            // 
            this._DefaultValue.AspectName = "DefaultValue";
            resources.ApplyResources(this._DefaultValue, "_DefaultValue");
            // 
            // __btRemove
            // 
            resources.ApplyResources(this.@__btRemove, "__btRemove");
            this.@__btRemove.Name = "__btRemove";
            this.@__btRemove.UseVisualStyleBackColor = true;
            this.@__btRemove.EnabledChanged += new System.EventHandler(this.Bt_enabledChanged);
            this.@__btRemove.Click += new System.EventHandler(this.@__btRemove_Click);
            // 
            // __BtRemoveInput
            // 
            resources.ApplyResources(this.@__BtRemoveInput, "__BtRemoveInput");
            this.@__BtRemoveInput.Name = "__BtRemoveInput";
            this.@__BtRemoveInput.UseVisualStyleBackColor = true;
            this.@__BtRemoveInput.EnabledChanged += new System.EventHandler(this.Bt_enabledChanged);
            this.@__BtRemoveInput.Click += new System.EventHandler(this.@__BtRemoveInput_Click);
            // 
            // __BtAddInput
            // 
            resources.ApplyResources(this.@__BtAddInput, "__BtAddInput");
            this.@__BtAddInput.Name = "__BtAddInput";
            this.@__BtAddInput.UseVisualStyleBackColor = true;
            this.@__BtAddInput.EnabledChanged += new System.EventHandler(this.Bt_enabledChanged);
            this.@__BtAddInput.Click += new System.EventHandler(this.@__BtAddInput_Click);
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatus});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // toolStripStatus
            // 
            resources.ApplyResources(this.toolStripStatus, "toolStripStatus");
            this.toolStripStatus.Name = "toolStripStatus";
            // 
            // ResultsConfiguration
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.@__BtRemoveInput);
            this.Controls.Add(this.@__BtAddInput);
            this.Controls.Add(this.@__btRemove);
            this.Controls.Add(this.@__listInputs);
            this.Controls.Add(this.@__btAddResult);
            this.Controls.Add(this.@__listinspResults);
            this.Name = "ResultsConfiguration";
            this.Load += new System.EventHandler(this.ResultsConfiguration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.@__listinspResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__listInputs)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public BrightIdeasSoftware.ObjectListView __listinspResults;
        public BrightIdeasSoftware.OLVColumn __IDName;
        public BrightIdeasSoftware.ObjectListView __listInputs;
        public BrightIdeasSoftware.OLVColumn __Parameter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        public BrightIdeasSoftware.OLVColumn __Input;
        public BrightIdeasSoftware.OLVColumn __Value;
        public System.Windows.Forms.Button __btAddResult;
        public System.Windows.Forms.Button __btRemove;
        public System.Windows.Forms.Button __BtRemoveInput;
        public System.Windows.Forms.Button __BtAddInput;
        public BrightIdeasSoftware.OLVColumn _DefaultValue;
        private BrightIdeasSoftware.OLVColumn olvColumn1;

    }
}