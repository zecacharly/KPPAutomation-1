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
using Accord.Statistics.Models.Regression.Linear;
using Accord.MachineLearning;
using Accord.Math;


namespace VisionModule {
    [ProcessingFunction("Line Fit", "Line")]
    public class ProcessingFunctionLines : ProcessingFunctionBase {

        public ProcessingFunctionLines() {
          
        }

        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionLines));

        private PointF _RectangleCenter = new PointF(0, 0);



        #region Pre-Processing


        //[XmlAttribute]
        //[Category("Pre-Processing"), Description("TODO")]
        //public Double Resolution { get; set; }


        #endregion



        #region Post Processing




        //[XmlIgnore]
        //[Category("Post-Processing"), Description("Num lines"), DisplayName("Number of Lines"), ReadOnly(true), RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        //[UseInRef(true)]
        //public int NumLines {
        //    get;

        //    set;
        //}




        #endregion

        public override bool Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {

                base.Process(ImageIn, ImageOut, RoiRegion);
                
                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(RoiRegion.Size);
                ImageIn.ROI = RoiRegion;
                ImageOut.ROI = RoiRegion;
                ImageIn.CopyTo(roiImage);
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
                        CvInvoke.cvCvtColor(roiImage, grayimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                        break;
                    default:
                        CvInvoke.cvCvtColor(roiImage, grayimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                        break;
                }






                grayimage.ROI = Rectangle.Empty;







                if (base.ImagePreProc1.UseChannel != Channel.Bgr) {
                    switch (base.ImagePreProc1.ThresholdType) {
                        case TypeOfThreshold.Normal:
                            grayimage = grayimage.ThresholdBinary(new Gray(base.ImagePreProc1.Threshold), new Gray(base.ImagePreProc1.Threshold));

                            break;
                        case TypeOfThreshold.Inverted:
                            grayimage = grayimage.ThresholdBinaryInv(new Gray(base.ImagePreProc1.Threshold), new Gray(base.ImagePreProc1.Threshold));

                            break;
                        default:
                            break;
                    }
                }

                using (MemStorage storage = new MemStorage()) {

                    List<Contour<Point>> largestContours = new List<Contour<Point>>();

                    for (Contour<Point> contours = grayimage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                                  Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                                  contours != null;
                                  contours = contours.HNext) {
                        Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);

                        Boolean OpenedContour = false;

                        MemStorage stor = new MemStorage();

                        Seq<Point> pts = new Seq<Point>(stor);

                        foreach (Point item in currentContour) {
                            if (item.X < 2 || item.Y < 2 || item.X >= RoiRegion.Width - 2 || item.Y >= RoiRegion.Height - 2) {
                                OpenedContour = true;
                            }
                            else {
                                pts.Push(item);
                            }
                        }

                        //Ransac

                        if (OpenedContour) {
                            if (pts.Count() < 5) {
                                continue;
                            }


                            if (ResultsInROI == OutputResultType.orContours) {
                                ImageOut.DrawPolyline(pts.ToArray(), false, new Bgr(Color.Yellow), 1);
                            }





                            // Gather the available data



                            // First, fit simple linear regression directly for comparison reasons.
                            //double[] x = pts.OrderBy(p => p.X).OrderBy(p => p.Y).Select(p => (double)p.X).ToArray(); // Extract the independent variable
                            //double[] y = pts.OrderBy(p => p.X).OrderBy(p => p.Y).Select(p => (double)p.Y).ToArray(); // Extract the dependent variable

                            double[] x = pts.Select(p => (double)p.X).ToArray(); // Extract the independent variable
                            double[] y = pts.Select(p => (double)p.Y).ToArray(); // Extract the dependent variable


                            double[][] data = new[] { x, y };

                            // Create a simple linear regression
                            SimpleLinearRegression slr = new SimpleLinearRegression();
                            slr.Regress(x, y);

                            // Compute the simple linear regression output
                            double[] slrY = slr.Compute(x);


                            // Now, fit simple linear regression using RANSAC
                            int maxTrials = (int)1000;
                            int minSamples = (int)20;
                            double probability = (double)0.950;
                            double errorThreshold = (double)1;
                             
                            
                            // Create a RANSAC algorithm to fit a simple linear regression
                            var ransac = new RANSAC<SimpleLinearRegression>(minSamples);
                            ransac.Probability = probability;
                            ransac.Threshold = errorThreshold;
                            ransac.MaxEvaluations = maxTrials;

                            // Set the RANSAC functions to evaluate and test the model

                            ransac.Fitting = // Define a fitting function
                                delegate(int[] sample) {
                                    // Retrieve the training data
                                    double[] inputs = x.Submatrix(sample);
                                    double[] outputs = y.Submatrix(sample);

                                    // Build a Simple Linear Regression model
                                    var r = new SimpleLinearRegression();
                                    r.Regress(inputs, outputs);
                                    return r;
                                };

                            ransac.Degenerate = // Define a check for degenerate samples
                                delegate(int[] sample) {
                                    // In this case, we will not be performing such checks.
                                    return false;
                                };

                            ransac.Distances = // Define a inlier detector function
                                delegate(SimpleLinearRegression r, double threshold) {
                                    List<int> inliers = new List<int>();
                                    for (int i = 0; i < x.Length; i++) {
                                        // Compute error for each point
                                        double error = r.Compute(x[i]) - y[i];

                                        // If the squared error is below the given threshold,
                                        //  the point is considered to be an inlier.
                                        if (error * error < threshold)
                                            inliers.Add(i);
                                    }
                                    return inliers.ToArray();
                                };


                            // Finally, try to fit the regression model using RANSAC
                            int[] idx;
                            SimpleLinearRegression rlr = ransac.Compute(x.Length, out idx);

                            // Check if RANSAC was able to build a consistent model
                            if (rlr == null) {
                                return false; // RANSAC was unsucessful, just return.
                            }
                            else {
                                // Compute the output of the model fitted by RANSAC
                                double[] rlrY = rlr.Compute(x);

                                // Create scatterplot comparing the outputs from the standard
                                //  linear regression and the RANSAC-fitted linear regression.
                                // CreateScatterplot(graphInput, x, y, slrY, rlrY, x.Submatrix(idx), y.Submatrix(idx));



                                Point pt1 = pts.OrderBy(p => p.X).OrderBy(p => p.Y).ToArray()[0];
                                Point pt2 = pts.OrderBy(p => p.X).OrderBy(p => p.Y).ToArray()[pts.Count() - 1];

                                //ImageOut.Draw(new Cross2DF(new PointF(pt1.X,pt1.Y),5,5),new Bgr(Color.Red),1);
                                //ImageOut.Draw(new Cross2DF(new PointF(pt2.X, pt2.Y), 5, 5), new Bgr(Color.Red), 1);

                                Point pt11 = new Point(pt1.X, (int)(rlr.Slope * pt1.X + rlr.Intercept));
                                Point pt22 = new Point(pt2.X, (int)(rlr.Slope * pt2.X + rlr.Intercept));


                                LineSegment2DF linefitted = new LineSegment2DF(pt11, pt22);

                                if (ResultsInROI == OutputResultType.orResults) {
                                    ImageOut.Draw(linefitted, new Bgr(Color.Green), 1);
                                }
                            }


                        }
                    }
                }
                //grayimage = grayimage.SmoothBlur(3, 3, false);



                switch (ResultsInROI) {
                    case OutputResultType.orContours:

                        break;
                    case OutputResultType.orResults:
                        break;
                    case OutputResultType.orNone:
                        break;
                    case OutputResultType.orPreProcessing:
                        ImageOut.ROI = RoiRegion;
                        // roiImage = new Image<Bgr, byte>(cannyImage.ToBitmap());
                        //roiImage.CopyTo(ImageOut);
                        break;
                    default:
                        break;
                }


                roiImage.Dispose();
                grayimage.Dispose();


            } catch (Exception exp) {

                log.Error(exp);

                return false;
            }



            return true;


        }


    }


}
