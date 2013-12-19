namespace KPPAutomationCore.KPPCustomControls {
    partial class KPPComboBox {
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
            this.textBox = new System.Windows.Forms.TextBox();
            this.borderLabel = new System.Windows.Forms.Label();
            this.buttonDropDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BackColor = System.Drawing.SystemColors.Window;
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Location = new System.Drawing.Point(3, -26);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(0, 13);
            this.textBox.TabIndex = 5;
            this.textBox.Visible = false;
            // 
            // borderLabel
            // 
            this.borderLabel.BackColor = System.Drawing.SystemColors.Window;
            this.borderLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.borderLabel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.borderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.borderLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.borderLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.borderLabel.Location = new System.Drawing.Point(0, 0);
            this.borderLabel.Name = "borderLabel";
            this.borderLabel.Size = new System.Drawing.Size(206, 23);
            this.borderLabel.TabIndex = 4;
            this.borderLabel.Text = "None selected";
            this.borderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDropDown
            // 
            this.buttonDropDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDropDown.Font = new System.Drawing.Font("Marlett", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonDropDown.Location = new System.Drawing.Point(206, 0);
            this.buttonDropDown.Name = "buttonDropDown";
            this.buttonDropDown.Size = new System.Drawing.Size(20, 23);
            this.buttonDropDown.TabIndex = 7;
            this.buttonDropDown.Text = "u";
            this.buttonDropDown.UseVisualStyleBackColor = true;
            this.buttonDropDown.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonDropDown_MouseClick);
            // 
            // KPPComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.borderLabel);
            this.Controls.Add(this.buttonDropDown);
            this.Name = "KPPComboBox";
            this.Size = new System.Drawing.Size(226, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label borderLabel;
        private System.Windows.Forms.Button buttonDropDown;
    }
}
