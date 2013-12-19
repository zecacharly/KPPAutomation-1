using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisionModule {
    internal partial class __InspectionsForm : Form {
        public __InspectionsForm() {
            InitializeComponent();
        }

        private void __InspectionsForm_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            if (__inspname.Text.Length > 0) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void __btcancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void groupBox5_Enter(object sender, EventArgs e) {



        }

        private void __editfileloc_MouseClick(object sender, MouseEventArgs e) {
          
        }

        private void __radiofile_CheckedChanged(object sender, EventArgs e) {

            if (__radiofile.Checked) {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    __editfileloc.Text = openFileDialog1.FileName;
                }
            }
        }
    }
}
