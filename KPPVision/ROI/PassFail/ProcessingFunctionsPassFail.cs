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



    [ProcessingFunction("Range", "Pass Fail")]
    public class ProcessingFunctionRange : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionRange));
        public ProcessingFunctionRange() {

            
        }

        
        //= new KPPLogger(typeof(ProcessingFunctionSum));



        #region Pre-Processing

        ResultReference _Input;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Input"), DisplayName("Input"),AcceptType(typeof(Double))]
        public ResultReference Input {
            get {
                return _Input;
            }
            set {
                _Input = value;
            }
        }

        String  _FailOutput;
        [AcceptDrop(true), XmlAttribute]
        [Category("Pre-Processing"), Description("Output if fail"), DisplayName("Fail Output"), AcceptType(typeof(Double))]
        public String FailOutput {
            get {
                return _FailOutput;
            }
            set {
                _FailOutput = value;
            }
        }

        String _PassOutput;
        [AcceptDrop(true), XmlAttribute]
        [Category("Pre-Processing"), Description("Output if pass"), DisplayName("Pass Output"), AcceptType(typeof(Double))]
        public String PassOutput{
            get {
                return _PassOutput;
            }
            set {
                _PassOutput = value;
            }
        }


        Double _ValueMax;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Max Value"), DisplayName("Max Value")]
        public Double ValueMax {
            get {
                return _ValueMax;
            }
            set {
                _ValueMax = value;
            }
        }

        Double _ValueMin;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Min Value"), DisplayName("Min Value")]
        public Double ValueMin {
            get {
                return _ValueMin;
            }
            set {
                _ValueMin = value;
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


        private String _Out;
        [XmlIgnore]
        [DisplayName("Output"), Category("Post-Processing"), Description("Is input in range"), ReadOnly(true)]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        [AllowDrag(true), UseInRef(true), UseInResultInput(true)]
        public String Out {
            get { return _Out; }
            set { _Out = value; }
        }


        #endregion

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                Out = FailOutput;
                if (Input == null ) {
                    return false;
                }
                Input.UpdateValue();


                if (!(Input.ResultOutput is Double)) {
                    return false;
                }

                //Double value1 = (Double)Value1.ResultOutput;
                //Double value2 = (Double)Value2.ResultOutput;

                Double inputval = 0;

                if (Input.ResultOutput is int) {
                    inputval = (Double)(int)Input.ResultOutput;
                }
                else {
                    inputval = (Double)Input.ResultOutput;
                }

                Out = (inputval <= ValueMax && inputval >= ValueMin) ? PassOutput : FailOutput;

                //PointF point3 = (PointF)Point3.ReferenceValue;







            } catch (Exception exp) {

                log.Error(exp);
            }



            return true;


        }


    }
}

