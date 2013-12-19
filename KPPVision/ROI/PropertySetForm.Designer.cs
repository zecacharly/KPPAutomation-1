namespace VisionModule {
    partial class PropertySetForm {
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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.@__btvalueok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 12);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(249, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 5;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // __btvalueok
            // 
            this.@__btvalueok.Location = new System.Drawing.Point(267, 12);
            this.@__btvalueok.Name = "__btvalueok";
            this.@__btvalueok.Size = new System.Drawing.Size(75, 23);
            this.@__btvalueok.TabIndex = 1;
            this.@__btvalueok.Text = "0";
            this.@__btvalueok.UseVisualStyleBackColor = true;
            this.@__btvalueok.Click += new System.EventHandler(this.@__btvalueok_Click);
            // 
            // PropertySetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 59);
            this.Controls.Add(this.@__btvalueok);
            this.Controls.Add(this.trackBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "PropertySetForm";
            this.Text = "Set property value";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PropertySetForm_FormClosed);
            
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button __btvalueok;
    }
}