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
    [ProcessingFunction("Shape Recognition")]
    [Serializable()]
    public class ProcessingFunctionShapes : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionShapes));
        
        public enum PolygonsTypes {ptRectangle,ptTriangle};

        private PointF _RectangleCenter = new PointF(0, 0);

        [XmlAttribute]
        [Category("Pre-processing"), Description("Minimum length to consider a valid contour")]
        public Double MinContourLength { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("Type of poygons to find")]
        public PolygonsTypes PolygonsType { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("Reference Point X")]
        public Double RefX { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("Reference Point Y")]
        public Double RefY { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("Reference Point Y")]
        public Double RefAngle { get; set; }


        [XmlAttribute]
        [Category("Pre-processing"), Description("Remove contours that are touching the ROI edges")]
        public Boolean RemoveTouchingROIEdges { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("minimum contour lenght")]
        public int MinimumArea { get; set; }

        [XmlAttribute]
        [Category("Pre-processing"), Description("minimum contour lenght")]
        public int MaximumArea { get; set; }        
        

        [XmlAttribute]
        [Category("Pre-processing"), Description("Number of poly points")]
        public int PolygnAcuray { get; set; } 

        [XmlIgnore]
        [Category("Post-processing"), Description("Number of rectangles found"), ReadOnly(true)]
        public int NumPolygons { get; set; }


        [XmlIgnore]
        [Category("Post-processing"), Description("Area of the rectangle"), ReadOnly(true)]
        public Double Area { get; set; }

        [XmlIgnore]
        [Category("Post-processing"), Description("Angle of the rectangle"), ReadOnly(true)]
        public Double Angle { get; set; }

        [XmlIgnore]
        [Category("Post-processing"), Description("Offset Point X"), ReadOnly(true)]
        public Double OffSetX { get; set; }
        [XmlIgnore]
        [Category("Post-processing"), Description("Offset Point Y"), ReadOnly(true)]
        public Double OffSetY { get; set; }

        [XmlIgnore]
        [Category("Post-processing"), Description("Offset Point Y"), ReadOnly(true)]
        public Double OffSetAngle { get; set; }


        //[XmlIgnore]
        //[Category("Post-processing"), Description("Ratio from points int the contour that are in the polygon "), ReadOnly(true)]
        //public Double PolygonFitRatio { get; set; }






        public override void Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, ShapeRectangle _procShape) {
            try {

                Area = -1;
                
                NumPolygons= 0;




                Image<Bgr, byte> roiImage = new Image<Bgr, byte>(_procShape.ShapeEfectiveBounds.Size);
                ImageIn.ROI = _procShape.ShapeEfectiveBounds;
                ImageIn.CopyTo(roiImage);
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(_procShape.ShapeEfectiveBounds.Size);


                switch (UseChannel) {
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

                
                if (UseChannel !=  Channel.Bgr) {
                    switch (ThresholdType) {
                        case TypeOfThreshold.Normal:
                         //   grayimage = grayimage.ThresholdBinary(new Gray(Threshold), new Gray(255));
                            
                            break;
                        case TypeOfThreshold.Inverted:
                         //   grayimage = grayimage.ThresholdBinaryInv(new Gray(Threshold), new Gray(255));
                            
                            break;
                        default:
                            break;
                    }

                    /*
                    grayimage = grayimage.Erode(1);
                    grayimage = grayimage.Dilate(1);
                    */


                    Image<Gray, Byte> cannyEdges = grayimage.Canny(new Gray(Threshold), new Gray(120));

                    if (ResultsInROI == OutputResultType.orPreProcessing) {
                        
                        roiImage.Bitmap =grayimage.ToBitmap();
                        roiImage.CopyTo(inImage);
                    }

                    

                    List<Contour<Point>> largestContours = new List<Contour<Point>>();
                    MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 0.4d, 0.4d);

                    using (MemStorage storage = new MemStorage()) {
                        for (Contour<Point> contours = cannyEdges.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                               Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                               contours != null;
                               contours = contours.HNext) {
                            Contour<Point> currentContour = contours;//. ApproxPoly(contours.Perimeter * 0.05, storage);

                            if (currentContour.Perimeter > MinContourLength) {


                                Boolean isOnEdge = false;

                                if (RemoveTouchingROIEdges) {


                                    if (currentContour.BoundingRectangle.Left == 1 || currentContour.BoundingRectangle.Top == 1)
                                        isOnEdge = true;

                                    if (currentContour.BoundingRectangle.Right == roiImage.Width - 1 || currentContour.BoundingRectangle.Bottom == roiImage.Height - 1)
                                        isOnEdge = true;

                                }

                                if (isOnEdge == false) {

                                    // faz aproximação dos contornos a um poligono
                                    if (ResultsInROI == OutputResultType.orContours)
                                        inImage.Draw(currentContour, new Bgr(Color.Yellow), 1);

                                    

                                    Contour<Point> polycontour = currentContour.ApproxPoly(currentContour.Perimeter * 0.01 * PolygnAcuray, storage);
                                   
                                    int numpolypoints = 0;
                                    switch (PolygonsType) {
                                        case PolygonsTypes.ptRectangle:
                                            numpolypoints = 4;
                                            break;
                                        case PolygonsTypes.ptTriangle:
                                            numpolypoints = 3;
                                            break;
                                        default:
                                            break;
                                    }

                                    if ((polycontour.Area > MinimumArea) && (polycontour.Area < MaximumArea) && (numpolypoints == polycontour.Total)) {
                                        largestContours.Add(polycontour);
                                        Area = polycontour.Area;
                                        //se o poligono tiver 4 pontos => rectangulo


                                        #region determine if all the angles in the contour are within [80, 100] degree

                                       

                                        //MCvFont font= new MCvFont();

                                        /*
                                        if (EdgeIndex>-1) {
                                            Angle = Math.Round(180-Math.Abs(edges[EdgeIndex].GetExteriorAngleDegree(RefEdge)),3);
                                            if (ResultsInROI != OutputResultType.orNone) {
                                                inImage.Draw(edges[EdgeIndex], new Bgr(Color.Yellow), 2);

                                                inImage.Draw(new LineSegment2D(edges[EdgeIndex].P2, new Point(edges[EdgeIndex].P2.X + 30, edges[EdgeIndex].P2.Y)), new Bgr(Color.Green), 2);
                                                inImage.Draw(new LineSegment2D(edges[EdgeIndex].P2, new Point(edges[EdgeIndex].P2.X, edges[EdgeIndex].P2.Y - 30)), new Bgr(Color.Green), 2);

                                                inImage.ROI = Rectangle.Empty;
                                                //inImage.Draw("Angle:" + Math.Round(Angle, 2).ToString(), ref font, new Point(edges[EdgeIndex].P2.X, edges[EdgeIndex].P2.Y + 30), new Bgr(Color.Green));

                                               // inImage.Draw("Angle:" + Math.Round(Angle, 2).ToString(), ref font, OriginPointConvert(new Point(edges[EdgeIndex].P2.X, edges[EdgeIndex].P2.Y + 15), _procShape), new Bgr(Color.Green));
                                                inImage.ROI = _procShape.ShapeEfectiveBounds;
                                            }

                                        }
                                        */
                                        #endregion

                                        // desenha os poligonos
                                        if (ResultsInROI == OutputResultType.orResults)
                                            inImage.Draw(polycontour, new Bgr(Color.Green), 1);
                                        
                                        Point[] pts = polycontour.ToArray();
                                        LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                        foreach (LineSegment2D item in edges) {
                                            if (ResultsInROI != OutputResultType.orNone) {
                                                inImage.Draw(new CircleF(item.P1, 1), new Bgr(Color.Blue), 1);
                                                //                                                inImage.Draw(new CircleF(item.P1, 1), new Bgr(Color.Blue), 1);
                                            }
                                        }

                                        float factor=0.3846F;

                                        /*

                                        int midX =(int) Math.Round((float)polycontour.BoundingRectangle.X + polycontour.BoundingRectangle.Width/2,0);
                                        int midY =(int) Math.Round((float)polycontour.BoundingRectangle.Y + polycontour.BoundingRectangle.Height / 2,0);


                                        double PosX = Math.Round(factor * OriginPointConvert(midX, _procShape).X, 3);
                                        double PosY = Math.Round(factor * OriginPointConvert(midY, _procShape).Y, 3);

                                        
                                        Double NewOffSetX, NewOffSetY, NewOffSetAngle;
                                        
                                        NewOffSetX=Math.Round(PosX - RefX, 0);
                                        NewOffSetY=Math.Round(RefY-PosY, 0);
                                        NewOffSetAngle=Math.Round(Angle - RefAngle, 0);

                                        if(Math.Abs(NewOffSetX-OffSetX)>=1){
                                            OffSetX = NewOffSetX;
                                        }

                                        if (Math.Abs(NewOffSetY - OffSetY) >=1) {
                                            OffSetY = NewOffSetY;
                                        }
                                        
                                        if (Math.Abs(NewOffSetAngle - OffSetAngle) >=1) {
                                            OffSetAngle = NewOffSetAngle;
                                        }
                                         
                                        if (ResultsInROI == OutputResultType.orResults)
                                            inImage.Draw(new Cross2DF(new PointF(midX, midY), 10, 10), new Bgr(Color.Red), 1);

                                        
                                        if (ResultsInROI == OutputResultType.orResults) {
                                            inImage.ROI = Rectangle.Empty;
                                            inImage.Draw("X:" + PosX.ToString(),
                                                            ref font,
                                                            OriginPointConvert(new Point(midX, midY + 15), _procShape),
                                                            new Bgr(Color.Green));
                                            inImage.Draw("Y:" + PosY.ToString(),
                                                            ref font,
                                                            OriginPointConvert(new Point(midX, midY + 30), _procShape),
                                                            new Bgr(Color.Green));

                                            inImage.Draw("Offset >>" + " X:" + OffSetX.ToString() + " Y:" + OffSetY.ToString() + " Angle:" + OffSetAngle.ToString(), ref font, new Point(_procShape.ShapeEfectiveBounds.Location.X + 5, _procShape.ShapeEfectiveBounds.Location.Y + 20), new Bgr(Color.Red));
                                            inImage.ROI = _procShape.ShapeEfectiveBounds;
                                        }
                                        */
                                        NumPolygons += 1;


                                    }
                                }
                            }
                        }

                        


                        largestContours.Clear();


                        switch (ResultsInROI) {
                            case OutputResultType.orContours:
                                ROIBitmapOut = roiImage.ToBitmap();
                                break;
                            case OutputResultType.orNone:
                                ROIBitmapOut = roiImage.ToBitmap();
                                break;
                            case OutputResultType.orPreProcessing:
                               /* inImage.ROI = _procShape.ShapeEfectiveBounds;
                                roiImage = new Image<Bgr, byte>(grayimage.ToBitmap());
                                roiImage.CopyTo(inImage);*/
                                ROIBitmapOut = grayimage.ToBitmap();
                                break;
                            default:                                
                                ROIBitmapOut = roiImage.ToBitmap();
                                break;
                        }

                        

                    }

                }
            } catch (Exception exp) {

                log.Error( exp);
            }


            //if ((RectangleFound) && (NumRectangles >= MinNumRectangles) && (NumRectangles <= MaxNumRectangles)) _pass = true;
            base.Process(inImage, _procShape);


        }


    }


}
