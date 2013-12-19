namespace VisionModule {
    partial class __InspectionsForm {
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
            this.@__inspname = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.@__btcancel = new System.Windows.Forms.Button();
            this._btok = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.@__combochannel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.@__combocamname = new System.Windows.Forms.ComboBox();
            this.@__editfileloc = new System.Windows.Forms.TextBox();
            this._radiocamera = new System.Windows.Forms.RadioButton();
            this.@__radiofile = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.@__inspname);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Name";
            // 
            // __inspname
            // 
            this.@__inspname.Location = new System.Drawing.Point(6, 19);
            this.@__inspname.Name = "__inspname";
            this.@__inspname.Size = new System.Drawing.Size(172, 20);
            this.@__inspname.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.@__btcancel);
            this.groupBox2.Controls.Add(this._btok);
            this.groupBox2.Location = new System.Drawing.Point(3, 326);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(569, 74);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // __btcancel
            // 
            this.@__btcancel.Location = new System.Drawing.Point(397, 10);
            this.@__btcancel.Name = "__btcancel";
            this.@__btcancel.Size = new System.Drawing.Size(162, 58);
            this.@__btcancel.TabIndex = 1;
            this.@__btcancel.Text = "&Cancel";
            this.@__btcancel.UseVisualStyleBackColor = true;
            this.@__btcancel.Click += new System.EventHandler(this.@__btcancel_Click);
            // 
            // _btok
            // 
            this._btok.Location = new System.Drawing.Point(6, 10);
            this._btok.Name = "_btok";
            this._btok.Size = new System.Drawing.Size(162, 58);
            this._btok.TabIndex = 0;
            this._btok.Text = "&Ok";
            this._btok.UseVisualStyleBackColor = true;
            this._btok.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(198, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(501, 308);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Capture Options";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.@__combochannel);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(6, 133);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(489, 169);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Settings";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // __combochannel
            // 
            this.@__combochannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.@__combochannel.FormattingEnabled = true;
            this.@__combochannel.Items.AddRange(new object[] {
            "RGB",
            "Mono",
            "R",
            "G",
            "B"});
            this.@__combochannel.Location = new System.Drawing.Point(55, 22);
            this.@__combochannel.Name = "__combochannel";
            this.@__combochannel.Size = new System.Drawing.Size(55, 21);
            this.@__combochannel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chanel:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.@__combocamname);
            this.groupBox4.Controls.Add(this.@__editfileloc);
            this.groupBox4.Controls.Add(this._radiocamera);
            this.groupBox4.Controls.Add(this.@__radiofile);
            this.groupBox4.Location = new System.Drawing.Point(6, 27);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(489, 100);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Input";
            // 
            // __combocamname
            // 
            this.@__combocamname.FormattingEnabled = true;
            this.@__combocamname.Location = new System.Drawing.Point(100, 63);
            this.@__combocamname.Name = "__combocamname";
            this.@__combocamname.Size = new System.Drawing.Size(348, 21);
            this.@__combocamname.TabIndex = 5;
            // 
            // __editfileloc
            // 
            this.@__editfileloc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.@__editfileloc.Location = new System.Drawing.Point(100, 19);
            this.@__editfileloc.Name = "__editfileloc";
            this.@__editfileloc.Size = new System.Drawing.Size(348, 20);
            this.@__editfileloc.TabIndex = 4;
            // 
            // _radiocamera
            // 
            this._radiocamera.AutoSize = true;
            this._radiocamera.Location = new System.Drawing.Point(6, 63);
            this._radiocamera.Name = "_radiocamera";
            this._radiocamera.Size = new System.Drawing.Size(90, 17);
            this._radiocamera.TabIndex = 3;
            this._radiocamera.TabStop = true;
            this._radiocamera.Text = "From Camera:";
            this._radiocamera.UseVisualStyleBackColor = true;
            // 
            // __radiofile
            // 
            this.@__radiofile.AutoSize = true;
            this.@__radiofile.Location = new System.Drawing.Point(6, 19);
            this.@__radiofile.Name = "__radiofile";
            this.@__radiofile.Size = new System.Drawing.Size(70, 17);
            this.@__radiofile.TabIndex = 2;
            this.@__radiofile.TabStop = true;
            this.@__radiofile.Text = "From File:";
            this.@__radiofile.UseVisualStyleBackColor = true;
            this.@__radiofile.CheckedChanged += new System.EventHandler(this.@__radiofile_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Image files | *.bmp;*.jpg;*.jpeg";
            // 
            // __InspectionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 412);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "__InspectionsForm";
            this.Text = "Inspection Configuration";
            this.Load += new System.EventHandler(this.@__InspectionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _btok;
        private System.Windows.Forms.Button __btcancel;
        public System.Windows.Forms.TextBox __inspname;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox __combochannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox __combocamname;
        public System.Windows.Forms.TextBox __editfileloc;
        private System.Windows.Forms.RadioButton _radiocamera;
        private System.Windows.Forms.RadioButton __radiofile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}