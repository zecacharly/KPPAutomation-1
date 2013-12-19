namespace KPPAutomationCore {
    partial class UndoRedoControl {
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
            this.@__btok = new System.Windows.Forms.Button();
            this.@__history = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.@__history)).BeginInit();
            this.SuspendLayout();
            // 
            // __btok
            // 
            this.@__btok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.@__btok.Location = new System.Drawing.Point(3, 182);
            this.@__btok.Name = "__btok";
            this.@__btok.Size = new System.Drawing.Size(346, 29);
            this.@__btok.TabIndex = 2;
            this.@__btok.Text = "OK";
            this.@__btok.UseVisualStyleBackColor = true;
            // 
            // __history
            // 
            this.@__history.AllColumns.Add(this.olvColumn1);
            this.@__history.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.@__history.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.@__history.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__history.Location = new System.Drawing.Point(3, 3);
            this.@__history.Name = "__history";
            this.@__history.ShowGroups = false;
            this.@__history.ShowSortIndicators = false;
            this.@__history.Size = new System.Drawing.Size(346, 173);
            this.@__history.SortGroupItemsByPrimaryColumn = false;
            this.@__history.TabIndex = 3;
            this.@__history.UseCompatibleStateImageBehavior = false;
            this.@__history.UseCustomSelectionColors = true;
            this.@__history.UseTranslucentSelection = true;
            this.@__history.View = System.Windows.Forms.View.Details;
            this.@__history.BeforeSorting += new System.EventHandler<BrightIdeasSoftware.BeforeSortingEventArgs>(this.@__history_BeforeSorting);
            this.@__history.CellClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.@__history_CellClick);
            this.@__history.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.@__history_ColumnClick);
            this.@__history.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.@__history_DrawItem);
            this.@__history.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.@__history_ItemMouseHover);
            this.@__history.SelectedIndexChanged += new System.EventHandler(this.@__history_SelectedIndexChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "HistoryCaption1";
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.MaximumWidth = 340;
            this.olvColumn1.MinimumWidth = 340;
            this.olvColumn1.Width = 340;
            // 
            // UndoRedoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.@__history);
            this.Controls.Add(this.@__btok);
            this.Name = "UndoRedoControl";
            this.Size = new System.Drawing.Size(352, 214);
            ((System.ComponentModel.ISupportInitialize)(this.@__history)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button __btok;
        public BrightIdeasSoftware.OLVColumn olvColumn1;
        public BrightIdeasSoftware.ObjectListView __history;




    }
}
