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
using System.Numerics;
using System.Drawing.Design;



namespace VisionModule {
    [ProcessingFunction("Points 2 Circle", "Measurement")]
    public class ProcessingFunctionPoints2Circle: ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionPoints2Circle));
        public ProcessingFunctionPoints2Circle() {

            
        }

        
        //= new KPPLogger(typeof(ProcessingFunctionPoints2Circle));

       

        #region Pre-Processing

        //ROIProcessReference _Point1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 1"), DisplayName("Point 1"),AcceptType(typeof(Point))]
        public ResultReference Point1 { get; set; }
        //    get {
        //        return _Point1;
        //    }
        //    set {
        //        if (_Point1!=value) {
        //            _Point1 = value;                   
        //        }
        //    }
        //}

        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 2"), DisplayName("Point 2"), AcceptType(typeof(Point))]
        public ResultReference Point2 { get; set; }

        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 3"), DisplayName("Point 3"), AcceptType(typeof(Point))]
        public ResultReference Point3 { get; set; }

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




        #endregion



        #region Post Processing


        
        private Double _Rad;
        [XmlIgnore]
        [DisplayName("Circle Radius"), Category("Post-Processing"), Description("Circle radius"), ReadOnly(true)]
        [UseInRef(true), UseInResultInput(true)]
        public Double Rad {
            get { return _Rad; }
            set { _Rad = value; }
        }



        
        private Point _Center;
        [XmlIgnore]
        [UseInRef(true), UseInResultInput(true)]
        //[TypeConverter(typeof(ExpandableObjectConverter)), EditorAttribute(typeof(OutputReferenceSelector), typeof(UITypeEditor))]
        [DisplayName("Circle Center"), Category("Post-Processing"), Description("Circle Center"), ReadOnly(true)]
        public Point Center {
            get { return _Center; }
            set { _Center = value; }
        }


        //[XmlIgnore]
        //[UseInResultInput(true),DisplayName("Pixel Count"), Category("Post-Processing"), Description("Pixel Count above average inside contour"), ReadOnly(true)]
        //public int PixelCount { get; set; }

        //[XmlIgnore]
        //[UseInResultInput(true),DisplayName("PixelDeviation"), Category("Post-Processing"), Description("Pixel Standard Deviation inside contour"), ReadOnly(true)]
        //public double PixelDeviation { get; set; }


       
        
        #endregion

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                if (Point1==null || Point2==null || Point3== null) {
                    return false;
                }

                Point1.UpdateValue();
                Point2.UpdateValue();
                Point3.UpdateValue();

                if (!(Point1.ResultOutput is Point) || !(Point2.ResultOutput is Point) || !(Point3.ResultOutput is Point)) {
                    return false;
                }

                Point point1 = (Point)Point1.ResultOutput;
                Point point2 = (Point)Point2.ResultOutput;
                Point point3 = (Point)Point3.ResultOutput;

                float radfloat = 0;

                KPPMath.FindCircle(point1, point2, point3, out _Center, out radfloat);

                Double.TryParse(radfloat.ToString(), out _Rad);

                if (double.IsNaN(_Center.X) || double.IsNaN(_Center.Y)) {
                    return false;
                }

                //Distance = Math.Round(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y,2)),3);
                //DistanceX = Math.Round(Math.Abs(point1.X-point2.X), 3);
                //DistanceY = Math.Round(Math.Abs(point1.Y - point2.Y), 3);
                base.IdentRegion = new Image<Gray, byte>(RoiRegion.Size);
                base.IdentRegion.Draw(new CircleF(Center,(float)Rad), new Gray(255), 1);
                switch (ResultsInROI) {
                    case OutputResultType.orContours:
                        break;
                    case OutputResultType.orResults:
                        //ImageOut.Draw(new Cross2DF(point1,10,10), new Bgr(Color.Green), AnnotationSize);
                        //ImageOut.Draw(new Cross2DF(point2, 10, 10), new Bgr(Color.Green), AnnotationSize);
                        //ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Green), AnnotationSize);
                        //ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point1.Y)), new Bgr(Color.Yellow), AnnotationSize);
                        //ImageOut.Draw(new LineSegment2D(new Point((int)point2.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Yellow), AnnotationSize);

                        ImageOut.Draw(new Cross2DF(Center,10,10), new Bgr(Color.Green), AnnotationSize);
                        ImageOut.Draw(new CircleF(Center,(float)Rad), new Bgr(Color.Green), AnnotationSize);
                        
                        break;
                    case OutputResultType.orNone:
                        break;
                    case OutputResultType.orPreProcessing:
                        break;
                    default:
                        break;
                }
                



                 
            } catch (Exception exp) {
                
                log.Error( exp);
            }



           return false;


        }


    }


}
