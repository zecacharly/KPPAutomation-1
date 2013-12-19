namespace KPPAutomationCore.KPPCustomControls {
    partial class ProcessingFunctionSelection {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.@__olvColumnGroup = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__listfunc = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.@__listfunc)).BeginInit();
            this.SuspendLayout();
            // 
            // __olvColumnGroup
            // 
            this.@__olvColumnGroup.AspectName = "FunctionGroup";
            this.@__olvColumnGroup.DisplayIndex = 1;
            this.@__olvColumnGroup.IsVisible = false;
            this.@__olvColumnGroup.Text = "Group";
            // 
            // __listfunc
            // 
            this.@__listfunc.AllColumns.Add(this.olvColumn1);
            this.@__listfunc.AllColumns.Add(this.@__olvColumnGroup);
            this.@__listfunc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.@__listfunc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__listfunc.HideSelection = false;
            this.@__listfunc.Location = new System.Drawing.Point(0, 0);
            this.@__listfunc.MultiSelect = false;
            this.@__listfunc.Name = "__listfunc";
            this.@__listfunc.SelectColumnsOnRightClick = false;
            this.@__listfunc.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.@__listfunc.Size = new System.Drawing.Size(322, 268);
            this.@__listfunc.TabIndex = 1;
            this.@__listfunc.UseCompatibleStateImageBehavior = false;
            this.@__listfunc.View = System.Windows.Forms.View.Details;
            this.@__listfunc.AfterCreatingGroups += new System.EventHandler<BrightIdeasSoftware.CreateGroupsEventArgs>(this.@__listfunc_AfterCreatingGroups);
            this.@__listfunc.GroupExpandingCollapsing += new System.EventHandler<BrightIdeasSoftware.GroupExpandingCollapsingEventArgs>(this.@__listfunc_GroupExpandingCollapsing);
            this.@__listfunc.GroupStateChanged += new System.EventHandler<BrightIdeasSoftware.GroupStateChangedEventArgs>(this.@__listfunc_GroupStateChanged);
            this.@__listfunc.GroupTaskClicked += new System.EventHandler<BrightIdeasSoftware.GroupTaskClickedEventArgs>(this.@__listfunc_GroupTaskClicked);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "FunctionType";
            this.olvColumn1.Text = "Name";
            this.olvColumn1.Width = 160;
            // 
            // ProcessingFunctionSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.@__listfunc);
            this.Name = "ProcessingFunctionSelection";
            this.Size = new System.Drawing.Size(322, 268);
            ((System.ComponentModel.ISupportInitialize)(this.@__listfunc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public BrightIdeasSoftware.ObjectListView __listfunc;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        public BrightIdeasSoftware.OLVColumn __olvColumnGroup;
    }
}
