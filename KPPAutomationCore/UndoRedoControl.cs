// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DejaVu;

namespace KPPAutomationCore {
    public partial class UndoRedoControl :UserControl{

        //public ToolStripComboBox newconmbo = new ToolStripComboBox();

        public UndoRedoControl() {
            InitializeComponent();
            
            UndoRedoManager.CommandDone += delegate {
                //undolist.DataSource = new List<string>(UndoRedoManager.UndoCommands);
                //redolist.DataSource = new List<string>(UndoRedoManager.RedoCommands);
                
                //btnUndo.Enabled = UndoRedoManager.CanUndo;
                //btnRedo.Enabled = UndoRedoManager.CanRedo;
            };
            
        }

       
        

        private void btnUndo_Click(object sender, EventArgs e) {
            UndoRedoManager.Undo();
        }

        private void btnRedo_Click(object sender, EventArgs e) {
            UndoRedoManager.Redo();
            
        }

        private void __history_Click(object sender, EventArgs e) {
            
            //int selindex = __history.SelectedIndex;
            //for (int i = 0; i <= selindex; i++) {
            //    __history.SetSelected(i, true);
            //}

            //for (int j = selindex + 1; j < __history.Items.Count; j++) {
            //    __history.SetSelected(j, false);
            //}
        }

        private void __history_ColumnClick(object sender, ColumnClickEventArgs e) {
            
        }

        private void __history_BeforeSorting(object sender, BrightIdeasSoftware.BeforeSortingEventArgs e) {
            e.Canceled = true;
        }

        public int SelectionCount = 0;

        private void __history_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e) {
            SelectionCount = e.Item.Index+1;
            for (int i = 0; i <= SelectionCount-1; i++) {
                __history.Items[i].Selected = true;
            }

            for (int j = SelectionCount + 1; j < __history.Items.Count; j++) {
                __history.Items[j].Selected = false;
            }
        }

        private void __history_CellClick(object sender, BrightIdeasSoftware.CellClickEventArgs e) {
            SelectionCount = __history.SelectedIndex+1;
            for (int i = 0; i <= SelectionCount-1; i++) {
                __history.Items[i].Selected = true;
            }

            for (int j = SelectionCount; j < __history.Items.Count; j++) {
                __history.Items[j].Selected = false;
            }
            
        }

        private void __history_DrawItem(object sender, DrawListViewItemEventArgs e) {
           
        }

        private void __history_SelectedIndexChanged(object sender, EventArgs e) {
            
        }

       

        
    }
}
