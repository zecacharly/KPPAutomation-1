using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VisionModule {
    internal partial class ExposurePropertySet : UserControl {
        public int BarValue;
        public bool _BtPressed = false;
        public IWindowsFormsEditorService _wfes;    

        public ExposurePropertySet() {
            InitializeComponent();
        }


    }
}
