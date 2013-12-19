namespace VisionModule {
    partial class BitmapOutProperty {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.@__pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.@__pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.@__pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(4, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 192);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ROI Image";
            // 
            // __pictureBox1
            // 
            this.@__pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.@__pictureBox1.Name = "__pictureBox1";
            this.@__pictureBox1.Size = new System.Drawing.Size(195, 173);
            this.@__pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.@__pictureBox1.TabIndex = 5;
            this.@__pictureBox1.TabStop = false;
            // 
            // BitmapOutProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 217);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BitmapOutProperty";
            this.Text = "BitmapOutProperty";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BitmapOutProperty_FormClosed);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.@__pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.PictureBox __pictureBox1;
    }
}