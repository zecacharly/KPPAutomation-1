using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisionModule.Forms {

    internal partial class ZoneSelectorForm : Form {
        public VisionProject SelectedProject;
        public ZoneSelectorForm(VisionProject selectedProject) {
            SelectedProject = selectedProject;
            InitializeComponent();
        }

        private ZoneInfo m_SelectedZone = null;

        public ZoneInfo SelectedZone {
            get { return m_SelectedZone; }
            set {
                if (m_SelectedZone!=value) {

                    m_SelectedZone = value;
                    if (value!=null) {
                        __selectedZone.Text = value.ZoneName;
                    } else {
                        __selectedZone.Text = "";
                    }

                    __image.Refresh();
                }
            }
        }

        int colsize = 0;
        int linesize = 0;

        private void updateZone(){
            colsize = (int)(__image.Image.Size.Width / __numericCol.Value);
            linesize = (int)(__image.Image.Size.Height / __numericLines.Value);
            __comboZones.Items.Clear();
            int totatzones=1;
            for (int i = 1; i < __numericCol.Value+1; i++) {
                int zone_x=0,zone_y=0,zone_w=0, zone_h=0;

                zone_x = colsize * (i - 1);
                zone_w = colsize;
                for (int j = 1; j < __numericLines.Value+1; j++) {
                    zone_y=linesize * (j- 1);
                    zone_h = linesize;

                    ZoneInfo newzone = new ZoneInfo("Zone " + totatzones.ToString(), new Rectangle(zone_x, zone_y, zone_w, zone_h));
                    __comboZones.Items.Add(newzone);
                    totatzones++;
                }

                
            }
            if (__comboZones.Items.Count>0) {
                __comboZones.SelectedIndex = 0;
            }
            __image.Refresh();
        }
        private void __numericCol_ValueChanged(object sender, EventArgs e) {
            updateZone();
            
            SelectedProject.SelectedRequest.ImageCol = (int)__numericCol.Value;
        }

        private void __numericLines_ValueChanged(object sender, EventArgs e) {
            updateZone();
            SelectedProject.SelectedRequest.ImageLine = (int)__numericLines.Value;
        }

        private void __image_SizeChanged(object sender, EventArgs e) {
            
        }

        private void ZoneSelectorForm_Load(object sender, EventArgs e) {
            updateZone();
        }
        double xfactor=1;
        double yfactor=1;
        private void __image_Paint(object sender, PaintEventArgs e) {
            xfactor = (double)__image.Image.Size.Width / (double)e.ClipRectangle.Width;
            yfactor = (double)__image.Image.Size.Height / (double)e.ClipRectangle.Height;
            foreach (ZoneInfo item in __comboZones.Items) {
                Rectangle ee = new Rectangle((int)(item.Zone.X / xfactor),(int)( item.Zone.Y / yfactor), (int)(item.Zone.Width / xfactor), (int)(item.Zone.Height / yfactor));
                //Rectangle ee = new Rectangle(0,0,100,100);
                if (item.ZoneName==__selectedZone.Text) {
                    using(Brush brush = new SolidBrush(Color.FromArgb(128, 106, 192, 117))){
                        e.Graphics.FillRectangle(brush, ee);
                    } 
                
                } else {
                    using (Pen pen = new Pen(Color.Red, 2)) {
                        e.Graphics.DrawRectangle(pen, ee);
                    } 
                }

                if (zoneover!=null) {
                    if (zoneover.ZoneName==item.ZoneName) {
                        using (Brush brush = new SolidBrush(Color.FromArgb(200, 250, 255, 180))) {
                            e.Graphics.FillRectangle(brush, ee);
                        } 
                    }
                }
            }
            
        }

        private void __btUse_Click(object sender, EventArgs e) {
            SelectedZone = __comboZones.SelectedItem as ZoneInfo;
        }

        private void __btClear_Click(object sender, EventArgs e) {
            SelectedZone = null;
        }

        private ZoneInfo zoneover = null;

        private void __image_MouseMove(object sender, MouseEventArgs e) {

            foreach (ZoneInfo item in __comboZones.Items) {
                Rectangle sized = new Rectangle((int)(item.Zone.X / xfactor), (int)(item.Zone.Y / yfactor), (int)(item.Zone.Width / xfactor), (int)(item.Zone.Height / yfactor));
                if (sized.Contains(e.X, e.Y)) {
                    zoneover = item;
                   
                    break;
                }
            }
            __image.Refresh();
        }

        private void __image_MouseLeave(object sender, EventArgs e) {
            zoneover = null;
            __image.Refresh();
        }

        private void __image_MouseClick(object sender, MouseEventArgs e) {
            __comboZones.SelectedItem = zoneover;
            SelectedZone = zoneover;
        }

        private void __btOk_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void __BtCancel_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
