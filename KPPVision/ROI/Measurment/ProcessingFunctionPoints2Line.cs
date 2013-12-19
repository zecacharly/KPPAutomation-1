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


    public class LineSegmentConverter : ExpandableObjectConverter {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        //public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
        //    StandardValuesCollection coll = new StandardValuesCollection(new Test[] { new TestImpl() });
        //    return coll;
        //}

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            //if (true) {

            //}
            return true;
            //return sourceType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (value is LineSegment2D) {
                LineSegment2D newline = (LineSegment2D)value;
                return new LineSegment2D(newline.P1,newline.P2);
            } 

            

            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            //ResultReference resrefinst = context.Instance as ResultReference;

            //if (resrefinst == null) {
            //    return value;
            //}
            //if (resrefinst.ResultOutput == null) {
            //    return value;
            //}
            //var converter = TypeDescriptor.GetConverter(resrefinst.ResultOutput.GetType());
            //if (converter.CanConvertFrom(value.GetType())) {
            //    object newObject = converter.ConvertFrom(value);
            //    value = newObject;
            //}


            return value;
        }
    }


    [ProcessingFunction("Points 2 Line", "Measurement")]
    public class ProcessingFunctionPoints2Line: ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionPoints2Line));

        public ProcessingFunctionPoints2Line() {

            

        }

        
        //= new KPPLogger(typeof(ProcessingFunctionPoints2Line));

       

        #region Pre-Processing

        private ResultReference _ExteriorLine = new ResultReference();//new LineSegment2D(new Point(50, 0), new Point(0, 0));
        //[XmlIgnore]
        [DisplayName("Exterior Line Segment"), Category("Post-Processing"), Description("Exterior Line for angle calculation"), ReadOnly(false)]
        //[TypeConverter(typeof(LineSegmentConverter))]
        [AcceptDrop(true), AcceptType(typeof(LineSegment2D))]
        public ResultReference ExteriorLine {
            get { return _ExteriorLine; }
            set { _ExteriorLine = value; }
        }

        ResultReference _Point1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 1"), DisplayName("Point 1"), AcceptType(typeof(Point))]
        public ResultReference Point1 { 
            get{
                return _Point1;
            }
            set {
                _Point1 = value;
            }
        }

        ResultReference _Point2;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Point 2"), DisplayName("Point 2"),AcceptType(typeof(Point))]
        public ResultReference Point2 {
            get {
                return _Point2;
            }
            set {
                _Point2 = value;
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




        #endregion



        #region Post Processing


        
        




        private LineSegment2D _Line= new LineSegment2D();
        [XmlIgnore]
        [DisplayName("Line Segment"), Category("Post-Processing"), Description("Line from the 2 points"), ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [AllowDrag(true),UseInRef(true), UseInResultInput(true)]
        public LineSegment2D Line {
            get { return _Line; }
            set { _Line = value; }
        }


        private Point _LineCenter;
        [XmlIgnore]
        [DisplayName("Segment Center"), Category("Post-Processing"), Description("Center point of line segment"), ReadOnly(true)]
        [AllowDrag(true), UseInRef(true), UseInResultInput(true)]
        public Point LineCenter {
            get {

                if (_Line.Length > 0) {
                    Point mid = new Point((_Line.P1.X + _Line.P2.X) / 2, (_Line.P1.Y + _Line.P2.Y) / 2);
                    return mid;
                }


                return new Point(0, 0);
            }

        }


        private Double _LineAngle=-1;
        [XmlIgnore]
        [DisplayName("Segment Angle"), Category("Post-Processing"), Description("Exterior Angle of line segment"), ReadOnly(true)]
        [AllowDrag(true), UseInRef(true), UseInResultInput(true)]
        public Double LineAngle {
            get {
                return _LineAngle;
            }
            set {
                if (_LineAngle!=value) {
                    _LineAngle = value;
                }
            }
        }

       
        
        #endregion

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                Line = new LineSegment2D();


                

                if (Point1==null || Point2==null ) {
                    return false;
                }

                if (ExteriorLine.ResultOutput == null) {
                    ExteriorLine.ResultOutput = new LineSegment2D();
                }

                Point1.UpdateValue();
                Point2.UpdateValue();
                ExteriorLine.UpdateValue();

                if (!(Point1.ResultOutput is Point) || !(Point2.ResultOutput is Point) || !(ExteriorLine.ResultOutput is LineSegment2D)) {
                    return false;
                }

                Point point1 = (Point)Point1.ResultOutput;
                Point point2 = (Point)Point2.ResultOutput;
                LineSegment2D exteriorLine = (LineSegment2D)ExteriorLine.ResultOutput;
                //PointF point3 = (PointF)Point3.ReferenceValue;


                Line = new LineSegment2D(new Point(point1.X,point1.Y), new Point(point2.X,point2.Y));

                if (Line.Length > 0 && exteriorLine.Length > 0) {



                    Double theangle = Math.Round(Line.GetExteriorAngleDegree(exteriorLine), 3);

                    LineAngle = theangle;
                }

              
                switch (ResultsInROI) {
                    case OutputResultType.orContours:
                        break;
                    case OutputResultType.orResults:
                        ImageOut.Draw(new Cross2DF(point1,10,10), new Bgr(Color.Yellow), AnnotationSize);
                        ImageOut.Draw(new Cross2DF(point2, 10, 10), new Bgr(Color.Yellow), AnnotationSize);
                        ImageOut.Draw(new Cross2DF(LineCenter, 10, 10), new Bgr(Color.Yellow), AnnotationSize);


                        //ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point2.Y)), new Bgr(Color.Green), AnnotationSize);
                        //ImageOut.Draw(new LineSegment2D(new Point((int)point1.X, (int)point1.Y), new Point((int)point2.X, (int)point1.Y)), new Bgr(Color.Yellow), AnnotationSize);
                        
                        ImageOut.Draw(Line,new Bgr(Color.Green), AnnotationSize);
                                                
                        
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



           return true;


        }


    }


}
