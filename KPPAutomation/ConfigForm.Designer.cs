namespace KPPAutomation {
    partial class ConfigForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.@__btexitconf = new System.Windows.Forms.Button();
            this.@__btsaveConf = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.@__PropertySettings = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.@__btexitconf, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.@__btsaveConf, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // __btexitconf
            // 
            resources.ApplyResources(this.@__btexitconf, "__btexitconf");
            this.@__btexitconf.Name = "__btexitconf";
            this.@__btexitconf.UseVisualStyleBackColor = true;
            this.@__btexitconf.Click += new System.EventHandler(this.@__btexitconf_Click);
            // 
            // __btsaveConf
            // 
            resources.ApplyResources(this.@__btsaveConf, "__btsaveConf");
            this.@__btsaveConf.Name = "__btsaveConf";
            this.@__btsaveConf.UseVisualStyleBackColor = true;
            this.@__btsaveConf.Click += new System.EventHandler(this.@__btsaveConf_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.@__PropertySettings);
            this.panel1.Name = "panel1";
            // 
            // __PropertySettings
            // 
            resources.ApplyResources(this.@__PropertySettings, "__PropertySettings");
            this.@__PropertySettings.Name = "__PropertySettings";
            // 
            // ConfigForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConfigForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button __btexitconf;
        public System.Windows.Forms.Button __btsaveConf;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PropertyGrid __PropertySettings;

    }
}