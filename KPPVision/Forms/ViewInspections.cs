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
using System.Threading;
using System.Globalization;
using KPPAutomationCore;

namespace VisionModule {
    internal partial class ViewInspections : DockContent {



        private static KPPLogger log = new KPPLogger(typeof(ViewInspections));

        public ViewInspections() {
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

        private void ViewInspections_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            //InspImageCtrl _InspImageCtrl = new InspImageCtrl(__containerpanel);            
            //_InspImageCtrl.Show();
        }

        public event EventHandler OnCtrSelected;
        public event EventHandler OnCtrDoubleClick;
        public void AddImage(Image img,String caption, object tag)
        {

            try {
                if (tag != null) {
                    InspImageCtrl ctrl = new InspImageCtrl();
                    ctrl.Image = img;
                    ctrl.Caption = caption;
                    ctrl.Tag = tag;
                    if (ctrl.Tag is Inspection) {
                        ((Inspection)ctrl.Tag).OnInspectionNameChanged += new Inspection.InspectionNameChanged(ViewInspections_OnInspectionNameChanged);
                    }
                    ctrl.OnCtrlClick += new EventHandler(ctrl_OnCtrlClick);
                    ctrl.OnCtrlDblClick += new EventHandler(ctrl_OnCtrlDblClick);
                    if (InvokeRequired) {
                        this.Invoke(new MethodInvoker(delegate { flowLayoutPanel1.Controls.Add(ctrl); }));
                    }
                    else {
                        flowLayoutPanel1.Controls.Add(ctrl);
                    }
                    
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
            
        }

        void ctrl_OnCtrlDblClick(object sender, EventArgs e) {
            try {
                foreach (Control ctrl in flowLayoutPanel1.Controls) {
                    if (ctrl == sender) {
                        if (OnCtrSelected != null) {
                            OnCtrSelected(ctrl.Tag, e);
                            OnCtrDoubleClick(ctrl.Tag, e);
                        }
                        break;
                    }
                }
            } catch (Exception exp) {
                
                log.Error(exp);
            }
        }

        void ctrl_OnCtrlClick(object sender, EventArgs e) {
            try {
                foreach (Control ctrl in flowLayoutPanel1.Controls) {
                    if (ctrl == sender) {
                        if (OnCtrSelected != null) {
                            OnCtrSelected(ctrl.Tag, e);
                        }
                        break;
                    }
                }
            } catch (Exception exp) {
                
                log.Error(exp);
            }

            
        }

        void ViewInspections_OnInspectionNameChanged(string RequestInspName,string NewInspName, object Insp) {
            try {
                if (Insp != null) {
                    foreach (Control ctrl in flowLayoutPanel1.Controls) {
                        if (ctrl.Tag != null && ctrl.Tag.Equals(Insp) && ctrl is InspImageCtrl) {
                            ((InspImageCtrl)ctrl).Caption = RequestInspName+"-"+NewInspName;
                            break;
                        }
                    }
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        public void SelectControl(object tag) {
            try {
                if (tag != null) {
                    foreach (Control ctrl in flowLayoutPanel1.Controls) {
                        if (ctrl.Tag != null && ctrl.Tag.Equals(tag) && ctrl is InspImageCtrl) {
                            ((InspImageCtrl)ctrl).IsSelected = true;

                        } else {
                            ((InspImageCtrl)ctrl).IsSelected = false;
                        }
                    }
                }
            } catch (Exception exp) {
                
                log.Error(exp);
            }
        }

        public void SetControlState(object tag,Boolean State) {
            try {
                if (tag != null) {
                    foreach (Control ctrl in flowLayoutPanel1.Controls) {
                        if (ctrl.Tag != null && ctrl.Tag.Equals(tag) && ctrl is InspImageCtrl) {
                            ((InspImageCtrl)ctrl).IsRejected = State;
                            break;
                        }
                    }
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }
        public void RemoveControl(object tag) {
            try {
                if (tag != null) {
                    foreach (Control ctrl in flowLayoutPanel1.Controls) {
                        if (ctrl.Tag != null && ctrl.Tag.Equals(tag) && ctrl is InspImageCtrl) {
                            flowLayoutPanel1.Controls.Remove(ctrl);
                            break;
                        }
                    }
                }
            } catch (Exception exp) {

                
            }                    
        }
        

        public void ClearControls() {
            // Clear => dispose control contents???
            flowLayoutPanel1.Controls.Clear();
        }

        public void UpdateImage(object tag) {
            try {
                if (tag != null) {
                    foreach (Control ctrl in flowLayoutPanel1.Controls) {
                        if (ctrl.Tag != null && ctrl.Tag.Equals(tag) && ctrl is InspImageCtrl) {
                            Inspection theinsp = (Inspection)tag;
                            //lock (theinsp.ImageLocker) {
                            if (theinsp.OriginalImageBgr != null) {
                                theinsp.OriginalImageBgr.ROI = Rectangle.Empty;
                                if (theinsp.OriginalImageBgr.Size != Size.Empty) {
                                    ((InspImageCtrl)ctrl).Image = theinsp.ResultImageBgr.ToBitmap();
                                } else {
                                    ((InspImageCtrl)ctrl).Image = null;
                                }

                                break;
                            }
                            // }
                        }
                    }
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        public Boolean ViewFormIsVisible = false;
        private void ViewInspections_Leave(object sender, EventArgs e) {
            ViewFormIsVisible = false;
        }

        private void ViewInspections_Enter(object sender, EventArgs e) {
            ViewFormIsVisible = true;
        }

      
    }
}
