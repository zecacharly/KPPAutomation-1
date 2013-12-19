using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using KPP.Core.Debug;
using KPP.Controls.Winforms.ImageEditorObjs;
using System.Windows.Controls;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace VisionModule {




    [ProcessingFunction("Pre-Positioning","Pre-Positioning")]
    [Serializable()]
    public class ProcessingFunctionPrePos : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionPrePos));

        public ProcessingFunctionPrePos() {
            
        }

        //public delegate void GetPreposInfo(ProcessingFunctionPrePos PrePosFunction);
        public delegate void NewPrepos(ProcessingFunctionPrePos PrePosFunction);
        public delegate void PreposNameChanging(String Oldvalue,String NewValue);

        //public event GetPreposInfo OnGetPreposInfo;
        public event NewPrepos OnNewPrepos;
        public event PreposNameChanging OnPreposNameChanging;


        



        #region Override

       


        //[XmlIgnore]
        //[Editor(typeof(BitmapOutPropertyViewer), typeof(System.Drawing.Design.UITypeEditor))]
        //[Category("Post-Processing"), Description("Result image"), Browsable(false)]
        //[field: NonSerialized]
        //public override Bitmap ROIBitmapOut { get; set; }


        [XmlIgnore]
        [Category("Post-Processing"), Description("Type of displayed results in ROI"),Browsable(false)]
        [field: NonSerialized]
        public override OutputResultType ResultsInROI { get; set;}

        
        [XmlIgnore]
        [CategoryAttribute("Common settings"), DescriptionAttribute("Show results in the ROI"), Browsable(false)]
        public override bool ShowResultsInROI { get; set;}

        #endregion

        [XmlIgnore]
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Reference Point"), DisplayName("Get Reference")]
        public Boolean GetRef { get;set;}
            

        private Point _RefPointX = new Point();
        
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Reference Point X"),DisplayName("Reference Point X")]        
        public Point RefPointX {
            get { 
                return _RefPointX; 
            }
            set {
                if (_RefPointX!=value) {
                    _RefPointX = value;  
                }
            }
        }

        private Point _RefPointY = new Point();

        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Reference Point Y"), DisplayName("Reference Point X")]
        public Point RefPointY {
            get {
                return _RefPointY;
            }
            set {
                if (_RefPointY != value) {
                    _RefPointY = value;
                }
            }
        }


        

        private Point _NewPoint = new Point();
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("New Point"), DisplayName("New Point")]
        [XmlIgnore]
        public Point NewPoint {
            get {
                return _NewPoint;
            }
            set {
                if (_NewPoint != value) {
                    _NewPoint = value;
                }
            }
        }

        



        private float _XOffset = 0;
        [CategoryAttribute("Post-Processing"), DescriptionAttribute("X Offset"), DisplayName("X Offset"),ReadOnly(true)]
        [XmlIgnore]
        public float XOffset {
            get {
                return _XOffset;
            }
            set {
                if (_XOffset != value) {
                    _XOffset = value;
                }
            }
        }

        private float _YOffset = 0;
        [CategoryAttribute("Post-Processing"), DescriptionAttribute("Y Offset"), DisplayName("Y Offset"), ReadOnly(true)]
        [XmlIgnore]
        public float YOffset {
            get {
                return _YOffset;
            }
            set {
                if (_YOffset != value) {
                    _YOffset = value;
                }
            }
        }

        private ResultReference _RefOrigX;
        [AcceptDrop(true), CategoryAttribute("Pre-Processing"), DescriptionAttribute("X Point from area that defines the reference "), DisplayName("Reference Origin X"), AcceptType(typeof(Point))]
        public ResultReference RefOrigX {
            get {
                return _RefOrigX;
            }
            set {
                if (_RefOrigX != value) {
                    _RefOrigX = value;
                }
            }
        }


        private ResultReference _RefOrigY;
        [AcceptDrop(true), CategoryAttribute("Pre-Processing"), DescriptionAttribute("Y Point from area that defines the reference "), DisplayName("Reference Origin Y"), AcceptType(typeof(Point))]
        public ResultReference RefOrigY {
            get {
                return _RefOrigY;
            }
            set {
                if (_RefOrigY != value) {
                    _RefOrigY = value;
                }
            }
        }




        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(false)]
        [XmlIgnore]
        public override ContourPreProc ContourPreProc1 {
            get;
            set;
        }




        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Image"), Category("Pre-Processing"), Browsable(false)]
        [XmlIgnore]
        public override ImagePreProc ImagePreProc1 {
            get;
            set;
        }




        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {

            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                //if (OnGetPreposInfo!=null) {                
                //        OnGetPreposInfo(this);                                    
                //}
                if (RefOrigX==null || RefOrigY==null) {
                    return false;
                }

                RefOrigX.UpdateValue();
                RefOrigY.UpdateValue();

                if (!(RefOrigX.ResultOutput is Point) || !(RefOrigY.ResultOutput is Point)) {
                    return false;
                }

                Point thepointx = (Point)RefOrigX.ResultOutput;
                Point thepointy = (Point)RefOrigY.ResultOutput;

                if (GetRef) {


                    RefPointX = new Point(thepointx.X, thepointx.Y);
                    RefPointY = new Point(thepointy.X, thepointy.Y);
                }
                else {
                    NewPoint = new Point(thepointx.X, thepointy.Y);

                }

                XOffset = NewPoint.X - RefPointX.X;
                YOffset = NewPoint.Y - RefPointY.Y;
                //ImageOut.ROI = _procShape.ShapeEfectiveBounds;
                ImageOut.ROI = Rectangle.Empty;
                ImageOut.Draw(new Cross2DF(new PointF((float)RefPointX.X, (float)RefPointX.Y), 15F, 15f), new Bgr(Color.Red), 1);
                ImageOut.Draw(new Cross2DF(new PointF((float)RefPointY.X, (float)RefPointY.Y), 15F, 15f), new Bgr(Color.Red), 1);
                ImageOut.ROI = Rectangle.Empty;

                GetRef = false;
                if (OnNewPrepos!=null) {
                    OnNewPrepos(this);
                }
                return true;
            } catch (Exception exp) {

                log.Error(exp);
                return false;
            }
        }

    }
}
