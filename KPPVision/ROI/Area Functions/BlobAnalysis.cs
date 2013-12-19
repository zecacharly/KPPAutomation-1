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
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using KPPAutomationCore;
using AForge.Imaging;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using AForge;
using System.Linq.Expressions;



namespace VisionModule {

    public enum SortType { Area, Position,None }

    public class BlobComparer : IComparer<BlobInfo> {
        public SortType Sortype = SortType.Area;

        public BlobComparer(SortType sortype) {
            Sortype = sortype;
        }
        public int Compare(BlobInfo b1, BlobInfo b2) {
            switch (Sortype) {
                case SortType.Area:
                    int res=(b1.Area.CompareTo(b2.Area));
                    return res;                                                       
                case SortType.Position:
                    //if (b1.Center.X == b2.Center.X && b1.Center.Y == b2.Center.Y)
                    //    return 0;
                    //if (b1.Center.X < b2.Center.X || (b1.Center.X == b2.Center.X && b1.Center.Y < b2.Center.Y))
                    //    return -1;
                    //return 1;

                    int r = b1.Center.X.CompareTo(b2.Center.X);
                    if (r<=0) 
                        return b1.Center.Y.CompareTo(b2.Center.Y);

                    else return r;
                default:
                    return -1;
                    
            }
        }
    }

    [TypeConverter(typeof(Customconverter<BlobInfo>))]
    public class BlobInfo {


        private String _ID = "No ID";
        [DisplayName("Blob ID")]
        [UseInRef(true), AllowDrag(true), UseInResultInput(true)]
        public String ID {
            get {
                return _ID.ToString();
            }
        }

        private System.Drawing.Point _Center = new System.Drawing.Point(0, 0);
        [DisplayName("Blob Center"), UseInRef(true), AllowDrag(true), UseInResultInput(true)]
        public System.Drawing.Point Center {
            get {
                return _Center;
            }
        }

        private Double _Area = 0;
        [UseInRef(true), AllowDrag(true), UseInResultInput(true)]
        public Double Area {
            get { return _Area; }

        }

        private Double _Angle = -1;
        [UseInRef(true), AllowDrag(true), UseInResultInput(true)]
        public Double Angle {
            get { return _Angle; }
        }

        private Rectangle _BoundingBox = Rectangle.Empty;
        [DisplayName("Bounding Box")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Rectangle BoundingBox {
            get {
                return _BoundingBox;
            }

        }


        private Double _PixelAverage;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Average"), Category("Post-Processing"), Description("Pixel Average inside contour"), ReadOnly(true)]
        public Double PixelAverage {
            get { return _PixelAverage; }
            private set{
                _PixelAverage=value;
            }
        }


        private Double _PixelMinVal;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Min Value"), Category("Post-Processing"), Description("Pixel Min Value inside contour"), ReadOnly(true)]
        public Double PixelMinVal {
            get { return _PixelMinVal; }
            private set {
                _PixelMinVal = value;
            }
        }


        private Double _PixelMaxVal;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Max Value"), Category("Post-Processing"), Description("Pixel Max Value inside contour"), ReadOnly(true),]
        public Double PixelMaxVal {
            get { return _PixelMaxVal; }
            private set {
                _PixelMaxVal = value;
            }
        }


        //private Double _PixelCount;
        //[XmlIgnore]
        //[UseInResultInput(true), DisplayName("Pixel Count"), Category("Post-Processing"), Description("Pixel Count above average inside contour"), ReadOnly(true)]
        //public Double PixelCount {
        //    get { return _PixelCount; }
        //    private set {
        //        _PixelCount = value;
        //    }
        //}



        private double _PixelDeviation;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Deviation"), Category("Post-Processing"), Description("Pixel Standard Deviation inside contour"), ReadOnly(true)]
        public double PixelDeviation {
            get { return _PixelDeviation; }
            private set { _PixelDeviation = value; }
        }

        private CvBlob _Blob;
        [XmlIgnore, Browsable(false)]
        public CvBlob Blob {
            get { return _Blob; }

        }

        public BlobInfo(CvBlob blob, Rectangle RoiRegion, Image<Gray, Byte> grayimage, Image<Gray, Byte> originalimage,Image<Bgr,Byte> OutImage) {


            Image<Gray, Byte> _grayimage = new Image<Gray, byte>(grayimage.Size);
            _grayimage = grayimage.Copy();

            _Blob = blob;

            _Area = blob.Area;
            _ID = blob.Label.ToString();
            //Translate

            _Center = new System.Drawing.Point((int)(blob.Centroid.X + RoiRegion.X), (int)(blob.Centroid.Y + RoiRegion.Y));
            _BoundingBox = blob.BoundingBox;


            using (MemStorage storage = new MemStorage()) {

                Contour<System.Drawing.Point> blobcontour = blob.GetContour(storage);
                MCvBox2D box = blobcontour.GetMinAreaRect();
                _Angle = 0 - Math.Round(box.angle, 3);
            }



            //Rectangle oldroi =new Rectangle(grayimage.ROI.Location,grayimage.ROI.Size);

            //Image<Gray, Byte> blobimagegray = new Image<Gray, byte>(_grayimage.Size);
            Image<Gray, Byte> mask1 = new Image<Gray, byte>(_grayimage.Size);
            Image<Gray, Byte> mask2 = new Image<Gray, byte>(blob.BoundingBox.Size);
            using (MemStorage ctrstor = new MemStorage()) {
                Contour<System.Drawing.Point> ctr = blob.GetContour(ctrstor);
                mask1.Draw(ctr, new Gray(255), -1);
            }
            mask1.ROI = blob.BoundingBox;
            mask1.CopyTo(mask2);
            _grayimage.ROI = blob.BoundingBox;
            mask2._And(_grayimage);
            mask2._Erode(1);
            
            _grayimage.ROI = blob.BoundingBox;
            

            OutImage.ROI = new Rectangle(RoiRegion.X + blob.BoundingBox.X, RoiRegion.Y + blob.BoundingBox.Y, blob.BoundingBox.Width, blob.BoundingBox.Height);
            //OutImage.SetValue(new Bgr(Color.Green), mask2);


            MCvScalar Mean = new MCvScalar();
            MCvScalar StdAvg = new MCvScalar();
            double Minval = -1, Maxval = -1;

            System.Drawing.Point MinValpt = new System.Drawing.Point();
            System.Drawing.Point MaxValpt = new System.Drawing.Point();
            originalimage.ROI = blob.BoundingBox;
            CvInvoke.cvMinMaxLoc(originalimage, ref Minval, ref Maxval, ref MinValpt, ref MaxValpt, mask2);
            CvInvoke.cvAvgSdv(originalimage, ref Mean, ref StdAvg, mask2);

            PixelAverage = Math.Round(Mean.v0, 3);
            PixelDeviation = Math.Round(StdAvg.v0, 3);

            PixelMinVal = Minval;
            PixelMaxVal = Maxval;

            //OutImage.ROI = new Rectangle(RoiRegion.X + blob.BoundingBox.X, RoiRegion.Y + blob.BoundingBox.Y, blob.BoundingBox.Width, blob.BoundingBox.Height);
            //OutImage.Draw(new Cross2DF(new PointF((float)MinValpt.X, (float)MinValpt.Y),2,2), new Bgr(Color.Blue), 2);
            //OutImage.Draw(new Cross2DF(new PointF((float)MaxValpt.X, (float)MaxValpt.Y), 2, 2), new Bgr(Color.Yellow), 2);

           

        }

        public override string ToString() {
            return "Blob";
        }
        public BlobInfo() {

        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BlobConstraint {



        private ShapeType _Shape = ShapeType.Unknown;
        [XmlAttribute]
        [Category("Pre-Processing"), Description("Filter shapes"), DisplayName("Shape")]
        public ShapeType Shape {
            get { return _Shape; }
            set { _Shape = value; }
        }


        private float _ShapeDistortion = 50;
        [XmlAttribute]
        [Category("Pre-Processing"), Description("Acceptable shape distortion (0-100%)"), DisplayName("Distortion")]
        public float ShapeDistortion {
            get {
                return _ShapeDistortion;
            }
            set {
                if (_ShapeDistortion!=value) {
                    _ShapeDistortion = Math.Max(0, Math.Min(100, value)); 
                }
            }
        }


        [XmlAttribute]
        [Category("Pre-Processing"), Description("Min blob area"), DisplayName("Min Blob Area")]
        public int MinBlobArea { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Max blob area"), DisplayName("Max Blob Area")]
        public int MaxBlobArea { get; set; }


        [XmlAttribute]
        [Category("Pre-Processing"), Description("Min blob width"), DisplayName("Min blob width")]
        public int MinBlobWidth { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Max blob width"), DisplayName("Max blob width")]
        public int MaxBlobWidth { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Min blob heigth"), DisplayName("Min blob heigth")]
        public int MinBlobHeigth { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Max blob heigth"), DisplayName("Max blob heigth")]
        public int MaxBlobHeigth { get; set; }
        


        private SortType _Sort = SortType.Area;
        [XmlAttribute]
        [Category("Pre-Processing"), DisplayName("Sort")]
        public SortType Sort {
            get { return _Sort; }
            set { _Sort = value; }
        }


        public BlobConstraint() {

        }
    }

    [ProcessingFunction("Blob Analysis", "Area")]
    public class BlobAnalysis : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(BlobAnalysis));

        public BlobAnalysis() {


        }


        //= new KPPLogger(typeof(ProcessingFunctionBoundingRectangle));


        #region Pre-Processing

        [XmlIgnore, Browsable(false)]
        public virtual ContourPreProc ContourPreProc1 { get; set; }

        private BlobConstraint _BlobSettings = new BlobConstraint();

        [Category("Pre-Processing"), DisplayName("Blob Settings")]
        public BlobConstraint BlobSettings {
            get {
                return _BlobSettings;
            }

            set {
                if (value != _BlobSettings) {
                    _BlobSettings = value;
                }
            }
        }



        #endregion



        #region Post Processing





        [XmlIgnore]
        [UseInResultInput(true), AllowDrag(true), Category("Post-Processing"), Description("Number of blobs found"), DisplayName("Number Blobs"), ReadOnly(true), Browsable(true)]
        public Double NumBlobs { get; set; }


        private CustomCollection<BlobInfo> _BlobsList = new CustomCollection<BlobInfo>();
        [Category("Post-Processing"), TypeConverter(typeof(ExpandableObjectConverter)), XmlIgnore, ReadOnly(true), DisplayName("Blobs List")]
        public CustomCollection<BlobInfo> BlobsList {
            get { return _BlobsList; }
            set { _BlobsList = value; }
        }



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


                NumBlobs = 0;
                BlobsList.Clear();

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
                            return false;
                            break;
                    }



                    grayimage.CopyTo(originalgrayimage);


                    //Grayimage.ROI = Rectangle.Empty;


                    //Grayimage.CopyTo(ThresholdImage);
                    grayimage._Erode(ImagePreProc1.Erode);
                    grayimage._Dilate(ImagePreProc1.Dilate);

                    if (base.ImagePreProc1.UseChannel != Channel.Bgr) {
                        switch (base.ImagePreProc1.ThresholdType) {
                            case TypeOfThreshold.Normal:
                                grayimage._ThresholdBinary(new Gray(base.ImagePreProc1.Threshold), new Gray(255));

                                break;
                            case TypeOfThreshold.Inverted:
                                grayimage._ThresholdBinaryInv(new Gray(base.ImagePreProc1.Threshold), new Gray(255));

                                break;
                            case TypeOfThreshold.Adaptive:

                                CvInvoke.cvCanny(grayimage, grayimage, base.ImagePreProc1.Threshold, base.ImagePreProc1.ThresholdLink, base.ImagePreProc1.ApertureSize);

                                break;
                            default:
                                break;
                        }

                        try {



                            CvBlobDetector blobdetector = new CvBlobDetector();
                            CvBlobs blobs = new CvBlobs();


                            blobdetector.Detect(grayimage, blobs);

                            if (BlobSettings.MinBlobArea != 0 || BlobSettings.MaxBlobArea != 0) {
                                blobs.FilterByArea(BlobSettings.MinBlobArea, BlobSettings.MaxBlobArea);
                            }



                            List<CvBlob> blobarray = blobs.Values.ToList();

                            //List<CvBlob> newbloblist = new List<CvBlob>();

                            foreach (CvBlob blob in blobarray) {
                                if (blob.BoundingBox.Left != grayimage.ROI.Left && blob.BoundingBox.Top != grayimage.ROI.Top && blob.BoundingBox.Right != grayimage.ROI.Right && blob.BoundingBox.Bottom != grayimage.ROI.Bottom) {
                                    if (blob.BoundingBox.Width > BlobSettings.MinBlobWidth && blob.BoundingBox.Width < BlobSettings.MaxBlobWidth &&
                                        blob.BoundingBox.Height > BlobSettings.MinBlobHeigth && blob.BoundingBox.Height < BlobSettings.MaxBlobHeigth) {
                                        
                                        //Image<Gray, Byte> BlobImage = new Image<Gray, byte>(blob.BoundingBox.Size);
                                        //originalgrayimage.ROI = blob.BoundingBox;
                                        //originalgrayimage.CopyTo(BlobImage);
                                        Boolean goodblob = false;



                                        using (MemStorage mem = new MemStorage()) {



                                            Contour<System.Drawing.Point> ctr = blob.GetContour(mem);


                                            switch (BlobSettings.Shape) {
                                                case ShapeType.Circle:
                                                    List<IntPoint> edgePoints = new List<IntPoint>();
                                                    foreach (System.Drawing.Point item in ctr) {
                                                        edgePoints.Add(new IntPoint(item.X, item.Y));
                                                    }

                                                    if (edgePoints.Count < 10) {
                                                        continue;
                                                    }
                                                    SimpleShapeChecker checkshape = new SimpleShapeChecker();
                                                    checkshape.RelativeDistortionLimit = BlobSettings.ShapeDistortion / 100;


                                                    ShapeType shape = checkshape.CheckShapeType(edgePoints);

                                                    if (shape == ShapeType.Circle) {
                                                        ImageOut.Draw(blob.BoundingBox, new Bgr(Color.Green), 2);

                                                        Double CircleArea = checkshape.Radius * checkshape.Radius * Math.PI;
                                                        Double ContourArea = ctr.Area;

                                                        Double dist = checkshape.MeanDistance;


                                                        goodblob = true;
                                                    }


                                                    break;
                                                case ShapeType.Quadrilateral:
                                                    break;
                                                case ShapeType.Triangle:
                                                    break;
                                                default:
                                                    goodblob = true;
                                                    break;
                                            }



                                            if (goodblob) {


                                                BlobsList.Add(new BlobInfo(blob, RoiRegion, grayimage, originalgrayimage, ImageOut));


                                            }

                                        }
                                        //Image<Gray,Byte> blobimage= new Image<Gray,byte>(blob.BoundingBox.Size);
                                        //originalgrayimage.ROI=blob.BoundingBox;
                                        //originalgrayimage.CopyTo(blobimage);
                                        
                                    }
                                }




                            }


                            
                            

                            List<BlobInfo> orderderedblobs = BlobsList.ToList();

                           
                           
                            orderderedblobs.Sort(new BlobComparer(BlobSettings.Sort));

                            //var lst =BlobsList.ToList();
                            //int cx = lst.Select(b => ((int)b.Center.X / 10)).Distinct().Count();
                            //int cy = lst.Select(b => ((int)b.Center.Y / 10)).Distinct().Count();
                            //IEnumerable<BlobInfo> orderderedblobs=null;
                            //if (cx < cy) {
                            //    orderderedblobs = lst.OrderBy(blb => ((int)blb.Center.X / 10)).ThenBy(blb => blb.Center.Y);
                            //} else {
                            //    orderderedblobs = lst.OrderBy(blb => ((int)blb.Center.Y / 10)).ThenBy(blb => blb.Center.X);
                            //}

                            //var orderderedblobs = from blb in BlobsList.ToList()
                            //                      orderby ((int)blb.Center.Y / 10), blb.Center.X ascending
                            //                      select blb;

                           
                            
                            BlobsList.Clear();

                            for (int i = 0; i < orderderedblobs.Count(); i++) {
                                BlobsList.Add(orderderedblobs.ToArray()[i]);
                            }

                            //if (BlobsList.Count>0) {
                            //    ReferenceBlob = BlobsList[0];
                            //}
                            ImageOut.ROI = RoiRegion;
                            if (ResultsInROI == OutputResultType.orResults) {

                                foreach (BlobInfo blobinf in BlobsList) {
                                    ImageOut.Draw(blobinf.BoundingBox, new Bgr(Color.Green), 1);
                                    ImageOut.Draw(new Cross2DF(blobinf.Center, 3, 3), new Bgr(Color.Green), 1);
                                }
                            } else if (ResultsInROI == OutputResultType.orContours) {
                                foreach (BlobInfo blobinf in BlobsList) {
                                    using (MemStorage contourstorage = new MemStorage()) {
                                        Contour<System.Drawing.Point> contour = blobinf.Blob.GetContour(contourstorage);
                                        ImageOut.Draw(contour, new Bgr(Color.Yellow), 1);
                                        ImageOut.Draw(new Cross2DF(blobinf.Blob.Centroid, 3, 3), new Bgr(Color.Yellow), 1);
                                    }
                                }
                            } else if (ResultsInROI == OutputResultType.orPreProcessing) {
                                ImageOut.ROI = RoiRegion;
                                CvInvoke.cvCvtColor(grayimage, ImageOut, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
                                ImageOut.ROI = Rectangle.Empty;
                            }



                            NumBlobs = BlobsList.Count;
                        } catch (DllNotFoundException exp) {
                            log.Error(exp);
                            return false;
                        }


                        grayimage.Dispose();
                        originalgrayimage.Dispose();

                    }
                } catch (Exception exp) {
                    log.Error(exp);
                    return false;
                }




                roiImage.Dispose();


            } catch (Exception exp) {

                log.Error(exp);
                return false;
            }




            return true;


        }


    }

}
