using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using KPP.Core.Debug;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing.Design;
using KPP.Controls.Winforms.ImageEditorObjs;
using System.Diagnostics;
using DejaVu;



namespace VisionModule {




    public enum AreaTypes { Polygon }

    public class AreaInfo {


        UndoRedo<AreaTypes> _AreaType = new UndoRedo<AreaTypes>(AreaTypes.Polygon);
        [Category("Pre-Processing"), Description("Area type"), DisplayName("Area Type")]
        public AreaTypes AreaType {
            get { return _AreaType.Value; }
            set {
                if (!UndoRedoManager.IsCommandStarted) {
                    switch (value) {
                        case AreaTypes.Polygon:
                            //StaticObjects.ChangeAttributeValue<BrowsableAttribute>(this, "CircleCenter", "browsable", true);
                            //StaticObjects.ChangeAttributeValue<BrowsableAttribute>(this, "MainCircleRad", "browsable", true);
                            //StaticObjects.ChangeAttributeValue<BrowsableAttribute>(this, "CirclePt1", "browsable", false);
                            //StaticObjects.ChangeAttributeValue<BrowsableAttribute>(this, "CirclePt2", "browsable", false);
                            //StaticObjects.ChangeAttributeValue<BrowsableAttribute>(this, "CirclePt3", "browsable", false);
                            break;
                        default:
                            break;
                    }

                    using (UndoRedoManager.Start("Area type changed to: " + value.ToString())) {
                        _AreaType.Value = value;
                        UndoRedoManager.Commit();






                    }
                }
                else {
                    _AreaType.Value = value;
                }
            }
        }


        private Point _PolyPoint1 = new Point();
        [Category("Pre-Processing"), Description("First polygon point"), DisplayName("Poligon Point 1"), Browsable(true)]
        [AllowDrag(true)]
        public Point PolyPoint1 {
            get { return _PolyPoint1; }
            set { _PolyPoint1 = value; }
        }


        private Point _PolyPoint2 = new Point();
        [Category("Pre-Processing"), Description("Second polygon point"), DisplayName("Poligon Point 2"), Browsable(true)]
        [AllowDrag(true)]
        public Point PolyPoint2 {
            get { return _PolyPoint2; }
            set { _PolyPoint2 = value; }
        }


        private Point _PolyPoint3 = new Point();
        [Category("Pre-Processing"), Description("Third polygon point"), DisplayName("Poligon Point 3"), Browsable(true)]
        [AllowDrag(true)]
        public Point PolyPoint3 {
            get { return _PolyPoint3; }
            set { _PolyPoint3 = value; }
        }


        private Point _PolyPoint4 = new Point();
        [Category("Pre-Processing"), Description("Fourth polygon point"), DisplayName("Poligon Point 4"), Browsable(true)]
        [AllowDrag(true)]
        public Point PolyPoint4 {
            get { return _PolyPoint4; }
            set { _PolyPoint4 = value; }
        }



        ////private List<Point> _PolygonPoints = new List<Point>();

        //public List<Point> PolygonPoints {
        //    get { return _PolygonPoints; }
        //    set { _PolygonPoints = value; }
        //}


        public override string ToString() {
            return "Area info";
        }

        public AreaInfo() {
        }
    }



    [ProcessingFunction("Area Analysis","Area")]
    public class ProcessingFunctionAreaAnalysis : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionAreaAnalysis));

        public ProcessingFunctionAreaAnalysis() {

            
        }
        
        
        //= new KPPLogger(typeof(ProcessingFunctionAreaAnalysis));


        public delegate void ProcessingFunctionDone(Image<Bgr, byte> ImageOut);
        public event ProcessingFunctionDone OnProcessingFunctionDone;

        

        #region Public Properties

        AreaInfo _areainfo = new AreaInfo();


        [Category("Pre-Processing"), Description("Area info"), DisplayName("Area Info")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AreaInfo areaInfo {
            get { return _areainfo; }
            set { _areainfo = value; }
        }


       

        readonly UndoRedo<int> _PixelCountvalAbove = new UndoRedo<int>(50);
        [XmlAttribute]
        [Category("Post-Processing"), Description("Count pixel above value"), DisplayName("Count Pixels Above")]
        public int PixelCountvalAbove {
            get { return _PixelCountvalAbove.Value; }
            set {
                if (_PixelCountvalAbove.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min Area value changed to: " + value.ToString())) {
                            _PixelCountvalAbove.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _PixelCountvalAbove.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _PixelCountvalBelow = new UndoRedo<int>(255);
        [XmlAttribute]
        [Category("Post-Processing"), Description("Count pixel above value"), DisplayName("Count Pixels Below")]
        public int PixelCountvalBelow {
            get { return _PixelCountvalBelow.Value; }
            set {
                if (_PixelCountvalBelow.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min Area value changed to: " + value.ToString())) {
                            _PixelCountvalBelow.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _PixelCountvalBelow.Value = value;
                    }
                }
            }
        }


       



        #endregion


        [UseInResultInput(true), Category("Post-Processing"), Description("Number of pixels above value"), DisplayName("Pixels Above Value"), ReadOnly(true)]
        public Double PixelCount { get; set; }




        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(false)]
        public override ContourPreProc ContourPreProc1 {
            get;
            set;
        }

        ImagePreProc _ImagePreProc = new ImagePreProc();
        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Image"), Category("Pre-Processing"), Browsable(true)]
        public override ImagePreProc ImagePreProc1 {
            get { return _ImagePreProc; }
            set { _ImagePreProc = value; }
        }

        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Deviation"), Category("Post-Processing"), Description("Pixel Standard Deviation"), ReadOnly(true)]
        public double PixelDeviation { get; set; }



        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {


            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                



                #region Pre-Processing
                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);








                switch (base.ImagePreProc1.UseChannel) {
                    case Channel.Bgr:
                        break;
                    case Channel.Red:
                        grayimage = roiImage[0];
                        break;
                    case Channel.Green:
                        grayimage = roiImage[1];
                        break;
                    case Channel.Blue:
                        grayimage = roiImage[2];
                        break;
                    case Channel.Mono:
                        CvInvoke.cvCvtColor(roiImage, grayimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                        break;
                    default:
                        CvInvoke.cvCvtColor(roiImage, grayimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                        break;
                }







                grayimage.ROI = Rectangle.Empty;


                grayimage._Erode(ImagePreProc1.Erode);
                grayimage._Dilate(ImagePreProc1.Dilate);


                // grayimage = grayimage.SmoothBilatral(7, 255, 34);
                #endregion


                Point[] polygonPoints = new Point[] { areaInfo.PolyPoint1, areaInfo.PolyPoint2, areaInfo.PolyPoint3, areaInfo.PolyPoint4};

                
                Image<Gray, Byte> mask_polygon = new Image<Gray, byte>(grayimage.Size);

                mask_polygon.FillConvexPoly(polygonPoints, new Gray(255));

                //mask_polygon.DrawPolyline(polygonPoints, true, new Gray(255), 1);

                ImageOut.DrawPolyline(polygonPoints, true, new Bgr(Color.Yellow), 1);

                Image<Gray, Byte> maskedImage = new Image<Gray, Byte>(grayimage.Size);
                CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, mask_polygon.Ptr);

                if (IdentRegion.Size== Size.Empty || maskedImage.Size!=IdentRegion.Size) {
                    IdentRegion = new Image<Gray, byte>(maskedImage.Size);
                    
                }

                CvInvoke.cvCopy(maskedImage.Ptr, IdentRegion.Ptr, IntPtr.Zero);

                Gray average = new Gray();
                MCvScalar StdDev = new MCvScalar();

                maskedImage.AvgSdv(out average, out StdDev);

                PixelDeviation = Math.Round(StdDev.v0, 3);

                maskedImage._ThresholdToZero(new Gray(PixelCountvalAbove));               

                CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, maskedImage.Ptr);

                maskedImage._ThresholdToZeroInv(new Gray(PixelCountvalBelow));
                PixelCount = maskedImage.CountNonzero()[0];
                if (ResultsInROI == OutputResultType.orResults) {
                    ImageOut.SetValue(new Bgr(Color.Green), maskedImage);
                }

                maskedImage.Dispose();                
                roiImage.Dispose();
                grayimage.Dispose();


            } catch (Exception exp) {


                log.Error(exp);

                return false;
            }



            ImageIn.ROI = Rectangle.Empty;
            return true;
        }

    }
}