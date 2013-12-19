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
using Emgu.CV.UI;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;



namespace VisionModule {


    public class LineEdgePreProc {

        

        public class EdgeDirectionConverter : EnumConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {

                EdgeDirection edge = EdgeDirection.Light_Dark;

                String name = (String)value;

                if (name != null) {

                    name = name.Replace(" ", "_");
                    Enum.TryParse(name, out edge);
                }
                return edge;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                return destinationType == typeof(string);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {

                return GetEnumDescription((EdgeDirection)value);

            }
            public EdgeDirectionConverter() : base(typeof(EdgeDirection)) { }
        }


        public class EdgeDetectionConverter : EnumConverter {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {

                EdgeDetection edge = EdgeDetection.First_Edge;

                String name = (String)value;

                if (name != null) {

                    name = name.Replace(" ", "_");
                    Enum.TryParse(name, out edge);
                }
                return edge;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                return destinationType == typeof(string);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {

                return GetEnumDescription((EdgeDetection)value);

            }
            public EdgeDetectionConverter() : base(typeof(EdgeDetection)) { }
        }



        internal static string GetEnumDescription(Enum value) {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        [Description("line start point"), DisplayName("Line Start Point")]
        [AllowDrag(true)]
        public Point StartPoint { get; set; }


        [Description("line end point"), DisplayName("Line End Point")]
        [AllowDrag(true)]
        public Point EndPoint { get; set; }


        [XmlAttribute]
        [Category("Pre-Processing"), Description("Direction of transition (Dark-Light / Light-Dark )")]
        [TypeConverter(typeof(EdgeDirectionConverter))]
        public EdgeDirection edgeDirection { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Direction of transition (Dark-Light / Light-Dark )")]
        [TypeConverter(typeof(EdgeDetectionConverter))]
        public EdgeDetection edgeDetection { get; set; }


        [XmlAttribute]
        [Description("Mini Gradient amplitude level"), DisplayName("Min Gradient")]
        public Double MinGradientLevel { get; set; }

        [XmlAttribute]
        [Description("Max Gradient amplitude level"), DisplayName("Max Gradient")]
        public Double MaxGradientLevel { get; set; }

        [XmlAttribute]
        [Description("Transition point must have pixel value below this level"), DisplayName("Max Gray Level")]
        public Double MaxGrayLevel { get; set; }

        [XmlAttribute]
        [Description("Transition point must have pixel value above this level"), DisplayName("Min Gray Level")]
        public Double MinGrayLevel { get; set; }


        //[XmlAttribute]
        //[Description("Minimum Gradient Amplitude value"), DisplayName("Min Amplitude")]
        //public Double MinAmplitude { get; set; }




        public LineEdgePreProc() {

        }

        public override string ToString() {
            return "Line Edge Settings";
        }

    }

    public enum EdgeDetection {

        [Description("First Edge")]
        First_Edge,
        [Description("Max Edge")]
        Max_Edge
    }

    public enum EdgeDirection {

        [Description("Dark Light")]
        Dark_Light,
        [Description("Light Dark")]
        Light_Dark
    }

    

    [ProcessingFunction("Line Edge", "Line")]
    public class ProcessingFunctionLineEdge : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionLineEdge));

        public ProcessingFunctionLineEdge() {

            
        }
        
        


        internal static IEnumerable<T> EnumToList<T>() {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray) {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }


      
        


        #region Pre-Processing

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


        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(false)]
        public override ContourPreProc ContourPreProc1 {
            get;
            set;
        }


        //[TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Image"), Category("Pre-Processing"), Browsable(true)]
        //public override ImagePreProc ImagePreProc1 {
        //    get;
        //    set;
        //}


        #endregion



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


        private void PointComparer(Point Pt1) {
        }
        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                EdgeLocation = new Point(0, 0);
                MaxGradient= -1;
                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);

                using (grayimage) {







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
                    }


                    if (base.ImagePreProc1.UseChannel != Channel.Bgr) {



                        LineSegment2D lineproc = new LineSegment2D(LineEdgePreProc1.StartPoint, LineEdgePreProc1.EndPoint);
                        ImageOut.Draw(lineproc, new Bgr(Color.Yellow), AnnotationSize);

                        Image<Gray, Byte> mask_line = new Image<Gray, byte>(grayimage.Size);
                        CvInvoke.cvLine(mask_line, lineproc.P1, lineproc.P2, new MCvScalar(255), 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);



                        base.IdentRegion = new Image<Gray, byte>(grayimage.Size);
                        CvInvoke.cvLine(base.IdentRegion, lineproc.P1, lineproc.P2, new MCvScalar(255), 0, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);

                        Image<Gray, Byte> maskedImage = new Image<Gray, Byte>(grayimage.Size);
                        CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, mask_line.Ptr);




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


                            incountour = incountour.OrderBy(ptx => ptx.X).ThenBy(pty => pty.Y).ToList();

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
                                }
                                else {
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



                                    if (LineEdgePreProc1.edgeDetection == EdgeDetection.Max_Edge) {
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
                                    }
                                    else {



                                        for (int i = 0; i < GradientAmplitude.Count; i++) {

                                            if (GradientAmplitude[i] >= LineEdgePreProc1.MinGradientLevel && Direction[i] < 0) {
                                                maxvalindex = i;
                                                break;
                                            }


                                        }
                                    }


                                    break;
                                case EdgeDirection.Light_Dark:

                                    //
                                    if (LineEdgePreProc1.edgeDetection == EdgeDetection.Max_Edge) {
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
                                    }
                                    else {
                                        for (int i = 0; i < GradientAmplitude.Count; i++) {

                                            if (GradientAmplitude[i] >= LineEdgePreProc1.MinGradientLevel && Direction[i] >= 0) {
                                                maxvalindex = i;
                                                break;
                                            }


                                        }
                                    }
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



                 }
                
                
                roiImage.Dispose();
                grayimage.Dispose();

            } catch (Exception exp) {
                
                log.Error( exp);
                return false;
            }



            return true;


        }


    }


}
