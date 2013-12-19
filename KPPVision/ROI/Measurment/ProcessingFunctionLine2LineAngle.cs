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
    [ProcessingFunction("Line 2 Line Angle", "Measurement")]
    public class ProcessingFunctionLine2LineAngle: ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionLine2LineAngle));
        public ProcessingFunctionLine2LineAngle() {

            
        }

        
        //= new KPPLogger(typeof(ProcessingFunctionPoints2Line));


       

        #region Pre-Processing

        private ResultReference _Line1; 
        //ROIProcessReference _Point1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Line Segment 1"), DisplayName("Line 1"), AcceptType(typeof(LineSegment2D))]
        public ResultReference Line1 {
            get {
                return _Line1;
            }

            set {
                _Line1 = value;
            }
        }

        private ResultReference _Line2;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Line Segment 2"), DisplayName("Line 2"), AcceptType(typeof(LineSegment2D))]
        public ResultReference Line2 {
            get {
                return _Line2;
            }

            set {
                _Line2 = value;
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


        
        




        private Double _Angle;
        [XmlIgnore]
        [UseInRef(true), UseInResultInput(true)]
        [DisplayName("Angle"), Category("Post-Processing"), Description("Angle from 2 lines"), ReadOnly(true)]
        public Double Angle {
            get { 
                return Math.Abs(_Angle); 
            }
            set { _Angle = value; }
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
                
                if (Line1==null || Line2==null ) {
                    return false;
                }

                Line1.UpdateValue();
                Line2.UpdateValue();

                if (!(Line1.ResultOutput is LineSegment2D) || !(Line2.ResultOutput is LineSegment2D)) {
                    return false;
                }

                LineSegment2D line1 = (LineSegment2D)Line1.ResultOutput;
                LineSegment2D line2 = (LineSegment2D)Line2.ResultOutput;
                //PointF point3 = (PointF)Point3.ReferenceValue;


                Angle = Math.Round(line1.GetExteriorAngleDegree(line2), 2);

                Double angle2 = Math.Atan2((line2.Direction.Y - line1.Direction.Y),(line1.Direction.X - line2.Direction.X)) * 180 / Math.PI;

                //if (Angle<0) {
                //    Angle = 360 + Angle;
                //}

                //Angle = Math.Abs(Angle);



                 
            } catch (Exception exp) {
                
                log.Error( exp);
            }



           return true;


        }


    }


}
