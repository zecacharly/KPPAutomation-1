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



namespace VisionModule {
    [ProcessingFunction("Point to Point Distance", "Measurement")]
    public class ProcessingFunctionPoint2PointDist : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionPoint2PointDist));

        public ProcessingFunctionPoint2PointDist() {

            
        }

        

       

        #region Pre-Processing

        ResultReference _Point1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 1"), DisplayName("Point 1"),AcceptType(typeof(Point))]
        public ResultReference Point1 {
            get {
                return _Point1;
            }
            set {
                if (_Point1!=value) {
                    _Point1 = value;
                    //if ((object)value is ROIProcessReference) {

                    //    if (((ROIProcessReference)value).ReferenceValue is PointF) {
                    //     _Point1 = value;   
                    //    }

                        
                    //}
                }
            }
        }

        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 2"), DisplayName("Point 2"), AcceptType(typeof(Point))]
        public ResultReference Point2 { get; set; }



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

       
        [XmlIgnore]
        [UseInResultInput(true)]
        [DisplayName("Distance"), Category("Post-Processing"), Description("Point to Point Distance"), ReadOnly(true)]
        public Double Distance { get; set; }

        [XmlIgnore]
        [DisplayName("X Axis Distance"), Category("Post-Processing"), Description("Point to Point Distance in the X axis"), ReadOnly(true)]
        [UseInResultInput(true)]
        public Double DistanceX { get; set; }


        [XmlIgnore]
        [UseInResultInput(true)]
        [DisplayName("Y Axis Distance"), Category("Post-Processing"), Description("Point to Point Distance in the Y axis"), ReadOnly(true)]
        public Double DistanceY { get; set; }


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
                
                if (Point1==null || Point2==null) {
                    return false;
                }

                Point1.UpdateValue();
                Point2.UpdateValue();

                if (!(Point1.ResultOutput is Point ) || !(Point2.ResultOutput is Point)) {
                    return false;
                }

                
                Point point1 = (Point)Point1.ResultOutput;
                Point point2 = (Point)Point2.ResultOutput;


                Distance = Math.Round(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)), 3);

                switch (ResultsInROI) {
                    case OutputResultType.orContours:
                        break;
                    case OutputResultType.orResults:
                        ImageOut.Draw(new Cross2DF(point1, 10, 10), new Bgr(Color.Green), AnnotationSize);
                        ImageOut.Draw(new Cross2DF(point2, 10, 10), new Bgr(Color.Green), AnnotationSize);
                        ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Green), AnnotationSize);
                        ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point1.Y)), new Bgr(Color.Yellow), AnnotationSize);
                        ImageOut.Draw(new LineSegment2D(new Point((int)point2.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Yellow), AnnotationSize);
                        break;
                    case OutputResultType.orNone:
                        break;
                    case OutputResultType.orPreProcessing:
                        break;
                    default:
                        break;
                }

                if (ValidCoordinateSystem) {
                    Point CoordPoint=(Point)CoordinateSystem.Center.ResultOutput;
                    Double CoordAngle=(Double)CoordinateSystem.Angle.ResultOutput;
                    Double CoordAngleRad = Math.PI * CoordAngle / 180.0;



                    point1.X = (int)((point1.X) / (Math.Cos(CoordAngleRad)));
                    point1.Y = (int)((point1.Y) / (Math.Sin((Math.PI/2)+CoordAngleRad)));
                        

                    point2.X = (int)((point2.X) / (Math.Cos(CoordAngleRad)));
                    point2.Y = (int)((point2.Y) / (Math.Sin((Math.PI / 2) + CoordAngleRad)));

                    //DistanceX = Math.Round(Math.Abs(point1.X - point2.X), 3);
                    //DistanceY = Math.Round(Math.Abs(point1.Y - point2.Y), 3);
                    
                    //DistanceX = Math.Round((point1.X - point2.X, 3);
                    //DistanceY = Math.Round(point1.Y - point2.Y, 3);

                    switch (ResultsInROI) {
                        case OutputResultType.orContours:
                            break;
                        case OutputResultType.orResults:
                            ImageOut.Draw(new Cross2DF(point1, 10, 10), new Bgr(Color.Red), AnnotationSize);
                            ImageOut.Draw(new Cross2DF(point2, 10, 10), new Bgr(Color.Red), AnnotationSize);
                            ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Green), AnnotationSize);
                            ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point1.Y)), new Bgr(Color.Red), AnnotationSize);
                            ImageOut.Draw(new LineSegment2D(new Point((int)point2.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Red), AnnotationSize);
                            break;
                        case OutputResultType.orNone:
                            break;
                        case OutputResultType.orPreProcessing:
                            break;
                        default:
                            break;
                    }

                } else {
                    DistanceX = Math.Abs(point1.X - point2.X);
                    DistanceY = Math.Abs(point1.Y - point2.Y);
                }

                Distance = Math.Round(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)), 3);

                


                

                
                



                 
            } catch (Exception exp) {
                
                log.Error( exp);
            }



           return true;


        }


    }


}
