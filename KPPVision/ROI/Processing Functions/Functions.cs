using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace VisionModule {

    public class PixelAnalisys {

        internal static void Process(int PixelCountVal,Contour<Point> contourmask, Image<Gray, Byte> procImage, out Double PixelAverage, out Double PixelDeviation, out int PixelCount) {

            #region Pixel information inside contour

            //Image<Gray, byte> Mask = new Image<Gray, byte>(procImage.Size);
            //procImage.CopyTo(RoiGray);


            //Rectangle box = contourmask.BoundingRectangle;

            Image<Gray, Byte> InContourIMG = new Image<Gray, byte>(procImage.Size);

            //RoiGray.ROI = box;

            


            //set the value of pixels not in the contour region to zero
            using (Image<Gray, Byte> mask = new Image<Gray, byte>(procImage.Size)) {
                mask.Draw(contourmask, new Gray(255), new Gray(255), 0, -1);
               //  mask.Save("mask.bmp");
                //double mean = CvInvoke.cvAvg(InContourIMG, mask).v0;

                MCvScalar Mean = new MCvScalar();
                MCvScalar StdAvg = new MCvScalar();
                double Minval = 0, Maxval = 0;

                Point MinValpt = new Point();
                Point MaxValpt = new Point();

                CvInvoke.cvCopy(procImage.Ptr, InContourIMG.Ptr, mask);

                CvInvoke.cvMinMaxLoc(InContourIMG, ref Minval, ref Maxval, ref MinValpt, ref MaxValpt, mask);
                CvInvoke.cvAvgSdv(InContourIMG, ref Mean, ref StdAvg, mask);

                PixelAverage = Math.Round(Mean.v0, 3);
                PixelDeviation = Math.Round(StdAvg.v0, 3);
                //  PixelMinVal = Minval;
                // PixelMaxVal = Maxval;

                
                
               // InContourIMG.Save("incontourimg1.bmp");
                InContourIMG._ThresholdBinary(new Gray(Math.Abs(PixelCountVal)), new Gray(255));
               // InContourIMG.Save("incontourimg2.bmp");
                //InContourIMG.Save("e:\\incontourimg2.bmp");

                PixelCount = CvInvoke.cvCountNonZero(InContourIMG);
            }
            #endregion

        }

        public PixelAnalisys() { 
        
        }
    }

    public class KPPMath {

        internal static void FindCircle(Point a, Point b, Point c, out Point center, out float radius) {
            FindCircle(new PointF((float)a.X, (float)a.Y), new PointF((float)b.X, (float)b.Y), new PointF((float)c.X, (float)c.Y), out center, out radius);
        }

        // Find a circle through the three points.
        internal static void FindCircle(PointF a, PointF b, PointF c, out Point center, out float radius) {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            float x1 = (b.X + a.X) / 2;
            float y1 = (b.Y + a.Y) / 2;
            float dy1 = b.X - a.X;
            float dx1 = -(b.Y - a.Y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            float x2 = (c.X + b.X) / 2;
            float y2 = (c.Y + b.Y) / 2;
            float dy2 = c.X - b.X;
            float dx2 = -(c.Y - b.Y);

            // See where the lines intersect.
            float cx = (y1 * dx1 * dx2 + x2 * dx1 * dy2 - x1 * dy1 * dx2 - y2 * dx1 * dx2)
                / (dx1 * dy2 - dy1 * dx2);
            float cy = (cx - x1) * dy1 / dx1 + y1;
            center = new Point((int)cx, (int)cy);

            float dx = cx - a.X;
            float dy = cy - a.Y;
            radius = (float)Math.Sqrt(dx * dx + dy * dy);
        }

    }

}
