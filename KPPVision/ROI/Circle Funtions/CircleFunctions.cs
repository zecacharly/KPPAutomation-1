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
using KPPAutomationCore;
using Accord.Statistics.Models.Regression.Linear;



namespace VisionModule {


    public enum CircleTypes { From3Points,FromCenterRadius }



    public class CircleInfo {


        CircleTypes _CircleType = CircleTypes.From3Points;
        [Category("Pre-Processing"), Description("Circle type"), DisplayName("Circle Type")]
        public CircleTypes CircleType {
            get { return _CircleType; }
            set {

                switch (value) {
                    case CircleTypes.FromCenterRadius:
                        this.ChangeAttributeValue<BrowsableAttribute>("CircleCenter", "browsable", true);
                        this.ChangeAttributeValue<BrowsableAttribute>("MainCircleRad", "browsable", true);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt1", "browsable", false);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt2", "browsable", false);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt3", "browsable", false);
                        break;
                    case CircleTypes.From3Points:
                        this.ChangeAttributeValue<BrowsableAttribute>("CircleCenter", "browsable", false);
                        this.ChangeAttributeValue<BrowsableAttribute>("MainCircleRad", "browsable", false);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt1", "browsable", true);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt2", "browsable", true);
                        this.ChangeAttributeValue<BrowsableAttribute>("CirclePt3", "browsable", true);
                        break;
                    default:
                        break;
                }

                _CircleType = value;
               



            }
        }              


        UndoRedo<ResultReference> _CircleCenter = new UndoRedo<ResultReference>();
        [Category("Pre-Processing"), Description("Circle center value"), DisplayName("Circle Center"),Browsable(true)]
        [AllowDrag(true), AcceptDrop(true), AcceptType(typeof(Point))]
        public ResultReference CircleCenter {
            get { return _CircleCenter.Value; }
            set {
                if (!UndoRedoManager.IsCommandStarted) {
                    using (UndoRedoManager.Start("Circle center changed to: " + value.ToString())) {
                        _CircleCenter.Value = value;
                        UndoRedoManager.Commit();
                    }
                }
                else {
                    _CircleCenter.Value = value;
                }
            }
        }

        UndoRedo<ResultReference> _CirclePt1 = new UndoRedo<ResultReference>();
        [Category("Pre-Processing"), Description("First circle point"), DisplayName("Circle Point 1"), Browsable(false)]
        [AllowDrag(true), AcceptDrop(true)]
        public ResultReference CirclePt1 {
            get { return _CirclePt1.Value; }
            set {
                if (!UndoRedoManager.IsCommandStarted) {
                    using (UndoRedoManager.Start("First circle point changed to: " + value.ToString())) {
                        _CirclePt1.Value = value;
                        UndoRedoManager.Commit();
                    }
                }
                else {
                    _CirclePt1.Value = value;
                }
            }
        }

        UndoRedo<ResultReference> _CirclePt2 = new UndoRedo<ResultReference>();
        [Category("Pre-Processing"), Description("Second circle point"), DisplayName("Circle Point 2"), Browsable(false)]
        [AllowDrag(true), AcceptDrop(true)]
        public ResultReference CirclePt2 {
            get { return _CirclePt2.Value; }
            set {
                if (!UndoRedoManager.IsCommandStarted) {
                    using (UndoRedoManager.Start("Second circle point changed to: " + value.ToString())) {
                        _CirclePt2.Value = value;
                        UndoRedoManager.Commit();
                    }
                }
                else {
                    _CirclePt2.Value = value;
                }
            }
        }


        UndoRedo<ResultReference> _CirclePt3 = new UndoRedo<ResultReference>();
        [Category("Pre-Processing"), Description("Third circle point"), DisplayName("Circle Point 3"), Browsable(false)]
        [AllowDrag(true), AcceptDrop(true)]
        public ResultReference CirclePt3 {
            get { return _CirclePt3.Value; }
            set {
                if (!UndoRedoManager.IsCommandStarted) {
                    using (UndoRedoManager.Start("Third circle point changed to: " + value.ToString())) {
                        _CirclePt3.Value = value;
                        UndoRedoManager.Commit();
                    }
                }
                else {
                    _CirclePt3.Value = value;
                }
            }
        }



        readonly UndoRedo<ResultReference> _CircleAngleStart = new UndoRedo<ResultReference>(); 
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Circle Angle Start"), DisplayName("Circle Angle Start")]
        [AllowDrag(true), AcceptDrop(true), AcceptType(typeof(Double))]
        public ResultReference CircleAngleStart {
            get { return _CircleAngleStart.Value; }
            set {
                if (_CircleAngleStart.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Circle Angle Start value changed to: " + value.ToString())) {

                            _CircleAngleStart.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _CircleAngleStart.Value = value;
                    }
                }
            }
        }
      

        readonly UndoRedo<ResultReference> _CircleAngleEnd = new UndoRedo<ResultReference>();        
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Circle Angle End"), DisplayName("Circle Angle End")]
        [AllowDrag(true), AcceptDrop(true), AcceptType(typeof(Double))]
        public ResultReference CircleAngleEnd {
            get { return _CircleAngleEnd.Value; }
            set {
                if (_CircleAngleEnd.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Circle Angle Start value changed to: " + value.ToString())) {
                            _CircleAngleEnd.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _CircleAngleEnd.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<ResultReference> _MainCircleRad = new UndoRedo<ResultReference>();

        
        [Browsable(true),CategoryAttribute("Pre-Processing"), DescriptionAttribute("Main Circle Radius"), DisplayName("Circle Radius")]
        [AllowDrag(true), AcceptDrop(true), AcceptType(typeof(Double))]
        public ResultReference MainCircleRad {
            get { return _MainCircleRad.Value; }
            set {
                if (_MainCircleRad.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Main Circle Radius value changed to: " + value.ToString())) {
                            _MainCircleRad.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _MainCircleRad.Value = value;
                    }
                }
            }

        }

        public override string ToString() {
            return "Circle info";
        }

        public CircleInfo(VisionProject selected) {
            SelectedVisionProject = selected;
        }

        public CircleInfo() {
        }

        private VisionProject _SelectedVisionProject;
        [XmlIgnore, Browsable(false)]
        public VisionProject SelectedVisionProject {
            get { return _SelectedVisionProject; }
            set { 
                _SelectedVisionProject = value;
                if (CircleCenter != null) {
                    CircleCenter.SelectedVisionProject = value;
                }
                if (CircleAngleStart != null) {
                    CircleAngleStart.SelectedVisionProject = value;
                }
                if (CircleAngleEnd != null) {
                    CircleAngleEnd.SelectedVisionProject = value;
                }
                
                if (MainCircleRad!= null) {
                    MainCircleRad.SelectedVisionProject = value;
                }

                if (CirclePt1!=null) {
                    CirclePt1.SelectedVisionProject = value; 
                }
                if (CirclePt2 != null) {
                    CirclePt2.SelectedVisionProject = value;
                }
                if (CirclePt3 != null) {
                    CirclePt3.SelectedVisionProject = value;
                }
            }
        }
        
        
    }

    [ProcessingFunction("Circle Analysis","Circle")]
    public class ProcessingFunctionCircleAnalysis : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionCircleAnalysis));

        public ProcessingFunctionCircleAnalysis() {

            
        }


        private VisionProject _SelectedVisionProject;
        [XmlIgnore, Browsable(false)]
        public override VisionProject SelectedVisionProject {
            get { return _SelectedVisionProject; }
            set {
                if (_SelectedVisionProject != value) {
                    _SelectedVisionProject = value;
                    if (circleInfo!=null) {
                        circleInfo.SelectedVisionProject = value;
                    }                  

                }
            }
        }



        public delegate void ProcessingFunctionDone(Image<Bgr, byte> ImageOut);
        public event ProcessingFunctionDone OnProcessingFunctionDone;

        

        #region Public Properties
        
        CircleInfo _circleinfo = new CircleInfo();


        [Category("Pre-Processing"), Description("Circle info"), DisplayName("Circle Info")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CircleInfo circleInfo {
            get { return _circleinfo; }
            set {
                if (_circleinfo!=value) {
                    _circleinfo = value; 
                }
            }
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


       




        readonly UndoRedo<int> _InnerCircleDist = new UndoRedo<int>(0);

        [XmlAttribute]
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Inner to Main Circle Distance"), DisplayName("Inner Circle Distance")]
        public int InnerCircleDist {
            get { return _InnerCircleDist.Value; }
            set {
                if (_InnerCircleDist.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Circle Angle Start value changed to: " + value.ToString())) {
                            _InnerCircleDist.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _InnerCircleDist.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _OuterCircleDist = new UndoRedo<int>(0);
        [XmlAttribute]
        [CategoryAttribute("Pre-Processing"), DescriptionAttribute("Outer to Main Circle Distance"), DisplayName("Outter Circle Distance")]
        public int OuterCircleDist {
            get { return _OuterCircleDist.Value; }
            set {
                if (_OuterCircleDist.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Circle Angle Start value changed to: " + value.ToString())) {
                            _OuterCircleDist.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _OuterCircleDist.Value = value;
                    }
                }
            }
        }


        #endregion

        double _PixelCount = 0;

        [XmlIgnore]
        [UseInResultInput(true), Category("Post-Processing"), Description("Number of pixels above value"), DisplayName("Pixels Above Value"), ReadOnly(true)]

        public double PixelCount {
            get {
                //Console.WriteLine("Val GET:" + _PixelCount.ToString());
                return _PixelCount; 
            }
            set { 
                _PixelCount = value;
                //Console.WriteLine("Val SET:" + _PixelCount.ToString());
            }
        }

       


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

        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Average"), Category("Post-Processing"), Description("Pixel Average"), ReadOnly(true)]
        public double PixelAverage { get; set; }



        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {

            base.Process(ImageIn, ImageOut, RoiRegion);

            try {

                

                if (circleInfo.CircleCenter!=null) {
                    circleInfo.CircleCenter.UpdateValue();
                }

                if (circleInfo.CirclePt1!=null) {
                    circleInfo.CirclePt1.UpdateValue();
                }

                if (circleInfo.CirclePt2 != null) {
                    circleInfo.CirclePt2.UpdateValue();
                }

                if (circleInfo.CirclePt3 != null) {
                    circleInfo.CirclePt3.UpdateValue();
                }

                if (circleInfo.CircleAngleStart!=null) {
                    circleInfo.CircleAngleStart.UpdateValue();
                }

                if (circleInfo.CircleAngleEnd!= null) {
                    circleInfo.CircleAngleEnd.UpdateValue();
                }

                if (circleInfo.MainCircleRad!= null) {
                    circleInfo.MainCircleRad.UpdateValue();
                }

               

                #region Pre-Processing
                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);








                switch (ImagePreProc1.UseChannel) {
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

                
                Image<Gray, Byte> mask_arcFilled = new Image<Gray, byte>(grayimage.Size);


                Double AngleStart = (Double)circleInfo.CircleAngleStart.ResultOutput;
                Double AngleEnd = (Double)circleInfo.CircleAngleEnd.ResultOutput;
                if (AngleStart < AngleEnd) {
                    AngleStart = 360 - AngleStart;
                    AngleEnd = 360 - AngleEnd;
                }
                else {
                    AngleStart = 360 - AngleStart;

                    if (AngleEnd < 360) {
                        AngleEnd = 0 - AngleEnd;
                    }
                    else {

                    }
                    //AngleEnd = 360 + AngleEnd;
                }

                Double radius=0;
                Point center=new Point(0,0);

                if (circleInfo.CircleType == CircleTypes.FromCenterRadius) {

                   
                        center = (Point)circleInfo.CircleCenter.ResultOutput;
                 
                    

                    //Translate pos

                    //center = new Point(Math.Abs(center.X - RoiRegion.X), Math.Abs(center.Y - RoiRegion.Y));
                    if (center.X==0 && center.Y==0) {
                        center = new Point((int)(RoiRegion.Width / 2), (int)(RoiRegion.Height/ 2));
                    }
                    //center = new Point(center.X - RoiRegion.X, center.Y - RoiRegion.Y);
                    radius = (Double)circleInfo.MainCircleRad.ResultOutput;
                   
                }
                else {
                    if (circleInfo.CirclePt1 == circleInfo.CirclePt2 || circleInfo.CirclePt2 == circleInfo.CirclePt3) {
                        return false;
                    }
                    float rad;
                    Point cent= new Point();
                    KPPMath.FindCircle((Point)circleInfo.CirclePt1.ResultOutput, (Point)circleInfo.CirclePt2.ResultOutput, (Point)circleInfo.CirclePt3.ResultOutput, out cent, out rad);
                    center.X = (int)cent.X;
                    center.Y = (int)cent.Y;
                    radius = (int)rad;
                }

                //if (AngleStart>AngleEnd) {
                //    AngleEnd = 360 + AngleEnd;
                //}


              
                ImageOut.Draw(new Cross2DF(center, 15F, 15F), new Bgr(Color.Blue), AnnotationSize); 
                
                CvInvoke.cvEllipse(mask_arcFilled.Ptr, center, new Size((int)radius + OuterCircleDist, (int)radius + OuterCircleDist), 0, AngleStart, AngleEnd, new MCvScalar(255), -1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                CvInvoke.cvCircle(mask_arcFilled.Ptr, center, (int)radius - InnerCircleDist, new MCvScalar(0), -1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                
                    //mask_arcFilled._Dilate(2);                    

                    if (ResultsInROI == OutputResultType.orResults) {
                        if (OuterCircleDist == 0 && InnerCircleDist == 0) {
                            CvInvoke.cvEllipse(ImageOut.Ptr, center, new Size((int)radius + OuterCircleDist, (int)radius + OuterCircleDist), 0, AngleStart, AngleEnd, new MCvScalar(0, 215, 255), 0, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                        } else {
                            using (MemStorage storage2 = new MemStorage()) {
                                for (Contour<Point> contours = mask_arcFilled.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage2);
                                         contours != null;
                                         contours = contours.HNext) {
                                    Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);
                                    if (currentContour != null) {
                                        ImageOut.Draw(currentContour, new Bgr(Color.Red), 1);

                                    }
                                }

                            }

                        }
                    }

                Image<Gray, Byte> maskedImage = new Image<Gray, Byte>(grayimage.Size);
                CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, mask_arcFilled.Ptr);

                if (IdentRegion.Size== Size.Empty || maskedImage.Size!=IdentRegion.Size) {
                    IdentRegion = new Image<Gray, byte>(maskedImage.Size);
                }

                
                CvInvoke.cvCopy(maskedImage.Ptr, IdentRegion.Ptr,IntPtr.Zero);





                MCvScalar Mean = new MCvScalar();
                MCvScalar StdDev = new MCvScalar();



                CvInvoke.cvAvgSdv(grayimage.Ptr, ref Mean, ref StdDev, mask_arcFilled.Ptr);
                

                PixelDeviation = Math.Round(StdDev.v0, 3);


                PixelAverage = Math.Round(Mean.v0, 3);
                maskedImage._ThresholdToZero(new Gray(PixelCountvalAbove));
                //   PixelCount = maskedImage.CountNonzero()[0];
                //if (ResultsInROI == OutputResultType.orResults) {
                //    ImageOut.SetValue(new Bgr(Color.Green), maskedImage);
                //}

                CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, maskedImage.Ptr);

                maskedImage._ThresholdToZeroInv(new Gray(PixelCountvalBelow));
                PixelCount = maskedImage.CountNonzero()[0];
                if (ResultsInROI == OutputResultType.orResults) {                    
                    ImageOut.SetValue(new Bgr(Color.Green), maskedImage);
                }

                maskedImage.Dispose();
                mask_arcFilled.Dispose();
                roiImage.Dispose();
                grayimage.Dispose();
            } catch (Exception exp) {                
                log.Error(exp);
                return false;
            }



            
            
            return true;
        }

    }

    [ProcessingFunction("Circle Edges", "Circle")]
    public class ProcessingFunctionCircleEdges : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionCircleEdges));

        public ProcessingFunctionCircleEdges() {
            
        }

        

        public override event UpdateResultImage OnUpdateResultImage;

        




        #region Pre-Processing


        CircleInfo _circleinfo = new CircleInfo();
        [Category("Pre-Processing"), Description("Circle info"), DisplayName("Circle Info")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [AllowDrag(true)]
        public CircleInfo circleInfo {
            get { return _circleinfo; }
            set { _circleinfo = value; }
        }

         LineEdgePreProc _LineEdgePreProc = new LineEdgePreProc();
        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Line Edge"), Category("Pre-Processing")]
        public LineEdgePreProc LineEdgePreProc1 {
            get {
                return _LineEdgePreProc;
            }
            set {
                _LineEdgePreProc = value;
            }
        }
       


        #endregion

        private VisionProject _SelectedVisionProject;
        [XmlIgnore, Browsable(false)]
        public override VisionProject SelectedVisionProject {
            get { return _SelectedVisionProject; }
            set {
                if (_SelectedVisionProject != value) {
                    _SelectedVisionProject = value;
                    if (circleInfo != null) {
                        circleInfo.SelectedVisionProject = value;
                    }

                }
            }
        }

        #region Post Processing

         [XmlIgnore]
        [Category("Post-Processing"), Description("Max Gradient level found"), DisplayName("Max Gradient"),ReadOnly(true),UseInResultInput(true)]
        public double MaxGradient { get; set; }

        //[XmlIgnore]
        //[Category("Post-Processing"), Description("Gradient level aceppted"), DisplayName("Gradient Value"), ReadOnly(true), UseInResultInput(true)]
        //public double DetectedGradientAmplitude { get; set; }



        [XmlIgnore]
        [DisplayName("Edge Location"), UseInRef(true), UseInResultInput(true),Category("Post-Processing"), Description("Edge Location"), ReadOnly(true), Browsable(true)]
        public Point EdgeLocation { get; set; }





         private List<Series> _FunctionSeries = new List<Series>();
        [XmlIgnore]
        public override List<Series> FunctionSeries {
            get { return _FunctionSeries; }
            set { _FunctionSeries = value; }
        }


        #endregion


        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);

        


                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);



                switch (ImagePreProc1.UseChannel) {
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


                Image<Gray, byte> RoiGray = new Image<Gray, byte>(roiImage.Size);
                grayimage.CopyTo(RoiGray);



                grayimage.ROI = Rectangle.Empty;



                grayimage._Erode(ImagePreProc1.Erode);
                grayimage._Dilate(ImagePreProc1.Dilate);


                if (ImagePreProc1.UseChannel != Channel.Bgr) {
                    switch (ImagePreProc1.ThresholdType) {
                        case TypeOfThreshold.Normal:
                            grayimage = grayimage.ThresholdBinary(new Gray(ImagePreProc1.Threshold), new Gray(255));

                            break;
                        case TypeOfThreshold.Inverted:
                            grayimage = grayimage.ThresholdBinaryInv(new Gray(ImagePreProc1.Threshold), new Gray(255));

                            break;
                        default:
                            break;
                    }
                }

                //            
                if (ImagePreProc1.UseChannel != Channel.Bgr) {

                    circleInfo.CircleCenter.UpdateValue();
                    circleInfo.MainCircleRad.UpdateValue();
                    circleInfo.CircleAngleStart.UpdateValue();
                    circleInfo.CircleAngleEnd.UpdateValue();
                    Point center = (Point)circleInfo.CircleCenter.ResultOutput;
                    
                    Point TranslatedCenter = new Point((int)Math.Abs(center.X - RoiRegion.X), (int)Math.Abs(center.Y - RoiRegion.Y));

                    Double doublerad=(Double)circleInfo.MainCircleRad.ResultOutput;
                    int Rad = (int)doublerad;
                    Double anglestart = (Double)circleInfo.CircleAngleStart.ResultOutput;
                    Double angleend = (Double)circleInfo.CircleAngleEnd.ResultOutput;

                    CvInvoke.cvEllipse(ImageOut.Ptr, TranslatedCenter, new Size(Rad, Rad), 0, 360 - anglestart, 360 - angleend, new MCvScalar(0, 255, 255), AnnotationSize, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);

                    Image<Gray, Byte> mask_line = new Image<Gray, byte>(grayimage.Size);
                    CvInvoke.cvEllipse(mask_line.Ptr, TranslatedCenter, new Size(Rad, Rad), 0, 360 - anglestart, 360 - angleend, new MCvScalar(255), 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
                    //CvInvoke.cvLine(mask_line, lineproc.P1, lineproc.P2, new MCvScalar(255), 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);



                    base.IdentRegion = new Image<Gray, byte>(grayimage.Size);
                    CvInvoke.cvEllipse(base.IdentRegion.Ptr, TranslatedCenter, new Size(Rad, Rad), 0, 360 - anglestart, 360 - angleend, new MCvScalar(255, 250, 205), AnnotationSize, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);

                    Image<Gray, Byte> maskedImage = new Image<Gray, Byte>(grayimage.Size);
                    CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, mask_line.Ptr);

                    //mask_line.Save("Temp.bmp");


                    mask_line._ThresholdBinary(new Gray(110), new Gray(255));

                    Contour<Point> _contour = mask_line.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST);
                    if (_contour != null) {

                        //ImageOut.Draw(_contour, new Bgr(Color.Green), 1);

                        Rectangle bound = _contour.BoundingRectangle;
                        mask_line.ROI = bound;
                        List<Point> incountour = new List<Point>();

                        for (int i = bound.X; i < bound.X + bound.Width; i++) {
                            for (int j = bound.Y; j < bound.Y + bound.Height; j++) {
                                Point newpoint = new Point(i, j);



                                double val = _contour.InContour(newpoint);
                                if (val == 0) {
                                    incountour.Add(new Point((int)newpoint.X, (int)newpoint.Y));
                                }
                            }
                        }


                        incountour = incountour.OrderBy(ptx => ptx.X).OrderBy(pty => pty.Y).ToList();

                        ImageOut.Draw(new Cross2DF(incountour[0], 3, 3), new Bgr(Color.Yellow), 1);

                        ImageOut.Draw(new Cross2DF(incountour[incountour.Count - 1], 3, 3), new Bgr(Color.Red), 1);
                        //_contour.in
                        // _contour = _contour.ApproxPoly(0.001);

                        Series pixelvalues = new Series("Pixel Gray value");
                        Series pixelgradiendvalues = new Series("Pixel Gradient value");

                        FunctionSeries.Clear();




                        //grayimage.ROI = _contour.BoundingRectangle;
                        Image<Gray, float> sobelorderx = grayimage.Sobel(1, 0, 3);
                        Image<Gray, float> sobelordery = grayimage.Sobel(0, 1, 3);


                        List<Double> GrayValues = new List<Double>();
                        List<Double> GradientAmplitude = new List<Double>();
                        List<Double> Direction = new List<Double>();



                        //int i1 = 0;
                        for (int i = 0; i < incountour.Count - 1; i++) {
                            Point item = incountour[i];



                            Double PixelValue = grayimage[item].Intensity;
                            GrayValues.Add(PixelValue);
                            DataPoint ptPixel = new DataPoint(i, PixelValue);
                            pixelvalues.Points.Add(ptPixel);

                            Double Gx = sobelorderx[item].Intensity;
                            Double Gy = sobelordery[item].Intensity;

                            Double GradMagnitude = Math.Sqrt(Math.Pow(Gx, 2) + Math.Pow(Gy, 2));
                            GradientAmplitude.Add(GradMagnitude);

                            DataPoint ptGrad = new DataPoint(i, GradMagnitude);
                            pixelgradiendvalues.Points.Add(ptGrad);


                            if (i > 0) {
                                //Double angle = (Math.Atan2(Math.Abs(Gy), Math.Abs(Gx)) / 3.14159) * 180;
                                Double angle = -1;
                                if (GrayValues[i - 1] > GrayValues[i]) {
                                    angle = 1;
                                }
                                Direction.Add(angle);
                            } else {
                                Direction.Add(0);
                            }




                        }




                        pixelvalues.ChartType = SeriesChartType.Line;
                        pixelgradiendvalues.ChartType = SeriesChartType.Spline;
                        FunctionSeries.Add(pixelvalues);
                        FunctionSeries.Add(pixelgradiendvalues);

                        int maxvalindex = -1;




                        switch (this.LineEdgePreProc1.edgeDirection) {
                            case EdgeDirection.Dark_Light:



                                maxvalindex = !GradientAmplitude.Any() ? -1 :
                                                      GradientAmplitude
                                                      .Select((value, index) => new { Value = value, Index = index })
                                                      .Aggregate((a, b) =>
                                                          ((a.Value > b.Value) &&
                                                          (GrayValues[a.Index] >= LineEdgePreProc1.MinGrayLevel) &&
                                                          (GrayValues[a.Index] <= LineEdgePreProc1.MaxGrayLevel) &&
                                                          (a.Value >= LineEdgePreProc1.MinGradientLevel) &&
                                                          (a.Value <= LineEdgePreProc1.MaxGradientLevel)) ? a :
                                                          ((Direction[b.Index] < 0) &&
                                                         (GrayValues[b.Index] >= LineEdgePreProc1.MinGrayLevel) &&
                                                          (GrayValues[b.Index] <= LineEdgePreProc1.MaxGrayLevel) &&
                                                          (b.Value >= LineEdgePreProc1.MinGradientLevel) &&
                                                          (b.Value <= LineEdgePreProc1.MaxGradientLevel)) ? b : a)
                                                      .Index;


                                break;
                            case EdgeDirection.Light_Dark:

                                //
                                maxvalindex = !GradientAmplitude.Any() ? -1 :
                                                      GradientAmplitude
                                                      .Select((value, index) => new { Value = value, Index = index })
                                                      .Aggregate((a, b) =>
                                                          ((a.Value > b.Value) &&
                                                          (GrayValues[a.Index] >= LineEdgePreProc1.MinGrayLevel) &&
                                                          (GrayValues[a.Index] <= LineEdgePreProc1.MaxGrayLevel) &&
                                                          (a.Value >= LineEdgePreProc1.MinGradientLevel) &&
                                                          (a.Value <= LineEdgePreProc1.MaxGradientLevel)) ? a :
                                                          ((Direction[b.Index] >= 0) &&
                                                          (GrayValues[b.Index] >= LineEdgePreProc1.MinGrayLevel) &&
                                                          (GrayValues[b.Index] <= LineEdgePreProc1.MaxGrayLevel) &&
                                                          (b.Value >= LineEdgePreProc1.MinGradientLevel) &&
                                                          (b.Value <= LineEdgePreProc1.MaxGradientLevel)) ? b : a)
                                                      .Index;
                                break;
                            default:
                                break;
                        }

                        if (maxvalindex >= 0) {

                            pixelvalues.Points[maxvalindex].Color = Color.Red;

                            double maxgrad = GradientAmplitude[maxvalindex];

                            MaxGradient = maxgrad;
                            EdgeLocation = new Point(incountour[maxvalindex].X, incountour[maxvalindex].Y);
                            ImageOut.Draw(new Cross2DF(EdgeLocation, 10, 10), new Bgr(Color.Green), AnnotationSize);
                            EdgeLocation = new Point((EdgeLocation.X + RoiRegion.X), (EdgeLocation.Y + RoiRegion.Y));

                        }
                    }
                }

                roiImage.Dispose();
                grayimage.Dispose();

            } catch (Exception exp) {
                
                log.Error(exp);
                return false;
            }



            return true;


        }


    }

    [TypeConverter(typeof(Customconverter<EllipseInfo>))]
    public class EllipseInfo {

        private Double _FitQuality = 0;
        [DisplayName("Fit Quality")]
        public Double FitQuality {
            get { return _FitQuality; }
            private set { _FitQuality = value; }
        }

        private Double _Angle;

        public Double Angle {
            get { return _Angle; }
            private set { _Angle = value; }
        }

        private Double _Height;

        public Double Height {
            get { return _Height; }
            private set { _Height = value; }
        }

        private Double _Width;

        public Double Width {
            get { return _Width; }
            private set { _Width = value; }
        }

        private Point _Center = new Point(0, 0);

        public Point Center {
            get { return _Center; }
            private set { _Center = value; }
        }
        

        public override string ToString() {
            return "Ellipse";
        }

        //private Double Wi

        public EllipseInfo(MCvBox2D Ellipse, Double Quality, Rectangle RoiRegion) {
            Angle = Math.Round(Ellipse.angle,3);
            Height = Math.Round(Ellipse.size.Height,3);
            Width = Math.Round(Ellipse.size.Width,3);
            FitQuality = Math.Round(Quality,3);
            Center = new Point((int)Ellipse.center.X + RoiRegion.X, (int)Ellipse.center.Y + RoiRegion.Y);
            
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EllipseConstraint {

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Min Quality"), DisplayName("Min Quality")]
        public Double MinQuality { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Min Ellipse area"), DisplayName("Min Ellipse Area")]
        public int MinEllipseArea { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Max Ellipse area"), DisplayName("Max Ellipse Area")]
        public int MaxEllipseArea { get; set; }



        public EllipseConstraint() {

        }
    }

    [ProcessingFunction("Ellipse Fitter", "Circle")]
    public class ProcessingFunctionEllipseFitter : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionEllipseFitter));
        public ProcessingFunctionEllipseFitter() {

            
        }


        



        public delegate void ProcessingFunctionDone(Image<Bgr, byte> ImageOut);
        public event ProcessingFunctionDone OnProcessingFunctionDone;

        

        #region pre processing
        

        private EllipseConstraint _EllipseSettings = new EllipseConstraint();

        [Category("Pre-Processing"), DisplayName("Blob Settings")]
        public EllipseConstraint EllipseSettings {
            get {
                return _EllipseSettings;
            }

            set {
                if (value != _EllipseSettings) {
                    _EllipseSettings = value;
                }
            }
        }

        #endregion

        [XmlIgnore]
        [UseInResultInput(true), AllowDrag(true), Category("Post-Processing"), Description("Number of ellipses found"), DisplayName("Number Blobs"), ReadOnly(true), Browsable(true)]
        public Double NumEllipses {
            get {
                return EllipsesFound.Count;
            }
        }


        CustomCollection<EllipseInfo> _EllipsesFound = new CustomCollection<EllipseInfo>();
        
        [XmlIgnore,UseInResultInput(true), Category("Post-Processing"), Description("List of valid ellipses"), DisplayName("Ellipses"), ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CustomCollection<EllipseInfo> EllipsesFound {
            get {
                //Console.WriteLine("Val GET:" + _PixelCount.ToString());
                return _EllipsesFound;
            }
            set {
                _EllipsesFound = value;
                //Console.WriteLine("Val SET:" + _PixelCount.ToString());
            }
        }





        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {

            base.Process(ImageIn, ImageOut, RoiRegion);

            try {
                EllipsesFound.Clear();
                

                //Point Center = 


                #region Pre-Processing
                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);








                switch (ImagePreProc1.UseChannel) {
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

                if (ImagePreProc1.UseChannel != Channel.Bgr) {
                    switch (ImagePreProc1.ThresholdType) {
                        case TypeOfThreshold.Normal:
                            grayimage._ThresholdBinary(new Gray(ImagePreProc1.Threshold), new Gray(255));

                            break;
                        case TypeOfThreshold.Inverted:
                            grayimage._ThresholdBinaryInv(new Gray(ImagePreProc1.Threshold), new Gray(255));

                            break;
                        case TypeOfThreshold.Adaptive:

                            CvInvoke.cvCanny(grayimage, grayimage, ImagePreProc1.Threshold, ImagePreProc1.ThresholdLink, ImagePreProc1.ApertureSize);

                            break;
                        default:
                            break;
                    }
                }


                using (MemStorage storage = new MemStorage()) {

                    List<Contour<Point>> largestContours = new List<Contour<Point>>();

                    //CvInvoke.cvFindContours(grayimage.Ptr

                    //grayimage.ROI = new Rectangle(10, 10, grayimage.Width - 30, grayimage.Height - 30);
                    //ImageOut.Draw(grayimage.ROI, new Bgr(Color.Red), 1);

                    for (Contour<Point> contours = grayimage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                               Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                               contours != null;
                               contours = contours.HNext) {
                        Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);





                        if (currentContour.Perimeter > ContourPreProc1.MinContourLength && currentContour.Perimeter < ContourPreProc1.MaxContourLength) {

                            Boolean isOnEdge = false;

                            if (ContourPreProc1.RemoveTouchingROIEdges) {

                                if (currentContour.BoundingRectangle.Left == 1 || currentContour.BoundingRectangle.Top == 1)
                                    isOnEdge = true;

                                if (currentContour.BoundingRectangle.Right >= roiImage.Width - 1 || currentContour.BoundingRectangle.Bottom >= roiImage.Height - 1)
                                    isOnEdge = true;

                            }

                            if (isOnEdge == false) {


                                if ((currentContour.Area > ContourPreProc1.MinArea) && (currentContour.Area < ContourPreProc1.MaxArea)) {


                                    if (ResultsInROI == OutputResultType.orContours) {
                                        ImageOut.Draw(currentContour, new Bgr(Color.Blue), 1);
                                        // ImageOut.DrawPolyline(currentContour.ToArray(),false, new Bgr(Color.Blue), 1);
                                    }


                                    Boolean OpenedContour = false;

                                    int contourpoint = currentContour.Count();


                                    MCvBox2D testEllipse;


                                    MemStorage stor = new MemStorage();

                                    Seq<Point> pts = new Seq<Point>(stor);

                                    foreach (Point item in currentContour) {
                                        if (item.X < 2 || item.Y < 2 || item.X >= RoiRegion.Width - 2 || item.Y >= RoiRegion.Height - 2) {
                                            OpenedContour = true;
                                        } else {
                                            pts.Push(item);
                                        }
                                    }

                                    //Ransac

                                    if (OpenedContour) {
                                        if (pts.Count() < 5) {
                                            continue;
                                        }

                                        testEllipse = CvInvoke.cvFitEllipse2(pts.Ptr);
                                        if (ResultsInROI == OutputResultType.orContours) {
                                            ImageOut.DrawPolyline(pts.ToArray(), false, new Bgr(Color.Red), 1);
                                        }
                                    } else {
                                        if (currentContour.Count()>5) {
                                            testEllipse = CvInvoke.cvFitEllipse2(currentContour.Ptr);
                                        } else {
                                            continue;
                                        }
                                        
                                    }


                                    //if (ResultsInROI == OutputResultType.orResults) {
                                    //    ImageOut.Draw(currentContour, new Bgr(Color.Green), 1);

                                    //}




                                    Image<Gray, Byte> maskimage = new Image<Gray, byte>(grayimage.Size);
                                    Image<Gray, Byte> countourImage = new Image<Gray, byte>(grayimage.Size);

                                    if (!OpenedContour) {
                                        countourImage.Draw(currentContour, new Gray(255), 1);
                                    } else {
                                        countourImage.DrawPolyline(pts.ToArray(), false, new Gray(255), 1);
                                    }

                                    maskimage.Draw(new Ellipse(testEllipse), new Gray(255), 2);

                                    if (ResultsInROI == OutputResultType.orContours) {
                                        ImageOut.Draw(new Ellipse(testEllipse), new Bgr(Color.Yellow), 1);
                                    }


                                    Image<Gray, Byte> resultimage = new Image<Gray, byte>(grayimage.Size);

                                    resultimage = countourImage.Copy(maskimage);


                                    int overlappedpoints = resultimage.CountNonzero()[0];

                                    Double quality = Math.Round((Double)overlappedpoints / (Double)contourpoint, 3) * 100;

                                    if (quality > EllipseSettings.MinQuality && 
                                        (testEllipse.size.Width*testEllipse.size.Height)>EllipseSettings.MinEllipseArea &&
                                        (testEllipse.size.Width * testEllipse.size.Height) < EllipseSettings.MaxEllipseArea) {
                                        if (ResultsInROI == OutputResultType.orResults) {
                                            ImageOut.Draw(new Ellipse(testEllipse), new Bgr(Color.DarkGreen), 1);
                                            ImageOut.Draw(new Cross2DF(testEllipse.center, 3, 3), new Bgr(Color.DarkGreen), 1);
                                        }
                                        EllipsesFound.Add(new EllipseInfo(testEllipse, quality, RoiRegion));

                                    }
                                    largestContours.Add(currentContour);
                                    stor = null;
                                }
                            }
                        }
                    }
                }



              

                
                #endregion

                
                roiImage.Dispose();
                grayimage.Dispose();

            } catch (Exception exp) {                
                log.Error(exp);
                return false;
            }



         
            return true;
        }

    }


    public enum CircleFittingMethod {Ransac,LMS}

    [ProcessingFunction("Circle Fitter", "Circle")]
    public class CircleFitter : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(CircleFitter));

        public CircleFitter() {
            ContourPreProc1 = new ContourPreProc();

        }


        //= new KPPLogger(typeof(ProcessingFunctionBoundingRectangle));


        #region Pre-Processing

        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(true)]
        public override ContourPreProc ContourPreProc1 {
            get;
            set;
        }

        private CircleFittingMethod _FittingMethod = CircleFittingMethod.Ransac;
        [XmlAttribute]
        [DisplayName("Fitting Method"), Category("Pre-Processing"), Browsable(true)]
        public CircleFittingMethod FittingMethod {
            get { return _FittingMethod; }
            set { _FittingMethod = value; }
        }


        private double _DistanceThreshold = 0.5;
        [XmlAttribute]
        [DisplayName("Distance Threshold"), Category("Pre-Processing"), Browsable(true)]
        public double DistanceThreshold {
            get { return _DistanceThreshold; }
            set { _DistanceThreshold = value; }
        }

        private double _AcceptanceLevel = 0.5;
        [XmlAttribute]
        [DisplayName("Acceptance Level"), Category("Pre-Processing"), Browsable(true)]
        public double AcceptanceLevel {
            get { return _AcceptanceLevel; }
            set { _AcceptanceLevel = value; }
        }

        #endregion



        #region Post Processing

        [XmlIgnore]
        [UseInResultInput(true), AllowDrag(true), Category("Post-Processing"), Description("Number of circles fitted"), DisplayName("Number Circles"), ReadOnly(true), Browsable(true)]
        public Double NumCircles { get; set; }


        private CustomCollection<CircleFitted> _CirclesList = new CustomCollection<CircleFitted>();
        [Category("Post-Processing"), TypeConverter(typeof(ExpandableObjectConverter)), XmlIgnore, ReadOnly(true), DisplayName("Circles List")]
        public CustomCollection<CircleFitted> CirclesList {
            get { return _CirclesList; }
            set { _CirclesList = value; }
        }

        


        //private double _FittingLevel = -1;
        //[XmlIgnore]
        //[Category("Post-Processing"), Description("Fitting level "), DisplayName("Fitting")]
        //public double FittingLevel {
        //    get { return _FittingLevel; }
            
        //}




        #endregion

        public override void UpdateRegionToHighligth(object Region) {
            if (Region is BlobInfo) {
                BlobInfo blobinf = Region as BlobInfo;
                base.IdentRegion = new Image<Gray, byte>(base.BaseRoi.Size);
                base.IdentRegion.Draw(blobinf.BoundingBox, new Gray(255), 2);
            }
        }

        public override Boolean Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                

                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);



                try {

                    Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);
                    Image<Gray, Byte> originalgrayimage = new Image<Gray, Byte>(roiImage.Size);



                    switch (ImagePreProc1.UseChannel) {
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

                    //CvInvoke.cvCopyMakeBorder(grayimage.Ptr, grayimage.Ptr, new Point(0, 0), Emgu.CV.CvEnum.BORDER_TYPE.REPLICATE, new MCvScalar(0));

                    grayimage.CopyTo(originalgrayimage);


                    //Grayimage.ROI = Rectangle.Empty;

                    
                    //Grayimage.CopyTo(ThresholdImage);
                    grayimage._Erode(ImagePreProc1.Erode);
                    grayimage._Dilate(ImagePreProc1.Dilate);

                    if (ImagePreProc1.UseChannel != Channel.Bgr) {
                        switch (ImagePreProc1.ThresholdType) {
                            case TypeOfThreshold.Normal:
                                grayimage._ThresholdBinary(new Gray(ImagePreProc1.Threshold), new Gray(255));

                                break;
                            case TypeOfThreshold.Inverted:
                                grayimage._ThresholdBinaryInv(new Gray(ImagePreProc1.Threshold), new Gray(255));

                                break;
                            case TypeOfThreshold.Adaptive:

                                CvInvoke.cvCanny(grayimage, grayimage, ImagePreProc1.Threshold, ImagePreProc1.ThresholdLink, ImagePreProc1.ApertureSize);

                                break;
                            default:
                                break;
                        }

                        try {


                            using (MemStorage storage = new MemStorage()) {

                                using (MemStorage onedgestorage = new MemStorage()) {

                                    List<Contour<Point>> largestContours = new List<Contour<Point>>();

                                    for (Contour<Point> contours = grayimage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                                           Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                                           contours != null;
                                           contours = contours.HNext) {
                                        Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);


                                        if (currentContour.Perimeter > ContourPreProc1.MinContourLength && currentContour.Perimeter < ContourPreProc1.MaxContourLength) {

                                            Boolean isOnEdge = false;

                                            if (ContourPreProc1.RemoveTouchingROIEdges) {

                                                //if (currentContour.BoundingRectangle.Left == 1 || currentContour.BoundingRectangle.Top == 1)
                                                //    isOnEdge = true;

                                                //if (currentContour.BoundingRectangle.Right >= roiImage.Width - 1 || currentContour.BoundingRectangle.Bottom >= roiImage.Height - 1)
                                                //    isOnEdge = true;



                                                Contour<Point> ctr2 = new Contour<Point>(onedgestorage);
                                                Point[] onedge = currentContour.Where(p => (p.X != 1 && p.Y != 1 && p.X != roiImage.Width - 2 && p.Y != roiImage.Height - 2)).ToArray();
                                                if (onedge != null) {
                                                    ctr2.PushMulti(onedge, Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
                                                    largestContours.Add(ctr2);
                                                    isOnEdge = true;
                                                    if (ResultsInROI == OutputResultType.orContours)
                                                        ImageOut.DrawPolyline(ctr2.ToArray(),false, new Bgr(Color.Blue), 1);

                                                }

                                            }



                                            if (isOnEdge == false) {


                                                if ((currentContour.Area > ContourPreProc1.MinArea) && (currentContour.Area < ContourPreProc1.MaxArea)) {


                                                    if (ResultsInROI == OutputResultType.orContours)
                                                        ImageOut.Draw(currentContour, new Bgr(Color.Blue), 1);


                                                    if (ResultsInROI == OutputResultType.orResults) {
                                                        //ImageOut.Draw(currentContour, new Bgr(Color.Green), 1);
                                                        //ImageOut.Draw(currentContour.GetMinAreaRect(), new Bgr(Color.Salmon), 1);

                                                    }


                                                    largestContours.Add(currentContour);
                                                }
                                            }
                                        }
                                    }
                                    //grayimage.

                                    _CirclesList.Clear();
                                    foreach (Contour<Point> ctr in largestContours) {

                                        switch (FittingMethod) {
                                            case CircleFittingMethod.Ransac:


                                                RansacCircle ransaccircle = new RansacCircle(DistanceThreshold, 0.95);
                                                ransaccircle.Compute(ctr.ToArray(), AcceptanceLevel);

                                                if (ransaccircle.CircleFound) {
                                                    _CirclesList.Add(ransaccircle);
                                                    foreach (Point item in ransaccircle.Inliers) {
                                                        ImageOut.Draw(new Cross2DF(new PointF(item.X, item.Y), 1, 1), new Bgr(Color.Yellow), 1);
                                                    }
                                                    if (ResultsInROI == OutputResultType.orResults) {
                                                        ImageOut.Draw(new CircleF(ransaccircle.CircleCenter, (float)ransaccircle.CircleRadius), new Bgr(Color.Green), 1);
                                                    }
                                                }

                                                break;
                                            case CircleFittingMethod.LMS:
                                                break;
                                            default:
                                                break;
                                        }




                                    }
                                    NumCircles = _CirclesList.Count;
                                    if (ResultsInROI == OutputResultType.orResults) {


                                    } else if (ResultsInROI == OutputResultType.orContours) {

                                    } else if (ResultsInROI == OutputResultType.orPreProcessing) {
                                        ImageOut.ROI = RoiRegion;
                                        CvInvoke.cvCvtColor(grayimage, ImageOut, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
                                        ImageOut.ROI = Rectangle.Empty;
                                    }


                                }
                            }
                        } catch (DllNotFoundException exp) {
                            log.Error(exp);
                            return false;
                        }


                        grayimage.Dispose();
                        originalgrayimage.Dispose();

                    }
                }
                catch (Exception exp) {
                    log.Error(exp);
                    return false;
                }




                roiImage.Dispose();


            }
            catch (Exception exp) {
                
                log.Error(exp);
                return false;
            }




            return true;


        }


    }


}