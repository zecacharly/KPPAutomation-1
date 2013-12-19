namespace VisionModule {
    partial class ListInspForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListInspForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.@__tabsInspconf = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.@__propertyGridinsp = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.@__listRoi = new BrightIdeasSoftware.ObjectListView();
            this.@__olvROIName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__olvROIPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPrePos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__olvNopart = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__contextROI = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__tooladdROI = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolRemoveRoi = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.@__ListAuxROIS = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__tabsRequest = new System.Windows.Forms.TabControl();
            this.Requeststab = new System.Windows.Forms.TabPage();
            this.@__ListRequests = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__contextRequest = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__tooladdRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolremoveRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolRequestSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.@__tabsInsp = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.@__listinspections = new BrightIdeasSoftware.ObjectListView();
            this._olvInspName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnInsppos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnID2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.@__contextInspection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__tooladdInspection = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolremoveInspection = new System.Windows.Forms.ToolStripMenuItem();
            this.@__toolInspSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.@__tabsInspconf.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__listRoi)).BeginInit();
            this.@__contextROI.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__ListAuxROIS)).BeginInit();
            this.@__tabsRequest.SuspendLayout();
            this.Requeststab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__ListRequests)).BeginInit();
            this.@__contextRequest.SuspendLayout();
            this.@__tabsInsp.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__listinspections)).BeginInit();
            this.@__contextInspection.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.@__tabsInspconf, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.@__tabsRequest, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.@__tabsInsp, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // __tabsInspconf
            // 
            this.@__tabsInspconf.Controls.Add(this.tabPage3);
            this.@__tabsInspconf.Controls.Add(this.tabPage2);
            this.@__tabsInspconf.Controls.Add(this.tabPage4);
            resources.ApplyResources(this.@__tabsInspconf, "__tabsInspconf");
            this.@__tabsInspconf.Name = "__tabsInspconf";
            this.@__tabsInspconf.SelectedIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.@__propertyGridinsp);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // __propertyGridinsp
            // 
            resources.ApplyResources(this.@__propertyGridinsp, "__propertyGridinsp");
            this.@__propertyGridinsp.Name = "__propertyGridinsp";
            this.@__propertyGridinsp.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.@__listRoi);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // __listRoi
            // 
            this.@__listRoi.AllColumns.Add(this.@__olvROIName);
            this.@__listRoi.AllColumns.Add(this.@__olvROIPos);
            this.@__listRoi.AllColumns.Add(this.olvColumnPrePos);
            this.@__listRoi.AllColumns.Add(this.@__olvNopart);
            this.@__listRoi.AllowDrop = true;
            this.@__listRoi.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__listRoi.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.@__olvROIName,
            this.@__olvROIPos,
            this.olvColumnPrePos,
            this.@__olvNopart});
            this.@__listRoi.ContextMenuStrip = this.@__contextROI;
            this.@__listRoi.CopySelectionOnControlC = false;
            resources.ApplyResources(this.@__listRoi, "__listRoi");
            this.@__listRoi.FullRowSelect = true;
            this.@__listRoi.GridLines = true;
            this.@__listRoi.HideSelection = false;
            this.@__listRoi.IsSimpleDropSink = true;
            this.@__listRoi.MultiSelect = false;
            this.@__listRoi.Name = "__listRoi";
            this.@__listRoi.ShowCommandMenuOnRightClick = true;
            this.@__listRoi.ShowGroups = false;
            this.@__listRoi.ShowItemToolTips = true;
            this.@__listRoi.ShowSortIndicators = false;
            this.@__listRoi.SortGroupItemsByPrimaryColumn = false;
            this.@__listRoi.UseCompatibleStateImageBehavior = false;
            this.@__listRoi.UseTranslucentSelection = true;
            this.@__listRoi.View = System.Windows.Forms.View.Details;
            this.@__listRoi.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.@__listRoi_CellEditFinishing);
            this.@__listRoi.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.@__listRoi_CellEditStarting);
            this.@__listRoi.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.@__listRoi_ItemSelectionChanged);
            this.@__listRoi.Click += new System.EventHandler(this.@__listRoi_Click);
            this.@__listRoi.DragOver += new System.Windows.Forms.DragEventHandler(this.@__listRoi_DragOver);
            this.@__listRoi.KeyUp += new System.Windows.Forms.KeyEventHandler(this.@__listRoi_KeyUp);
            this.@__listRoi.MouseClick += new System.Windows.Forms.MouseEventHandler(this.@__listRoi_MouseClick);
            // 
            // __olvROIName
            // 
            this.@__olvROIName.AspectName = "Name";
            resources.ApplyResources(this.@__olvROIName, "__olvROIName");
            // 
            // __olvROIPos
            // 
            this.@__olvROIPos.AspectName = "ROIPos";
            resources.ApplyResources(this.@__olvROIPos, "__olvROIPos");
            // 
            // olvColumnPrePos
            // 
            this.olvColumnPrePos.AspectName = "referencePoint";
            resources.ApplyResources(this.olvColumnPrePos, "olvColumnPrePos");
            // 
            // __olvNopart
            // 
            this.@__olvNopart.AspectName = "NoPartCheck";
            resources.ApplyResources(this.@__olvNopart, "__olvNopart");
            // 
            // __contextROI
            // 
            this.@__contextROI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__tooladdROI,
            this.@__toolRemoveRoi});
            this.@__contextROI.Name = "__contextAddInsp";
            resources.ApplyResources(this.@__contextROI, "__contextROI");
            // 
            // __tooladdROI
            // 
            resources.ApplyResources(this.@__tooladdROI, "__tooladdROI");
            this.@__tooladdROI.Image = global::VisionModule.Properties.Resources.Actions_list_add_icon;
            this.@__tooladdROI.Name = "__tooladdROI";
            this.@__tooladdROI.Click += new System.EventHandler(this.@__tooladdROI_Click);
            // 
            // __toolRemoveRoi
            // 
            resources.ApplyResources(this.@__toolRemoveRoi, "__toolRemoveRoi");
            this.@__toolRemoveRoi.Image = global::VisionModule.Properties.Resources.Actions_remove_icon;
            this.@__toolRemoveRoi.Name = "__toolRemoveRoi";
            this.@__toolRemoveRoi.Click += new System.EventHandler(this.@__toolRemoveRoi_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.@__ListAuxROIS);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // __ListAuxROIS
            // 
            this.@__ListAuxROIS.AllColumns.Add(this.olvColumn1);
            this.@__ListAuxROIS.AllowDrop = true;
            this.@__ListAuxROIS.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__ListAuxROIS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.@__ListAuxROIS.CopySelectionOnControlC = false;
            resources.ApplyResources(this.@__ListAuxROIS, "__ListAuxROIS");
            this.@__ListAuxROIS.FullRowSelect = true;
            this.@__ListAuxROIS.GridLines = true;
            this.@__ListAuxROIS.HideSelection = false;
            this.@__ListAuxROIS.IsSimpleDropSink = true;
            this.@__ListAuxROIS.MultiSelect = false;
            this.@__ListAuxROIS.Name = "__ListAuxROIS";
            this.@__ListAuxROIS.ShowCommandMenuOnRightClick = true;
            this.@__ListAuxROIS.ShowGroups = false;
            this.@__ListAuxROIS.ShowItemToolTips = true;
            this.@__ListAuxROIS.ShowSortIndicators = false;
            this.@__ListAuxROIS.SortGroupItemsByPrimaryColumn = false;
            this.@__ListAuxROIS.UseCompatibleStateImageBehavior = false;
            this.@__ListAuxROIS.UseTranslucentSelection = true;
            this.@__ListAuxROIS.View = System.Windows.Forms.View.Details;
            this.@__ListAuxROIS.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.@__ListAuxROIS_ItemSelectionChanged);
            this.@__ListAuxROIS.Click += new System.EventHandler(this.@__ListAuxROIS_Click);
            this.@__ListAuxROIS.MouseClick += new System.Windows.Forms.MouseEventHandler(this.@__ListAuxROIS_MouseClick);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Name";
            this.olvColumn1.IsEditable = false;
            resources.ApplyResources(this.olvColumn1, "olvColumn1");
            // 
            // __tabsRequest
            // 
            this.@__tabsRequest.Controls.Add(this.Requeststab);
            resources.ApplyResources(this.@__tabsRequest, "__tabsRequest");
            this.@__tabsRequest.Name = "__tabsRequest";
            this.@__tabsRequest.SelectedIndex = 0;
            // 
            // Requeststab
            // 
            this.Requeststab.Controls.Add(this.@__ListRequests);
            resources.ApplyResources(this.Requeststab, "Requeststab");
            this.Requeststab.Name = "Requeststab";
            this.Requeststab.UseVisualStyleBackColor = true;
            // 
            // __ListRequests
            // 
            this.@__ListRequests.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.@__ListRequests.AllColumns.Add(this.olvColumn2);
            this.@__ListRequests.AllColumns.Add(this.olvColumn3);
            this.@__ListRequests.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__ListRequests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2,
            this.olvColumn3});
            this.@__ListRequests.ContextMenuStrip = this.@__contextRequest;
            resources.ApplyResources(this.@__ListRequests, "__ListRequests");
            this.@__ListRequests.FullRowSelect = true;
            this.@__ListRequests.GridLines = true;
            this.@__ListRequests.HideSelection = false;
            this.@__ListRequests.IsSimpleDropSink = true;
            this.@__ListRequests.LabelEdit = true;
            this.@__ListRequests.MultiSelect = false;
            this.@__ListRequests.Name = "__ListRequests";
            this.@__ListRequests.ShowCommandMenuOnRightClick = true;
            this.@__ListRequests.ShowGroups = false;
            this.@__ListRequests.ShowItemToolTips = true;
            this.@__ListRequests.ShowSortIndicators = false;
            this.@__ListRequests.SortGroupItemsByPrimaryColumn = false;
            this.@__ListRequests.UseCompatibleStateImageBehavior = false;
            this.@__ListRequests.UseTranslucentSelection = true;
            this.@__ListRequests.View = System.Windows.Forms.View.Details;
            this.@__ListRequests.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.@__ListRequests_CellEditFinishing);
            this.@__ListRequests.SelectionChanged += new System.EventHandler(this.@__ListRequests_SelectionChanged);
            this.@__ListRequests.DragOver += new System.Windows.Forms.DragEventHandler(this.@__ListRequests_DragOver);
            this.@__ListRequests.KeyUp += new System.Windows.Forms.KeyEventHandler(this.@__ListRequests_KeyUp);
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Name";
            resources.ApplyResources(this.olvColumn2, "olvColumn2");
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "ID";
            resources.ApplyResources(this.olvColumn3, "olvColumn3");
            // 
            // __contextRequest
            // 
            this.@__contextRequest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__tooladdRequest,
            this.@__toolremoveRequest,
            this.@__toolRequestSettings});
            this.@__contextRequest.Name = "__contextAddInsp";
            resources.ApplyResources(this.@__contextRequest, "__contextRequest");
            // 
            // __tooladdRequest
            // 
            resources.ApplyResources(this.@__tooladdRequest, "__tooladdRequest");
            this.@__tooladdRequest.Image = global::VisionModule.Properties.Resources.Actions_list_add_icon;
            this.@__tooladdRequest.Name = "__tooladdRequest";
            this.@__tooladdRequest.Click += new System.EventHandler(this.@__tooladdRequest_Click);
            // 
            // __toolremoveRequest
            // 
            resources.ApplyResources(this.@__toolremoveRequest, "__toolremoveRequest");
            this.@__toolremoveRequest.Image = global::VisionModule.Properties.Resources.Actions_remove_icon;
            this.@__toolremoveRequest.Name = "__toolremoveRequest";
            this.@__toolremoveRequest.Click += new System.EventHandler(this.@__toolremoveRequest_Click);
            // 
            // __toolRequestSettings
            // 
            resources.ApplyResources(this.@__toolRequestSettings, "__toolRequestSettings");
            this.@__toolRequestSettings.Image = global::VisionModule.Properties.Resources.Settings_icon2;
            this.@__toolRequestSettings.Name = "__toolRequestSettings";
            this.@__toolRequestSettings.Click += new System.EventHandler(this.@__toolRequestSettings_Click);
            // 
            // __tabsInsp
            // 
            this.@__tabsInsp.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.@__tabsInsp, "__tabsInsp");
            this.@__tabsInsp.Name = "__tabsInsp";
            this.@__tabsInsp.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.@__listinspections);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // __listinspections
            // 
            this.@__listinspections.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.@__listinspections.AllColumns.Add(this._olvInspName);
            this.@__listinspections.AllColumns.Add(this.olvColumnInsppos);
            this.@__listinspections.AllColumns.Add(this.olvColumnID2);
            this.@__listinspections.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__listinspections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._olvInspName,
            this.olvColumnInsppos});
            this.@__listinspections.ContextMenuStrip = this.@__contextInspection;
            resources.ApplyResources(this.@__listinspections, "__listinspections");
            this.@__listinspections.FullRowSelect = true;
            this.@__listinspections.GridLines = true;
            this.@__listinspections.HideSelection = false;
            this.@__listinspections.IsSimpleDropSink = true;
            this.@__listinspections.LabelEdit = true;
            this.@__listinspections.MultiSelect = false;
            this.@__listinspections.Name = "__listinspections";
            this.@__listinspections.ShowGroups = false;
            this.@__listinspections.ShowItemToolTips = true;
            this.@__listinspections.ShowSortIndicators = false;
            this.@__listinspections.SortGroupItemsByPrimaryColumn = false;
            this.@__listinspections.UseCompatibleStateImageBehavior = false;
            this.@__listinspections.UseTranslucentSelection = true;
            this.@__listinspections.View = System.Windows.Forms.View.Details;
            this.@__listinspections.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.@__listinspections_CellEditFinishing);
            this.@__listinspections.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.@__listinspections_CellEditStarting);
            this.@__listinspections.SelectionChanged += new System.EventHandler(this.@__listinspections_SelectionChanged);
            this.@__listinspections.DragOver += new System.Windows.Forms.DragEventHandler(this.@__listinspections_DragOver);
            this.@__listinspections.KeyUp += new System.Windows.Forms.KeyEventHandler(this.@__listinspections_KeyUp);
            // 
            // _olvInspName
            // 
            this._olvInspName.AspectName = "Name";
            resources.ApplyResources(this._olvInspName, "_olvInspName");
            // 
            // olvColumnInsppos
            // 
            this.olvColumnInsppos.AspectName = "InspPos";
            resources.ApplyResources(this.olvColumnInsppos, "olvColumnInsppos");
            // 
            // olvColumnID2
            // 
            this.olvColumnID2.AspectName = "ID";
            resources.ApplyResources(this.olvColumnID2, "olvColumnID2");
            this.olvColumnID2.IsEditable = false;
            this.olvColumnID2.IsVisible = false;
            // 
            // __contextInspection
            // 
            this.@__contextInspection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__tooladdInspection,
            this.@__toolremoveInspection,
            this.@__toolInspSettings});
            this.@__contextInspection.Name = "__contextAddInsp";
            resources.ApplyResources(this.@__contextInspection, "__contextInspection");
            // 
            // __tooladdInspection
            // 
            resources.ApplyResources(this.@__tooladdInspection, "__tooladdInspection");
            this.@__tooladdInspection.Image = global::VisionModule.Properties.Resources.Actions_list_add_icon;
            this.@__tooladdInspection.Name = "__tooladdInspection";
            this.@__tooladdInspection.Click += new System.EventHandler(this.@__tooladdInspection_Click);
            // 
            // __toolremoveInspection
            // 
            resources.ApplyResources(this.@__toolremoveInspection, "__toolremoveInspection");
            this.@__toolremoveInspection.Image = global::VisionModule.Properties.Resources.Actions_remove_icon;
            this.@__toolremoveInspection.Name = "__toolremoveInspection";
            this.@__toolremoveInspection.Click += new System.EventHandler(this.@__toolremoveInspection_Click);
            // 
            // __toolInspSettings
            // 
            resources.ApplyResources(this.@__toolInspSettings, "__toolInspSettings");
            this.@__toolInspSettings.Image = global::VisionModule.Properties.Resources.Settings_icon2;
            this.@__toolInspSettings.Name = "__toolInspSettings";
            this.@__toolInspSettings.Click += new System.EventHandler(this.@__toolInspSettings_Click);
            // 
            // ListInspForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.panel1);
            this.Name = "ListInspForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListInspForm_FormClosing);
            this.Load += new System.EventHandler(this.ListInspForm_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.@__tabsInspconf.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__listRoi)).EndInit();
            this.@__contextROI.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__ListAuxROIS)).EndInit();
            this.@__tabsRequest.ResumeLayout(false);
            this.Requeststab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__ListRequests)).EndInit();
            this.@__contextRequest.ResumeLayout(false);
            this.@__tabsInsp.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__listinspections)).EndInit();
            this.@__contextInspection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ContextMenuStrip __contextRequest;
        public System.Windows.Forms.ToolStripMenuItem __tooladdRequest;
        public System.Windows.Forms.ToolStripMenuItem __toolremoveRequest;
        public System.Windows.Forms.ContextMenuStrip __contextInspection;
        public System.Windows.Forms.ToolStripMenuItem __tooladdInspection;
        public System.Windows.Forms.ToolStripMenuItem __toolremoveInspection;
        public System.Windows.Forms.ContextMenuStrip __contextROI;
        public System.Windows.Forms.ToolStripMenuItem __tooladdROI;
        public System.Windows.Forms.ToolStripMenuItem __toolRemoveRoi;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPage2;
        public BrightIdeasSoftware.ObjectListView __listRoi;
        public BrightIdeasSoftware.OLVColumn __olvROIName;
        public BrightIdeasSoftware.OLVColumn __olvROIPos;
        public BrightIdeasSoftware.OLVColumn olvColumnPrePos;
        private BrightIdeasSoftware.OLVColumn __olvNopart;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.PropertyGrid __propertyGridinsp;
        private System.Windows.Forms.TabPage Requeststab;
        public BrightIdeasSoftware.ObjectListView __ListRequests;
        public BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private System.Windows.Forms.TabPage tabPage1;
        public BrightIdeasSoftware.ObjectListView __listinspections;
        public BrightIdeasSoftware.OLVColumn _olvInspName;
        public BrightIdeasSoftware.OLVColumn olvColumnInsppos;
        private BrightIdeasSoftware.OLVColumn olvColumnID2;
        public System.Windows.Forms.TabControl __tabsRequest;
        public System.Windows.Forms.TabControl __tabsInsp;
        public System.Windows.Forms.TabControl __tabsInspconf;
        public System.Windows.Forms.ToolStripMenuItem __toolRequestSettings;
        public System.Windows.Forms.ToolStripMenuItem __toolInspSettings;
        private System.Windows.Forms.TabPage tabPage4;
        public BrightIdeasSoftware.ObjectListView __ListAuxROIS;
        public BrightIdeasSoftware.OLVColumn olvColumn1;

    }
}