using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;

using KPP.Core.Debug;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Diagnostics;
using BrightIdeasSoftware;
using DejaVu;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;




namespace VisionModule {





    internal partial class ListInspForm : DockContent{

        private static KPPLogger log = new KPPLogger(typeof(ListInspForm));


        public ListInspForm() {
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


        private VisionProject _SelectedProject = null;

        internal VisionProject SelectedProject {
            get { return _SelectedProject; }
            set {
                _SelectedProject = value;
                auxTB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                auxTB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }


        private void __listinspections_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void ListInspForm_Load(object sender, EventArgs e) {
            __listinspections.AddDecoration(new BrightIdeasSoftware.EditingCellBorderDecoration { UseLightbox = true });

            SimpleDropSink sink1 = (SimpleDropSink)__listinspections.DropSink;
            sink1.FeedbackColor = Color.Green;

            sink1 = (SimpleDropSink)__ListRequests.DropSink;
            sink1.FeedbackColor = Color.Green;
            sink1 = (SimpleDropSink)__listRoi.DropSink;
            sink1.FeedbackColor = Color.Green;
            __listRoi.KeyDown += new KeyEventHandler(__listRoi_KeyDown);

            AcessManagement.OnAcesslevelChanged += new AcessManagement.AcesslevelChanged(StaticObjects_OnAcesslevelChanged);
        }

        void StaticObjects_OnAcesslevelChanged(Acesslevel NewLevel) {
            Boolean state = NewLevel == Acesslevel.Admin;

            __contextRequest.Enabled = state;
            __propertyGridinsp.Enabled = state;
            __contextInspection.Enabled = state;
            __contextROI.Enabled = state;

            switch (NewLevel) {
                case Acesslevel.Admin:
                    __listinspections.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    __listRoi.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    __ListRequests.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    __ListAuxROIS.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    
                    break;
                case Acesslevel.User:
                    __listinspections.CellEditActivation = ObjectListView.CellEditActivateMode.None;
                    __listRoi.CellEditActivation = ObjectListView.CellEditActivateMode.None;
                    __ListRequests.CellEditActivation = ObjectListView.CellEditActivateMode.None;
                    __ListAuxROIS.CellEditActivation = ObjectListView.CellEditActivateMode.None;

                    break;
                case Acesslevel.NotSet:
                    break;
                default:
                    break;
            }
        }




        private void __propertyGridinsp_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {


        }

        private void __propertyGridinsp_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e) {

        }

        private void __propertyGridinsp_Click(object sender, EventArgs e) {
            __propertyGridinsp.Refresh();
        }


        public void _SelectedROI_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            ROI _roi = (ROI)sender;

            switch (e.PropertyName) {

                case "ROIName":
                    __listRoi.RefreshObject(_roi);
                    break;
                case "ROIRectangle":

                    __listRoi.RefreshObject(_roi);
                    break;
                case "UpdatePropertyGrid":
                    //__propertyGridFunction.Refresh();
                    break;

                default:
                    break;
            }
        }


        // TODO COPY PASTE

        ROI CopiedROI = null;
        public void PasteROI() {

            try {
                if (CopiedROI != null) {


                    Rectangle rect = new Rectangle(CopiedROI.ROIShape.ShapeEfectiveBounds.Location, CopiedROI.ROIShape.ShapeEfectiveBounds.Size);
                    rect.X += 30;
                    rect.Y += 30;


                    ROI newroi = SelectedProject.SelectedRequest.SelectedInspection.AddROI(rect);
                    // SelectedInspection.UpdateROIRect(rect, newroi);


                    foreach (ProcessingFunctionBase item in CopiedROI.ProcessingFunctions) {

                        using (UndoRedoManager.StartInvisible("Init")) {
                            ProcessingFunctionBase newitem = (ProcessingFunctionBase)item.Clone();                            
                            newroi.ProcessingFunctions.Add(newitem);
                            UndoRedoManager.Commit();

                        }
                    }


                    __listRoi.Objects=SelectedProject.SelectedRequest.SelectedInspection.ROIList;
                    SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Last();

                }
            } catch (Exception exp) {

                log.Error( exp);
            }
        }
        public Boolean CopyROI(ROI ROIToCopy = null) {
            try {
                if (ROIToCopy == null) {
                    if (SelectedProject.SelectedRequest.SelectedInspection != null) {
                        CopiedROI = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI;
                    }
                } else {
                    CopiedROI = ROIToCopy;
                }

                return true;
            } catch (Exception exp) {

                log.Error( exp);
                return false;
            }
        }

        private void __listRoi_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.C) {
                if (e.Control) {
                    CopyROI();
                }
            } else if (e.KeyCode == Keys.V) {
                if (e.Control) {
                    PasteROI();
                }
            }
        }

        private void __listinspections_SelectedIndexChanged_1(object sender, EventArgs e) {

        }



        private void __listRoi_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(BrightIdeasSoftware.OLVListItem))) {
                e.Effect = DragDropEffects.Move;
            }

        }

        private void __listRoi_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void __listRoi_DragDrop(object sender, DragEventArgs e) {

            if (e.Data.GetDataPresent(typeof(BrightIdeasSoftware.OLVListItem))) {

                ListViewItem dragitem = (ListViewItem)e.Data.GetData(typeof(BrightIdeasSoftware.OLVListItem));
                int dragIndex = dragitem.Index;
                Point cp = __listRoi.PointToClient(new Point(e.X, e.Y));
                ListViewItem dropItem = __listRoi.GetItemAt(cp.X, cp.Y);
                int dropIndex = dropItem.Index;

                __listRoi.RemoveObject(dragitem);
                __listRoi.RemoveObject(dropItem);





            }



        }

        private void __listRoi_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = true;
            if (e.Control) {
                if (e.KeyCode == Keys.C) {
                    //CopyROI();
                } else if (e.KeyCode == Keys.V) {
                   // PasteROI();
                }
            }
        }

        private void __listRoi_KeyPress(object sender, KeyPressEventArgs e) {

        }
        Stopwatch stopWatch = new Stopwatch();
        private void __ListRequests_CanDrop(object sender, BrightIdeasSoftware.OlvDropEventArgs e) {
            try {


                e.Handled = true;
                e.Effect = DragDropEffects.None;
                if (!stopWatch.IsRunning) {
                    stopWatch.Reset();
                    stopWatch.Start();

                }

                if (e.DropTargetItem != null) {
                    if (stopWatch.ElapsedMilliseconds > 1000) {
                        SelectedProject.SelectedRequest = e.DropTargetItem.RowObject as Request;
                        stopWatch.Stop();
                        stopWatch.Reset();
                    }




                }
            } catch (Exception exp) {
                log.Error( exp);

            }
        }

        private void __listinspections_CanDrop(object sender, BrightIdeasSoftware.OlvDropEventArgs e) {
            try {


                e.Handled = true;
                e.Effect = DragDropEffects.None;
                if (!stopWatch.IsRunning) {
                    stopWatch.Reset();
                    stopWatch.Start();

                }

                if (e.DropTargetItem != null) {
                    if (stopWatch.ElapsedMilliseconds > 1000) {

                        SelectedProject.SelectedRequest.SelectedInspection = e.DropTargetItem.RowObject as Inspection;
                        stopWatch.Stop();
                        stopWatch.Reset();
                    }




                }
            } catch (Exception exp) {
                log.Error( exp);

            }
        }

        private void __listRoi_CanDrop(object sender, BrightIdeasSoftware.OlvDropEventArgs e) {
            try {


                e.Handled = true;
                e.Effect = DragDropEffects.None;
                if (!stopWatch.IsRunning) {
                    stopWatch.Reset();
                    stopWatch.Start();

                }

                if (e.DropTargetItem != null) {
                    if (stopWatch.ElapsedMilliseconds > 1000) {

                        SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = e.DropTargetItem.RowObject as ROI;
                        stopWatch.Stop();
                        stopWatch.Reset();
                    }

                }
            } catch (Exception exp) {
                log.Error( exp);

            }
        }

        private void __comboListAuxROI_DropDown(object sender, EventArgs e) {
            //__comboListAuxROI.Items.Clear();
            //foreach (Request req in SelectedProject.RequestList) {
            //    foreach (Inspection insp in req.Inspections) {
            //        __comboListAuxROI.Items.Add(req + "." + insp);
            //    }
            //}

        }

        private void __auxinsp_Enter(object sender, EventArgs e) {

        }

        private void __auxinsp_KeyPress(object sender, KeyPressEventArgs e) {

        }

        private void __auxinsp_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                //SelectedProject.SelectedRequest.SelectedInspection.AuxiaryInspection = StaticObjects.InspectionReferences.Find(a => a.Reference == __auxinsp.Text);
            }
        }

        TextBox auxTB = new TextBox();
        AutoCompleteStringCollection AutocompRef = new AutoCompleteStringCollection();
        private void __listinspections_CellEditStarting(object sender, CellEditEventArgs e) {
          
        }

        private void __listinspections_CellEditFinishing(object sender, CellEditEventArgs e) {

            try {

                if (e.Cancel) {
                    return;
                }

                Inspection _inspect = (Inspection)e.RowObject;



                #region change inspect name (checking duplicates)
                if (e.Column.Text == "Name") {
                    //
                    String newnameval = (String)e.NewValue;


                    int Numinsps = SelectedProject.SelectedRequest.Inspections.Count(insp => insp.Name == newnameval);

                    if (Numinsps == 0 && newnameval != (String)e.Value) {


                        
                        

                        using (UndoRedoManager.Start("Inspection name changed: " + newnameval)) {
                            _inspect.Name = newnameval;
                            UndoRedoManager.Commit();
                        }


                        e.Cancel = false;
                    } else {
                        e.Cancel = true;

                        if (Numinsps > 1) {
                            MessageBox.Show("Inspection name exists", "Invalid inepction name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }


                } else if (e.Column.Text == "Position") {
                    e.Cancel = true;
                    int newposval = (int)e.NewValue;



                    int totalinsp = SelectedProject.SelectedRequest.Inspections.Count;
                    //if (newposval > totalinsp) {
                    //    return;
                    //}
                    //Inspection theinsp = SelectedProject.SelectedRequest.Inspections.Find(pos => pos.InspPos == newposval);
                    //if (theinsp != null) {

                    //if (_inspect.Name != _inspect.Name) {


                    int actpos = _inspect.InspPos;
                    if (newposval >= _inspect.InspPos) {
                        _inspect.InspPos = newposval;
                        var list = SelectedProject.SelectedRequest.Inspections.Where(Pos => (Pos.InspPos < newposval) && (Pos.InspPos < actpos)).OrderBy(bypos => bypos.InspPos);
                        foreach (Inspection insp in list) {
                            insp.InspPos--;
                        }
                        _inspect.InspPos--;
                    } else {
                        _inspect.InspPos = newposval;
                        var list = SelectedProject.SelectedRequest.Inspections.Where(Pos => (Pos.InspPos > newposval) && (Pos.InspPos > actpos)).OrderBy(bypos => bypos.InspPos);
                        foreach (Inspection insp in list) {
                            insp.InspPos++;
                        }
                        _inspect.InspPos++;
                    }




                    e.Cancel = false;
                   
                }
                
                  else {
                    e.Cancel = false;
                }

                __listinspections.Sort(olvColumnInsppos);
                __listinspections.RefreshObjects(SelectedProject.SelectedRequest.Inspections);

                #endregion

                
                
            } catch (Exception exp) {

                log.Error( exp);
            }
        }




        private void __listRoi_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
           
        }

        private void __ListAuxROIS_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
           
        }

        private void __ListAuxROIS_MouseClick(object sender, MouseEventArgs e) {
           
        }

        private void __listRoi_MouseClick(object sender, MouseEventArgs e) {
           
        }

        private void __listRoi_Click(object sender, EventArgs e) {
            try {
                ROI selected = __listRoi.SelectedObject as ROI;

                SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = selected;



            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __ListAuxROIS_Click(object sender, EventArgs e) {
            try {
                ROI selected = __ListAuxROIS.SelectedObject as ROI;

                SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = selected;



            } catch (Exception exp) {

                log.Error( exp);
            }
        }


        

        

       
        private void __listinspections_SelectionChanged(object sender, EventArgs e) {
            try {
                if (__listinspections.SelectedObject == null) {
                    __listinspections.SelectObject(SelectedProject.SelectedRequest.SelectedInspection);
                }
                SelectedProject.SelectedRequest.SelectedInspection = (Inspection)__listinspections.SelectedObject;

            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __listinspections_KeyUp(object sender, KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.Delete) {
                    if (__listinspections.SelectedIndex > -1 && AcessManagement.AcessLevel == Acesslevel.Admin) {

                        SelectedProject.SelectedRequest.RemoveInspection(__listinspections.SelectedObject as Inspection);

                    }
                }
            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void ListInspForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }


        private void __listRoi_CellEditFinishing(object sender, CellEditEventArgs e) {
            try {

                if (e.Cancel) {
                    return;
                }

                ROI _roi = (ROI)e.RowObject;

                if (e.Control is ComboBox) {
                    object selecteobject = ((ComboBox)e.Control).SelectedItem;
                    if (selecteobject is ReferencePoint) {
                        if (e.Column.Text == "Reference Point") {
                            using (UndoRedoManager.Start(_roi.Name + ": Reference Position Changed to: " + ((ReferencePoint)selecteobject).ReferencePointName)) {
                                _roi.referencePoint = (ReferencePoint)selecteobject;
                                UndoRedoManager.Commit();
                            }
                            __listRoi.RefreshObject(_roi);
                            e.Cancel = true;
                        }
                    }
                } else if (e.Column == __olvROIName) {
                    e.Cancel = true;
                    String newnameval = (String)e.NewValue;

                    var roinamelist = from newroi in SelectedProject.SelectedRequest.SelectedInspection.ROIList
                                      where newroi.Name == newnameval
                                      select newroi;

                    if (roinamelist != null) {
                        if (roinamelist.Count() > 0) {
                            return;
                        } else {
                            e.Cancel = false;

                            using (UndoRedoManager.Start("ROI name changed to: " + newnameval)) {
                                _roi.Name = newnameval;
                                UndoRedoManager.Commit();
                            }

                        }

                    }


                } else if (e.Column == __olvROIPos) {
                    e.Cancel = true;
                    int newposval = (int)e.NewValue;



                    int totalroi = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Count;
                    if (newposval > totalroi) {
                        return;
                    }

                    ROI theroi = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Find(pos => pos.ROIPos == newposval);
                    if (theroi != null) {

                        if (theroi.Name != _roi.Name) {
                            using (UndoRedoManager.Start("ROI position changed to: " + newposval)) {
                                int actpos = _roi.ROIPos;
                                if (newposval > _roi.ROIPos) {
                                    _roi.ROIPos = newposval;
                                    var list = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Where(Pos => (Pos.ROIPos < newposval) && (Pos.ROIPos > actpos)).OrderBy(bypos => bypos.ROIPos);
                                    foreach (ROI roi in list) {
                                        roi.ROIPos--;
                                    }
                                    theroi.ROIPos--;
                                } else {
                                    _roi.ROIPos = newposval;
                                    var list = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Where(Pos => (Pos.ROIPos > newposval) && (Pos.ROIPos < actpos)).OrderBy(bypos => bypos.ROIPos);
                                    foreach (ROI roi in list) {
                                        roi.ROIPos++;
                                    }
                                    theroi.ROIPos++;
                                }
                                UndoRedoManager.Commit();
                            }

                            e.Cancel = false;
                        }


                    } else {
                        using (UndoRedoManager.Start("ROI position changed to: " + newposval)) {
                            _roi.ROIPos = newposval;
                            UndoRedoManager.Commit();
                        }
                        e.Cancel = false;
                    }

                    __listRoi.Sort(__olvROIPos);
                    __listRoi.RefreshObjects(SelectedProject.SelectedRequest.SelectedInspection.ROIList);

                }
               
            } catch (Exception exp) {
                log.Error( exp);
            }
        }

       

        private void __toolremoveRequest_Click(object sender, EventArgs e) {
            try {
                if (SelectedProject.SelectedRequest != null) {
                    SelectedProject.RemoveRequest(SelectedProject.SelectedRequest);
                }
            } catch (Exception exp) {

                log.Error( exp);
            }
        }


        public void AddRequest() {
            try {

                int itemcount = SelectedProject.RequestList.Count(String => String.Name.Contains("New Request")) + 1;

                Request newRequest = new Request("New Request " + itemcount.ToString());



                int maxid = 0;

                if (SelectedProject.RequestList.Count > 0) {

                    maxid = SelectedProject.RequestList.Max(ids => ids.ID);

                }

                int i;
                for (i = 1; i < maxid + 1; i++) {
                    if (!SelectedProject.RequestList.Exists(ids => ids.ID == i)) break;
                }


                newRequest.ID = i;
                using (UndoRedoManager.Start("Request Add:" + newRequest.Name)) {

                    SelectedProject.RequestList.Add(newRequest);
                    UndoRedoManager.Commit();
                }

                //RoiInfo.ListRequests.Add(new RoiInfo.RequestInfo(newRequest.Name));

                
            } catch (Exception exp) {

                log.Error( exp);
            }

        }


        public void DialogNewRequest() {
            //DialogResult dlgResult = MessageBox.Show("Add new Request", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            DialogResult dlgResult = System.Windows.Forms.DialogResult.Yes;
            if (dlgResult == System.Windows.Forms.DialogResult.Yes) {

                AddRequest();

            }


        }

        private void __tooladdRequest_Click(object sender, EventArgs e) {
            DialogNewRequest();
        }

        private void __toolRequestSettings_Click(object sender, EventArgs e) {
            if (SelectedProject.SelectedRequest != null) {
                //_IOSettingsForm.SelectedObject = SelectedProject.SelectedRequest;
                //_IOSettingsForm.__ListIOEvents.Objects = SelectedProject.SelectedRequest.IOEvents;
                //_IOSettingsForm.ShowDialog();
            }
        }
        public void DialogNewInsp() {
            try {
                //DialogResult dlgResult = MessageBox.Show("Add new inspection", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                DialogResult dlgResult = System.Windows.Forms.DialogResult.Yes;
                if (dlgResult == System.Windows.Forms.DialogResult.Yes) {


                    SelectedProject.SelectedRequest.AddInspection(SelectedProject);



                }
            } catch (Exception exp) {

                log.Error( exp);
            }


        }

        private void __toolInspSettings_Click(object sender, EventArgs e) {
            //if (SelectedProject.SelectedRequest != null) {
            //    if (SelectedProject.SelectedRequest.SelectedInspection != null) {
            //        _IOSettingsForm.SelectedObject = SelectedProject.SelectedRequest.SelectedInspection;
            //        _IOSettingsForm.__ListIOEvents.Objects = SelectedProject.SelectedRequest.SelectedInspection.IOEvents;
            //        _IOSettingsForm.ShowDialog();
            //    }
            //}
        }

        private void __tooladdInspection_Click(object sender, EventArgs e) {
            DialogNewInsp();
        }



        void AddRoiDialog() {
            try {
                //DialogResult dlgResult = MessageBox.Show("Add new ROI", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                DialogResult dlgResult = System.Windows.Forms.DialogResult.Yes;
                if (dlgResult == System.Windows.Forms.DialogResult.Yes) {

                    ROI newroi = SelectedProject.SelectedRequest.SelectedInspection.AddROI(new Rectangle(0, 0, 100, 100));
                    
                    for (int i = 0; i < SelectedProject.SelectedRequest.SelectedInspection.ROIList.Count; i++) {
                        String roiname = "ROI"+i.ToString();
                        if (!SelectedProject.SelectedRequest.SelectedInspection.ROIList.Exists(r=>r.Name==roiname)) {
                            newroi.Name = roiname;
                            break;
                        }
                    }

                    __listRoi.Objects = SelectedProject.SelectedRequest.SelectedInspection.ROIList;


                    __listRoi.SelectedIndex = __listRoi.GetItemCount() - 1;
                    SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = newroi;
                }


            } catch (Exception exp) {

                log.Error( exp);
            }

        }


        private void __toolremoveInspection_Click(object sender, EventArgs e) {
            try {
                if (SelectedProject.SelectedRequest.SelectedInspection != null) {
                    SelectedProject.SelectedRequest.RemoveInspection(SelectedProject.SelectedRequest.SelectedInspection);
                }
            } catch (Exception exp) {
                log.Error( exp);
            }
        }

        private void __tooladdROI_Click(object sender, EventArgs e) {
            AddRoiDialog();
        }
        void RemoveRoi() {

            try {
                ROI _roi = (ROI)__listRoi.SelectedObject;
                if (_roi != null) {
                    if (SelectedProject.SelectedRequest.SelectedInspection != null) {
                        SelectedProject.SelectedRequest.SelectedInspection.RemoveROI(_roi);                        
                    }
                }
            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __toolRemoveRoi_Click(object sender, EventArgs e) {
            RemoveRoi();
        }

        private void __ListRequests_KeyUp(object sender, KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.Delete) {
                    if (__ListRequests.SelectedIndex > -1) {
                        if (AcessManagement.AcessLevel == Acesslevel.Admin) {
                            SelectedProject.RemoveRequest(SelectedProject.SelectedRequest);
                        }


                    }
                }
            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __ListRequests_SelectionChanged(object sender, EventArgs e) {
            try {

                if (__ListRequests.SelectedObject == null) {
                    __ListRequests.SelectObject(SelectedProject.SelectedRequest);
                }

                SelectedProject.SelectedRequest = (Request)__ListRequests.SelectedObject;



            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __ListRequests_CellEditFinishing(object sender, CellEditEventArgs e) {

            try {
                if (e.Cancel) {
                    return;
                }
                Request _req = (Request)e.RowObject;

                #region change request name (checking duplicates)

                if (e.Column.Text == "Name") {


                    String newnameval = (String)e.NewValue;

                    int NumRequests = SelectedProject.RequestList.Count(req => req.Name == newnameval);

                    if (NumRequests == 0 && newnameval != (String)e.Value) {




                        using (UndoRedoManager.Start("Request name changed:" + newnameval)) {
                            _req.Name = newnameval;
                            UndoRedoManager.Commit();
                        }


                        e.Cancel = false;
                    } else {
                        e.Cancel = true;
                        if (NumRequests > 1) {
                            MessageBox.Show("Request name exists", "Invalid request name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                } else if (e.Column.Text == "ID") {
                    if (SelectedProject.RequestList.Find(i => (i.ID == (int)(e.NewValue))) != null) {
                        e.Cancel = true;
                    }
                }
               
                #endregion

                
            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __listRoi_CellEditStarting(object sender, CellEditEventArgs e) {
            try {
                // Ignore edit events for other columns 
                //if (e.Column != olvColumnPrePos)
                //    return;
                //ComboBox cb = new ComboBox();
                //cb.Bounds = e.CellBounds;
                //cb.Font = ((ObjectListView)sender).Font;
                //cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //cb.Items.AddRange(StaticObjects.ReferencePoints.ToArray());
                //int i = 0;
                //for (; i < cb.Items.Count; i++) {
                //    if (((ReferencePoint)cb.Items[i]).ReferencePointName == SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.referencePoint.ReferencePointName) {
                //        break;
                //    }
                //}

                //if (i >= cb.Items.Count) {
                //    i = -1;
                //}
                //cb.SelectedIndex = i;
                //if (cb.SelectedIndex < 0) {
                //    cb.SelectedIndex = 0;
                //}
                //e.Control = cb;
            } catch (Exception exp) {

                log.Error( exp);
            }
        }

        private void __listRoi_KeyUp_1(object sender, KeyEventArgs e) {

        }

        private void __listRoi_DragOver(object sender, DragEventArgs e) {
            if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 3000) {
                stopWatch.Reset();
                stopWatch.Start();
            }
            if (stopWatch.ElapsedMilliseconds > 1000) {
                ObjectListView list = sender as ObjectListView;
                if (list.DropSink != null) {
                    if (((SimpleDropSink)list.DropSink).DropTargetItem == null) {
                        return;
                    }
                    SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = SelectedProject.SelectedRequest.SelectedInspection.ROIList.Find(name => name.Name == ((SimpleDropSink)list.DropSink).DropTargetItem.Text);
                }
                stopWatch.Stop();
            }
        }

        private void __listinspections_DragOver(object sender, DragEventArgs e) {
            if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 3000) {
                stopWatch.Reset();
                stopWatch.Start();
            }
            if (stopWatch.ElapsedMilliseconds > 1000) {
                ObjectListView list = sender as ObjectListView;
                if (list.DropSink != null) {
                    if (((SimpleDropSink)list.DropSink).DropTargetItem == null) {
                        return;
                    }
                    SelectedProject.SelectedRequest.SelectedInspection = SelectedProject.SelectedRequest.Inspections.Find(name => name.Name == ((SimpleDropSink)list.DropSink).DropTargetItem.Text);
                }
                stopWatch.Stop();
            }
        }

        private void __ListRequests_DragOver(object sender, DragEventArgs e) {

            if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 3000) {
                stopWatch.Reset();
                stopWatch.Start();
            }
            if (stopWatch.ElapsedMilliseconds > 1000) {
                ObjectListView list = sender as ObjectListView;
                if (list.DropSink != null) {
                    if (((SimpleDropSink)list.DropSink).DropTargetItem == null) {
                        return;
                    }
                    SelectedProject.SelectedRequest = SelectedProject.RequestList.Find(name => name.Name == ((SimpleDropSink)list.DropSink).DropTargetItem.Text);
                }
                stopWatch.Stop();
            }
        }
    }
}