using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KPP.Core.Debug;

namespace KPPAutomationCore.KPPCustomControls {
    public partial class ProcessingFunctionSelection  : UserControl {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionSelection));

        public ProcessingFunctionSelection() {
            InitializeComponent();
        }

        private void __listfunc_GroupTaskClicked(object sender, BrightIdeasSoftware.GroupTaskClickedEventArgs e) {
            
        }

        private void __listfunc_AfterCreatingGroups(object sender, BrightIdeasSoftware.CreateGroupsEventArgs e) {
            try {
                foreach (BrightIdeasSoftware.OLVGroup item in e.Groups) {
                    item.Collapsed = true;

                }
                finishedCollapsed = true;
            }
            catch (Exception exp) {
                log.Error(exp);
                
            }
        }

        private Boolean finishedCollapsed = false;
        private void __listfunc_GroupExpandingCollapsing(object sender, BrightIdeasSoftware.GroupExpandingCollapsingEventArgs e) {
            try {
                
                
            }
            catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __listfunc_GroupStateChanged(object sender, BrightIdeasSoftware.GroupStateChangedEventArgs e) {
            try {
                
            }
            catch (Exception exp) {

                log.Error(exp);
            }
        }
    }
}
