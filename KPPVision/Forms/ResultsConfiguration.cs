using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BrightIdeasSoftware;
using System.Diagnostics;
using DejaVu;
using KPP.Core.Debug;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;

namespace VisionModule {

    internal partial class ResultsConfiguration : DockContent {
        private static KPPLogger log = new KPPLogger(typeof(ResultsConfiguration));

        public ResultsConfiguration() {
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

        Request _SelectedRequest;

        public Request SelectedRequest {
            get {
                return _SelectedRequest; 
            }
            set {
                if (_SelectedRequest != value) {
                    _SelectedRequest = value;
                    if (value != null) {
                        _SelectedRequest = value;
                        __btAddResult.Enabled = true;
                        //__listinspResults.ClearObjects();
                        __listInputs.ClearObjects();
                        
                        __listinspResults.Objects = _SelectedRequest.Results;

                        if (_SelectedRequest.Results.Count > 0) {
                            __listinspResults.SelectedObject = _SelectedRequest.Results.First();
                            
                        }
                    } else {
                        __btAddResult.Enabled = false;
                        __listInputs.ClearObjects();
                        
                    }
                }
            }
        }


        private void __btAddResult_Click(object sender, EventArgs e) {
        
            try {
                ResultInfo newresult = new ResultInfo();

                //SelectedInspection.SetOverrideID(StaticObjects.SelectedProject.SelectedRequest.Name, StaticObjects.SelectedProject.SelectedRequest.ID);

                using (UndoRedoManager.Start(SelectedRequest.Name + " Result info added: " + newresult.ID)) {
                    SelectedRequest.Results.Add(newresult);
                    UndoRedoManager.Commit();

                }


               

                //__listinspResults.ClearObjects();
                __listinspResults.Objects = SelectedRequest.Results;
                __listinspResults.SelectedIndex = __listinspResults.Items.Count - 1;
            } catch (Exception exp) {

                log.Error(exp);
            }
        }


        

        private void ResultsConfiguration_Load(object sender, EventArgs e) {
            try {

                __listinspResults.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(__listinspResults_ItemSelectionChanged);
                __listInputs.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(__listInputs_ItemSelectionChanged);
                SimpleDropSink sink1 = (SimpleDropSink)__listInputs.DropSink;
                sink1.CanDropOnSubItem = true;
                sink1.FeedbackColor = Color.IndianRed;
                AcessManagement.OnAcesslevelChanged += new AcessManagement.AcesslevelChanged(StaticObjects_OnAcesslevelChanged);
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        void StaticObjects_OnAcesslevelChanged(Acesslevel NewLevel) {

            Boolean state = (NewLevel == Acesslevel.Admin);

            __btAddResult.Enabled = state;
            __BtAddInput.Enabled = state;
            __btRemove.Enabled = state;
            __BtRemoveInput.Enabled = state;

            switch (NewLevel) {
                case Acesslevel.Admin:
                    __listInputs.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
                    
                    break;
                case Acesslevel.User:
                    __listInputs.CellEditActivation = ObjectListView.CellEditActivateMode.None;

                    break;
                case Acesslevel.NotSet:
                    break;
                default:
                    break;
            }

        }

        void __listInputs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            try {
                if (__listInputs.SelectedObject != null) {
                    if (e.Item != null) {                        
                        __BtRemoveInput.Enabled = true;
                        
                    } else {
                        __BtRemoveInput.Enabled = false;                        
                        
                        
                    }
                } else {
                    __BtRemoveInput.Enabled = false;
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        void __listinspResults_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            try {
                if (__listinspResults.SelectedObject!=null) {
                    if (e.Item != null) {
                        __BtAddInput.Enabled = true;
                        __btRemove.Enabled = true;
                        
                        ResultInfo resinf=(ResultInfo)__listinspResults.SelectedObject;
                        
                        __listInputs.Objects=resinf.Inputs;
                        if (__listInputs.GetItemCount()>0) {
                            __listInputs.SelectedIndex=0;
                        } else {
                            __BtRemoveInput.Enabled = false;
                        }
                    } else {
                        __BtAddInput.Enabled = false;
                        
                        __btRemove.Enabled = false;
                    } 
                } else {
                    __listInputs.SelectedIndex=-1;
                    __BtAddInput.Enabled = false;
                    __listInputs.ClearObjects();
                    __btRemove.Enabled = false;
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __btRemove_Click(object sender, EventArgs e) {
            try {
                int lastindex=__listinspResults.SelectedIndex;
                
                ResultInfo toRemove = __listinspResults.SelectedObject as ResultInfo;
                if (toRemove != null) {
                    using (UndoRedoManager.Start(SelectedRequest.Name + " Removing Result info:" + toRemove.ID)) {
                        SelectedRequest.Results.Remove(toRemove);
                        UndoRedoManager.Commit();
                    }
                    __listinspResults.Objects = SelectedRequest.Results;
                }
                if (lastindex==0) {
                    __listinspResults.SelectedIndex = lastindex;
                }
                else if (lastindex==-1) {
                    
                }
                else {
                    __listinspResults.SelectedIndex = lastindex - 1;
                }

              

            } catch (Exception exp) {
                log.Error(exp);
            }
        }

        private void __BtAddInput_Click(object sender, EventArgs e) {
            try {
                ResultInput newinput;
                using (UndoRedoManager.StartInvisible("Init")) {
                    newinput = new ResultInput();
                    UndoRedoManager.Commit();
                }


                ResultInfo theresinfo=(ResultInfo)__listinspResults.SelectedObject;

                using (UndoRedoManager.Start(theresinfo.ID+" Result input added: "+newinput)) {

                    theresinfo.Inputs.Add(newinput);
                    UndoRedoManager.Commit();
                }
                __listInputs.ClearObjects();
                __listInputs.AddObjects(((ResultInfo)__listinspResults.SelectedObject).Inputs);


            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __BtRemoveInput_Click(object sender, EventArgs e) {
            try {
                int lastindex = __listInputs.SelectedIndex;

                ResultInput toRemove = __listInputs.SelectedObject as ResultInput;
                if (toRemove != null) {

                    ResultInfo selected = __listinspResults.SelectedObject as ResultInfo;

                    if (selected!=null) {
                        using (UndoRedoManager.Start(selected.ID + " Result input removed: " + toRemove)) {
                            selected.Inputs.Remove(toRemove);
                            UndoRedoManager.Commit();
                        }
                        __listInputs.RemoveObject(toRemove);
                    }
                }

                if (lastindex == 0) {
                    __listInputs.SelectedIndex = lastindex;
                } else {
                    __listInputs.SelectedIndex = lastindex - 1;
                }

            } catch (Exception exp) {
                log.Error(exp);
            }
        }


        Type[] allowedTypes = new Type[] { typeof(int), typeof(double)};

        
        int droppedindex = -1;
        private void __listInputs_CanDrop(object sender, OlvDropEventArgs e) {
            try {
                e.Handled = true;
                e.Effect = DragDropEffects.None;
                
                if (e.DropTargetItem != null) {
                    
                    SimpleDropSink sink1 = (SimpleDropSink)__listInputs.DropSink;
                    
                    OLVListSubItem subitem = e.DropTargetItem.GetSubItem(e.DropTargetSubItemIndex);
                    //Boolean teste = e.DataObject.GetType().GetCustomAttributes(true).Contains(new UseInResultInput(true));
                    if (subitem != null ) {
                        if (__listInputs.Columns[e.DropTargetSubItemIndex] == __Input) {
                            e.Effect = DragDropEffects.Copy;
                            e.InfoMessage = e.DataObject as String;
                            sink1.FeedbackColor = Color.Green;
                            droppedindex = e.DropTargetIndex;
                        }
                        else {
                            
                            
                            sink1.FeedbackColor = Color.IndianRed;
                        }
                    }
                    
                    
                }
            } catch (Exception exp) {
                log.Error(exp);
                
            }
        }

     

        private void __listInputs_DragDrop(object sender, DragEventArgs e) {
            try {
                if (e.Data != null) {
                    object passedData = e.Data.GetData(typeof(ResultReference));
                    if (passedData!=null) {
                        ResultInput newresin = __listInputs.GetModelObject(droppedindex) as ResultInput;
                        if (newresin != null) {
                            ResultInfo theresinfo = (ResultInfo)__listinspResults.SelectedObject;
                            using (UndoRedoManager.Start(theresinfo.ID + " Input changed: " + (passedData as ResultReference))) {
                                newresin.Input = passedData as ResultReference;
                                UndoRedoManager.Commit();
                            }
                            __listInputs.RefreshObject(newresin);
                        }
                    }

                }
            } catch (Exception exp) {
                log.Error(exp);
                
            }
        }

        private void __listInputs_SelectedIndexChanged(object sender, EventArgs e) {

        }


        private void __listInputs_Enter(object sender, EventArgs e) {
            __listInputs.RefreshObject(__listInputs.SelectedObject);
        }

       

        private void Bt_enabledChanged(object sender, EventArgs e) {
            if ((sender as Button).Enabled && AcessManagement.AcessLevel == Acesslevel.User) {
                (sender as Button).Enabled = false;
            }
        }

        

      
    }
}
