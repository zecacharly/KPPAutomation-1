using KPP.Controls.Winforms;

namespace VisionModule {
    partial class ImageContainerForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageContainerForm));
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.@__openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.@__toolZoomIn = new System.Windows.Forms.ToolStripButton();
            this.@__toolZoomOut = new System.Windows.Forms.ToolStripButton();
            this.@__toolZoomFit = new System.Windows.Forms.ToolStripButton();
            this.@__toolZoomOriginal = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveImageOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.originalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImagetoolStrip = new System.Windows.Forms.ToolStrip();
            this.@__OpentoolStrip = new System.Windows.Forms.ToolStripDropDownButton();
            this.newImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._lastcaptureimages = new System.Windows.Forms.ToolStripMenuItem();
            this.refeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__setCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.@__refImagetool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.@__addrectROI = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.@__roicontainer = new KPP.Controls.Winforms.ImageEditor();
            this.ImagetoolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // _saveFileDialog1
            // 
            resources.ApplyResources(this._saveFileDialog1, "_saveFileDialog1");
            // 
            // __openFileDialog
            // 
            resources.ApplyResources(this.@__openFileDialog, "__openFileDialog");
            // 
            // __toolZoomIn
            // 
            resources.ApplyResources(this.@__toolZoomIn, "__toolZoomIn");
            this.@__toolZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__toolZoomIn.Name = "__toolZoomIn";
            this.@__toolZoomIn.Click += new System.EventHandler(this.@__toolZoomIn_Click);
            // 
            // __toolZoomOut
            // 
            resources.ApplyResources(this.@__toolZoomOut, "__toolZoomOut");
            this.@__toolZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__toolZoomOut.Name = "__toolZoomOut";
            this.@__toolZoomOut.Click += new System.EventHandler(this.@__toolZoomOut_Click);
            // 
            // __toolZoomFit
            // 
            resources.ApplyResources(this.@__toolZoomFit, "__toolZoomFit");
            this.@__toolZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__toolZoomFit.Name = "__toolZoomFit";
            this.@__toolZoomFit.Click += new System.EventHandler(this.@__toolZoomFit_Click);
            // 
            // __toolZoomOriginal
            // 
            resources.ApplyResources(this.@__toolZoomOriginal, "__toolZoomOriginal");
            this.@__toolZoomOriginal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__toolZoomOriginal.Name = "__toolZoomOriginal";
            this.@__toolZoomOriginal.Click += new System.EventHandler(this.@__toolZoomOriginal_Click);
            // 
            // toolStripZoomIn
            // 
            resources.ApplyResources(this.toolStripZoomIn, "toolStripZoomIn");
            this.toolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Click += new System.EventHandler(this.@__toolZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            resources.ApplyResources(this.toolStripZoomOut, "toolStripZoomOut");
            this.toolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Click += new System.EventHandler(this.@__toolZoomOut_Click);
            // 
            // toolStripZoomFit
            // 
            resources.ApplyResources(this.toolStripZoomFit, "toolStripZoomFit");
            this.toolStripZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomFit.Name = "toolStripZoomFit";
            this.toolStripZoomFit.Click += new System.EventHandler(this.@__toolZoomFit_Click);
            // 
            // toolStripZoomAll
            // 
            resources.ApplyResources(this.toolStripZoomAll, "toolStripZoomAll");
            this.toolStripZoomAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomAll.Name = "toolStripZoomAll";
            this.toolStripZoomAll.Click += new System.EventHandler(this.@__toolZoomOriginal_Click);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // toolStripButton5
            // 
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageOptionsToolStripMenuItem,
            this.toolStripSeparator9,
            this.originalToolStripMenuItem,
            this.redChannelToolStripMenuItem,
            this.greenChannelToolStripMenuItem,
            this.blueChannelToolStripMenuItem});
            this.toolStripButton5.Name = "toolStripButton5";
            // 
            // saveImageOptionsToolStripMenuItem
            // 
            resources.ApplyResources(this.saveImageOptionsToolStripMenuItem, "saveImageOptionsToolStripMenuItem");
            this.saveImageOptionsToolStripMenuItem.Name = "saveImageOptionsToolStripMenuItem";
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // originalToolStripMenuItem
            // 
            resources.ApplyResources(this.originalToolStripMenuItem, "originalToolStripMenuItem");
            this.originalToolStripMenuItem.Name = "originalToolStripMenuItem";
            // 
            // redChannelToolStripMenuItem
            // 
            resources.ApplyResources(this.redChannelToolStripMenuItem, "redChannelToolStripMenuItem");
            this.redChannelToolStripMenuItem.Name = "redChannelToolStripMenuItem";
            // 
            // greenChannelToolStripMenuItem
            // 
            resources.ApplyResources(this.greenChannelToolStripMenuItem, "greenChannelToolStripMenuItem");
            this.greenChannelToolStripMenuItem.Name = "greenChannelToolStripMenuItem";
            // 
            // blueChannelToolStripMenuItem
            // 
            resources.ApplyResources(this.blueChannelToolStripMenuItem, "blueChannelToolStripMenuItem");
            this.blueChannelToolStripMenuItem.Name = "blueChannelToolStripMenuItem";
            // 
            // ImagetoolStrip
            // 
            resources.ApplyResources(this.ImagetoolStrip, "ImagetoolStrip");
            this.ImagetoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripZoomFit,
            this.toolStripZoomAll,
            this.toolStripSeparator8,
            this.toolStripButton5,
            this.@__OpentoolStrip,
            this.toolStripSeparator5,
            this.@__addrectROI,
            this.toolStripSeparator6});
            this.ImagetoolStrip.Name = "ImagetoolStrip";
            // 
            // __OpentoolStrip
            // 
            resources.ApplyResources(this.@__OpentoolStrip, "__OpentoolStrip");
            this.@__OpentoolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__OpentoolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newImageFileToolStripMenuItem,
            this._lastcaptureimages,
            this.refeToolStripMenuItem});
            this.@__OpentoolStrip.Image = global::VisionModule.Properties.Resources.Open_icon;
            this.@__OpentoolStrip.Name = "__OpentoolStrip";
            this.@__OpentoolStrip.Click += new System.EventHandler(this.OpentoolStrip_Click);
            // 
            // newImageFileToolStripMenuItem
            // 
            resources.ApplyResources(this.newImageFileToolStripMenuItem, "newImageFileToolStripMenuItem");
            this.newImageFileToolStripMenuItem.Name = "newImageFileToolStripMenuItem";
            this.newImageFileToolStripMenuItem.Click += new System.EventHandler(this.newImageFileToolStripMenuItem_Click);
            // 
            // _lastcaptureimages
            // 
            resources.ApplyResources(this._lastcaptureimages, "_lastcaptureimages");
            this._lastcaptureimages.Name = "_lastcaptureimages";
            // 
            // refeToolStripMenuItem
            // 
            resources.ApplyResources(this.refeToolStripMenuItem, "refeToolStripMenuItem");
            this.refeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__setCurrentToolStripMenuItem,
            this.toolStripSeparator7,
            this.@__refImagetool});
            this.refeToolStripMenuItem.Name = "refeToolStripMenuItem";
            // 
            // __setCurrentToolStripMenuItem
            // 
            resources.ApplyResources(this.@__setCurrentToolStripMenuItem, "__setCurrentToolStripMenuItem");
            this.@__setCurrentToolStripMenuItem.Name = "__setCurrentToolStripMenuItem";
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // __refImagetool
            // 
            resources.ApplyResources(this.@__refImagetool, "__refImagetool");
            this.@__refImagetool.Name = "__refImagetool";
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // __addrectROI
            // 
            resources.ApplyResources(this.@__addrectROI, "__addrectROI");
            this.@__addrectROI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.@__addrectROI.Name = "__addrectROI";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // __roicontainer
            // 
            resources.ApplyResources(this.@__roicontainer, "__roicontainer");
            this.@__roicontainer.Active = false;
            this.@__roicontainer.AllowDrop = true;
            this.@__roicontainer.BackColor = System.Drawing.Color.AliceBlue;
            this.@__roicontainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.@__roicontainer.GridSpace = 10;
            this.@__roicontainer.ImageSize = new System.Drawing.Size(0, 0);
            this.@__roicontainer.MouseGuidesColor = System.Drawing.Color.LightGray;
            this.@__roicontainer.MouseGuidesThickness = 1;
            this.@__roicontainer.Multiselect = false;
            this.@__roicontainer.Name = "__roicontainer";
            this.@__roicontainer.ReferenceRectangleDisplacement = new System.Drawing.Point(0, 0);
            this.@__roicontainer.ReferenceRectangleSize = new System.Drawing.Size(0, 0);
            this.@__roicontainer.ShowGrid = false;
            this.@__roicontainer.ShowMouseGuides = false;
            this.@__roicontainer.ShowReferenceRectangle = false;
            this.@__roicontainer.ShowRulers = false;
            this.@__roicontainer.OnSelectedShapesChanged += new KPP.Controls.Winforms.OnSelectedShapesChangedHandler(this.@__roicontainer_OnSelectedShapesChanged);
            this.@__roicontainer.OnShapeSizeChanging += new KPP.Controls.Winforms.OnShapeSizeChangingHandler(this.@__roicontainer_OnShapeSizeChanging);
            this.@__roicontainer.OnShapeLocationChanging += new KPP.Controls.Winforms.OnShapeLocationChangingHandler(this.@__roicontainer_OnShapeLocationChanging);
            this.@__roicontainer.OnShapeSizeChanged += new KPP.Controls.Winforms.ImageEditorObjs.OnShapeSizeChangedHandler(this.@__roicontainer_OnShapeSizeChanged);
            this.@__roicontainer.OnShapeLocationChanged += new KPP.Controls.Winforms.ImageEditorObjs.OnShapeLocationChangedHandler(this.@__roicontainer_OnShapeLocationChanged);
            this.@__roicontainer.BackgroundImageChanged += new System.EventHandler(this.@__roicontainer_BackgroundImageChanged);
            this.@__roicontainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.@__roicontainer_DragDrop);
            this.@__roicontainer.DragOver += new System.Windows.Forms.DragEventHandler(this.@__roicontainer_DragOver);
            this.@__roicontainer.DragLeave += new System.EventHandler(this.@__roicontainer_DragLeave);
           
            this.@__roicontainer.MouseHover += new System.EventHandler(this.@__roicontainer_MouseHover);
            // 
            // ImageContainerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.@__roicontainer);
            this.Controls.Add(this.ImagetoolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Name = "ImageContainerForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageContainerForm_FormClosing);
            this.Load += new System.EventHandler(this.ImageContainerForm_Load);
            this.ImagetoolStrip.ResumeLayout(false);
            this.ImagetoolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton __toolZoomIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton __toolZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton __toolZoomFit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton __toolZoomOriginal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog1;
        public System.Windows.Forms.ToolStripButton toolStripZoomIn;
        public System.Windows.Forms.ToolStripButton toolStripZoomOut;
        public System.Windows.Forms.ToolStripButton toolStripZoomFit;
        public System.Windows.Forms.ToolStripButton toolStripZoomAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton5;
        private System.Windows.Forms.ToolStripMenuItem saveImageOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        public System.Windows.Forms.ToolStripMenuItem originalToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem redChannelToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem greenChannelToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem blueChannelToolStripMenuItem;
        public System.Windows.Forms.ToolStrip ImagetoolStrip;
        public System.Windows.Forms.OpenFileDialog __openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripButton __addrectROI;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        public System.Windows.Forms.ToolStripDropDownButton __OpentoolStrip;
        private System.Windows.Forms.ToolStripMenuItem newImageFileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem _lastcaptureimages;
        private System.Windows.Forms.ToolStripMenuItem refeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        public System.Windows.Forms.ToolStripMenuItem __refImagetool;
        public System.Windows.Forms.ToolStripMenuItem __setCurrentToolStripMenuItem;
        public ImageEditor __roicontainer;
        

    }
}