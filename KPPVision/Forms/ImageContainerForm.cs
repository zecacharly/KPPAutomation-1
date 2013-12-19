using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using KPP.Controls.Winforms.ImageEditorObjs;

using KPP.Core.Debug;
using System.Diagnostics;
using DejaVu;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;

namespace VisionModule {
    internal partial class ImageContainerForm : DockContent {
        public ImageContainerForm() {
            switch (LanguageSettings.Language) {
                case LanguageName.Unk:
                    break;
                case LanguageName.PT:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");

                    break;
                case LanguageName.EN:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

                    break;
                default:
                    break;
            }

            InitializeComponent();
            __roicontainer.AutoSize = true;
        }

        private VisionProject _SelectedProject = null;

        internal VisionProject SelectedProject {
            get { return _SelectedProject; }
            set {
                _SelectedProject = value;

            }
        }



        private void __toolZoomIn_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                __roicontainer.ZoomIn();
            }
        }

        private void __toolZoomOut_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                __roicontainer.ZoomOut();
            }
        }

        private void __toolZoomFit_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                __roicontainer.ZoomFit();
            }
        }

        private void __toolZoomOriginal_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                __roicontainer.ZoomOriginal();
            }
        }

        private void __roicontainer_Click(object sender, EventArgs e) {

        }

        private void __toolStripSave_Click(object sender, EventArgs e) {

        }

        private void originalToolStripMenuItem_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                if (_saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    __roicontainer.BackgroundImage.Save(_saveFileDialog1.FileName);
                }
            }
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e) {
            if (__roicontainer.BackgroundImage != null) {
                if (_saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    MemoryStream ms = new MemoryStream();
                    __roicontainer.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);


                    switch (__roicontainer.BackgroundImage.PixelFormat) {

                        case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                            Image<Bgr, byte> _image = new Image<Bgr, byte>(__roicontainer.BackgroundImage.Size);
                            _image.Bitmap = (Bitmap)Image.FromStream(ms);
                            _image[0].Save(_saveFileDialog1.FileName);
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        private void OpentoolStrip_Click(object sender, EventArgs e) {


        }




        private void newImageFileToolStripMenuItem_Click(object sender, EventArgs e) {
            __openFileDialog.ShowDialog();
        }

        private void __roicontainer_KeyDown(object sender, KeyEventArgs e) {

        }



        private void histogramBox1_Load(object sender, EventArgs e) {

        }

        private void histogramBox1_Paint(object sender, PaintEventArgs e) {
            if (InvokeRequired) {

            } else {

            }
        }

        private void __roicontainer_BackgroundImageChanged(object sender, EventArgs e) {

            if (InvokeRequired) {
                BeginInvoke(new MethodInvoker(delegate { __roicontainer_BackgroundImageChanged(sender, e); }));
            } else {

                
                if (toolStripZoomIn.Enabled == false) {
                    if (__roicontainer.BackgroundImage != null) {
                        toolStripZoomIn.Enabled = true;
                        toolStripZoomOut.Enabled = true;
                        toolStripZoomAll.Enabled = true;
                        toolStripZoomFit.Enabled = true;
                        //SelectedProject.SelectedRequest.SelectedInspection.InspLayer.Visible = true;

                        switch (__roicontainer.BackgroundImage.PixelFormat) {

                            case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                                originalToolStripMenuItem.Enabled = true;
                                redChannelToolStripMenuItem.Enabled = true;
                                greenChannelToolStripMenuItem.Enabled = true;
                                blueChannelToolStripMenuItem.Enabled = true;
                                break;
                            case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                                originalToolStripMenuItem.Enabled = true;
                                redChannelToolStripMenuItem.Enabled = false;
                                greenChannelToolStripMenuItem.Enabled = false;
                                blueChannelToolStripMenuItem.Enabled = false;
                                break;
                            default:
                                originalToolStripMenuItem.Enabled = true;
                                break;
                        }
                    } else {
                        toolStripZoomIn.Enabled = false;
                        toolStripZoomOut.Enabled = false;
                        toolStripZoomAll.Enabled = false;
                        toolStripZoomFit.Enabled = false;

                        originalToolStripMenuItem.Enabled = false;
                        redChannelToolStripMenuItem.Enabled = false;
                        greenChannelToolStripMenuItem.Enabled = false;
                        blueChannelToolStripMenuItem.Enabled = false;
                        //SelectedProject.SelectedRequest.SelectedInspection.InspLayer.Visible = false;
                    }
                }
            }
        }


        public void DoLoadBackImage(Image<Bgr, Byte> cvimage, Boolean fit) {
            Bitmap _img = null;
            if (cvimage != null) {
                if (cvimage.Size!=Size.Empty ) {
                    _img = cvimage.ToBitmap(); 
                }
            }
            DoLoadBackImage(_img, fit);


        }

        public void DoLoadBackImage(Bitmap _img, Boolean fit) {
            Boolean doresize = false;
            if (_img != null) {
                if (_img.Size != __roicontainer.ImageSize) {
                    doresize = true;
                }
            }
            if (__roicontainer.BackgroundImage == null && _img!=null) {
                fit = true;
            }
            __roicontainer.LoadBackgroundImage(_img, resize: doresize);
            //__roicontainer.Layers[0].im
            if (fit) {
                    __roicontainer.ZoomFit();
            }
        }

        

        private static KPPLogger log = new KPPLogger(typeof(ImageContainerForm));

        private void __roicontainer_OnSelectedShapesChanged(object sender, KPP.Controls.Winforms.SelectedShapesEventArgs e) {
            try {
                if (SelectedProject.SelectedRequest != null) {
                    if (SelectedProject.SelectedRequest.SelectedInspection != null) {

                        var shapes = __roicontainer.GetAllShapes();

                        foreach (Shape item in shapes) {
                            item.BorderColor = Color.Green;
                        }




                        if (e.SelectedShapes.Count > 0) {
                            ROI _roi = (ROI)SelectedProject.SelectedRequest.SelectedInspection.GetROI(e.SelectedShapes[0].Name);
                            SelectedProject.SelectedRequest.SelectedInspection.SelectedROI = _roi;
                            if (_roi != null) {
                                _roi.ROIShape.BorderColor = Color.Yellow;
                            }

                        } else {


                        }
                    }
                }

            } catch (Exception exp) {
                log.Error(exp);
            }
        }

        Stopwatch stopWatch2 = new Stopwatch();

        private void __roicontainer_DragOver(object sender, DragEventArgs e) {

            e.Effect = DragDropEffects.Link; // Okay

            if (!stopWatch2.IsRunning) {
                stopWatch2.Reset();
                stopWatch2.Start();

            } else {
                if (stopWatch2.ElapsedMilliseconds > 2000) {

                    stopWatch2.Reset();
                    stopWatch2.Stop();
                }
            }

        }

        private void __roicontainer_DragDrop(object sender, DragEventArgs e) {
            try {

                Point thepoint = __roicontainer.GetPositionOnPic(new Point(e.X, e.Y), true);


                // var listshapes = _ImageContainer.__roicontainer.GetShapesAt(thepoint);

                if (SelectedProject.SelectedRequest.SelectedInspection.SelectedROI != null) {
                    Shape selectedshape = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.ROIShape;
                    __roicontainer.SelectShapes(selectedshape);
                    Point roiPoint = new Point(Math.Abs(thepoint.X - selectedshape.ShapeEfectiveBounds.Left), Math.Abs(thepoint.Y - selectedshape.ShapeEfectiveBounds.Top));
                    lock (SelectedProject.SelectedRequest.SelectedInspection.ImageLocker) {

                        Image<Bgr, Byte> temp = SelectedProject.SelectedRequest.SelectedInspection.ResultImageBgr;

                        temp.ROI = selectedshape.ShapeEfectiveBounds;


                        if (e.Data.GetDataPresent(typeof(DroppedProcessingInfo))) {
                            DroppedProcessingInfo droppedObject = (DroppedProcessingInfo)e.Data.GetData(typeof(DroppedProcessingInfo));

                            //(droppedObject.DraggedObject as ProcessingFunctionBase).DraggedPointOption(roiPoint, droppedObject.DraggedProperty);

                            if (droppedObject.DraggedObject is ProcessingFunctionLineEdge) {
                                ProcessingFunctionLineEdge proc = (ProcessingFunctionLineEdge)droppedObject.DraggedObject;

                                LineEdgePreProc LineEdgePreProcObject = (LineEdgePreProc)proc.GetType().GetProperty("LineEdgePreProc1").GetValue(proc, null);

                                if (droppedObject.DraggedProperty == "StartPoint") {
                                    LineEdgePreProcObject.StartPoint = roiPoint;
                                } else if (droppedObject.DraggedProperty == "EndPoint") {
                                    LineEdgePreProcObject.EndPoint = roiPoint;
                                }

                            }

                            #region ProcessingFunctionCircleAnalysis

                            else if (droppedObject.DraggedObject is ProcessingFunctionCircleAnalysis || droppedObject.DraggedObject is ProcessingFunctionCircleEdges) {

                                ProcessingFunctionBase proc = (ProcessingFunctionBase)droppedObject.DraggedObject;// as typeof(droppedObject.DraggedObject.GetType());

                                //ProcessingFunctionCircleAnalysis proc = (ProcessingFunctionCircleAnalysis)droppedObject.DraggedObject;

                                CircleInfo CircleInfoObject = (CircleInfo)proc.GetType().GetProperty("circleInfo").GetValue(proc, null);

                                if (CircleInfoObject != null) {
                                    var CircleInfoProp = CircleInfoObject.GetType().GetProperty(droppedObject.DraggedProperty);

                                    if (CircleInfoProp != null) {
                                        //Point roiPoint = new Point(Math.Abs(thepoint.X - selectedshape.ShapeEfectiveBounds.Left), Math.Abs(thepoint.Y - selectedshape.ShapeEfectiveBounds.Top));

                                        if (droppedObject.DraggedProperty == "CircleCenter" || droppedObject.DraggedProperty == "CirclePt1" || droppedObject.DraggedProperty == "CirclePt2" || droppedObject.DraggedProperty == "CirclePt3") {

                                            ResultReference newresref = new ResultReference(SelectedProject,roiPoint);

                                            CircleInfoProp.SetValue(CircleInfoObject, newresref, null);

                                        } else if (droppedObject.DraggedProperty == "CircleAngleStart") {
                                            LineSegment2D horizontline = new LineSegment2D((Point)CircleInfoObject.CircleCenter.ResultOutput, new Point(((Point)CircleInfoObject.CircleCenter.ResultOutput).X + 20, ((Point)CircleInfoObject.CircleCenter.ResultOutput).Y));
                                            LineSegment2D angleline = new LineSegment2D((Point)CircleInfoObject.CircleCenter.ResultOutput, roiPoint);

                                            Double angle = angleline.GetExteriorAngleDegree(horizontline);
                                            if (angle < 0) {
                                                angle = 360 + angle;
                                            }
                                            ResultReference newresref = new ResultReference(SelectedProject,Math.Round(angle, 0));
                                            CircleInfoObject.CircleAngleStart = newresref;
                                        } else if (droppedObject.DraggedProperty == "CircleAngleEnd") {
                                            LineSegment2D horizontline = new LineSegment2D((Point)CircleInfoObject.CircleCenter.ResultOutput, new Point(((Point)CircleInfoObject.CircleCenter.ResultOutput).X + 20, ((Point)CircleInfoObject.CircleCenter.ResultOutput).Y));
                                            LineSegment2D angleline = new LineSegment2D((Point)CircleInfoObject.CircleCenter.ResultOutput, roiPoint);

                                            Double angle = angleline.GetExteriorAngleDegree(horizontline);
                                            if (angle < 0) {
                                                angle = 360 + angle;
                                            }
                                            ResultReference newresref = new ResultReference(SelectedProject,Math.Round(angle, 0));
                                            CircleInfoObject.CircleAngleEnd = newresref;
                                        } else if (droppedObject.DraggedProperty == "MainCircleRad") {

                                            if ((Point)CircleInfoObject.CircleCenter.ResultOutput != new Point(0, 0)) {

                                                double x1 = Math.Abs(roiPoint.X - ((Point)CircleInfoObject.CircleCenter.ResultOutput).X);
                                                double y1 = Math.Abs(roiPoint.Y - ((Point)CircleInfoObject.CircleCenter.ResultOutput).Y);
                                                double dist = Math.Round(Math.Sqrt((x1 * x1) + (y1 * y1)), 3);

                                                ResultReference newresref = new ResultReference(SelectedProject,dist);
                                                CircleInfoObject.MainCircleRad = newresref;
                                            }
                                        }



                                    }


                                }
                                //propertyInfo.SetValue(proc, value, null);

                            } else
                            #endregion


                                #region ProcessingFunctionAreaAnalysis

                                if (droppedObject.DraggedObject is ProcessingFunctionAreaAnalysis) {

                                    temp.ROI = selectedshape.ShapeEfectiveBounds;

                                    ProcessingFunctionAreaAnalysis proc = (ProcessingFunctionAreaAnalysis)droppedObject.DraggedObject;

                                    AreaInfo areaInfoObject = (AreaInfo)proc.GetType().GetProperty("areaInfo").GetValue(proc, null);


                                    if (areaInfoObject != null) {
                                        var AreaInfoProp = areaInfoObject.GetType().GetProperty(droppedObject.DraggedProperty);

                                        if (AreaInfoProp != null) {
                                            //Point roiPoint = new Point(Math.Abs(thepoint.X - selectedshape.ShapeEfectiveBounds.Left), Math.Abs(thepoint.Y - selectedshape.ShapeEfectiveBounds.Top));

                                            AreaInfoProp.SetValue(areaInfoObject, roiPoint, null);

                                            if (roiPoint.X > -1 && roiPoint.Y > -1) {
                                                temp.Draw(new Cross2DF(new PointF(roiPoint.X, roiPoint.Y), 10, 10), new Bgr(Color.Red), 1);
                                            }

                                        }


                                    }
                                    //propertyInfo.SetValue(proc, value, null);

                                }
                                #endregion


                            //_ListROIForm.__propertyGridFunction.Refresh();
                        }


                        if (roiPoint.X > -1 && roiPoint.Y > -1) {
                            temp.Draw(new Cross2DF(new PointF(roiPoint.X, roiPoint.Y), 10, 10), new Bgr(Color.Red), 1);
                        }

                        temp.ROI = Rectangle.Empty;

                        DoLoadBackImage(temp, false);
                    }
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }
        Stopwatch stopWatch = new Stopwatch();
        private void __roicontainer_DragLeave(object sender, EventArgs e) {
            stopWatch.Stop();
        }

        private void ImageContainerForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        Boolean ShapeLocationChanging = false;
        Boolean ShapeSizeChanging = false;

        private void __roicontainer_OnShapeSizeChanged(Shape sh, EventArgs e) {
            ShapeSizeChanging = false;
            using (UndoRedoManager.Start(SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.Name + ": Size Changed")) {
                SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.UndoRedoRectangle = sh.ShapeEfectiveBounds;
                UndoRedoManager.Commit();
            }

        }

        private void __roicontainer_OnShapeLocationChanging(Shape sh, EventArgs e) {
            try {
                if (!ShapeLocationChanging) {
                    ShapeLocationChanging = true;

                    //_ListInspForm.__listRoi.RefreshObject(_ListInspForm.__listRoi.SelectedObject);
                }

            } catch (Exception exp) {
                ShapeLocationChanging = false;
                log.Error(exp);
            }
        }

        private void __roicontainer_OnShapeSizeChanging(Shape sh, EventArgs e) {
            try {
                if (!ShapeLocationChanging) {
                    ShapeLocationChanging = true;

                }

            } catch (Exception exp) {
                ShapeLocationChanging = false;
                log.Error(exp);
            }
        }

        private void __roicontainer_OnShapeLocationChanged(Shape sh, EventArgs e) {
            try {
                if (SelectedProject.SelectedRequest.SelectedInspection.SelectedROI!=null) {
                    ShapeLocationChanging = false;
                    using (UndoRedoManager.Start(SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.Name + ": Position Changed")) {
                        SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.UndoRedoRectangle = sh.ShapeEfectiveBounds;
                        UndoRedoManager.Commit();
                    } 
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void ImageContainerForm_Load(object sender, EventArgs e) {
         //  mousepointmask[0, 0] = new Gray(255);
            __openFileDialog.FileOk += new CancelEventHandler(__openFileDialog_FileOk);
            __setCurrentToolStripMenuItem.Click += new EventHandler(__setCurrentToolStripMenuItem_Click);
        }

        void __setCurrentToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                string imageDir = Path.GetDirectoryName(Application.ExecutablePath) + "\\Images\\";
                if (!Directory.Exists(imageDir)) {

                    Directory.CreateDirectory(imageDir);
                }
                string imageFile = Path.Combine(imageDir, SelectedProject.SelectedRequest.SelectedInspection.Name + "_RefImage.bmp");


                MemoryStream ms = new MemoryStream();
                __roicontainer.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);


                switch (__roicontainer.BackgroundImage.PixelFormat) {

                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        Image<Bgr, byte> _image = new Image<Bgr, byte>(__roicontainer.BackgroundImage.Size);
                        _image.Bitmap = (Bitmap)Image.FromStream(ms);
                        _image.Save(imageFile);
                        break;
                    default:
                        break;
                }

                if (File.Exists(imageFile)) {
                    __refImagetool.Text = imageFile;
                    __refImagetool.Visible = true;
                }



            } catch (Exception exp) {
                __refImagetool.Visible = false;
                log.Error(exp);
            }
        }

        void __openFileDialog_FileOk(object sender, CancelEventArgs e) {
            try {
                FileDialog _filedialog = (FileDialog)sender;

            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __roicontainer_MouseHover(object sender, EventArgs e) {
            if (stopWatch.ElapsedMilliseconds > 2000) {
                stopWatch.Stop();
            }

            // Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;
        }
        //private Image<Gray, Byte> mousepointmask = new Image<Gray, byte>(new Size(2, 2));
        //private Image<Bgr, Byte> mousepointimg = new Image<Bgr, byte>(new Size(2, 2));
       




    }
}
