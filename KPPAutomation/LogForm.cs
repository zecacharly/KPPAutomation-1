using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using KPP.Core.Debug;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;


namespace KPPAutomation {
    internal partial class LogForm : DockContent {
        private static KPPLogger log = new KPPLogger(typeof(LogForm));
        public LogForm() {
            switch (LanguageSettings.Language) {
                case LanguageName.Unk:
                    break;
                case LanguageName.PT:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");

                    break;
                case LanguageName.EN:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

                    break;
                default:
                    break;
            }
            InitializeComponent();
        }

        public System.Windows.Forms.Timer TimeChangeColor = new System.Windows.Forms.Timer();

        private void LogForm_Load(object sender, EventArgs e) {
            TimeChangeColor.Interval = 500;
            TimeChangeColor.Tick += new EventHandler(TimeChangeColor_Tick);
            
        }

        void TimeChangeColor_Tick(object sender, EventArgs e) {
            if ( __tabexceptions.BackColor == Color.Transparent) {
                 __tabexceptions.BackColor = Color.Red;
            }
            else {
                __tabexceptions.BackColor = Color.Transparent;
            }
            
        }

        
        private void __tabControlLog_Click(object sender, EventArgs e) {
            TimeChangeColor.Enabled = false;
            __tabexceptions.BackColor = Color.Transparent;
        }

        private void __textBoxWarnings_TextChanged(object sender, EventArgs e) {
            if (__textBoxWarnings.Lines.Count() > numlines) {
                __textBoxWarnings.Text = "";
            }
        }

        private void __textBoxExceptions_TextChanged(object sender, EventArgs e) {
            if (__textBoxExceptions.Lines.Count() > numlines) {
                __textBoxExceptions.Text = "";
            }
        }

        private void __textBoxExceptions_Click(object sender, EventArgs e) {
            TimeChangeColor.Enabled = false;
            __tabexceptions.BackColor = Color.Transparent;
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        private int numlines=50;
        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar==13) {
                bool ok= int.TryParse(toolStripTextBox1.Text, out numlines);
                if (ok==false) {
                    toolStripTextBox1.Text = "50";
                }
            }
        }

        
    }
}
