namespace VisionModule {
    partial class ListROIForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListROIForm));
            this.@__tabrois = new System.Windows.Forms.TabControl();
            this.@__tabROIProc = new System.Windows.Forms.TabPage();
            this.@__grpROIProc = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.@__RoiProcList = new BrightIdeasSoftware.ObjectListView();
            this._olvInspName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnProcPOS = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFuncType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label1 = new System.Windows.Forms.Label();
            this.@__btProcROI = new System.Windows.Forms.Button();
            this.@__propertyGridFunction = new System.Windows.Forms.PropertyGrid();
            this.@__btAddProc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.@__btProcFunc = new System.Windows.Forms.Button();
            this.@__btRemoveProc = new System.Windows.Forms.Button();
            this.@__cbProcFunc = new KPPAutomationCore.KPPCustomControls.KPPComboBox();
            this.@__tabrois.SuspendLayout();
            this.@__tabROIProc.SuspendLayout();
            this.@__grpROIProc.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__RoiProcList)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // __tabrois
            // 
            this.@__tabrois.Controls.Add(this.@__tabROIProc);
            resources.ApplyResources(this.@__tabrois, "__tabrois");
            this.@__tabrois.Name = "__tabrois";
            this.@__tabrois.SelectedIndex = 0;
            // 
            // __tabROIProc
            // 
            this.@__tabROIProc.Controls.Add(this.@__grpROIProc);
            resources.ApplyResources(this.@__tabROIProc, "__tabROIProc");
            this.@__tabROIProc.Name = "__tabROIProc";
            this.@__tabROIProc.UseVisualStyleBackColor = true;
            // 
            // __grpROIProc
            // 
            this.@__grpROIProc.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.@__grpROIProc, "__grpROIProc");
            this.@__grpROIProc.Name = "__grpROIProc";
            this.@__grpROIProc.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.@__RoiProcList, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.@__btProcROI, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.@__propertyGridFunction, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.@__btAddProc, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.@__cbProcFunc, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // __RoiProcList
            // 
            this.@__RoiProcList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.@__RoiProcList.AllColumns.Add(this._olvInspName);
            this.@__RoiProcList.AllColumns.Add(this.olvColumnProcPOS);
            this.@__RoiProcList.AllColumns.Add(this.olvColumnFuncType);
            this.@__RoiProcList.AllowDrop = true;
            this.@__RoiProcList.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.@__RoiProcList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._olvInspName,
            this.olvColumnProcPOS,
            this.olvColumnFuncType});
            resources.ApplyResources(this.@__RoiProcList, "__RoiProcList");
            this.@__RoiProcList.FullRowSelect = true;
            this.@__RoiProcList.GridLines = true;
            this.@__RoiProcList.HideSelection = false;
            this.@__RoiProcList.IsSimpleDropSink = true;
            this.@__RoiProcList.LabelEdit = true;
            this.@__RoiProcList.MultiSelect = false;
            this.@__RoiProcList.Name = "__RoiProcList";
            this.@__RoiProcList.ShowGroups = false;
            this.@__RoiProcList.ShowItemToolTips = true;
            this.@__RoiProcList.ShowSortIndicators = false;
            this.@__RoiProcList.SortGroupItemsByPrimaryColumn = false;
            this.@__RoiProcList.UseCompatibleStateImageBehavior = false;
            this.@__RoiProcList.UseTranslucentSelection = true;
            this.@__RoiProcList.View = System.Windows.Forms.View.Details;
            this.@__RoiProcList.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.@__RoiProcList_CellEditFinishing);
            this.@__RoiProcList.SelectedIndexChanged += new System.EventHandler(this.@__RoiProcList_SelectedIndexChanged);
            this.@__RoiProcList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.@__RoiProcList_KeyUp);
            // 
            // _olvInspName
            // 
            this._olvInspName.AspectName = "FunctionName";
            resources.ApplyResources(this._olvInspName, "_olvInspName");
            // 
            // olvColumnProcPOS
            // 
            this.olvColumnProcPOS.AspectName = "ProcPos";
            resources.ApplyResources(this.olvColumnProcPOS, "olvColumnProcPOS");
            // 
            // olvColumnFuncType
            // 
            this.olvColumnFuncType.AspectName = "FunctionType";
            resources.ApplyResources(this.olvColumnFuncType, "olvColumnFuncType");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // __btProcROI
            // 
            resources.ApplyResources(this.@__btProcROI, "__btProcROI");
            this.@__btProcROI.Name = "__btProcROI";
            this.@__btProcROI.UseVisualStyleBackColor = true;
            this.@__btProcROI.Click += new System.EventHandler(this.@__btProcROI_Click);
            // 
            // __propertyGridFunction
            // 
            resources.ApplyResources(this.@__propertyGridFunction, "__propertyGridFunction");
            this.@__propertyGridFunction.Name = "__propertyGridFunction";
            this.@__propertyGridFunction.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.@__propertyGridFunction.Click += new System.EventHandler(this.@__propertyGridFunction_Click);
            this.@__propertyGridFunction.Leave += new System.EventHandler(this.@__propertyGridFunction_Leave);
            // 
            // __btAddProc
            // 
            resources.ApplyResources(this.@__btAddProc, "__btAddProc");
            this.@__btAddProc.Name = "__btAddProc";
            this.@__btAddProc.UseVisualStyleBackColor = true;
            this.@__btAddProc.Click += new System.EventHandler(this.@__btAddProc_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.@__btProcFunc, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.@__btRemoveProc, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // __btProcFunc
            // 
            resources.ApplyResources(this.@__btProcFunc, "__btProcFunc");
            this.@__btProcFunc.Name = "__btProcFunc";
            this.@__btProcFunc.UseVisualStyleBackColor = true;
            // 
            // __btRemoveProc
            // 
            resources.ApplyResources(this.@__btRemoveProc, "__btRemoveProc");
            this.@__btRemoveProc.Name = "__btRemoveProc";
            this.@__btRemoveProc.UseVisualStyleBackColor = true;
            this.@__btRemoveProc.Click += new System.EventHandler(this.@__btRemoveProc_Click);
            // 
            // __cbProcFunc
            // 
            resources.ApplyResources(this.@__cbProcFunc, "__cbProcFunc");
            this.@__cbProcFunc.DefaultText = "Nada Selecionado";
            this.@__cbProcFunc.Name = "__cbProcFunc";
            this.@__cbProcFunc.Objects = null;
            this.@__cbProcFunc.Load += new System.EventHandler(this.@__cbProcFunc_Load);
            // 
            // ListROIForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.@__tabrois);
            this.Name = "ListROIForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListROIForm_FormClosing);
            this.Load += new System.EventHandler(this.ListROIForm_Load);
            this.Enter += new System.EventHandler(this.ListROIForm_Enter);
            this.Leave += new System.EventHandler(this.ListROIForm_Leave);
            this.MouseLeave += new System.EventHandler(this.ListROIForm_MouseLeave);
            this.@__tabrois.ResumeLayout(false);
            this.@__tabROIProc.ResumeLayout(false);
            this.@__grpROIProc.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__RoiProcList)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button __btProcROI;
        public System.Windows.Forms.TabControl __tabrois;
        public System.Windows.Forms.GroupBox __grpROIProc;
        public System.Windows.Forms.Button __btAddProc;
        public System.Windows.Forms.PropertyGrid __propertyGridFunction;
        public System.Windows.Forms.TabPage __tabROIProc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public KPPAutomationCore.KPPCustomControls.KPPComboBox __cbProcFunc;
        public BrightIdeasSoftware.ObjectListView __RoiProcList;
        public BrightIdeasSoftware.OLVColumn _olvInspName;
        public BrightIdeasSoftware.OLVColumn olvColumnProcPOS;
        public BrightIdeasSoftware.OLVColumn olvColumnFuncType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button __btProcFunc;
        public System.Windows.Forms.Button __btRemoveProc;
        private System.Windows.Forms.Label label1;
    }
}