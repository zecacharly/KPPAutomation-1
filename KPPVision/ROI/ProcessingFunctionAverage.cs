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

namespace ClassInspection {
    [ProcessingFunction("Average", "Pixel Analysis")]
    [Serializable()]
    public class ProcessingFunctionAverage : ProcessingFunctionBase {

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionBase));

        private double _pixelaverage;
        private double _pixelminval;

       

        [XmlIgnore]
        [Category("Post-Processing"), Description("Gray scale pixel value average"), ReadOnly(true)]
        public double PixelAverage {
            get {
                return _pixelaverage;
            }

            set {
                _pixelaverage = value;                
            }
        }

        private double _PixelStdDv;
        [Category("Post-Processing"), Description("Gray scale pixel standard deviation"), ReadOnly(true)]
        public double PixelStdDv
        {
            get
            {
                return _PixelStdDv;
            }

            set
            {
                _PixelStdDv = value;
            }
        }


        [XmlIgnore]
        [Category("Post-Processing"), Description("Min value pixel"), ReadOnly(true)]
        public double PixelMinValue
        {
            get
            {
                return _pixelminval;
            }

            set
            {
                _pixelminval = value;
            }
        }

     
        

        public override object Clone() {
            return base.Clone();
        }

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {

            try {

                Pass = false;

                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);

                ImageIn.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
                ImageOut.ROI = RoiRegion;

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
                        grayimage = roiImage.Convert<Gray, Byte>();
                        break;
                    default:
                        grayimage = roiImage.Convert<Gray, Byte>();
                        break;
                }

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

                //   grayimage = grayimage.PyrDown().PyrUp();

                if (ResultsInROI == OutputResultType.orPreProcessing) {
                    ImageOut.ROI = RoiRegion;
                    roiImage = new Image<Bgr, byte>(grayimage.ToBitmap());
                    roiImage.CopyTo(ImageOut);
                }

                Gray outavg;
                MCvScalar outstd;

                Double[] MinValues,MaxValues;
                Point [] MinLoc,MaxLoc;
                grayimage.AvgSdv(out outavg, out outstd);
                
                
                grayimage.MinMax(out MinValues, out MaxValues, out MinLoc, out MaxLoc);

                
                PixelAverage = Math.Round(outavg.Intensity, 4);
                PixelStdDv = Math.Round(outstd.v1, 4);

             /*   var result = from c in MinValues
                             where c > 5
                             select c;


                //CvInvoke.cvCalcHist(grayimage,
                CvInvoke.cvGet2D(grayimage, 0, 0);
                PixelMinValue = grayimage.get;
                */


             
            } catch (Exception exp) {
                Pass = false;
                log.Error(exp);
            }
            return base.Process(ImageIn, ImageOut, RoiRegion);

        }

    }
}
