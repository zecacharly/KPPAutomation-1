namespace VisionModule {
    partial class InspImageCtrl {
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
            this.@__groupcontainer = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.@__groupcontainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // __groupcontainer
            // 
            this.@__groupcontainer.Controls.Add(this.pictureBox1);
            this.@__groupcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.@__groupcontainer.Location = new System.Drawing.Point(0, 0);
            this.@__groupcontainer.Name = "__groupcontainer";
            this.@__groupcontainer.Size = new System.Drawing.Size(198, 191);
            this.@__groupcontainer.TabIndex = 0;
            this.@__groupcontainer.TabStop = false;
            this.@__groupcontainer.Text = "InspectionName";
            this.@__groupcontainer.Enter += new System.EventHandler(this.@__groupcontainer_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(192, 172);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // InspImageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.@__groupcontainer);
            this.MaximumSize = new System.Drawing.Size(200, 200);
            this.Name = "InspImageCtrl";
            this.Size = new System.Drawing.Size(198, 191);
            this.@__groupcontainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox __groupcontainer;

    }
}
