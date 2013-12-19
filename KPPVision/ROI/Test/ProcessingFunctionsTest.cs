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


    public enum CompareConditions {And,Or}

    public enum TestConditions { Equal, Greater, Less, Compare }

     [ProcessingFunction("Comparer", "Test")]
    public class ProcessingFunctionTest: ProcessingFunctionBase {
         private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionTest));
         public ProcessingFunctionTest() {

             
         }

         
         //= new KPPLogger(typeof(ProcessingFunctionTest));

       

        #region Pre-Processing

         String _FailOutput;
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
         public String PassOutput {
             get {
                 return _PassOutput;
             }
             set {
                 _PassOutput = value;
             }
         }



         private CompareConditions _CompareCondition = CompareConditions.And;
         [Category("Pre-Processing"), Description("Compare Condition"), DisplayName("Compare Condition"),XmlAttribute]
         public CompareConditions CompareCondition {
             get {
                 return _CompareCondition; 
             }
             set {
                 _CompareCondition = value; 
             }
         }

         private TestConditions _TestCondition = TestConditions.Equal;
         [Category("Pre-Processing"), Description("Test Condition"), DisplayName("Condition"), XmlAttribute]
         public TestConditions TestCondition {
             get {
                 return _TestCondition;
             }
             set {
                 _TestCondition = value;
             }
         }


        ResultReference _Value1;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Input 1"), DisplayName("Input 1"), AcceptType(typeof(String))]
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
        [Category("Pre-Processing"), Description("Input 2"), DisplayName("Input 2"), AcceptType(typeof(String))]
        public ResultReference Value2 {
            get {
                return _Value2;
            }
            set {
                _Value2 = value;
            }
        }


        ResultReference _CompareValue;
        [AcceptDrop(true)]
        [Category("Pre-Processing"), Description("Compare Value"), DisplayName("Compare Value"), AcceptType(typeof(String))]
        public ResultReference CompareValue {
            get {
                return _CompareValue;
            }
            set {
                _CompareValue = value;
            }

        }

        #region Ignore

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
                
                
                if (Value1 == null || Value2== null) {
                    return false;
                }
                Value1.UpdateValue();
                Value2.UpdateValue();

                if (Value1.ResultOutput == null || Value2.ResultOutput == null) {
                    return false;
                }

                switch (TestCondition) {
                    case TestConditions.Equal:
                        if (Value1.ResultOutput.Equals(Value2.ResultOutput)) {
                            Out = PassOutput;
                        } else {
                            Out = FailOutput;
                        }
                        break;
                    case TestConditions.Greater:

                        var converter = TypeDescriptor.GetConverter(Value1.ResultOutput);
                        if (converter.CanConvertTo(typeof(Double)) && converter.CanConvertTo(typeof(Double))) {
                            Double val1 = (Double)converter.ConvertTo(Value1.ResultOutput, typeof(Double));
                            Double val2 = (Double)converter.ConvertTo(Value2.ResultOutput, typeof(Double));


                            if (val1 > val2) {
                                Out = PassOutput;
                            } else {
                                Out = FailOutput;
                            }

                        }


                        break;
                    case TestConditions.Less:
                        var converter2 = TypeDescriptor.GetConverter(Value1.ResultOutput);
                        if (converter2.CanConvertTo(typeof(Double)) && converter2.CanConvertTo(typeof(Double))) {
                            Double val1 = (Double)converter2.ConvertTo(Value1.ResultOutput, typeof(Double));
                            Double val2 = (Double)converter2.ConvertTo(Value2.ResultOutput, typeof(Double));


                            if (val1 < val2) {
                                Out = PassOutput;
                            } else {
                                Out = FailOutput;
                            }

                        }


                        break;
                    case TestConditions.Compare:

                        if (CompareValue == null) {
                            return false;
                        }
                        CompareValue.UpdateValue();
                        if (CompareValue.ResultOutput == null) {
                            return false;
                        }

                        if (CompareCondition == CompareConditions.And) {
                            if (Value1.ResultOutput.ToString() == CompareValue.ResultOutput.ToString() &&
                                Value2.ResultOutput.ToString() == CompareValue.ResultOutput.ToString()) {
                                Out = PassOutput;
                            } else {
                                Out = FailOutput;
                            }
                        } else if (CompareCondition == CompareConditions.Or) {
                            if (Value1.ResultOutput.ToString() == CompareValue.ResultOutput.ToString() ||
                                Value2.ResultOutput.ToString() == CompareValue.ResultOutput.ToString()) {
                                Out = PassOutput;
                            } else {
                                Out = FailOutput;
                            }
                        }
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

