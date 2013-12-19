using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PopupControl;
using System.Reflection;
using BrightIdeasSoftware;
using KPPAutomationCore.KPPCustomControls;

namespace KPPAutomationCore.KPPCustomControls {
    public partial class KPPComboBox : UserControl {

        Popup popup;

        ProcessingFunctionSelection ProcessingFunctions = new ProcessingFunctionSelection();

        private List<Object> _Objects;
        public List<Object> Objects {
            get { return _Objects; }
            set {
                if (_Objects!=value) {
                    _Objects = value;
                    //SetObjects();
                    ProcessingFunctions.__listfunc.Objects = Objects;
                }
            }
        }

        private void SetObjects() {
          
        }

        private String m_DefaultText = "None Selected";

        public String DefaultText {
            get { return m_DefaultText; }
            set { 
                m_DefaultText = value;
                borderLabel.Text = value;
            }
        }



        private object _SelectedObject;
        public object SelectedObject {
            get { return _SelectedObject; }
            private set {
                if (_SelectedObject != value) {
                    _SelectedObject = value;
                }
            }

        }

        //public void SetVisible(object item, Boolean visible) {
        //    //ProcessingFunctions.__listfunc.Items[].
        //}

        public KPPComboBox() {
            InitializeComponent();
            popup = new Popup(ProcessingFunctions);
            
            if (SystemInformation.IsComboBoxAnimationEnabled) {
                popup.ShowingAnimation = PopupAnimations.Slide | PopupAnimations.TopToBottom;
                popup.HidingAnimation = PopupAnimations.Slide | PopupAnimations.BottomToTop;
            }
            else {
                popup.ShowingAnimation = popup.HidingAnimation = PopupAnimations.None;
                
            }

            //ProcessingFunctions.__listfunc.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(__listfunc_ItemSelectionChanged);
            popup.Opened += new EventHandler(popup_Opened);
            popup.Closed += new ToolStripDropDownClosedEventHandler(popup_Closed);
            popup.AutoClose = true;
            ProcessingFunctions.__listfunc.SelectedIndexChanged += new EventHandler(__listfunc_SelectedIndexChanged);
            ProcessingFunctions.__listfunc.AlwaysGroupByColumn = ProcessingFunctions.__olvColumnGroup;
        }

        void popup_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            popupopened = false;
        }

        private Boolean popupopened = false;
        void popup_Opened(object sender, EventArgs e) {
            popupopened = true;
        }

        void __listfunc_SelectedIndexChanged(object sender, EventArgs e) {
            if (ProcessingFunctions.__listfunc.SelectedIndex>=0) {
                this.borderLabel.Text = ProcessingFunctions.__listfunc.SelectedObject.ToString();
                this.SelectedObject = ProcessingFunctions.__listfunc.SelectedObject;
                //foreach (OLVGroup item in ProcessingFunctions.__listfunc.Groups) {
                //    if (item.Items.ToList().Find(lst => lst.Name == ProcessingFunctions.__listfunc.SelectedItem.Group.Name)==null) {
                //        item.Collapsed = true;
                //    }   
                //}
                popup.Close();
            }
        }

        private void buttonDropDown_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button== System.Windows.Forms.MouseButtons.Left) {
                if (popup.Width < Width) {
                    popup.Width = Width;
                }

                if (!popupopened) {
                    popup.Show(this);
                }
                else {
                    popup.Close();
                }
            }
        }

        //void __listfunc_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            
        //}

                
    }
}
