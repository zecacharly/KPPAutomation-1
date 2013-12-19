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
using System.Reflection;



namespace VisionModule {


    [ProcessingFunction("Constant", "Math")]
    public class ProcessingFunctionConstant : ProcessingFunctionBase {

        
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionConstant));


        #region Pre-Processing

        ResultReference _Value1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Constant Value"), DisplayName("Constant Value"), AcceptType(typeof(Double))]
        public ResultReference Value1 {
            get {
                return _Value1;
            }
            set {
                _Value1 = value;
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


        private ResultReference _K;
        [XmlIgnore]
        [DisplayName("Out"), Category("Post-Processing"), Description("Sum value"), ReadOnly(true)]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        [AllowDrag(true), UseInRef(true), UseInResultInput(true), AcceptType(typeof(Double))]
        public ResultReference Out {
            get { return _K; }
            set { _K = value; }
        }


        #endregion



        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                

                if (Value1 == null || Out == null) {
                    return false;
                }
                Value1.UpdateValue();
            

                if (Value1.ResultOutput is Double) {

                }
                else {
                    return false;
                }

                //Double value1 = (Double)Value1.ResultOutput;
                //Double value2 = (Double)Value2.ResultOutput;

                Out.ResultOutput = Math.Round((Double)Value1.ResultOutput, 2);

                //PointF point3 = (PointF)Point3.ReferenceValue;







            } catch (Exception exp) {

                log.Error(exp);
            }



            return true;


        }

        public ProcessingFunctionConstant() {
            
        }

    }


     [ProcessingFunction("Sum", "Math")]
    public class ProcessingFunctionSum: ProcessingFunctionBase {
         private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionSum));
         public ProcessingFunctionSum() {

             
         }

         
         //= new KPPLogger(typeof(ProcessingFunctionSum));

       

        #region Pre-Processing

        ResultReference _Value1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Value 1"), DisplayName("Value 1"), AcceptType(typeof(Double))]
        public ResultReference Value1 { 
            get{
                return _Value1;
            }
            set {
                _Value1 = value;
            }
        }


        ResultReference _Value2;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Value 2"), DisplayName("Value 2"), AcceptType(typeof(Double))]
        public ResultReference Value2 {
            get {
                return _Value2;
            }
            set {
                _Value2 = value;
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


        private Double _Sum;
        [XmlIgnore]
        [DisplayName("Sum"), Category("Post-Processing"), Description("Sum value"), ReadOnly(true)]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        [AllowDrag(true),UseInRef(true), UseInResultInput(true)]
        public Double Sum {
            get { return _Sum; }
            set { _Sum= value; }
        }

        
        #endregion



        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                
                if (Value1 == null || Value2== null) {
                    return false;
                }
                Value1.UpdateValue();
                Value2.UpdateValue();

                if (Value1.ResultOutput is Double &&  Value2.ResultOutput is Double) {
                    
                } else {
                    return false;
                }

                //Double value1 = (Double)Value1.ResultOutput;
                //Double value2 = (Double)Value2.ResultOutput;

                Sum = Math.Round((Double)Value1.ResultOutput + (Double)Value2.ResultOutput,2);
                
                //PointF point3 = (PointF)Point3.ReferenceValue;


                



                 
            } catch (Exception exp) {
                
                log.Error( exp);
                return false;
            }



            return true;


        }


    }

     [ProcessingFunction("Sub", "Math")]
     public class ProcessingFunctionSub : ProcessingFunctionBase {
         private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionSub));
         public ProcessingFunctionSub() {

             
         }

         
         //= new KPPLogger(typeof(ProcessingFunctionSub));



         #region Pre-Processing

         ResultReference _Value1;
         [AcceptDrop(true)]
         [Category("Pre-Processing"), Description("Value 1"), DisplayName("Value 1"), AcceptType(typeof(Double))]
         public ResultReference Value1 {
             get {
                 return _Value1;
             }
             set {
                 _Value1 = value;
             }
         }


         ResultReference _Value2;
         [AcceptDrop(true)]
         [Category("Pre-Processing"), Description("Value 2"), DisplayName("Value 2"), AcceptType(typeof(Double))]
         public ResultReference Value2 {
             get {
                 return _Value2;
             }
             set {
                 _Value2 = value;
             }
         }

         //private ProcessingCondition _ProcessCondition = new ProcessingCondition();
         //[Category("Pre-Processing"), DisplayName("Condition"), Browsable(true)]
         //public override ProcessingCondition ProcessCondition {
         //    get { 
         //        return _ProcessCondition; 
         //    }
         //    set { 
         //        _ProcessCondition = value;
         //        if (_ProcessCondition.Input1==null) {
         //            _ProcessCondition.Input1 = (Double)0.0;
         //        }
         //        if (_ProcessCondition.Input2 == null) {
         //            _ProcessCondition.Input2 = (Double)0.0;
         //        }
         //    }
         //}


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


         private Double _Sub;
         [XmlIgnore]
         [DisplayName("Sub"), Category("Post-Processing"), Description("Sub value"), ReadOnly(true)]
         //[TypeConverter(typeof(ExpandableObjectConverter))]
         [AllowDrag(true), UseInRef(true), UseInResultInput(true)]
         public Double Sub {
             get { return _Sub; }
             set { _Sub = value; }
         }


         #endregion

         public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
             try {
                 
                 base.Process(ImageIn, ImageOut, RoiRegion);
                 if (Value1 == null || Value2 == null) {
                     return false;
                 }
                 Value1.UpdateValue();
                 Value2.UpdateValue();

                 if (Value1.ResultOutput is Double && Value2.ResultOutput is Double) {
                     
                 } else {
                     return false;

                 }

                

                 Sub = (Double)Value1.ResultOutput - (Double)Value2.ResultOutput;
                 Sub = Math.Round(Sub, 2);




             } catch (Exception exp) {
                 
                 log.Error(this.FunctionName,exp);
                 return false;
             }



             return true;


         }


     }

     [ProcessingFunction("Division", "Math")]
     public class ProcessingFunctionDivision : ProcessingFunctionBase {

         private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionDivision));
         public ProcessingFunctionDivision() {

             
         }

         
         //= new KPPLogger(typeof(ProcessingFunctionDivision));



         #region Pre-Processing

         ResultReference _Value1;
         [AcceptDrop(true)]
         [Category("Pre-Processing"), Description("Value 1"), DisplayName("Value 1"),AcceptType(typeof(Double))]
         public ResultReference Value1 {
             get {
                 return _Value1;
             }
             set {
                 _Value1 = value;
             }
         }


         ResultReference _Value2;
         [AcceptDrop(true)]
         [Category("Pre-Processing"), Description("Value 2"), DisplayName("Value 2"), AcceptType(typeof(Double))]
         public ResultReference Value2 {
             get {
                 return _Value2;
             }
             set {
                 _Value2 = value;
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


         private Double _Div;
         [XmlIgnore]
         [DisplayName("Div"), Category("Post-Processing"), Description("Division value"), ReadOnly(true)]
         //[TypeConverter(typeof(ExpandableObjectConverter))]
         [AllowDrag(true), UseInRef(true), UseInResultInput(true)]
         public Double Div {
             get { return _Div; }
             set { _Div = value; }
         }


         #endregion

         public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
             try {
                 
                 base.Process(ImageIn, ImageOut, RoiRegion);
                 if (Value1 == null || Value2 == null) {
                     return false;
                 }
                 Value1.UpdateValue();
                 Value2.UpdateValue();

                 if (Value1.ResultOutput is Double && Value2.ResultOutput is Double) {
                     
                 } else {
                     return false;
                 }


                 Div = (Double)Value1.ResultOutput / (Double)Value2.ResultOutput;
                 Div = Math.Round(Div, 2);


             } catch (Exception exp) {
                 
                 log.Error(exp);
             }



             return false;


         }


     }


}

