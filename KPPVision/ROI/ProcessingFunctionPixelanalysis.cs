using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

using KPP.Core.Debug;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using KPP.Controls.Winforms.ImageEditorObjs;
using DejaVu;



namespace VisionModule {



    [ProcessingFunction("Pixel Analysis", "Area")]
    public class ProcessingFunctionPixelanalysis : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionPixelanalysis));

        public ProcessingFunctionPixelanalysis() {
            
        }
        

        private Point _RectangleCenter = new Point(0, 0);


        readonly UndoRedo<int> _PixelCountvalAbove = new UndoRedo<int>(50);
        [XmlAttribute]
        [Category("Pre-Processing"), Description("Count pixel above value"), DisplayName("Count Pixels Above")]
        /// <summary>
        /// Count pixel above value
        /// </summary>
        public int PixelCountvalAbove {
            get { return _PixelCountvalAbove.Value; }
            set {
                if (_PixelCountvalAbove.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min Area value changed to: " + value.ToString())) {
                            _PixelCountvalAbove.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _PixelCountvalAbove.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _PixelCountvalBelow = new UndoRedo<int>(255);
        [XmlAttribute]
        [Category("Pre-Processing"), Description("Count pixel above value"), DisplayName("Count Pixels Below")]
        public int PixelCountvalBelow {
            get { return _PixelCountvalBelow.Value; }
            set {
                if (_PixelCountvalBelow.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min Area value changed to: " + value.ToString())) {
                            _PixelCountvalBelow.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _PixelCountvalBelow.Value = value;
                    }
                }
            }
        }



        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(false)]
        [XmlIgnore]
        public override ContourPreProc ContourPreProc1 {
            get;
            set;
        }

        private Image<Gray, Byte> _MaskImage = new Image<Gray, byte>(Size.Empty);
        [Category("Pre-Processing"), Description("Mask image to apply"), DisplayName("Mask Image")]
        [XmlIgnore]
        public Image<Gray, Byte> MaskImage {
            get { return _MaskImage; }
            set { _MaskImage = value; }
        }




        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Average"), Category("Post-Processing"), Description("Pixel Average inside contour"), ReadOnly(true)]
        public Double PixelAverage { get; set; }

        private Double _PixelMinVal;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Min Value"), Category("Post-Processing"), Description("Pixel Min Value inside contour"), ReadOnly(true), Browsable(false)]
        public Double PixelMinVal {
            get { return _PixelMinVal; }
            set { _PixelMinVal = value; }
        }



        private Double _PixelMaxVal;
        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Max Value"), Category("Post-Processing"), Description("Pixel Max Value inside contour"), ReadOnly(true), Browsable(false)]
        public Double PixelMaxVal {
            get { return _PixelMaxVal; }
            set { _PixelMaxVal = value; }
        }

        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Count"), Category("Post-Processing"), Description("Pixel Count above average inside contour"), ReadOnly(true)]
        public Double PixelCount { get; set; }

        [XmlIgnore]
        [UseInResultInput(true), DisplayName("Pixel Deviation"), Category("Post-Processing"), Description("Pixel Standard Deviation inside contour"), ReadOnly(true)]
        public double PixelDeviation { get; set; }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageIn"></param>
        /// <param name="ImageOut"></param>
        /// <param name="RoiRegion"></param>
        /// <returns></returns>
        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                


                Image<Bgr, byte> roiImage;

                if (RoiRegion == Rectangle.Empty) {
                    roiImage = new Image<Bgr, byte>(ImageIn.Size);
                }
                else {
                    roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                }

                ImageIn.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(roiImage.Size);
                ImageOut.ROI = RoiRegion;

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


                if (base.ImagePreProc1.UseChannel != Channel.Bgr) {
                    switch (base.ImagePreProc1.ThresholdType) {
                        case TypeOfThreshold.Normal:
                            grayimage = grayimage.ThresholdBinary(new Gray(base.ImagePreProc1.Threshold), new Gray(255));

                            break;
                        case TypeOfThreshold.Inverted:
                            grayimage = grayimage.ThresholdBinaryInv(new Gray(base.ImagePreProc1.Threshold), new Gray(255));

                            break;
                        default:
                            break;
                    }






                    #region Pixel information


                    Image<Gray, Byte> maskingImage;

                    if (MaskImage.Size != Size.Empty) {
                        maskingImage = new Image<Gray, byte>(MaskImage.Size);
                        MaskImage.CopyTo(maskingImage);
                        //MaskImage.Save("e:\\mask.bmp");
                        //                        CvInvoke.cvCopy(grayimage.Ptr,

                    }
                    else {
                        maskingImage = new Image<Gray, Byte>(grayimage.Size);
                        maskingImage._ThresholdBinaryInv(new Gray(0), new Gray(255));

                    }





                    if (IdentRegion.Size != maskingImage.Size) {
                        IdentRegion = new Image<Gray, byte>(maskingImage.Size);
                    }

                    CvInvoke.cvCopy(maskingImage.Ptr, IdentRegion.Ptr, IntPtr.Zero);

                    MCvScalar Mean = new MCvScalar();
                    MCvScalar StdDev = new MCvScalar();


                    Image<Gray, Byte> maskedImage = new Image<Gray, byte>(maskingImage.Size);

                    CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, maskingImage.Ptr);

                    //maskedImage.Save("e:\\masked.bmp");

                    //        maskedImage.AvgSdv(out average, out StdDev);

                    //        DenseHistogram hist = new DenseHistogram(256, new RangeF(0.0f, 255.0f));


                    //      hist.Calculate<Byte>(new Image<Gray, byte>[] { grayimage }, true, maskingImage);

                    Point minloc = new Point(0, 0), maxloc = new Point(0, 0);

                    CvInvoke.cvAvgSdv(maskedImage.Ptr, ref Mean, ref StdDev, maskingImage.Ptr);
                    CvInvoke.cvMinMaxLoc(maskedImage.Ptr, ref _PixelMinVal, ref _PixelMaxVal, ref minloc, ref maxloc, maskingImage.Ptr);

                    PixelDeviation = Math.Round(StdDev.v0, 3);
                    PixelAverage = Math.Round(Mean.v0, 3);

                    //CvInvoke.cou

                    maskedImage._ThresholdToZero(new Gray(PixelCountvalAbove));


                    CvInvoke.cvCopy(grayimage.Ptr, maskedImage.Ptr, maskedImage.Ptr);

                    maskedImage._ThresholdToZeroInv(new Gray(PixelCountvalBelow));
                    PixelCount = maskedImage.CountNonzero()[0];
                    if (ResultsInROI == OutputResultType.orResults) {
                        ImageOut.SetValue(new Bgr(Color.Green), maskedImage);
                    }


                    

                    #endregion

                    maskedImage.Dispose();
                    
                    roiImage.Dispose();
                    grayimage.Dispose();


                }
            } catch (Exception exp) {
                
                log.Error(this.FunctionName,exp);
            }


          

            
            return true;


        }


    }


}
