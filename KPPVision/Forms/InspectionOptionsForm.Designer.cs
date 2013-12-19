namespace VisionModule {
    partial class InspectionOptionsForm {
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
            this.@__InspOptions = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.@__tabSource = new System.Windows.Forms.TabPage();
            this.@__tCsource = new System.Windows.Forms.TabControl();
            this.@__tabfile = new System.Windows.Forms.TabPage();
            this.@__openFileLoc = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.@__EditFileLoc = new System.Windows.Forms.TextBox();
            this.@__tabcam = new System.Windows.Forms.TabPage();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.@__tabROI = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.@__grpROIProc = new System.Windows.Forms.GroupBox();
            this.@__threshold = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.@__cbProcFunc = new System.Windows.Forms.ComboBox();
            this.@__btProcROI = new System.Windows.Forms.Button();
            this._DialogFileLoc = new System.Windows.Forms.OpenFileDialog();
            this._InspectionBindigSource = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._ROIBindigSource = new System.Windows.Forms.BindingSource(this.components);
            this.@__InspOptions.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.@__tabSource.SuspendLayout();
            this.@__tCsource.SuspendLayout();
            this.@__tabfile.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.@__tabcam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.@__tabROI.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.@__grpROIProc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._InspectionBindigSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ROIBindigSource)).BeginInit();
            this.SuspendLayout();
            // 
            // __InspOptions
            // 
            this.@__InspOptions.Controls.Add(this.tabControl2);
            this.@__InspOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__InspOptions.Location = new System.Drawing.Point(0, 0);
            this.@__InspOptions.Name = "__InspOptions";
            this.@__InspOptions.Size = new System.Drawing.Size(1004, 762);
            this.@__InspOptions.TabIndex = 3;
            this.@__InspOptions.TabStop = false;
            this.@__InspOptions.Text = "Inspection Options";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.@__tabSource);
            this.tabControl2.Controls.Add(this.@__tabROI);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 16);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(998, 743);
            this.tabControl2.TabIndex = 0;
            // 
            // __tabSource
            // 
            this.@__tabSource.Controls.Add(this.@__tCsource);
            this.@__tabSource.Location = new System.Drawing.Point(4, 22);
            this.@__tabSource.Name = "__tabSource";
            this.@__tabSource.Padding = new System.Windows.Forms.Padding(3);
            this.@__tabSource.Size = new System.Drawing.Size(990, 717);
            this.@__tabSource.TabIndex = 0;
            this.@__tabSource.Text = "Source";
            this.@__tabSource.UseVisualStyleBackColor = true;
            // 
            // __tCsource
            // 
            this.@__tCsource.Controls.Add(this.@__tabfile);
            this.@__tCsource.Controls.Add(this.@__tabcam);
            this.@__tCsource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__tCsource.Location = new System.Drawing.Point(3, 3);
            this.@__tCsource.Name = "__tCsource";
            this.@__tCsource.SelectedIndex = 0;
            this.@__tCsource.Size = new System.Drawing.Size(984, 711);
            this.@__tCsource.TabIndex = 2;
            // 
            // __tabfile
            // 
            this.@__tabfile.Controls.Add(this.@__openFileLoc);
            this.@__tabfile.Controls.Add(this.groupBox3);
            this.@__tabfile.Location = new System.Drawing.Point(4, 22);
            this.@__tabfile.Name = "__tabfile";
            this.@__tabfile.Padding = new System.Windows.Forms.Padding(3);
            this.@__tabfile.Size = new System.Drawing.Size(976, 685);
            this.@__tabfile.TabIndex = 0;
            this.@__tabfile.Text = "File";
            this.@__tabfile.UseVisualStyleBackColor = true;
            // 
            // __openFileLoc
            // 
            this.@__openFileLoc.Location = new System.Drawing.Point(391, 8);
            this.@__openFileLoc.Name = "__openFileLoc";
            this.@__openFileLoc.Size = new System.Drawing.Size(72, 47);
            this.@__openFileLoc.TabIndex = 1;
            this.@__openFileLoc.Text = "Open";
            this.@__openFileLoc.UseVisualStyleBackColor = true;
            this.@__openFileLoc.Click += new System.EventHandler(this.@__openFileLoc_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.@__EditFileLoc);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 50);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Location";
            // 
            // __EditFileLoc
            // 
            this.@__EditFileLoc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._InspectionBindigSource, "FileLocation", true));
            this.@__EditFileLoc.Location = new System.Drawing.Point(6, 19);
            this.@__EditFileLoc.Name = "__EditFileLoc";
            this.@__EditFileLoc.ReadOnly = true;
            this.@__EditFileLoc.Size = new System.Drawing.Size(365, 20);
            this.@__EditFileLoc.TabIndex = 2;
            // 
            // __tabcam
            // 
            this.@__tabcam.Controls.Add(this.numericUpDown1);
            this.@__tabcam.Location = new System.Drawing.Point(4, 22);
            this.@__tabcam.Name = "__tabcam";
            this.@__tabcam.Padding = new System.Windows.Forms.Padding(3);
            this.@__tabcam.Size = new System.Drawing.Size(976, 685);
            this.@__tabcam.TabIndex = 1;
            this.@__tabcam.Text = "Camera";
            this.@__tabcam.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(6, 6);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(73, 20);
            this.numericUpDown1.TabIndex = 3;
            // 
            // __tabROI
            // 
            this.@__tabROI.Controls.Add(this.splitContainer1);
            this.@__tabROI.Location = new System.Drawing.Point(4, 22);
            this.@__tabROI.Name = "__tabROI";
            this.@__tabROI.Padding = new System.Windows.Forms.Padding(3);
            this.@__tabROI.Size = new System.Drawing.Size(990, 717);
            this.@__tabROI.TabIndex = 1;
            this.@__tabROI.Text = "ROI\'s";
            this.@__tabROI.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.@__grpROIProc);
            this.splitContainer1.Size = new System.Drawing.Size(984, 711);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 711);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ROI Image";
            // 
            // __grpROIProc
            // 
            this.@__grpROIProc.Controls.Add(this.@__threshold);
            this.@__grpROIProc.Controls.Add(this.label1);
            this.@__grpROIProc.Controls.Add(this.@__cbProcFunc);
            this.@__grpROIProc.Controls.Add(this.@__btProcROI);
            this.@__grpROIProc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__grpROIProc.Location = new System.Drawing.Point(0, 0);
            this.@__grpROIProc.Name = "__grpROIProc";
            this.@__grpROIProc.Size = new System.Drawing.Size(673, 711);
            this.@__grpROIProc.TabIndex = 8;
            this.@__grpROIProc.TabStop = false;
            this.@__grpROIProc.Text = "ROI Processing";
            // 
            // __threshold
            // 
            this.@__threshold.Location = new System.Drawing.Point(216, 33);
            this.@__threshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.@__threshold.Name = "__threshold";
            this.@__threshold.Size = new System.Drawing.Size(64, 20);
            this.@__threshold.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Processing Functions";
            // 
            // __cbProcFunc
            // 
            this.@__cbProcFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__cbProcFunc.FormattingEnabled = true;
            this.@__cbProcFunc.Location = new System.Drawing.Point(9, 32);
            this.@__cbProcFunc.Name = "__cbProcFunc";
            this.@__cbProcFunc.Size = new System.Drawing.Size(163, 21);
            this.@__cbProcFunc.TabIndex = 8;
            // 
            // __btProcROI
            // 
            this.@__btProcROI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.@__btProcROI.Location = new System.Drawing.Point(3, 678);
            this.@__btProcROI.Name = "__btProcROI";
            this.@__btProcROI.Size = new System.Drawing.Size(667, 30);
            this.@__btProcROI.TabIndex = 7;
            this.@__btProcROI.Text = "Process ROI";
            this.@__btProcROI.UseVisualStyleBackColor = true;
            // 
            // _DialogFileLoc
            // 
            this._DialogFileLoc.Filter = "Image Files | *.bmp;*.jpeg;*.jpg";
            // 
            // _InspectionBindigSource
            // 
            this._InspectionBindigSource.DataSource = typeof(Inspection);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(301, 692);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // _ROIBindigSource
            // 
            this._ROIBindigSource.DataSource = typeof(ROI);
            // 
            // InspectionOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 762);
            this.Controls.Add(this.@__InspOptions);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InspectionOptionsForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.Text = "Inspection Options";
            this.Load += new System.EventHandler(this.InspectionOptionsForm_Load);
            this.@__InspOptions.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.@__tabSource.ResumeLayout(false);
            this.@__tCsource.ResumeLayout(false);
            this.@__tabfile.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.@__tabcam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.@__tabROI.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.@__grpROIProc.ResumeLayout(false);
            this.@__grpROIProc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._InspectionBindigSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ROIBindigSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox __InspOptions;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage __tabSource;
        public System.Windows.Forms.TabControl __tCsource;
        public System.Windows.Forms.TabPage __tabfile;
        private System.Windows.Forms.Button __openFileLoc;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.TextBox __EditFileLoc;
        public System.Windows.Forms.TabPage __tabcam;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TabPage __tabROI;
        public System.Windows.Forms.BindingSource _ROIBindigSource;
        private System.Windows.Forms.OpenFileDialog _DialogFileLoc;
        public System.Windows.Forms.BindingSource _InspectionBindigSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox __grpROIProc;
        public System.Windows.Forms.NumericUpDown __threshold;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox __cbProcFunc;
        public System.Windows.Forms.Button __btProcROI;
    }
}