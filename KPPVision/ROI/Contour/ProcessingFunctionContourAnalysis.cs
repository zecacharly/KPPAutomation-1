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
    [ProcessingFunction("Contour Analysis", "Contour")]
    public class ProcessingFunctionContourAnalysis : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionContourAnalysis));

        public ProcessingFunctionContourAnalysis() {
            
        }

        

        private Point _GravityCenter = new Point(0, 0);



        #region Pre-Processing


        [XmlAttribute]
        [Category("Pre-Processing"), Description("Minimum length to consider a valid contour")]
        public Double MinContourLength { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Remove contours that are touching the ROI edges")]
        public Boolean RemoveTouchingROIEdges { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("minimum contour length")]
        public int MinimumArea { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("minimum contour length")]
        public int MaximumArea { get; set; }

        #endregion



        #region Post Processing




        [XmlIgnore]
        [Category("Post-Processing"), Description("Gravity Center of the contour"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [UseInRef(true)]
        public Point GravityCenter {
            get {
                return _GravityCenter;
            }

            set {
                _GravityCenter = value;
                //NotifyPropertyChanged("RectangleCenter");
            }
        }



        //[XmlIgnore]
        //[Category("Post-Processing"), Description("Area of the rectangle"), ReadOnly(true)]
        //public Double Area { get; set; }

        [XmlIgnore]
        [DisplayName("Contour Angle"), Category("Post-Processing"), Description("Contour Angle"), ReadOnly(true)]
        public Double Angle { get; set; }




        #endregion

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {

                base.Process(ImageIn, ImageOut, RoiRegion);

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


                Image<Gray, byte> RoiGray = new Image<Gray, byte>(roiImage.Size);
                grayimage.CopyTo(RoiGray);



                grayimage.ROI = Rectangle.Empty;



                grayimage = grayimage.SmoothBilatral(7, 255, 34);



                using (MemStorage storage = new MemStorage()) {

                    List<Contour<Point>> largestContours = new List<Contour<Point>>();
                    int threshold = base.ImagePreProc1.Threshold;
                    int thresholdlink = base.ImagePreProc1.ThresholdLink;

                    Image<Gray,byte> canny=    grayimage.Canny(threshold, thresholdlink);

                    for (Contour<Point> contours = canny.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                           Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                           contours != null;
                           contours = contours.HNext) {
                        Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);

                        if (ResultsInROI == OutputResultType.orContours)
                            ImageOut.Draw(currentContour, new Bgr(Color.Blue), 1);

                        if (currentContour.Perimeter > MinContourLength) {

                            Boolean isOnEdge = false;

                            if (RemoveTouchingROIEdges) {

                                if (currentContour.BoundingRectangle.Left == 1 || currentContour.BoundingRectangle.Top == 1)
                                    isOnEdge = true;

                                if (currentContour.BoundingRectangle.Right >= roiImage.Width || currentContour.BoundingRectangle.Bottom >= roiImage.Height)
                                    isOnEdge = true;

                            }

                            if (isOnEdge == false) {


                                if ((currentContour.Area > MinimumArea) && (currentContour.Area < MaximumArea)) {




                                    MCvMoments contourMoments = new MCvMoments();

                                    contourMoments = currentContour.GetMoments();
                                    double centralmomentorder1 = CvInvoke.cvGetCentralMoment(ref contourMoments, 1, 1);
                                    double centralmomentx = CvInvoke.cvGetCentralMoment(ref contourMoments, 2, 0);
                                    double centralmomenty = CvInvoke.cvGetCentralMoment(ref contourMoments, 0, 2);



                                    double theta = 0.5 * Math.Atan((2 * centralmomentorder1 / (centralmomentx - centralmomentx)));
                                    //theta = (theta / Math.PI) * 180;


                                    //MCvBox2D box = CvInvoke.cvFitEllipse2(currentContour.Ptr);

                                    MCvBox2D box = currentContour.GetMinAreaRect();
                                    Angle = Math.Round(box.angle, 3);
                                    GravityCenter = new Point((int)(contourMoments.GravityCenter.x + RoiRegion.X), (int)(contourMoments.GravityCenter.y + RoiRegion.Y));

                                    //Translate to image location


                                    //CvInvoke.cvEllipseBox(ImageOut.Ptr, box, new MCvScalar(0, 255, 0), 1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);

                                    //Rectangle minrect = box.MinAreaRect();






                                    if (ResultsInROI == OutputResultType.orResults) {
                                        ImageOut.Draw(box, new Bgr(Color.Green), 1);
                                        ImageOut.Draw(new Cross2DF(new PointF((float)contourMoments.GravityCenter.x, (float)contourMoments.GravityCenter.y), 3, 3), new Bgr(Color.Green), 1);
                                    }

                                    largestContours.Add(currentContour);
                                }
                            }
                        }
                    }



                    
                }


                
                roiImage.Dispose();
                grayimage.Dispose();

            } catch (Exception exp) {
                
                log.Error(this.FunctionName,exp);
                return false;
            }






            return true;
        }


        
    }

    

}
