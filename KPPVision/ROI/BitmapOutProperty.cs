using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VisionModule {
    internal partial class BitmapOutProperty : Form {
        public IWindowsFormsEditorService _wfes; 

        public BitmapOutProperty() {
            InitializeComponent();
            TopLevel = false;
            TopMost = true;
        }

        private void BitmapOutProperty_FormClosed(object sender, FormClosedEventArgs e) {
            _wfes.CloseDropDown();
        }
    }
}
