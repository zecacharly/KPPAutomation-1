using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ClassInspection {
    public partial class BitmapTemplates : Form {
        public IWindowsFormsEditorService _wfes;
        public ClassInspection.ProcessingFunctionShapes.ShapesClass _shapesClass= null;

        public BitmapTemplates() {
            InitializeComponent();
            TopLevel = false;
            TopMost = true;
        }

      
       
    }
}
