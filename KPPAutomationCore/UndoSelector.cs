using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KPPAutomationCore {

   public class ToolStripUndoSelector : ToolStripDropDown {
        public ToolStripUndoSelector() {
            Items.Add(new ToolStripControlHost(_UndoRedoControl));

           
        }

        public UndoRedoControl Selector {
            get { return this._UndoRedoControl; }
        }

        private void control_SelectionCancelled(object sender, EventArgs e) {
            this.Close(ToolStripDropDownCloseReason.CloseCalled);
        }

       

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e) {
            base.OnOpening(e);

            ToolStripProfessionalRenderer renderer = Renderer as ToolStripProfessionalRenderer;

            if (renderer != null)
                _UndoRedoControl.BackColor = renderer.ColorTable.ToolStripDropDownBackground;

            //control.SelectedSize = new Size(0, 0);
            //control.VisibleRange = new Size(5, 4);
        }

        protected override void OnOpened(EventArgs e) {
            base.OnOpened(e);
            _UndoRedoControl.Focus();
        }

        UndoRedoControl _UndoRedoControl = new UndoRedoControl();
    }

}
