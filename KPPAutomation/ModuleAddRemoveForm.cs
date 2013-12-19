using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KPP.Core.Debug;

namespace KPPAutomation {
    public partial class ModuleAddRemoveForm : Form {
        public ModuleAddRemoveForm() {
            InitializeComponent();
        }
        private static KPPLogger log = new KPPLogger(typeof(ModuleAddRemoveForm));

        public ApplicationSettings AppSettings = null;

        private void ModuleAddRemoveForm_Load(object sender, EventArgs e) {

            if (AppSettings!=null) {
                __ListModules.Objects = AppSettings.Modules;
            }
        }

        private void __comboModuleTypes_SelectedIndexChanged(object sender, EventArgs e) {
            if (__comboModuleTypes.SelectedIndex>-1) {
                __btAddModule.Enabled = true;

            }
            else {
                __btAddModule.Enabled = false;
            }
        }

        private void __btAddModule_Click(object sender, EventArgs e) {
            try {
                Type tp = (Type)__comboModuleTypes.SelectedItem;
                AppSettings.Modules.Add((KPPModule)Activator.CreateInstance(tp));
                __ListModules.Objects = AppSettings.Modules;
            }
            catch (Exception exp) {

                log.Error(exp);
            }

        }

        private void __ListModules_SelectedIndexChanged(object sender, EventArgs e) {
            if (__ListModules.SelectedIndex>-1) {
                __btRemoveModule.Enabled = true;
            }
        }

        private void __btRemoveModule_Click(object sender, EventArgs e) {
            AppSettings.Modules.Remove((KPPModule)__ListModules.SelectedObject);
            __ListModules.Objects = AppSettings.Modules;
        }
    }
}
