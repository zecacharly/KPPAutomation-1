namespace KPPAutomation {
    partial class LogForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.@__tabControlLog = new System.Windows.Forms.TabControl();
            this.@__tabwarnings = new System.Windows.Forms.TabPage();
            this.@__textBoxWarnings = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.linesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.@__tabexceptions = new System.Windows.Forms.TabPage();
            this.@__textBoxExceptions = new System.Windows.Forms.TextBox();
            this._timerblinkexception = new System.Windows.Forms.Timer(this.components);
            this.@__tabControlLog.SuspendLayout();
            this.@__tabwarnings.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.@__tabexceptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // __tabControlLog
            // 
            resources.ApplyResources(this.@__tabControlLog, "__tabControlLog");
            this.@__tabControlLog.Controls.Add(this.@__tabwarnings);
            this.@__tabControlLog.Controls.Add(this.@__tabexceptions);
            this.@__tabControlLog.Multiline = true;
            this.@__tabControlLog.Name = "__tabControlLog";
            this.@__tabControlLog.SelectedIndex = 0;
            this.@__tabControlLog.Click += new System.EventHandler(this.@__tabControlLog_Click);
            // 
            // __tabwarnings
            // 
            resources.ApplyResources(this.@__tabwarnings, "__tabwarnings");
            this.@__tabwarnings.BackColor = System.Drawing.Color.Transparent;
            this.@__tabwarnings.Controls.Add(this.@__textBoxWarnings);
            this.@__tabwarnings.Name = "__tabwarnings";
            // 
            // __textBoxWarnings
            // 
            resources.ApplyResources(this.@__textBoxWarnings, "__textBoxWarnings");
            this.@__textBoxWarnings.ContextMenuStrip = this.contextMenuStrip1;
            this.@__textBoxWarnings.Name = "__textBoxWarnings";
            this.@__textBoxWarnings.TextChanged += new System.EventHandler(this.@__textBoxWarnings_TextChanged);
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // linesToolStripMenuItem
            // 
            resources.ApplyResources(this.linesToolStripMenuItem, "linesToolStripMenuItem");
            this.linesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.linesToolStripMenuItem.Name = "linesToolStripMenuItem";
            // 
            // toolStripTextBox1
            // 
            resources.ApplyResources(this.toolStripTextBox1, "toolStripTextBox1");
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // __tabexceptions
            // 
            resources.ApplyResources(this.@__tabexceptions, "__tabexceptions");
            this.@__tabexceptions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.@__tabexceptions.Controls.Add(this.@__textBoxExceptions);
            this.@__tabexceptions.Name = "__tabexceptions";
            this.@__tabexceptions.UseVisualStyleBackColor = true;
            // 
            // __textBoxExceptions
            // 
            resources.ApplyResources(this.@__textBoxExceptions, "__textBoxExceptions");
            this.@__textBoxExceptions.Name = "__textBoxExceptions";
            this.@__textBoxExceptions.Click += new System.EventHandler(this.@__textBoxExceptions_Click);
            this.@__textBoxExceptions.TextChanged += new System.EventHandler(this.@__textBoxExceptions_TextChanged);
            // 
            // _timerblinkexception
            // 
            this._timerblinkexception.Interval = 500;
            // 
            // LogForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.@__tabControlLog);
            this.Name = "LogForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.@__tabControlLog.ResumeLayout(false);
            this.@__tabwarnings.ResumeLayout(false);
            this.@__tabwarnings.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.@__tabexceptions.ResumeLayout(false);
            this.@__tabexceptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage __tabwarnings;
        public System.Windows.Forms.TabControl __tabControlLog;
        public System.Windows.Forms.TextBox __textBoxExceptions;
        public System.Windows.Forms.TextBox __textBoxWarnings;
        private System.Windows.Forms.Timer _timerblinkexception;
        public System.Windows.Forms.TabPage __tabexceptions;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem linesToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}