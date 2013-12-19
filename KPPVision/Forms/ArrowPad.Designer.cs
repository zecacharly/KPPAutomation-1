namespace VisionModule {
    partial class ArrowPadControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrowPadControl));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutpanel = new System.Windows.Forms.TableLayoutPanel();
            this.@__btdrag = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPagePosSize = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.@__btUP = new System.Windows.Forms.Button();
            this.@__btRIGHT = new System.Windows.Forms.Button();
            this.@__btLEFT = new System.Windows.Forms.Button();
            this.@__btDOWN = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.@__checkedsize = new System.Windows.Forms.RadioButton();
            this.@__checkPosition = new System.Windows.Forms.RadioButton();
            this.@__btClose = new System.Windows.Forms.Button();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.@__checkKeepVisible = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.layoutpanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPagePosSize.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.layoutpanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 410);
            this.panel1.TabIndex = 0;
            this.panel1.MouseHover += new System.EventHandler(this.panel1_MouseHover);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // layoutpanel
            // 
            this.layoutpanel.ColumnCount = 3;
            this.layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.layoutpanel.Controls.Add(this.@__btdrag, 0, 2);
            this.layoutpanel.Controls.Add(this.tabControl1, 0, 0);
            this.layoutpanel.Controls.Add(this.@__checkKeepVisible, 0, 1);
            this.layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutpanel.Location = new System.Drawing.Point(0, 0);
            this.layoutpanel.Margin = new System.Windows.Forms.Padding(2);
            this.layoutpanel.Name = "layoutpanel";
            this.layoutpanel.RowCount = 3;
            this.layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutpanel.Size = new System.Drawing.Size(635, 408);
            this.layoutpanel.TabIndex = 2;
            // 
            // __btdrag
            // 
            this.layoutpanel.SetColumnSpan(this.@__btdrag, 3);
            this.@__btdrag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__btdrag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btdrag.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.@__btdrag.Location = new System.Drawing.Point(3, 385);
            this.@__btdrag.Name = "__btdrag";
            this.@__btdrag.Size = new System.Drawing.Size(629, 20);
            this.@__btdrag.TabIndex = 15;
            this.@__btdrag.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.layoutpanel.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPagePosSize);
            this.tabControl1.Controls.Add(this.tabPageChart);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(629, 353);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPagePosSize
            // 
            this.tabPagePosSize.Controls.Add(this.tableLayoutPanel2);
            this.tabPagePosSize.Location = new System.Drawing.Point(4, 22);
            this.tabPagePosSize.Name = "tabPagePosSize";
            this.tabPagePosSize.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePosSize.Size = new System.Drawing.Size(621, 327);
            this.tabPagePosSize.TabIndex = 0;
            this.tabPagePosSize.Text = "Pos/Size";
            this.tabPagePosSize.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.@__btUP, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.@__btRIGHT, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.@__btLEFT, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.@__btDOWN, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.@__btClose, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(615, 321);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // __btUP
            // 
            this.@__btUP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.@__btUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btUP.Image = ((System.Drawing.Image)(resources.GetObject("__btUP.Image")));
            this.@__btUP.Location = new System.Drawing.Point(286, 31);
            this.@__btUP.Name = "__btUP";
            this.@__btUP.Size = new System.Drawing.Size(41, 36);
            this.@__btUP.TabIndex = 0;
            this.@__btUP.UseVisualStyleBackColor = true;
            // 
            // __btRIGHT
            // 
            this.@__btRIGHT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.@__btRIGHT.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btRIGHT.Image = ((System.Drawing.Image)(resources.GetObject("__btRIGHT.Image")));
            this.@__btRIGHT.Location = new System.Drawing.Point(491, 130);
            this.@__btRIGHT.Name = "__btRIGHT";
            this.@__btRIGHT.Size = new System.Drawing.Size(41, 36);
            this.@__btRIGHT.TabIndex = 2;
            this.@__btRIGHT.UseVisualStyleBackColor = true;
            // 
            // __btLEFT
            // 
            this.@__btLEFT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.@__btLEFT.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btLEFT.Image = ((System.Drawing.Image)(resources.GetObject("__btLEFT.Image")));
            this.@__btLEFT.Location = new System.Drawing.Point(81, 130);
            this.@__btLEFT.Name = "__btLEFT";
            this.@__btLEFT.Size = new System.Drawing.Size(41, 36);
            this.@__btLEFT.TabIndex = 3;
            this.@__btLEFT.UseVisualStyleBackColor = true;
            // 
            // __btDOWN
            // 
            this.@__btDOWN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.@__btDOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btDOWN.Image = ((System.Drawing.Image)(resources.GetObject("__btDOWN.Image")));
            this.@__btDOWN.Location = new System.Drawing.Point(286, 229);
            this.@__btDOWN.Name = "__btDOWN";
            this.@__btDOWN.Size = new System.Drawing.Size(41, 36);
            this.@__btDOWN.TabIndex = 1;
            this.@__btDOWN.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 3);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.@__checkedsize, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.@__checkPosition, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 297);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(615, 24);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // __checkedsize
            // 
            this.@__checkedsize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.@__checkedsize.AutoSize = true;
            this.@__checkedsize.Location = new System.Drawing.Point(3, 3);
            this.@__checkedsize.Name = "__checkedsize";
            this.@__checkedsize.Size = new System.Drawing.Size(45, 17);
            this.@__checkedsize.TabIndex = 4;
            this.@__checkedsize.TabStop = true;
            this.@__checkedsize.Text = "Size";
            this.@__checkedsize.UseVisualStyleBackColor = true;
            // 
            // __checkPosition
            // 
            this.@__checkPosition.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.@__checkPosition.AutoSize = true;
            this.@__checkPosition.Checked = true;
            this.@__checkPosition.Location = new System.Drawing.Point(310, 3);
            this.@__checkPosition.Name = "__checkPosition";
            this.@__checkPosition.Size = new System.Drawing.Size(62, 17);
            this.@__checkPosition.TabIndex = 4;
            this.@__checkPosition.TabStop = true;
            this.@__checkPosition.Text = "Position";
            this.@__checkPosition.UseVisualStyleBackColor = true;
            // 
            // __btClose
            // 
            this.@__btClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.@__btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__btClose.Location = new System.Drawing.Point(286, 130);
            this.@__btClose.Name = "__btClose";
            this.@__btClose.Size = new System.Drawing.Size(41, 36);
            this.@__btClose.TabIndex = 6;
            this.@__btClose.Text = "OK";
            this.@__btClose.UseVisualStyleBackColor = true;
            this.@__btClose.Click += new System.EventHandler(this.@__btClose_Click);
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.chart1);
            this.tabPageChart.Location = new System.Drawing.Point(4, 22);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChart.Size = new System.Drawing.Size(621, 327);
            this.tabPageChart.TabIndex = 1;
            this.tabPageChart.Text = "Chart";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.AxisX.Title = "Pixel";
            chartArea1.AxisY.Title = "Pixel value";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(615, 321);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // __checkKeepVisible
            // 
            this.@__checkKeepVisible.AutoSize = true;
            this.@__checkKeepVisible.Location = new System.Drawing.Point(3, 362);
            this.@__checkKeepVisible.Name = "__checkKeepVisible";
            this.@__checkKeepVisible.Size = new System.Drawing.Size(84, 17);
            this.@__checkKeepVisible.TabIndex = 16;
            this.@__checkKeepVisible.Text = "Keep Visible";
            this.@__checkKeepVisible.UseVisualStyleBackColor = true;
            // 
            // ArrowPadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "ArrowPadControl";
            this.Size = new System.Drawing.Size(637, 410);
            this.VisibleChanged += new System.EventHandler(this.ArrowPadControl_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.layoutpanel.ResumeLayout(false);
            this.layoutpanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPagePosSize.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPageChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button __btdrag;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPagePosSize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button __btUP;
        public System.Windows.Forms.Button __btRIGHT;
        public System.Windows.Forms.Button __btLEFT;
        public System.Windows.Forms.Button __btDOWN;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.RadioButton __checkedsize;
        private System.Windows.Forms.RadioButton __checkPosition;
        public System.Windows.Forms.Button __btClose;
        private System.Windows.Forms.TabPage tabPageChart;
        public System.Windows.Forms.TableLayoutPanel layoutpanel;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox __checkKeepVisible;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;






    }
}
