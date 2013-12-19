using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisionModule {
    internal partial class InspImageCtrl : UserControl {

        private Image _img = null;


        public Image Image {
            get {
                return _img;
            }
            set {
                _img = value;
                pictureBox1.BackgroundImage = _img;
            }
        }

        private bool _isselected = false;
        public bool IsSelected {
            get {
                return _isselected;
            }
            set {
                if (value != _isselected) {
                    _isselected=value;
                    if (value) {
                        groupcolor = SystemColors.InactiveCaption;
                        __groupcontainer.Refresh();
                    }
                    else {
                        groupcolor = SystemColors.Control;
                        __groupcontainer.Refresh();
                    }
                }
            }
        }

        private bool _isrejected = false;
        public bool IsRejected {
            get {
                return _isrejected;
            }
            set {
                if (value != _isrejected) {
                    _isrejected = value;
                    if (value) {
                        groupcolor = Color.FromArgb(255,128,128);
                        __groupcontainer.Refresh();
                    } else {
                        groupcolor = SystemColors.Control;
                        __groupcontainer.Refresh();
                    }
                }
            }
        }


        private String _caption = "";
        public String Caption {
            get {
                return _caption;
            }
            set {
                _caption = value != null ? value : "";
                __groupcontainer.Text = _caption;
            }
        }

        public event EventHandler OnCtrlClick;
        public event EventHandler OnCtrlDblClick;

        public InspImageCtrl() {
            InitializeComponent();
            
            __groupcontainer.Paint += new PaintEventHandler(__groupcontainer_Paint);
            __groupcontainer.MouseEnter += new EventHandler(__groupcontainer_MouseEnter);
            __groupcontainer.MouseLeave += new EventHandler(__groupcontainer_MouseLeave);
            pictureBox1.MouseEnter += new EventHandler(pictureBox1_MouseEnter);
            pictureBox1.MouseLeave += new EventHandler(pictureBox1_MouseLeave);

            __groupcontainer.DoubleClick += new EventHandler(__groupcontainer_DoubleClick);
            pictureBox1.DoubleClick += new EventHandler(__groupcontainer_DoubleClick);

            __groupcontainer.Click += new EventHandler(__groupcontainer_Click);
            pictureBox1.Click += new EventHandler(__groupcontainer_Click);
        }

        void __groupcontainer_DoubleClick(object sender, EventArgs e) {
            if (OnCtrlDblClick != null) {
                OnCtrlDblClick(this, e);
            }
        }

        void __groupcontainer_Click(object sender, EventArgs e) {
            if (OnCtrlClick != null) {                
                OnCtrlClick(this, e);
            }
        }

        void pictureBox1_MouseLeave(object sender, EventArgs e) {
            if (!IsSelected) {
                groupcolor = SystemColors.Control;
                __groupcontainer.Refresh();
            }
        }

        void pictureBox1_MouseEnter(object sender, EventArgs e) {
            if (!IsSelected) {
                groupcolor = Color.LemonChiffon;
                __groupcontainer.Refresh();  
            }
        }

        void __groupcontainer_MouseLeave(object sender, EventArgs e) {
            if (!IsSelected) {
                groupcolor = SystemColors.Control;
                __groupcontainer.Refresh(); 
            }
        }

        void __groupcontainer_MouseEnter(object sender, EventArgs e) {
            if (!IsSelected) {
                groupcolor = Color.LemonChiffon;
                __groupcontainer.Refresh(); 
            }
        }

        Color groupcolor = SystemColors.Control;
        void __groupcontainer_Paint(object sender, PaintEventArgs e) {
            GroupBox box = (GroupBox)sender;
            //e.Graphics.Clear(SystemColors.Control);
            e.Graphics.Clear(groupcolor);
            e.Graphics.DrawString(box.Text, box.Font, Brushes.Black, 0, 0);
        }
 
        private void pictureBox1_Click(object sender, EventArgs e) {

        }

        private void __groupcontainer_Enter(object sender, EventArgs e) {

        }
    }
}
