using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisionModule {
    internal partial class InspectionOptionsForm : DockContent {
        public InspectionOptionsForm() {
            InitializeComponent();
        }

        private void InspectionOptionsForm_Load(object sender, EventArgs e) {

        }
        private void __openFileLoc_Click(object sender, EventArgs e) {
            if (_DialogFileLoc.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                __EditFileLoc.Focus();
                __EditFileLoc.Text = _DialogFileLoc.FileName;
                __openFileLoc.Focus();

            }
        }

        private void __btProcROI_Click(object sender, EventArgs e) {

        }   
    }
}
