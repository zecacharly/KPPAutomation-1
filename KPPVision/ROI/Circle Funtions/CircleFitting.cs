using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.MachineLearning;
using AForge;
using Accord.Math;
using System.Drawing;
using System.ComponentModel;

namespace VisionModule{

    [TypeConverter(typeof(Customconverter<CircleFitted>))]
    public class CircleFitted {

        private System.Drawing.Point[] _Inliers;
        [Browsable(false)]
        public System.Drawing.Point[] Inliers {
            get {                
                return _Inliers; 
            
            }
            protected set { _Inliers = value; }
        }

        private Boolean _CircleFound = false;
        [Browsable(false)]
        public Boolean CircleFound {
            get { return _CircleFound; }
            protected set { _CircleFound = value; }
        }

        private Double _Fitting = -1;
        public Double Fitting {
            get { return _Fitting; }
            protected set { _Fitting = value; }
        }

        private Double _CircleRadius = -1;
        [DisplayName("Radius")]
        public Double CircleRadius {
            get { return _CircleRadius; }
            protected set { _CircleRadius = value; }
        }

        private System.Drawing.Point _CircleCenter = System.Drawing.Point.Empty;
        [DisplayName("Center")]
        public System.Drawing.Point CircleCenter {
            get { return _CircleCenter; }
            protected set { _CircleCenter = value; }
        }

        public virtual void Compute<T>(T[] pts, Double acceptanceLevel) {

        }

        public CircleFitted() { 
        
        }
    }



    public class RansacCircle : CircleFitted {

        private RANSAC<EstimatedCircle> ransac;
        private int[] inliers;
        
        //public AForge.Point[] InliersPoints {
        //    get { 
        //        return points.Submatrix(inliers); 
        //    }
            
        //}

        private AForge.Point[] points;
        private double[] d2;
        [Browsable(false)]
        public RANSAC<EstimatedCircle> Ransac { get { return this.ransac; } }
       
        

        public RansacCircle(double threshold, double probability) {
            ransac = new RANSAC<EstimatedCircle>(3, threshold,probability);
            ransac.Fitting = definecircle;
            ransac.Distances = distance;
            
            ransac.Degenerate = degenerate;
            ransac.MaxEvaluations = 5000;
            
           
        }



        public override void Compute<T>(T[] pts, Double acceptanceLevel) {
            if (typeof(T) == typeof(PointF)) {
                PointF[] ptsf = pts as PointF[];
                points = Array.ConvertAll<PointF, AForge.Point>(ptsf, p => new AForge.Point(p.X, p.Y));
            } else if (typeof(T) == typeof(System.Drawing.Point)) {
                System.Drawing.Point[] ptsf = pts as System.Drawing.Point[];
                points = Array.ConvertAll<System.Drawing.Point, AForge.Point>(ptsf, p => new AForge.Point(p.X, p.Y));
            } else {
                return;
            }

            Estimate(acceptanceLevel);
        }


        //private EstimatedCircle Estimate(IEnumerable<IntPoint> points) {
        //    return Estimate(points.Select(p => new AForge.Point(p.X, p.Y)).ToArray());
        //}

        //private EstimatedCircle Estimate(IEnumerable<AForge.Point> points) {
        //    return Estimate(points.ToArray());
        //}

        //private EstimatedCircle Estimate(IntPoint[] points) {
        //    return Estimate(points.Apply(p => new AForge.Point(p.X, p.Y)));
        //}

        private EstimatedCircle Estimate(Double acceptanceLevel) {
            if (points.Length < 3)
                throw new ArgumentException("At least three points are required to fit a circle");

            this.d2 = new double[points.Length];
            //this.points = points;
            base.CircleFound = false;
            Double fitted = 0;
            double lastfit = fitted;

         
            ransac.Compute(points.Length, out inliers);

            base.Inliers = Array.ConvertAll<AForge.Point, System.Drawing.Point>(points.Submatrix(inliers), p => new System.Drawing.Point((int)p.X, (int)p.Y));
            EstimatedCircle circle = null;
            circle = fitting(points.Submatrix(inliers));

            int totalpoints = points.Count();
            int inlierspoints = Inliers.Count();


            fitted = (Double)inlierspoints / (Double)totalpoints;


            if (fitted>=acceptanceLevel) {
                base.CircleFound = true;    
            } else {
                base.CircleFound = false;
            }

            base.CircleCenter = new System.Drawing.Point((int)(circle.Origin.X), (int)(circle.Origin.Y));
            base.CircleRadius = circle.Radius;
            base.Fitting = Math.Round(fitted * 100, 3);
            //if (fitted < 0.4) {
            //    return null;
            //}

            return circle;

        }

        public override string ToString() {
            return "Ransac Fitted Circle";
        }

        private EstimatedCircle definecircle(int[] x) {


            return FitCircle(points.Submatrix(x));
            //return new EstimatedCircle(points[x[0]], points[x[1]], points[x[2]]);
        }

        private int[] distance(EstimatedCircle c, double t) {

            d2 = new double[points.Length];

            for (int i = 0; i < points.Length; i++)
                d2[i] = c.DistanceToPoint(points[i]);

            int[] teste = Matrix.Find(d2, z => z < t);
            return teste;

        }

        private bool degenerate(int[] indices) {
           // System.Diagnostics.Debug.Assert(indices.Length == 3);
            AForge.Point p1 = points[indices[0]];
            AForge.Point p2 = points[indices[1]];
            AForge.Point p3 = points[indices[2]];
            Boolean dif = (p1 == p2 || p2 == p3 || p1 == p3);
            AForge.Point[] pts = points.Submatrix(indices);
            Boolean different = pts.Distinct<AForge.Point>().Count() == pts.Count();

            return !different;

          
            

            return p1 == p2 || p2 == p3 || p1 == p3;
        }

        private static EstimatedCircle FitCircle(AForge.Point[] points) {
            double[,] A = new double[points.Count(), 3];
            double[,] Y = new double[points.Count(), 1];

            // setup the matrices
            for (int i = 0; i < points.Count(); ++i) {
                // we solve for [ 2*c1 2*c2 c3 ] here,
                // avoid doing 2*xn / 2*yn in the loop.
                A[i, 0] = points[i].X;
                A[i, 1] = points[i].Y;
                A[i, 2] = 1;
                Y[i, 0] = points[i].X * points[i].X + points[i].Y * points[i].Y;
            }

            // get AT * A and AT * Y
            double[,] AT = A.Transpose();
            double[,] B = AT.Multiply(A);
            double[,] Z = AT.Multiply(Y);

            // solve for c
            double[,] c = Matrix.Solve(B, Z, true);

            // okay now we get the circle :-D
            double x = c[0, 0] * 0.5;
            double y = c[1, 0] * 0.5;
            double r = System.Math.Sqrt(c[2, 0] + x * x + y * y);

            return new EstimatedCircle((float)x, (float)y, r);
        }

        static EstimatedCircle fitting(AForge.Point[] points) {
            if (points.Length == 3)
                return new EstimatedCircle(points[0], points[1], points[2]);

 
            return FitCircle(points);
            //return new EstimatedCircle(new AForge.Point(fitter.getCenter().X, fitter.getCenter().Y), (float)fitter.getRadius());
        }
    }

    public class EstimatedCircle {
        public double Area { get { return Radius * Radius * System.Math.PI; } }
        public double Radius { get; set; }
        public AForge.Point Origin { get; set; }

        public EstimatedCircle() {
            Origin = new AForge.Point(0, 0);
            Radius = 0;
        }

        public EstimatedCircle(float x, float y, double radius) {
            Origin = new AForge.Point(x, y);
            Radius = radius;
        }

        public EstimatedCircle(AForge.Point origin, double radius) {
            Origin = origin;
            Radius = radius;
        }

        public EstimatedCircle(AForge.Point p1, AForge.Point p2, AForge.Point p3) {
            // ya = ma * (x - x1) + y1
            // yb = mb * (x - x2) + y2
            //
            // ma = (y2 - y1) / (x2 - x1)
            // mb = (y3 - y2) / (x3 - x2)
            double ma = (p2.Y - p1.Y) / (p2.X - p1.X);
            double mb = (p3.Y - p2.Y) / (p3.X - p2.X);

            //       (ma * mb * (y1 - y3) + mb * (x1 + x2) - ma * (x2 + x3)
            // x = ----------------------------------------------------------
            //                          2 * (mb - ma)
            double x = (ma * mb * (p1.Y - p3.Y) + mb * (p1.X + p2.Y) - ma * (p2.X + p3.X)) / (2 * (mb - ma));
            double y = ma * (x - p1.X) + p1.Y;

            Origin = new AForge.Point((float)x, (float)y);
            Radius = Origin.DistanceTo(p1);
        }

        public double DistanceToPoint(AForge.Point point) {
            return System.Math.Abs(Origin.DistanceTo(point) - Radius);
        }
    }



 //   public class CircleFit {

 //       /** Circular ring sample points. */
 //       private PointF[] points;

 //       /** Current circle center. */
 //       private PointF center;
 //       /** Current circle radius. */
 //       private double rHat;

 //       /** Current cost function value. */
 //       private double J;

 //       /** Current cost function gradient. */
 //       private double dJdx;
 //       private double dJdy;





 //       /** Initialize an approximate circle based on all triplets.
 //  * @param points circular ring sample points
 //  * @exception LocalException if all points are aligned
 //  */

 //       public void initialize(PointF[] points) {

 //           // store the points array
 //           this.points = points;

          


 //               // analyze all possible points triplets
 //               center.X = 0.0f;
 //               center.Y = 0.0f;
 //               int n = 0;
 //               for (int i = 0; i < (points.Count() - 2); ++i) {
 //                   PointF p1 = points[i];
 //                   for (int j = i + 1; j < (points.Count() - 1); ++j) {
 //                       PointF p2 = points[j];
 //                       for (int k = j + 1; k < points.Count(); ++k) {
 //                           PointF p3 = points[k];

 //                           // compute the triangle circumcenter
 //                           PointF cc = circumcenter(p1, p2, p3);
 //                           if (cc != null) {
 //                               // the points are not aligned, we have a circumcenter
 //                               ++n;
 //                               center.X += cc.X;
 //                               center.Y += cc.Y;
 //                           }
 //                       }
 //                   }
 //               }

 //               if (n == 0) {
 //                   //throw new LocalException("all points are aligned");
 //               }

 //               // initialize using the circumcenters average
 //               center.X /= n;
 //               center.Y /= n;
           
 //           updateRadius();

 //       }

 //       /** Update the circle radius.
 //   */
 //       private void updateRadius() {
 //           rHat = 0;
 //           for (int i = 0; i < points.Count(); ++i) {
 //               double dx = points[i].X - center.X;
 //               double dy = points[i].Y - center.Y;
 //               rHat += Math.Sqrt(dx * dx + dy * dy);
 //           }
 //           rHat /= points.Count();
 //       }


 //       /** Compute the circumcenter of three points.
 //     * @param pI first point
 //     * @param pJ second point
 //     * @param pK third point
 //     * @return circumcenter of pI, pJ and pK or null if the points are aligned
 //     */
 //       private PointF circumcenter(PointF pI, PointF pJ, PointF pK) {

 //           // some temporary variables
 //           PointF dIJ = new PointF(pJ.X - pI.X, pJ.Y - pI.Y);
 //           PointF dJK = new PointF(pK.X - pJ.X, pK.Y - pJ.Y);
 //           PointF dKI = new PointF(pI.X - pK.X, pI.Y - pK.Y);
 //           double sqI = pI.X * pI.X + pI.Y * pI.Y;
 //           double sqJ = pJ.X * pJ.X + pJ.Y * pJ.Y;
 //           double sqK = pK.X * pK.X + pK.Y * pK.Y;

 //           // determinant of the linear system: 0 for aligned points
 //           double det = dJK.X * dIJ.Y - dIJ.X * dJK.Y;
 //           if (Math.Abs(det) < 1.0e-10) {
 //               // points are almost aligned, we cannot compute the circumcenter
 //               return PointF.Empty;
 //           }

 //           // beware, there is a minus sign on Y coordinate!
 //           return new PointF((float)((sqI * dJK.Y + sqJ * dKI.Y + sqK * dIJ.Y) / (2 * det)), (float)(-(sqI * dJK.X + sqJ * dKI.X + sqK * dIJ.X) / (2 * det)));

 //       }


 //       /** Minimize the distance residuals between the points and the circle.
 // * <p>We use a non-linear conjugate gradient method with the Polak and
 // * Ribiere coefficient for the computation of the search direction. The
 // * inner minimization along the search direction is performed using a
 // * few Newton steps. It is worthless to spend too much time on this inner
 // * minimization, so the convergence threshold can be rather large.</p>
 // * @param maxIter maximal iterations number on the inner loop (cumulated
 // * across outer loop iterations)
 // * @param innerThreshold inner loop threshold, as a relative difference on
 // * the cost function value between the two last iterations
 // * @param outerThreshold outer loop threshold, as a relative difference on
 // * the cost function value between the two last iterations
 // * @return number of inner loop iterations performed (cumulated
 // * across outer loop iterations)
 // * @exception LocalException if we come accross a singularity or if
 // * we exceed the maximal number of iterations
 // */
 //       public int minimize(int iterMax, double innerThreshold, double outerThreshold) {

 //           computeCost();
 //           if ((J < 1.0e-10) || (Math.Sqrt(dJdx * dJdx + dJdy * dJdy) < 1.0e-10)) {
 //               // we consider we are already at a local minimum
 //               return 0;
 //           }

 //           double previousJ = J;
 //           double previousU = 0.0, previousV = 0.0;
 //           double previousDJdx = 0.0, previousDJdy = 0.0;
 //           for (int iterations = 0; iterations < iterMax; ) {

 //               // search direction
 //               double u = -dJdx;
 //               double v = -dJdy;
 //               if (iterations != 0) {
 //                   // Polak-Ribiere coefficient
 //                   double beta =
 //                     (dJdx * (dJdx - previousDJdx) + dJdy * (dJdy - previousDJdy))
 //                   / (previousDJdx * previousDJdx + previousDJdy * previousDJdy);
 //                   u += beta * previousU;
 //                   v += beta * previousV;
 //               }
 //               previousDJdx = dJdx;
 //               previousDJdy = dJdy;
 //               previousU = u;
 //               previousV = v;

 //               // rough minimization along the search direction
 //               double innerJ;
 //               do {
 //                   innerJ = J;
 //                   double lambda = newtonStep(u, v);
 //                   center.X += (float)(lambda * u);
 //                   center.Y += (float)(lambda * v);
 //                   updateRadius();
 //                   computeCost();
 //               } while ((++iterations < iterMax)
 //                        && ((Math.Abs(J - innerJ) / J) > innerThreshold));

 //               // global convergence test
 //               if ((Math.Abs(J - previousJ) / J) < outerThreshold) {
 //                   return iterations;
 //               }
 //               previousJ = J;

 //           }

 //           throw new Exception("unable to converge after "
 //                                    + iterMax + " iterations");

 //       }

 //       /** Compute the cost function and its gradient.
 //  * <p>The results are stored as instance attributes.</p>
 //  */
 //       private void computeCost() {
 //           J = 0;
 //           dJdx = 0;
 //           dJdy = 0;
 //           for (int i = 0; i < points.Count(); ++i) {
 //               double dx = points[i].X - center.X;
 //               double dy = points[i].Y - center.Y;
 //               double di = Math.Sqrt(dx * dx + dy * dy);
 //               if (di < 1.0e-10) {
 //                   throw new Exception("cost singularity:"
 //                                            + " point at the circle center");
 //               }
 //               double dr = di - rHat;
 //               double ratio = dr / di;
 //               J += dr * (di + rHat);
 //               dJdx += dx * ratio;
 //               dJdy += dy * ratio;
 //           }
 //           dJdx *= 2.0;
 //           dJdy *= 2.0;
 //       }

 //       /** Compute the length of the Newton step in the search direction.
 //        * @param u abscissa of the search direction
 //        * @param v ordinate of the search direction
 //        * @return value of the step along the search direction
 //        */
 //       private double newtonStep(double u, double v) {

 //           // compute the first and second derivatives of the cost
 //           // along the specified search direction
 //           double sum1 = 0, sum2 = 0, sumFac = 0, sumFac2R = 0;
 //           for (int i = 0; i < points.Count(); ++i) {
 //               double dx = center.X - points[i].X;
 //               double dy = center.Y - points[i].Y;
 //               double di = Math.Sqrt(dx * dx + dy * dy);
 //               double coeff1 = (dx * u + dy * v) / di;
 //               double coeff2 = di - rHat;
 //               sum1 += coeff1 * coeff2;
 //               sum2 += coeff2 / di;
 //               sumFac += coeff1;
 //               sumFac2R += coeff1 * coeff1 / di;
 //           }

 //           // step length attempting to nullify the first derivative
 //           return -sum1 / ((u * u + v * v) * sum2
 //                           - sumFac * sumFac / points.Count()
 //                           + rHat * sumFac2R);

 //       }

 //       /** Get the circle center.
 //* @return circle center
 //*/
 //       public PointF getCenter() {
 //           return center;
 //       }

 //       /** Get the circle radius.
 //        * @return circle radius
 //        */
 //       public double getRadius() {
 //           return rHat;
 //       }

 //       public CircleFit() {

 //           rHat = 1.0;

 //       }
 //   }

}
