using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using KPP.Core.Debug;
using VisionModule;
using System.Reflection;
using BrightIdeasSoftware;
using System.Diagnostics;
using DejaVu;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;

namespace VisionModule {



    internal partial class ListROIForm : DockContent {

        private static KPPLogger log = new KPPLogger(typeof(ListROIForm));


        public ListROIForm() {
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


        public VisionProject SelectedProject = null;






        private void __RoiProcList_ItemsChanged(object sender, BrightIdeasSoftware.ItemsChangedEventArgs e) {
            //__propertyGridFunction.SelectedObject = (ProcessingFunctionBase)__RoiProcList.GetModelObject(e.NewObjectCount-1);            
            if (e.NewObjectCount > 0) {
                __RoiProcList.SelectedIndex = e.NewObjectCount - 1;

            } else {
                __RoiProcList.SelectedIndex = -1;
                __propertyGridFunction.SelectedObject = null; ;

            }
        }

        private void __RoiProcList_SelectedIndexChanged(object sender, EventArgs e) {
            __propertyGridFunction.SelectedObject = (ProcessingFunctionBase)__RoiProcList.GetModelObject(__RoiProcList.SelectedIndex);

            foreach (Control c in __propertyGridFunction.Controls) {
                c.MouseMove += new MouseEventHandler(c_MouseMove);
                c.MouseClick += new MouseEventHandler(c_MouseClick);
                c.MouseDown += new MouseEventHandler(c_MouseDown);
                c.AllowDrop = true;
                c.DragDrop += new DragEventHandler(c_DragDrop);
                c.DragOver += new DragEventHandler(c_DragOver);
                c.GiveFeedback += new GiveFeedbackEventHandler(c_GiveFeedback);
            }
        }

        void c_MouseDown(object sender, MouseEventArgs e) {
            if (true) {

            }
        }

        void c_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
            e.UseDefaultCursors = false;
            Cursor.Current = Cursors.Cross;
        }



        void c_DragOver(object sender, DragEventArgs e) {

            GridItem item = GetItemAtMouse();
            e.Effect = DragDropEffects.None;
            if (e.Data == null) {
                return;
            }
            try {
                if (item != null) {
                    //item.Expanded = true;
                    Boolean candrop = false;
                    if (item.Parent != null) {
                        if (item.Parent.Parent != null) {
                            if (item.Parent.Parent.Label == "Pre-Processing") {
                                candrop = true;
                            }
                        }

                    }
                    if ((item.Parent.Label == "Pre-Processing") || candrop) {

                        object selected = __propertyGridFunction.SelectedObject;

                        PropertyDescriptor _propdesc = item.PropertyDescriptor;


                        if (_propdesc.Attributes.Contains(new AcceptDrop(true))) {

                            e.Effect = DragDropEffects.Copy; // Okay
                        } else {
                            e.Effect = DragDropEffects.None; // Okay
                        }


                    }
                }

            } catch (Exception exp) {
                log.Error(exp);
            }


        }

        void c_DragDrop(object sender, DragEventArgs e) {
            try {


                GridItem selected = __propertyGridFunction.SelectedGridItem;

                if (e.Data != null) {
                    Object passedData = e.Data.GetData(typeof(ResultReference));
                    if (passedData != null) {

                        PropertyInfo propinfo = __propertyGridFunction.SelectedObject.GetType().GetProperty(selected.PropertyDescriptor.Name);

                        if (propinfo != null) {
                            propinfo.SetValue(__propertyGridFunction.SelectedObject, passedData, null);
                        }

                        __propertyGridFunction.Refresh();
                    }

                }
            } catch (Exception exp) {
                log.Error(exp);

            }

        }

        // public ContextMenuStrip fruitContextMenuStrip = new ContextMenuStrip();

        KeyPadControl KeyPad = new KeyPadControl();

        public delegate void ControlRightClick(object sender, MouseEventArgs e);
        public event ControlRightClick OnControlRightClick;


        void c_MouseClick(object sender, MouseEventArgs e) {

            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                WaitMouseRelease = false;
                GridItem _parent = __propertyGridFunction.SelectedGridItem;
                if (_parent != null) {



                    if ((_parent.Value is int || _parent.Value is Double || _parent.Value is float) && _parent.PropertyDescriptor.Category == "Pre-Processing") {

                        KeyPad.Left = this.__propertyGridFunction.Location.X; //- KeyPad.Width+5;
                        KeyPad.Top = this.PointToClient(Cursor.Position).Y - KeyPad.Height - 15;
                        KeyPad.__txtValue.Text = _parent.Value.ToString();
                        KeyPad.FirstClick = true;
                        KeyPad.textBox1.Text = _parent.Value.GetType().ToString();
                        KeyPad.Show();
                        return;
                    }
                }

                KeyPad.Hide();
            } else {
                ProcessingFunctionBase selectedproc = __propertyGridFunction.SelectedObject as ProcessingFunctionBase;


                if (selectedproc != null) {
                    if (__propertyGridFunction.SelectedGridItem != null) {
                        selectedproc.UpdateRegionToHighligth(__propertyGridFunction.SelectedGridItem.Value);
                        if (OnControlRightClick != null) {
                            OnControlRightClick(selectedproc, new MouseEventArgs(System.Windows.Forms.MouseButtons.Right, 1, 0, 0, 0));
                        }


                    }
                }

            }
        }


        Boolean WaitMouseRelease = true;
        object dragobject = null;

        void c_MouseMove(object sender, MouseEventArgs e) {
            try {

                if (e.Button == System.Windows.Forms.MouseButtons.Left) {



                    PropertyDescriptor _propdesc;
                    GridItem _parent = __propertyGridFunction.SelectedGridItem;

                    if (_parent != null) {
                        //_parent.Expanded = true;
                    }
                    //if (KeyPad.SelectedItem!=_parent) {
                    //    KeyPad.Hide();
                    //}

                    if (!WaitMouseRelease) {

                        WaitMouseRelease = true;
                        List<String> varnames = new List<string>();
                        //StringBuilder varnames = new StringBuilder(); 
                        while (true) {


                            if (_parent.Parent == null) {
                                break;
                            } else {
                                _propdesc = _parent.PropertyDescriptor;

                                varnames.Add(_propdesc.Name);

                                if (_propdesc.Attributes.Contains(new UseInResultInput(true))) {

                                    if ((_propdesc.Category == "Post-Processing") || (_propdesc.Category == "Pass/Fail Options")) {


                                        ResultReference NewRoiReference = new ResultReference(SelectedProject, (ProcessingFunctionBase)__RoiProcList.SelectedObject, _propdesc.Name);

                                        dragobject = NewRoiReference;

                                        ((Control)sender).DoDragDrop(NewRoiReference, DragDropEffects.Copy);
                                    }

                                    break;
                                } else if (_propdesc.Attributes.Contains(new AllowDrag(true))) {

                                    ((Control)sender).DoDragDrop(new DroppedProcessingInfo(__RoiProcList.SelectedObject, _propdesc.Name), DragDropEffects.Link);
                                }

                                _parent = _parent.Parent;
                            }

                        }
                    } else {
                        DoDragDrop(dragobject, DragDropEffects.Copy);
                    }






                } else {

                    WaitMouseRelease = false;
                }



            } catch (Exception) {


            }
        }




        #region Get Grid Item at point

        private GridItem GetItemAtGridPoint(Point thePoint) {
            object propertyGridView = GetPropertyGridView(this.__propertyGridFunction);

            GridItemCollection allGridEntries = GetAllGridEntries(propertyGridView);
            int top = GetTop(propertyGridView);
            int itemHeight = GetCachedRowHeight(propertyGridView);
            VScrollBar scrollBar = GetVScrollBar(propertyGridView);

            Point mousept = this.__propertyGridFunction.PointToClient(thePoint);
            return GetItemAtPoint(allGridEntries, top, itemHeight, scrollBar.Value, mousept);
        }


        private GridItem GetItemAtMouse() {
            object propertyGridView = GetPropertyGridView(this.__propertyGridFunction);

            GridItemCollection allGridEntries = GetAllGridEntries(propertyGridView);
            int top = GetTop(propertyGridView);
            int itemHeight = GetCachedRowHeight(propertyGridView);
            VScrollBar scrollBar = GetVScrollBar(propertyGridView);

            Point mousept = this.__propertyGridFunction.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            return GetItemAtPoint(allGridEntries, top, itemHeight, scrollBar.Value, mousept);
        }

        GridItem GetItemAtPoint(GridItemCollection items, int top, int itemHeight, int scrollItems, Point pt) {
            try {

                int idx = (pt.Y - top) / (itemHeight + 1);
                idx += scrollItems;

                GridItem item = items[idx];
                item.Select();

                return item;

            } catch (Exception) {


            }
            return null;
        }


        object GetPropertyGridView(PropertyGrid propertyGrid) {
            foreach (Control c in propertyGrid.Controls) {
                if (c.GetType().Name == "PropertyGridView")
                    return c;
            }
            return null;
        }


        GridItemCollection GetAllGridEntries(object propertyGridView) {
            FieldInfo fi = propertyGridView.GetType().GetField("allGridEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            return (GridItemCollection)fi.GetValue(propertyGridView);
        }


        int GetTop(object propertyGridView) {
            Control ctrl = (Control)propertyGridView;
            return ctrl.Top;
        }


        int GetCachedRowHeight(object propertyGridView) {
            FieldInfo fi = propertyGridView.GetType().GetField("cachedRowHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)fi.GetValue(propertyGridView);
        }


        VScrollBar GetVScrollBar(object propertyGridView) {
            FieldInfo fi = propertyGridView.GetType().GetField("scrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            return (VScrollBar)fi.GetValue(propertyGridView);
        }


        #endregion





        private void __propertyGridFunction_Click(object sender, EventArgs e) {

        }

        Stopwatch stopWatch = new Stopwatch();


        private void __RoiProcList_CanDrop(object sender, OlvDropEventArgs e) {
            try {


                e.Handled = true;
                e.Effect = DragDropEffects.None;
                if (!stopWatch.IsRunning) {
                    stopWatch.Reset();
                    stopWatch.Start();

                }

                if (e.DropTargetItem != null) {
                    if (stopWatch.ElapsedMilliseconds > 1000) {

                        __RoiProcList.SelectedObject = e.DropTargetItem.RowObject;
                        stopWatch.Stop();
                        stopWatch.Reset();
                    }




                }
            } catch (Exception exp) {
                log.Error(exp);

            }
        }

        private void ListROIForm_MouseLeave(object sender, EventArgs e) {
            KeyPad.Hide();
        }

        private void ListROIForm_Leave(object sender, EventArgs e) {
            KeyPad.Hide();
        }

        private void __propertyGridFunction_Leave(object sender, EventArgs e) {
            // KeyPad.Hide();
        }

        private void __cbProcFunc_Load(object sender, EventArgs e) {

        }

        private void ListROIForm_Enter(object sender, EventArgs e) {          
            __RoiProcList.Sort(1);
        }

        private void __RoiProcList_CellEditFinishing(object sender, CellEditEventArgs e) {
            try {
                ProcessingFunctionBase _procFunc = (ProcessingFunctionBase)e.RowObject;

                if (e.Cancel) {
                    return;
                } else if (e.Column.Text == "Name") {
                    foreach (ROI roi in SelectedProject.SelectedRequest.SelectedInspection.ROIList) {
                        if (roi.ProcessingFunctions.FindAll(name => name.FunctionName.Contains(_procFunc.FunctionName)).Count > 1) {
                            e.Cancel = true;
                        }
                    }
                    _procFunc.FunctionName = e.NewValue as string;

                } else if (e.Column.Text == "Position") {
                    e.Cancel = true;
                    int newposval = (int)e.NewValue;



                    int totalproc = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Count;
                    if (newposval > totalproc) {
                        return;
                    }

                    ProcessingFunctionBase theprocfunc = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Find(pos => pos.ProcPos == newposval);
                    if (theprocfunc != null) {

                        if (theprocfunc.FunctionName != _procFunc.FunctionName) {

                            int actpos = _procFunc.ProcPos;
                            if (newposval > _procFunc.ProcPos) {
                                _procFunc.ProcPos = newposval;
                                var list = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Where(Pos => (Pos.ProcPos < newposval) && (Pos.ProcPos > actpos)).OrderBy(bypos => bypos.ProcPos);
                                foreach (ProcessingFunctionBase proc in list) {
                                    proc.ProcPos--;
                                }
                                theprocfunc.ProcPos--;
                            } else {
                                _procFunc.ProcPos = newposval;
                                var list = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Where(Pos => (Pos.ProcPos > newposval) && (Pos.ProcPos < actpos)).OrderBy(bypos => bypos.ProcPos);
                                foreach (ProcessingFunctionBase proc in list) {
                                    proc.ProcPos++;
                                }
                                theprocfunc.ProcPos++;

                            }


                            e.Cancel = false;
                        }


                    } else {
                        e.Cancel = false;
                    }


                }
                __RoiProcList.Sort(olvColumnProcPOS);
                __RoiProcList.RefreshObjects(SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions);
                __propertyGridFunction.Refresh();

            } catch (Exception exp) {
                log.Error(exp);
            }
        }




        private void __btAddProc_Click(object sender, EventArgs e) {
            try {
                if (__cbProcFunc.SelectedObject != null) {
                    ProcessingFunctionDefinition selected = __cbProcFunc.SelectedObject as ProcessingFunctionDefinition;
                    
                    if (selected != null) {

                        ProcessingFunctionBase proc = Activator.CreateInstance(selected.BaseType) as ProcessingFunctionBase;
                        int i = 1;
                        foreach (Request req in SelectedProject.RequestList) {
                            foreach (Inspection insp in req.Inspections) {
                                foreach (ROI roi in insp.ROIList) {
                                    if (roi.ProcessingFunctions.Find(name => name.FunctionName.Contains(selected.FunctionType + " " + i.ToString())) != null) {
                                        i++;
                                    }
                                }
                            }
                        }
                        proc.FunctionName = selected.FunctionType + " " + i.ToString();
                        proc.FunctionType = selected.FunctionType;
                        proc.ProcPos = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Count() + 1;

                        proc.SelectedVisionProject = SelectedProject;

                        if (SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Contains(proc) == false) {
                            SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.AddProcessingFunction(proc);
                            __RoiProcList.Objects=SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions;


                        }

                    }
                }
            } catch (Exception exp) {
                log.Error(exp);
            }
        }

        private void __btProcROI_Click(object sender, EventArgs e) {
            try {
                if (SelectedProject.SelectedRequest.SelectedInspection != null) {
                    
                    SelectedProject.SelectedRequest.SelectedInspection.ProcessSelectedROI();

                }
            } catch (Exception exp) {
                log.Error(exp);

            }
        }
        void UpdatePrePosFunction(ROI theroi) {
            try {

                //if (StaticObjects.ReferencePoints.Find(name => name.ReferencePointName == theroi.referencePoint.ReferencePointName) == null) {
                //    ReferencePoint thepoint = StaticObjects.ReferencePoints[0];

                //    theroi.referencePoint = thepoint;


                //}

            } catch (Exception exp) {

                log.Error(exp);
            }

        }

        private void __btRemoveProc_Click(object sender, EventArgs e) {
            try {
                ProcessingFunctionBase proc = (ProcessingFunctionBase)__RoiProcList.SelectedObject;
                using (UndoRedoManager.Start(Name + ": Processing Function removed: " + proc.FunctionName)) {
                    SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions.Remove(proc);
                    UndoRedoManager.Commit();
                }
                __RoiProcList.Objects = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions;

                //if (proc is ProcessingFunctionPrePos) {
                //    StaticObjects.ReferencePoints.RemoveAll(name => name.ReferencePointName == proc.FunctionName);



                //    using (UndoRedoManager.Start(SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.Name + ": refrence point changed")) {
                //        SelectedProject.RequestList.ForEach(req => req.Inspections.ForEach(insp => insp.ROIList.ForEach(UpdatePrePosFunction)));
                //        UndoRedoManager.Commit();
                //    }

                //}

                
                __propertyGridFunction.SelectedObject = __RoiProcList.GetModelObject(__RoiProcList.Items.Count - 1);

            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __RoiProcList_KeyUp(object sender, KeyEventArgs e) {
            try {
                //if (e.Control && e.KeyCode == Keys.C) {
                //    ProcessingFunctionBase copiedproc = __RoiProcList.SelectedObject as ProcessingFunctionBase;
                //    if (copiedproc != null) {
                        
                //        ProcessingFunctionBase newproc = copiedproc.Copy() as ProcessingFunctionBase;
                //        SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.AddProcessingFunction(newproc);
                //        __RoiProcList.Objects = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ProcessingFunctions;
                //    }
                //}
                if (e.KeyCode == Keys.Delete && AcessManagement.AcessLevel == Acesslevel.Admin) {
                    __btRemoveProc_Click(sender, null);
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void ListROIForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();

        }


        private void ListROIForm_Load(object sender, EventArgs e) {
            SimpleDropSink sink1 = (SimpleDropSink)__RoiProcList.DropSink;
            sink1.FeedbackColor = Color.Green;
            this.Controls.Add(KeyPad);


            KeyPad.Hide();
            KeyPad.BringToFront();
            KeyPad.TheGrid = __propertyGridFunction;

            AcessManagement.OnAcesslevelChanged += new AcessManagement.AcesslevelChanged(StaticObjects_OnAcesslevelChanged);
        }

        void StaticObjects_OnAcesslevelChanged(Acesslevel NewLevel) {

            Boolean state = NewLevel == Acesslevel.Admin;

            __cbProcFunc.Enabled = state;
            __btRemoveProc.Enabled = state;

            state = (NewLevel == Acesslevel.Admin || NewLevel == Acesslevel.Man);

            __propertyGridFunction.Enabled = state;
            

            switch (NewLevel) {
                case Acesslevel.Admin:
                    __RoiProcList.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    break;
                case Acesslevel.User:
                    __RoiProcList.CellEditActivation = ObjectListView.CellEditActivateMode.None;
                    break;
                case Acesslevel.NotSet:
                    break;
                default:
                    break;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
          
        }

    }


    public class DroppedProcessingInfo {

        Object _draggedObject = null;

        public Object DraggedObject {
            get { return _draggedObject; }
            
        }

        String _draggedProperty = "";

        public String DraggedProperty {
            get { return _draggedProperty; }
            
        }


        public DroppedProcessingInfo(Object draggedObject, String DraggedProperty) {
            _draggedProperty = DraggedProperty;
            _draggedObject = draggedObject;
        }
    }


}
