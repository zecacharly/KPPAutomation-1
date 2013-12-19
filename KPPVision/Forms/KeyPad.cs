using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace VisionModule {
    internal partial class KeyPadControl : UserControl {
        public KeyPadControl() {
            InitializeComponent();
            
        }

        /*
         * P
         * */

        public PropertyGrid TheGrid = null;

        private void btok_Click(object sender, EventArgs e) {
            try {


                PropertyDescriptor propdesc = TheGrid.SelectedGridItem.PropertyDescriptor;
                Object component=null;

                if (TheGrid.SelectedGridItem.Parent.Value==null) {
                    component = TheGrid.SelectedObject;
                } else {
                    component = TheGrid.SelectedGridItem.Parent.Value;
                }
                if (propdesc.Converter != null) {

                    TypeConverter tc = propdesc.Converter; 

                    if (!tc.CanConvertFrom(typeof(String))) {
                        this.Hide();
                    }
                    Object converted = tc.ConvertTo(__txtValue.Text,TheGrid.SelectedGridItem.Value.GetType());

                    propdesc.SetValue(component, converted);

                    TheGrid.Refresh();
                }

                this.Hide();
            } catch (Exception exp) {                
                
            }
        
        }

        private void btesc_Click(object sender, EventArgs e) {
            this.Hide();
        }

        private void bt_Click(object sender, EventArgs e) {
            Button bt = (Button)sender;
            if (FirstClick) {
                FirstClick = false;
                __txtValue.Text = "";
            }
            if (bt!=btbs) {
                btbs.Enabled = true;
            }
            __txtValue.Text = __txtValue.Text + bt.Text;
        }
        public Boolean FirstClick = true;
        private void btbs_Click(object sender, EventArgs e) {
            __txtValue.Text = __txtValue.Text.Remove(__txtValue.Text.Count() - 1);
            btbs.Enabled = (__txtValue.Text.Count() > 0);
        }

        private void btdel_Click(object sender, EventArgs e) {

        }
    }
}
