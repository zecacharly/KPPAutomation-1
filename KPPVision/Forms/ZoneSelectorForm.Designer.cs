namespace VisionModule.Forms {
    partial class ZoneSelectorForm {
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
            this.@__image = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.@__numericCol = new System.Windows.Forms.NumericUpDown();
            this.@__numericLines = new System.Windows.Forms.NumericUpDown();
            this.@__comboZones = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.@__selectedZone = new System.Windows.Forms.TextBox();
            this.@__btUse = new System.Windows.Forms.Button();
            this.@__btClear = new System.Windows.Forms.Button();
            this.@__btOk = new System.Windows.Forms.Button();
            this.@__BtCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.@__image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__numericCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__numericLines)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // __image
            // 
            this.@__image.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.@__image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.@__image.Location = new System.Drawing.Point(12, 12);
            this.@__image.Name = "__image";
            this.@__image.Size = new System.Drawing.Size(581, 468);
            this.@__image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.@__image.TabIndex = 0;
            this.@__image.TabStop = false;
            this.@__image.SizeChanged += new System.EventHandler(this.@__image_SizeChanged);
            this.@__image.Paint += new System.Windows.Forms.PaintEventHandler(this.@__image_Paint);
            this.@__image.MouseClick += new System.Windows.Forms.MouseEventHandler(this.@__image_MouseClick);
            this.@__image.MouseLeave += new System.EventHandler(this.@__image_MouseLeave);
            this.@__image.MouseMove += new System.Windows.Forms.MouseEventHandler(this.@__image_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Avaible Zones";
            // 
            // __numericCol
            // 
            this.@__numericCol.Location = new System.Drawing.Point(74, 18);
            this.@__numericCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.@__numericCol.Name = "__numericCol";
            this.@__numericCol.Size = new System.Drawing.Size(59, 20);
            this.@__numericCol.TabIndex = 2;
            this.@__numericCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.@__numericCol.ValueChanged += new System.EventHandler(this.@__numericCol_ValueChanged);
            // 
            // __numericLines
            // 
            this.@__numericLines.Location = new System.Drawing.Point(74, 48);
            this.@__numericLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.@__numericLines.Name = "__numericLines";
            this.@__numericLines.Size = new System.Drawing.Size(59, 20);
            this.@__numericLines.TabIndex = 2;
            this.@__numericLines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.@__numericLines.ValueChanged += new System.EventHandler(this.@__numericLines_ValueChanged);
            // 
            // __comboZones
            // 
            this.@__comboZones.FormattingEnabled = true;
            this.@__comboZones.Items.AddRange(new object[] {
            "Zone 1 - "});
            this.@__comboZones.Location = new System.Drawing.Point(6, 103);
            this.@__comboZones.Name = "__comboZones";
            this.@__comboZones.Size = new System.Drawing.Size(287, 21);
            this.@__comboZones.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.@__btClear);
            this.groupBox1.Controls.Add(this.@__btUse);
            this.groupBox1.Controls.Add(this.@__selectedZone);
            this.groupBox1.Controls.Add(this.@__comboZones);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.@__numericLines);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.@__numericCol);
            this.groupBox1.Location = new System.Drawing.Point(599, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 227);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zone Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Lines:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Selected Zone:";
            // 
            // __selectedZone
            // 
            this.@__selectedZone.Location = new System.Drawing.Point(6, 197);
            this.@__selectedZone.Name = "__selectedZone";
            this.@__selectedZone.ReadOnly = true;
            this.@__selectedZone.Size = new System.Drawing.Size(287, 20);
            this.@__selectedZone.TabIndex = 4;
            // 
            // __btUse
            // 
            this.@__btUse.Location = new System.Drawing.Point(6, 130);
            this.@__btUse.Name = "__btUse";
            this.@__btUse.Size = new System.Drawing.Size(149, 39);
            this.@__btUse.TabIndex = 5;
            this.@__btUse.Text = "Use Selected";
            this.@__btUse.UseVisualStyleBackColor = true;
            this.@__btUse.Click += new System.EventHandler(this.@__btUse_Click);
            // 
            // __btClear
            // 
            this.@__btClear.Location = new System.Drawing.Point(161, 130);
            this.@__btClear.Name = "__btClear";
            this.@__btClear.Size = new System.Drawing.Size(132, 39);
            this.@__btClear.TabIndex = 5;
            this.@__btClear.Text = "Clear";
            this.@__btClear.UseVisualStyleBackColor = true;
            this.@__btClear.Click += new System.EventHandler(this.@__btClear_Click);
            // 
            // __btOk
            // 
            this.@__btOk.Location = new System.Drawing.Point(599, 245);
            this.@__btOk.Name = "__btOk";
            this.@__btOk.Size = new System.Drawing.Size(155, 47);
            this.@__btOk.TabIndex = 5;
            this.@__btOk.Text = "Ok";
            this.@__btOk.UseVisualStyleBackColor = true;
            this.@__btOk.Click += new System.EventHandler(this.@__btOk_Click);
            // 
            // __BtCancel
            // 
            this.@__BtCancel.Location = new System.Drawing.Point(760, 245);
            this.@__BtCancel.Name = "__BtCancel";
            this.@__BtCancel.Size = new System.Drawing.Size(138, 47);
            this.@__BtCancel.TabIndex = 5;
            this.@__BtCancel.Text = "Cancel";
            this.@__BtCancel.UseVisualStyleBackColor = true;
            this.@__BtCancel.Click += new System.EventHandler(this.@__BtCancel_Click);
            // 
            // ZoneSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 492);
            this.Controls.Add(this.@__BtCancel);
            this.Controls.Add(this.@__btOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.@__image);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ZoneSelectorForm";
            this.Text = "ZoneSelectorForm";
            this.Load += new System.EventHandler(this.ZoneSelectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.@__image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__numericCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.@__numericLines)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox __image;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox __comboZones;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button __btClear;
        private System.Windows.Forms.Button __btUse;
        public System.Windows.Forms.NumericUpDown __numericCol;
        public System.Windows.Forms.NumericUpDown __numericLines;
        public System.Windows.Forms.TextBox __selectedZone;
        private System.Windows.Forms.Button __btOk;
        private System.Windows.Forms.Button __BtCancel;
    }
}