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
    internal partial class PropertySetForm : Form {
        public int BarValue;
        public bool _BtPressed=false;
        public IWindowsFormsEditorService _wfes;       

        
        /// <summary>
        /// Required designer variable.
        /// </summary>
      //  private System.ComponentModel.Container components = null;

        public PropertySetForm() {
            InitializeComponent();
            
            TopLevel = false;
            //TopMost = true;
            __btvalueok.Text = BarValue.ToString();

        }


        private void __btvalueok_Click(object sender, EventArgs e) {
            _BtPressed = true;
            Close();
        }

        private void PropertySetForm_FormClosed(object sender, FormClosedEventArgs e) {
            _wfes.CloseDropDown();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) {
            BarValue = trackBar1.Value;
            __btvalueok.Text = BarValue.ToString();
        }

    }
}
