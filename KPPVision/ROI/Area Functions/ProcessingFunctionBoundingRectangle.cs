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



namespace VisionModule {
    [ProcessingFunction("Bounding Rectangle","Area")]
    public class ProcessingFunctionBoundingRectangle : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionBoundingRectangle));

        public ProcessingFunctionBoundingRectangle() {
            
        }
        



        #region Pre-Processing


        #endregion



        #region Post Processing

        [XmlAttribute, UseInResultInput(true)]
        [Category("Pre-Processing"), Description("Count pixel above value"), DisplayName("Count Pixels Above")]
        public int PixelCountVal { get; set; }


       

        [XmlIgnore]
        [DisplayName("Rectangle Center"), Category("Post-Processing"), Description("Bounding Rectangle Center Point"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]        
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [UseInRef(true), UseInResultInput(true)]
        public Point RectangleCenter {
            get;
            set;
        }

        [XmlIgnore]
        [DisplayName("Upper Left Corner"), Category("Post-Processing"), Description("Bounding Rectangle"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [UseInRef(true), UseInResultInput(true)]
        public Point RectangleCornerUpL {
            get;
            set;
        }


        [XmlIgnore]
        [DisplayName("Upper Right Corner"), Category("Post-Processing"), Description("Bounding Rectangle"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [UseInRef(true), UseInResultInput(true)]
        public Point RectangleCornerUpR {
            get;
            set;
        }

        [XmlIgnore]
        [DisplayName("Down Left Corner"), Category("Post-Processing"), Description("Bounding Rectangle"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [UseInRef(true), UseInResultInput(true)]
        public Point RectangleCornerDownL {
            get;
            set;
        }

        [XmlIgnore]
        [DisplayName("Down Right Corner"), Category("Post-Processing"), Description("Bounding Rectangle"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [UseInRef(true), UseInResultInput(true)]
        public Point RectangleCornerDownR {
            get;
            set;
        }


        [XmlIgnore]
        [UseInResultInput(true), Category("Post-Processing"), Description("Number of rectangles found"), DisplayName("Number Rectangles"), ReadOnly(true), Browsable(true)]
        public int NumRectangles { get; set; }


        [XmlIgnore]
        [UseInResultInput(true), Category("Post-Processing"), Description("Area of the rectangle"), ReadOnly(true)]
        public Double Area { get; set; }

        [XmlIgnore]
        [UseInResultInput(true), Category("Post-Processing"), Description("Width of the rectangle"), ReadOnly(true)]
        public Double Width { get; set; }

        [XmlIgnore]
        [UseInResultInput(true), Category("Post-Processing"), Description("Heigth of the rectangle"), ReadOnly(true)]
        public Double Heigth { get; set; }

        Double _PixelAverage;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Average"), Category("Post-Processing"), Description("Pixel Average inside contour"), ReadOnly(true)]        
        public Double PixelAverage {
            get { return _PixelAverage; }
            set { _PixelAverage = value; }
        }


        int _PixelCount;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Count"), Category("Post-Processing"), Description("Pixel Count above average inside contour"), ReadOnly(true)]        
        public int PixelCount {
            get { return _PixelCount; }
            set { _PixelCount = value; }
        }

        double _PixelDeviation;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("PixelDeviation"), Category("Post-Processing"), Description("Pixel Standard Deviation inside contour"), ReadOnly(true)]        
        public double PixelDeviation {
            get { return _PixelDeviation; }
            set { _PixelDeviation = value; }
        }


        [XmlIgnore]
        [UseInResultInput(true),Category("Post-Processing"), Description("A rectangle is avaible"), ReadOnly(true), Browsable(false)]
        public Boolean RectangleFound { get; set; }

        
        #endregion

        public override Boolean Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                Area = -1;
                Width = -1;
                Heigth = -1;
                PixelAverage = -1;
                PixelDeviation = -1; 
                

                PixelCount = -1;

                RectangleFound = false;
                
                NumRectangles = 0;

                RectangleCenter = new Point();

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


                   
                    
                    using (MemStorage storage = new MemStorage()) {

                        List<Contour<Point>> largestContours = new List<Contour<Point>>();

                        for (Contour<Point> contours = grayimage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                               Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                               contours != null;
                               contours = contours.HNext) {
                            Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);

                            
                            if (currentContour.Perimeter > base.ContourPreProc1.MinContourLength && currentContour.Perimeter < base.ContourPreProc1.MaxContourLength) {

                                Boolean isOnEdge = false;

                                if (base.ContourPreProc1.RemoveTouchingROIEdges) {

                                    if (currentContour.BoundingRectangle.Left == 1 || currentContour.BoundingRectangle.Top == 1)
                                        isOnEdge = true;

                                    if (currentContour.BoundingRectangle.Right >= roiImage.Width-1  || currentContour.BoundingRectangle.Bottom >= roiImage.Height-1)
                                        isOnEdge = true;

                                }

                                if (isOnEdge == false) {

                                    
                                    if ((currentContour.Area > base.ContourPreProc1.MinArea) && (currentContour.Area < base.ContourPreProc1.MaxArea)) {


                                        if (ResultsInROI == OutputResultType.orContours)
                                            ImageOut.Draw(currentContour, new Bgr(Color.Blue), 1);


                                        if (ResultsInROI == OutputResultType.orResults){
                                            ImageOut.Draw(currentContour, new Bgr(Color.Green), 1);
                                            
                                            ImageOut.Draw(currentContour.GetMinAreaRect(), new Bgr(Color.Salmon), 1);
                                            
                                        }

                                        NumRectangles += 1;
                                        RectangleFound = true;
                                        largestContours.Add(currentContour);
                                    }
                                }
                            }
                        }

                        

                        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 0.5d, 0.5d);

                        int MinX = -1, MinY = -1, MaxX = -1, MaxY = -1;
                        foreach (Contour<Point> largest in largestContours) {

                            if ((largest.BoundingRectangle.X <= MinX) || (MinX == -1))
                                MinX = largest.BoundingRectangle.X;

                            if ((largest.BoundingRectangle.Y <= MinY) || (MinY == -1))
                                MinY = largest.BoundingRectangle.Y;

                            if (((largest.BoundingRectangle.Width + largest.BoundingRectangle.X) >= MaxX))
                                MaxX = (largest.BoundingRectangle.Width + largest.BoundingRectangle.X);

                            if (((largest.BoundingRectangle.Y + largest.BoundingRectangle.Height) >= MaxY))
                                MaxY = largest.BoundingRectangle.Y + largest.BoundingRectangle.Height;

                            PixelAnalisys.Process(PixelCountVal, largest, grayimage, out _PixelAverage, out _PixelDeviation, out _PixelCount);

                           
                            //roiImage
                        }
                        
                      //  inImage.ROI = _procShape.ShapeEfectiveBounds;
                        if (RectangleFound) {

                            
                            float midX = ((float)(MaxX - MinX) / (float)2) + MinX;
                            float midY = (float)((MaxY - MinY) / (float)2) + MinY;


                            RectangleCenter = new Point((int)midX, (int)midY);

                            Rectangle BoundingRectangle=new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY);
                            Point CenterPoint= new Point((int)(midX),(int)(midY));

                            base.IdentRegion = new Image<Gray, byte>(grayimage.Size);                                                        
                            base.IdentRegion.Draw(BoundingRectangle, new Gray(255),1);
                            //CvInvoke.cvLine(base.IdentRegion, lineproc.P1, lineproc.P2, new MCvScalar(255), 0, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                     


                            if (ResultsInROI == OutputResultType.orResults) {
                                ImageOut.Draw(new Cross2DF(CenterPoint, 15F, 15f), new Bgr(Color.Yellow), AnnotationSize);
                                ImageOut.Draw(BoundingRectangle, new Bgr(Color.Yellow), 1);
                            }

                            Width = Math.Abs(MaxX - MinX);
                            Heigth= Math.Abs(MaxY - MinY);
                            Area = Math.Abs(MaxX - MinX) * Math.Abs(MaxY - MinY);

                            //Translate to image position

                            Point newpoint = new Point((CenterPoint.X + RoiRegion.X), (CenterPoint.Y + RoiRegion.Y));
                            RectangleCenter = newpoint;
                            RectangleCornerUpL = new Point((RoiRegion.X + BoundingRectangle.X), (RoiRegion.Y+BoundingRectangle.Y));
                            RectangleCornerUpR = new Point(RectangleCornerUpL.X+BoundingRectangle.Width, RectangleCornerUpL.Y);
                            RectangleCornerDownL = new Point(RectangleCornerUpL.X, RectangleCornerUpL.Y+BoundingRectangle.Height);
                            RectangleCornerDownR = new Point(RectangleCornerUpL.X + BoundingRectangle.Width, RectangleCornerUpL.Y+BoundingRectangle.Height);

                            if (ResultsInROI == OutputResultType.orResults) {
                                ImageOut.ROI = Rectangle.Empty;
                                ImageOut.Draw(new Cross2DF(RectangleCornerUpL, 15F, 15f), new Bgr(Color.Red), AnnotationSize);
                                ImageOut.Draw(new Cross2DF(RectangleCornerUpR, 15F, 15f), new Bgr(Color.Red), AnnotationSize);
                                ImageOut.Draw(new Cross2DF(RectangleCornerDownL, 15F, 15f), new Bgr(Color.Red), AnnotationSize);
                                ImageOut.Draw(new Cross2DF(RectangleCornerDownR, 15F, 15f), new Bgr(Color.Red), AnnotationSize); 
                            }
                        }

                        
                    }


                }

                 
                 roiImage.Dispose();
                 grayimage.Dispose();

            } catch (Exception exp) {
                
                Console.WriteLine(exp);
                log.Error( exp);
            }


            

            //return base.Process(ImageIn, ImageOut, RoiRegion);
            return true;

        }


    }


}
