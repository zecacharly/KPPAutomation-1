using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using KPP.Controls.Winforms.ImageEditorObjs;
using KPP.Controls.Winforms;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using TIS.Imaging;
using System.Xml.Serialization;
using KPP.Core.Debug;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Threading;
using DejaVu;
using Emgu.CV.CvEnum;
using System.Net.Sockets;
using System.Drawing;
using IOModule;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Reflection;
using uEye.Types;
using VisionModule.Forms;
using KPPAutomationCore;
namespace VisionModule {


    #region Costum Poperty editors




    public class MyFileNameEditor : FileNameEditor {

        protected override void InitializeDialog(System.Windows.Forms.OpenFileDialog openFileDialog) {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Filter = "Image Files | *.bmp;*.jpeg;*.jpg;*.tiff";
            openFileDialog.Title = "Select image File";
        }
    }



    #endregion


    public class uEyeCamera : BaseCapture {

        private static KPPLogger log = new KPPLogger(typeof(uEyeCamera));

        

        readonly UndoRedo<int> _PixelClock = new UndoRedo<int>(-1);
        [XmlAttribute, ReadOnly(false)]
        [EditorAttribute(typeof(uEyeCameraConfigSelector), typeof(UITypeEditor))]
        public int PixelClock {
            get {

                return _PixelClock.Value;
            }
            set {
                if (_PixelClock.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Pixel Clock changed to:" + value)) {

                            if (Camera != null) {
                                uEye.Types.Range<int> range;
                                Camera.Timing.PixelClock.GetRange(out range);
                                if (value > range.Minimum && value < range.Maximum) {
                                    Camera.Timing.PixelClock.Set(value);
                                    _PixelClock.Value = value;
                                }

                            }

                            UndoRedoManager.Commit();
                        }
                    } else {

                        _PixelClock.Value = value;
                    }

                }
            }
        }

        readonly UndoRedo<double> _Framerate = new UndoRedo<double>(-1);
        [XmlAttribute, ReadOnly(false)]
        [EditorAttribute(typeof(uEyeCameraConfigSelector), typeof(UITypeEditor))]
        public double Framerate {
            get {
                return Math.Round(_Framerate.Value, 3);
            }
            set {
                if (_Framerate.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Framerate changed to:" + value)) {

                            if (Camera != null) {
                                uEye.Types.Range<double> range;
                                Camera.Timing.Framerate.GetFrameRateRange(out range);
                                if (value > range.Minimum && value < range.Maximum) {
                                    Camera.Timing.Framerate.Set(value);
                                    _Framerate.Value = value;
                                }

                            }

                            UndoRedoManager.Commit();
                        }
                    } else {

                        _Framerate.Value = value;
                    }

                }
            }
        }

        readonly UndoRedo<Rectangle> _AOI = new UndoRedo<Rectangle>(Rectangle.Empty);
        [ReadOnly(false)]
        //[EditorAttribute(typeof(uEyeCameraConfigSelector), typeof(UITypeEditor))]
        public Rectangle AOI {
            get {
                return _AOI.Value;
            }
            set {
                if (_AOI.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " AOI changed to:" + value)) {

                            if (Camera != null && value != null) {
                                uEye.Types.Range<Int32> rangex, rangey, rangew, rangeh;

                                Camera.Size.AOI.GetPosRange(out rangex, out rangey);
                                Camera.Size.AOI.GetSizeRange(out rangew, out rangeh);

                                value.X = value.X.NextEven();
                                value.Y = value.Y.NextEven();
                                value.Width = value.Width.RoundUp(4);
                                value.Height = value.Height.NextEven();


                                if (true || value.X > rangex.Minimum && value.X < rangex.Maximum
                                    && value.Y > rangey.Minimum && value.Y < rangey.Maximum
                                    && value.Width > rangew.Minimum && value.Width < rangew.Maximum
                                    && value.Height > rangeh.Minimum && value.Height < rangeh.Maximum) {
                                    uEye.Defines.Status statusRet = uEye.Defines.Status.NO_SUCCESS;
                                    if (value.Width <= 4 || value.Height <= 2) {
                                        value = new Rectangle(0, 0, _FormatSize.Width, _FormatSize.Height);
                                    }
                                    statusRet = Camera.Size.AOI.Set(value);
                                    if (statusRet != uEye.Defines.Status.SUCCESS) {

                                        //Camera = null;
                                        throw new Exception("Error setting AOI");

                                    }
                                    _AOI.Value = value;
                                }

                            }

                            UndoRedoManager.Commit();
                        }
                    } else {

                        _AOI.Value = value;
                    }

                }
            }
        }

        readonly UndoRedo<double> _exposure = new UndoRedo<double>(-1);
        [XmlAttribute, ReadOnly(false)]
        [EditorAttribute(typeof(uEyeCameraConfigSelector), typeof(UITypeEditor))]
        public double Exposure {
            get {
                return Math.Round(_exposure.Value, 2);
            }
            set {
                if (_exposure.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Exposure changed to:" + value)) {

                            if (Camera != null) {
                                uEye.Types.Range<Double> range;
                                Camera.Timing.Exposure.GetRange(out range);
                                if (value > range.Minimum && value < range.Maximum) {
                                    Camera.Timing.Exposure.Set(value);
                                    _exposure.Value = value;
                                }

                            }

                            UndoRedoManager.Commit();
                        }
                    } else {

                        _exposure.Value = value;
                    }

                }
            }
        }

        private Size<int> _FormatSize = new Size<int>(0, 0);

        private Boolean SetCamera(String name) {
            try {

                if (name.Contains("#") == false) {
                    return false;
                }
                String SerialNumber = name.Split(new String[] { "#" }, StringSplitOptions.None)[1];

                uEye.Types.CameraInformation[] cameraList;
                uEye.Info.Camera.GetCameraList(out cameraList);
                uEye.Types.CameraInformation SelectedCameraInfo = cameraList.ToList().Find(byserial => byserial.SerialNumber.Replace("\0", "") == SerialNumber);


                uEye.Camera _SelectedCamera = new uEye.Camera();

                if (SelectedCameraInfo.CameraID <= 0) {
                    return false;
                }

                Int32 id = Convert.ToInt32(SelectedCameraInfo.CameraID);
                uEye.Defines.Status statusRet = uEye.Defines.Status.NO_SUCCESS;
                statusRet = _SelectedCamera.Init(id);



                if (statusRet != uEye.Defines.Status.SUCCESS) {

                    throw new Exception("Error initializing camera");


                }
                Camera = _SelectedCamera;
                statusRet = Camera.Memory.Allocate();

                if (statusRet != uEye.Defines.Status.SUCCESS) {

                    throw new Exception("Error allocating memory");

                }
                Camera.Acquisition.Stop();

                Camera.AutoFeatures.Software.SetEnableAutoWhitebalanceOnce(false);
                Camera.AutoFeatures.Software.SetEnableAutoWhiteBalance(false);
                Camera.AutoFeatures.Software.SetEnableAutoShutter(false);
                Camera.AutoFeatures.Software.SetEnableAutoGain(false);
                Camera.AutoFeatures.Software.SetEnableAutoFramerate(false);
                Camera.AutoFeatures.Software.SetEnableAutoBrightnessOnce(false);

                Camera.Timing.PixelClock.Set(PixelClock);
                Camera.Timing.Framerate.Set(Framerate);
                Camera.Timing.Exposure.Set(Exposure);

                Camera.EventFrame += new EventHandler(Camera_EventFrame);

                ImageFormatInfo[] FormatInfoList;
                Camera.Size.ImageFormat.GetList(out FormatInfoList);

                //int maxsize=0;
                foreach (ImageFormatInfo item in FormatInfoList) {
                    if (item.Size.Width > _FormatSize.Width) {
                        _FormatSize.Width = item.Size.Width;
                        _FormatSize.Height = item.Size.Height;
                    }
                }
                if (!AOI.IsEmpty) {
                    statusRet = Camera.Size.AOI.Set(AOI);
                    if (statusRet != uEye.Defines.Status.SUCCESS) {

                        //Camera = null;
                        throw new Exception("Error setting AOI");

                    }
                }

            } catch (DllNotFoundException exp) {

                log.Error(exp);
            }

            return true;

        }

        void Camera_EventFrame(object sender, EventArgs e) {
            waitimage.Set();
        }



        String _CameraName = "No camera selected";
        [EditorAttribute(typeof(uEyeCameraSelector), typeof(UITypeEditor))]
        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(false), Browsable(true)]
        public override String CameraName {
            get {

                return _CameraName;
            }

            set {

                if (_CameraName != value) {
                    _CameraName = value;

                    SetCamera(value);
                }
            }

        }





        uEye.Camera _Camera = null;
        [XmlIgnore]
        [Browsable(false)]
        public uEye.Camera Camera {
            get {
                return _Camera;
            }
            set {
                if (_Camera != value) {
                    _Camera = value;
                }
            }
        }


        ManualResetEvent waitimage = new ManualResetEvent(true);
        private object lockobject = new object();

        public override Image<Bgr, Byte> GetImage() {

            Image<Bgr, Byte> newimage = null;

            uEye.Defines.Status statusRet = uEye.Defines.Status.NO_SUCCESS;
            waitimage.Reset();

            if (Camera.IsOpened == false) {
                SetCamera(_CameraName);
            }
            Camera.Timing.PixelClock.Set(PixelClock);
            Camera.Timing.Framerate.Set(Framerate);
            Camera.Timing.Exposure.Set(Exposure);

            statusRet = Camera.Acquisition.Freeze();
            //Camera.Acquisition.Freeze(uEye.Defines.DeviceParameter.Wait);

            if (statusRet != uEye.Defines.Status.SUCCESS) {

                //Camera = null;
                throw new Exception("Error setting snapshot");

            }
            waitimage.WaitOne();
            uEye.Defines.DisplayMode mode;
            Camera.Display.Mode.Get(out mode);

            // only display in dib mode
            if (mode == uEye.Defines.DisplayMode.DiB) {
                Int32 s32MemID;
                Camera.Memory.GetActive(out s32MemID);
                Camera.Memory.Lock(s32MemID);



                Bitmap bitmap = null;
                Camera.Memory.ToBitmap(s32MemID, out bitmap);
                //Camera.Memory.
                if (bitmap != null) {

                    //DoDrawing(ref graphics, s32MemID);
                    newimage = new Image<Bgr, byte>(bitmap);

                    bitmap.Dispose();
                }

                Camera.Memory.Unlock(s32MemID);

            }




            return newimage;
            //Camera.Acquisition.;





        }

        //private int framecounter = 0;


        public override string ToString() {
            return "uEye Capture";
        }

        public uEyeCamera(VisionProject selectedproject) {

            this.CameraName = "uEye camera input";
            this.Camtype = CameraTypes.uEye;
        }

        public uEyeCamera() {

            this.CameraName = "uEye camera input";
            this.Camtype = CameraTypes.uEye;
        }

        public override void Dispose() {
            if (Camera != null) {
                //Camera.Stop();
                Camera.Exit();

                Console.WriteLine("Camera Stopped");
            }
        }

    }


    public class ICScamera {
        
        String _name;


        VCDRangeProperty _zoominterface;
        public VCDRangeProperty Zoominterface {
            get {
                return _zoominterface;
            }
            private set {
                if (_zoominterface != value) {
                    _zoominterface = value;
                    if (_zoominterface == null) {

                    }
                }
            }

        }

        VCDRangeProperty _irisinterface;
        public VCDRangeProperty Irisinterface {
            get {
                return _irisinterface;
            }
            private set {
                if (_irisinterface != value) {
                    _irisinterface = value;
                    if (_irisinterface == null) {

                    }
                }
            }
        }


        VCDRangeProperty _focusinterface;
        public VCDRangeProperty Focusinterface {
            get {
                return _focusinterface;
            }
            private set {
                if (_focusinterface != value) {
                    _focusinterface = value;
                    if (_focusinterface == null) {


                    }
                }
            }
        }


        VCDRangeProperty _gaininterface;
        public VCDRangeProperty Gaininterface {
            get {
                return _gaininterface;
            }

        }



        VCDRangeProperty _redbalanceinterface;
        public VCDRangeProperty Redbalanceinterface {
            get {
                return _redbalanceinterface;
            }

        }

        VCDRangeProperty _bluebalanceinterface;
        public VCDRangeProperty Bluebalanceinterface {
            get {
                return _bluebalanceinterface;
            }

        }

        VCDRangeProperty _greenbalanceinterface;
        public VCDRangeProperty Greenbalanceinterface {
            get {
                return _greenbalanceinterface;
            }

        }


        VCDAbsoluteValueProperty _exposureinterface;
        public VCDAbsoluteValueProperty Exposureinterface {
            get {
                return _exposureinterface;
            }

        }

        ICImagingControl _Camera;
        public ICImagingControl Camera {
            get { return _Camera; }
            private set {
                if (_Camera != value) {
                    try {

                        if (value != null) {
                            if (value.LiveVideoRunning) {
                                value.LiveStop();
                            }
                            value.Device = Name;
                            
                            value.ImageRingBufferSize = 1;
                            value.ImageAvailableExecutionMode = EventExecutionMode.MultiThreaded;
                            value.LiveStart();
                            Thread.Sleep(1000);
                            value.LiveStop();

                        }
                    } catch (ICException exp) {

                        log.Warn(exp.Message);
                    }

                    _Camera = value;
                }

            }
        }

        public String Name {
            get { return _name; }

        }

        public void Dispose() {

            //if (Camera!=null) {

            //    if (!Camera.Disposing && !Camera.IsDisposed) {
            //        Camera.LiveStop();
            //        Camera.Dispose();
            //        Camera = null; 
            //    }
            //}

        }

        private static KPPLogger log = new KPPLogger(typeof(ICScamera));
        public ICScamera(String cameraName,String ModuleName) {
            
            try {
                _name = cameraName;

                Camera = ICSDevices.Find(cam => cam.Device == _name);

                if (Camera!=null) {



                    VCDSwitchProperty _whitebalance = (VCDSwitchProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_WhiteBalance, VCDGUIDs.VCDElement_Auto, VCDGUIDs.VCDInterface_Switch);
                    if (_whitebalance != null) {
                        if (_whitebalance.Switch) {
                            _whitebalance.Switch = false;
                        }
                    }


                    VCDSwitchProperty _gainautointerface = (VCDSwitchProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Gain, VCDGUIDs.VCDElement_Auto, VCDGUIDs.VCDInterface_Switch);
                    if (_gainautointerface != null) {
                        if (_gainautointerface.Switch) {
                            _gainautointerface.Switch = false;
                        }
                    }

                    VCDSwitchProperty _irisautointerface = (VCDSwitchProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Iris, VCDGUIDs.VCDElement_Auto, VCDGUIDs.VCDInterface_Switch);
                    if (_irisautointerface != null) {
                        if (_irisautointerface.Switch) {
                            _irisautointerface.Switch = false;
                        }
                    }



                    Zoominterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Zoom, VCDGUIDs.VCDElement_Value, VCDGUIDs.VCDInterface_Range);
                    Irisinterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Iris, VCDGUIDs.VCDElement_Value, VCDGUIDs.VCDInterface_Range);
                    Focusinterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Focus, VCDGUIDs.VCDElement_Value, VCDGUIDs.VCDInterface_Range);


                    _gaininterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Gain, VCDGUIDs.VCDElement_Value, VCDGUIDs.VCDInterface_Range);
                    _exposureinterface = (VCDAbsoluteValueProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Exposure, VCDGUIDs.VCDElement_Value, VCDGUIDs.VCDInterface_AbsoluteValue);
                    _redbalanceinterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_WhiteBalance, VCDGUIDs.VCDElement_WhiteBalanceRed, VCDGUIDs.VCDInterface_Range);
                    _bluebalanceinterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_WhiteBalance, VCDGUIDs.VCDElement_WhiteBalanceBlue, VCDGUIDs.VCDInterface_Range);
                    _greenbalanceinterface = (VCDRangeProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_WhiteBalance, VCDGUIDs.VCDElement_WhiteBalanceGreen, VCDGUIDs.VCDInterface_Range);

                    VCDSwitchProperty exposureautointerface = (VCDSwitchProperty)Camera.VCDPropertyItems.FindInterface(VCDGUIDs.VCDID_Exposure, VCDGUIDs.VCDElement_Auto, VCDGUIDs.VCDInterface_Switch);
                    if (exposureautointerface != null) {
                        if (exposureautointerface.Switch) {
                            exposureautointerface.Switch = false;
                        }
                    }


                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }



        void Camera_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e) {

        }

        void _Camera_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e) {
            //e.ImageBuffer.Bitmap
        }


        private static List<ICImagingControl> _ICSDevices;//=new ICImagingControl();

        public static void StopDevices() {
            if (_ICSDevices!=null) {
                foreach (ICImagingControl item in _ICSDevices) {
                    item.Dispose();
                }
            }
            _ICSDevices = null;
        }

        public static List<ICImagingControl> ICSDevices {
            get {
                if (_ICSDevices == null) {
                    InitICSDevices();
                }
                return ICScamera._ICSDevices;
            }
            
        }

        private static void InitICSDevices() {
            
                _ICSDevices = new List<ICImagingControl>();
                ICImagingControl getdevices = new ICImagingControl();
                foreach (Device item in getdevices.Devices) {
                    ICImagingControl newimagingdevice = new ICImagingControl();
                    newimagingdevice.Device = item;
                    _ICSDevices.Add(newimagingdevice);
                }
                getdevices.Dispose();
                getdevices = null;
            
        }

    }

    //internal static class ICSCameraInterface {

    //    internal static List<ICScamera> ICSCameras = new List<ICScamera>();

    //    public ICSCameraInterface() {
    //        ICImagingControl teste = new ICImagingControl();
    //        teste.Devices
    //    }
    //}


    public class ICSCameraCapture : BaseCapture {

        private static KPPLogger log = new KPPLogger(typeof(ICSCameraCapture));

        void SetCamera(String value) {
            try {

                if (_ICSCamera != null) {
                    _ICSCamera.Dispose();
                }

                _ICSCamera = new ICScamera(value, base.ModuleName);

                if (_ICSCamera.Zoominterface != null) {
                        this.ChangeAttributeValue<BrowsableAttribute>("Zoom", "browsable", true);
                        _maxzoom = _ICSCamera.Zoominterface.RangeMax;
                        _minzoom = _ICSCamera.Zoominterface.RangeMin;
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("Zoom", "browsable", false);
                    }

                if (_ICSCamera.Irisinterface != null) {
                        this.ChangeAttributeValue<BrowsableAttribute>("Iris", "browsable", true);
                        _maxiris = _ICSCamera.Irisinterface.RangeMax;
                        _miniris = _ICSCamera.Irisinterface.RangeMin;
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("Iris", "browsable", false);
                    }

                if (_ICSCamera.Gaininterface != null) {
                    _maxgain = _ICSCamera.Gaininterface.RangeMax;
                    _mingain = _ICSCamera.Gaininterface.RangeMin;
                    }



                if (_ICSCamera.Focusinterface != null) {
                        this.ChangeAttributeValue<BrowsableAttribute>("Focus", "browsable", true);
                        _maxfocus = _ICSCamera.Focusinterface.RangeMax;
                        _minfocus = _ICSCamera.Focusinterface.RangeMin;
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("Focus", "browsable", false);
                    }

                if (_ICSCamera.Greenbalanceinterface != null) {
                    _MinGreenBalance = _ICSCamera.Greenbalanceinterface.RangeMin;
                    _MaxGreenBalance = _ICSCamera.Greenbalanceinterface.RangeMax;
                        this.ChangeAttributeValue<BrowsableAttribute>("GreenBalance", "browsable", true);
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("GreenBalance", "browsable", false);
                    }

                if (_ICSCamera.Redbalanceinterface != null) {
                    _MinRedBalance = _ICSCamera.Redbalanceinterface.RangeMin;
                    _MaxRedBalance = _ICSCamera.Redbalanceinterface.RangeMax;
                        this.ChangeAttributeValue<BrowsableAttribute>("RedBalance", "browsable", true);
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("RedBalance", "browsable", false);
                    }

                if (_ICSCamera.Bluebalanceinterface != null) {
                    _MinBlueBalance = _ICSCamera.Bluebalanceinterface.RangeMin;
                    _MaxBlueBalance = _ICSCamera.Bluebalanceinterface.RangeMax;
                        this.ChangeAttributeValue<BrowsableAttribute>("BlueBalance", "browsable", true);
                    } else {
                        this.ChangeAttributeValue<BrowsableAttribute>("BlueBalance", "browsable", false);
                    }


                    _maxexposure = 100;
                    _minexposure = 1;
                    base.UpdateAttributes();


                
            } catch (Exception exp) {

                log.Error(exp);
            }
        }



        public int GetIntValue() {
            double rmin = 0;
            double rmax = 0;
            double absval = 0;
            double rangelen = 0;
            double p = 0;

            // Get the property data from the interface
            rmin = _ICSCamera.Exposureinterface.RangeMin;
            rmax = _ICSCamera.Exposureinterface.RangeMax;
            absval = _ICSCamera.Exposureinterface.Value;

            // Do calculation depending of the dimension function of the property
            if (_ICSCamera.Exposureinterface.DimFunction == TIS.Imaging.AbsDimFunction.eAbsDimFunc_Log) {

                rangelen = System.Math.Log(rmax) - System.Math.Log(rmin);
                p = 100 / rangelen * (System.Math.Log(absval) - System.Math.Log(rmin));
            } else // AbsValItf.DimFunction = AbsDimFunction.eAbsDimFunc_Linear
            {
                rangelen = rmax - rmin;
                p = 100 / rangelen * (absval - rmin);
            }

            // Round to integer
            return (int)System.Math.Round(p, 0);
        }

        private double GetAbsVal(int Val) {

            double rmin = 0;
            double rmax = 0;
            double rangelen = 0;
            double value = 0;

            // Get the property data from the interface
            rmin = _ICSCamera.Exposureinterface.RangeMin;
            rmax = _ICSCamera.Exposureinterface.RangeMax;

            // Do calculation depending of the dimension function of the property
            if (_ICSCamera.Exposureinterface.DimFunction == TIS.Imaging.AbsDimFunction.eAbsDimFunc_Log) {

                rangelen = System.Math.Log(rmax) - System.Math.Log(rmin);
                value = System.Math.Exp(System.Math.Log(rmin) + rangelen / 100 * Val);

            } else // AbsValItf.DimFunction = AbsDimFunction.eAbsDimFunc_Linear
            {

                rangelen = rmax - rmin;
                value = rmin + rangelen / 100 * Val;

            }

            // Correct the value if it is out of bounds
            if (value > rmax) {
                value = rmax;
            }
            if (value < rmin) {
                value = rmin;
            }

            return value;
        }


        public override void UpdateAttributes() {

            if (_ICSCamera == null) {
                return;
            }

            if (_ICSCamera.Zoominterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("Zoom", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("Zoom", "browsable", false);
            }

            if (_ICSCamera.Irisinterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("Iris", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("Iris", "browsable", false);
            }

            if (_ICSCamera.Focusinterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("Focus", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("Focus", "browsable", false);
            }

            if (_ICSCamera.Greenbalanceinterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("GreenBalance", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("GreenBalance", "browsable", false);
            }

            if (_ICSCamera.Redbalanceinterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("RedBalance", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("RedBalance", "browsable", false);
            }

            if (_ICSCamera.Bluebalanceinterface != null) {
                this.ChangeAttributeValue<BrowsableAttribute>("BlueBalance", "browsable", true);
            } else {
                this.ChangeAttributeValue<BrowsableAttribute>("BlueBalance", "browsable", false);
            }

            base.UpdateAttributes();
        }

        readonly UndoRedo<String> _CameraName = new UndoRedo<string>("Capture from IC Imaging Camera");


        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(false), Browsable(true)]
        [EditorAttribute(typeof(ICSCameraSelector), typeof(UITypeEditor))]
        public override String CameraName {
            get {
                return _CameraName.Value;
            }
            set {

                if (_CameraName.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {

                        using (UndoRedoManager.Start(this.CameraName + " Camera changed to: " + value)) {
                            SetCamera(value);
                            _CameraName.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        SetCamera(value);
                        _CameraName.Value = value;
                    }
                }
            }
        }

        [XmlIgnore]

        internal ICScamera _ICSCamera;




        int _minzoom = 0;
        int _maxzoom = 15;

        int _miniris = 0;
        int _maxiris = 4000;

        int _mingain = 0;
        int _maxgain = 4000;

        int _MinRedBalance = 0;
        int _MaxRedBalance = 4000;

        int _MinGreenBalance = 0;
        int _MaxGreenBalance = 4000;

        int _MinBlueBalance = 0;
        int _MaxBlueBalance = 4000;

        int _minfocus = 0;
        int _maxfocus = 350;
        double _minexposure = 1;
        double _maxexposure = 4000;

        readonly UndoRedo<int> _iris = new UndoRedo<int>();
        [XmlAttribute]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true)]
        public int Iris {
            get {

                return _iris.Value;
            }
            set {
                if (_iris.Value != value) {

                    if (value >= _miniris && value <= _maxiris) {


                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Iris changed to:" + value)) {
                                _iris.Value = value;
                                UndoRedoManager.Commit();
                            }


                        } else {

                            _iris.Value = value;

                        }

                    }
                }
            }
        }

        readonly UndoRedo<int> _gain = new UndoRedo<int>();
        [XmlAttribute]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public int Gain {
            get {

                return _gain.Value;
            }
            set {
                if (_gain.Value != value) {

                    if (value >= _mingain && value <= _maxgain) {


                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Gain changed to:" + value)) {
                                _gain.Value = value;
                                UndoRedoManager.Commit();
                            }


                        } else {

                            _gain.Value = value;

                        }

                    }
                }
            }
        }

        readonly UndoRedo<int> _RedBalance = new UndoRedo<int>();
        [XmlAttribute, Browsable(true), DisplayName("Red Balance")]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public int RedBalance {
            get {

                return _RedBalance.Value;
            }
            set {
                if (_RedBalance.Value != value) {

                    if (value >= _MinRedBalance && value <= _MaxRedBalance) {


                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Red White balance changed to:" + value)) {
                                _RedBalance.Value = value;
                                UndoRedoManager.Commit();
                            }


                        } else {

                            _RedBalance.Value = value;

                        }

                    }
                }
            }
        }

        readonly UndoRedo<int> _BlueBalance = new UndoRedo<int>();
        [XmlAttribute, Browsable(true), DisplayName("Blue Balance")]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public int BlueBalance {
            get {

                return _BlueBalance.Value;
            }
            set {
                if (_BlueBalance.Value != value) {

                    if (value >= _MinBlueBalance && value <= _MaxBlueBalance) {


                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Blue White balance changed to:" + value)) {
                                _BlueBalance.Value = value;
                                UndoRedoManager.Commit();
                            }
                        } else {
                            _BlueBalance.Value = value;

                        }

                    }
                }
            }
        }

        readonly UndoRedo<int> _GreenBalance = new UndoRedo<int>();
        [XmlAttribute, Browsable(true), DisplayName("Green Balance")]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public int GreenBalance {
            get {

                return _GreenBalance.Value;
            }
            set {
                if (_GreenBalance.Value != value) {

                    if (value >= _MinGreenBalance && value <= _MaxGreenBalance) {


                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Green White balance changed to:" + value)) {
                                _GreenBalance.Value = value;
                                UndoRedoManager.Commit();
                            }
                        } else {
                            _GreenBalance.Value = value;

                        }

                    }
                }
            }
        }

        readonly UndoRedo<string> _PixelFormat = new UndoRedo<string>();
        [XmlAttribute]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true)]
        public string PixelFormat {
            get {
                return _PixelFormat.Value;
            }
            set {
                if (_PixelFormat.Value != value) {


                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " PixelFormat changed to:" + value)) {
                            _PixelFormat.Value = value;
                            UndoRedoManager.Commit();
                        }


                    } else {

                        _PixelFormat.Value = value;

                    }

                    if (_ICSCamera != null) {
                        if (_ICSCamera.Camera != null) {
                            if (this._ICSCamera.Camera.DeviceValid) {
                                if (_ICSCamera.Camera.VideoFormats.ToList().Find(byname => byname.Name == value) != null) {
                                    if (_ICSCamera.Camera.LiveVideoRunning) {
                                        _ICSCamera.Camera.LiveStop();
                                    }
                                    _ICSCamera.Camera.VideoFormat = value;
                                }

                            }
                        }
                    }
                }
            }
        }


        readonly UndoRedo<int> _zoom = new UndoRedo<int>();
        [XmlAttribute]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true)]
        public int Zoom {
            get {
                return _zoom.Value;
            }
            set {
                if (_zoom.Value != value) {

                    if (value >= _minzoom && value <= _maxzoom) {

                        if (value > _minfocus && value < _maxfocus) {
                            if (!UndoRedoManager.IsCommandStarted) {
                                using (UndoRedoManager.Start(this.CameraName + " Zoom changed to:" + value)) {
                                    _zoom.Value = value;
                                    UndoRedoManager.Commit();
                                }


                            } else {

                                _zoom.Value = value;

                            }
                        }
                    }
                }
            }
        }

        readonly UndoRedo<int> _focus = new UndoRedo<int>();
        [XmlAttribute]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true)]
        public int Focus {
            get {

                return _focus.Value;
            }
            set {
                if (_focus.Value != value) {

                    if (value > _minfocus && value < _maxfocus) {
                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Focus changed to:" + value)) {
                                _focus.Value = value;
                                UndoRedoManager.Commit();
                            }

                        } else {

                            _focus.Value = value;
                        }
                    }
                }
            }
        }

        readonly UndoRedo<int> _exposure = new UndoRedo<int>();

        [XmlAttribute, Browsable(true)]
        [EditorAttribute(typeof(InterfaceValueSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public int Exposure {
            get {
                return _exposure.Value;
            }
            set {
                if (_exposure.Value != value) {

                    if (value > _minexposure && value < _maxexposure) {

                        if (!UndoRedoManager.IsCommandStarted) {
                            using (UndoRedoManager.Start(this.CameraName + " Exposure changed to:" + value)) {
                                _exposure.Value = value;
                                UndoRedoManager.Commit();
                            }


                        } else {

                            _exposure.Value = value;
                        }

                    }
                }
            }
        }



        public override Image<Bgr, Byte> GetImage() {


            try {

                if (_ICSCamera == null) {
                    return null;
                }


                Boolean updatesettings = false;

                if (!_ICSCamera.Camera.IsLivePrepared) {
                    _ICSCamera.Camera.LiveStart();
                }

                if (_ICSCamera.Zoominterface != null) {
                    if (_ICSCamera.Zoominterface.Value != Zoom) {
                        int zoomdif = Math.Abs(_ICSCamera.Zoominterface.Value - Zoom);
                        _ICSCamera.Zoominterface.Value = Zoom;
                        _ICSCamera.Camera.MemorySnapImageSequence(1, 5000);
                        int sleeptime = (int)(0.40 * zoomdif);
                        Thread.Sleep(sleeptime * 1000);
                    }
                }


                if (_ICSCamera.Exposureinterface != null) {

                    if (Math.Round(_ICSCamera.Exposureinterface.Value, 4) != Math.Round(GetAbsVal(Exposure), 4)) {
                        _ICSCamera.Exposureinterface.Value = Math.Round(GetAbsVal(Exposure), 4);
                        updatesettings = true;
                    }


                }

                if (_ICSCamera.Focusinterface != null) {

                    if (_ICSCamera.Focusinterface.Value != Focus) {
                        _ICSCamera.Focusinterface.Value = Focus;
                        updatesettings = true;
                    }
                }

                if (_ICSCamera.Irisinterface != null) {

                    if (_ICSCamera.Irisinterface.Value != Iris) {
                        _ICSCamera.Irisinterface.Value = Iris;
                        updatesettings = true;
                    }
                }

                if (_ICSCamera.Gaininterface != null) {

                    if (_ICSCamera.Gaininterface.Value != Gain) {
                        _ICSCamera.Gaininterface.Value = Gain;
                        updatesettings = true;
                    }
                }

                if (_ICSCamera.Redbalanceinterface != null) {

                    if (_ICSCamera.Redbalanceinterface.Value != RedBalance) {
                        _ICSCamera.Redbalanceinterface.Value = RedBalance;
                        updatesettings = true;
                    }
                }

                if (_ICSCamera.Greenbalanceinterface != null) {

                    if (_ICSCamera.Greenbalanceinterface.Value != GreenBalance) {
                        _ICSCamera.Greenbalanceinterface.Value = GreenBalance;
                        updatesettings = true;
                    }
                }

                if (_ICSCamera.Bluebalanceinterface != null) {

                    if (_ICSCamera.Bluebalanceinterface.Value != BlueBalance) {
                        _ICSCamera.Bluebalanceinterface.Value = BlueBalance;
                        updatesettings = true;
                    }
                }


                if (updatesettings) {
                    _ICSCamera.Camera.MemorySnapImageSequence(2, 5000);
                }


                int ct1 = 0;

                while (true) {
                    log.Info("Capturing image from : " + _ICSCamera.Camera.Device);

                    try {
                        _ICSCamera.Camera.MemorySnapImageSequence(2, 5000);
                        break;
                    } catch (Exception exp) {

                        _ICSCamera.Camera.LiveStop();

                        ct1++;
                        if (ct1 > 2) {
                            log.Error(exp);
                            return null;
                        }

                    }
                }
                log.Info("Capturing Done");

                _ICSCamera.Camera.ImageActiveBuffer.Lock();
                Image<Bgr, Byte> outimage = null;
                if (_ICSCamera.Camera.ImageActiveBuffer.BitsPerPixel == 8) {
                    outimage = new Image<Bgr, byte>(_ICSCamera.Camera.ImageActiveBuffer.Bitmap);
                } else {
                    using (Image<Bgr, Int32> temp = new Image<Bgr, Int32>(_ICSCamera.Camera.ImageActiveBuffer.Bitmap)) {
                        if (_ICSCamera.Camera.ImageActiveBuffer.BitsPerPixel == 24) {
                            outimage = new Image<Bgr, byte>(temp.Size);
                            outimage.ConvertFrom<Bgr, Int32>(temp);
                            //CvInvoke.cvCvtColor(temp, outimage, Emgu.CV.CvEnum.COLOR_CONVERSION.cvbgr24);
                        } else {
                            CvInvoke.cvCvtColor(temp, outimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
                        }
                    }

                }
               

                _ICSCamera.Camera.ImageActiveBuffer.Unlock();

                return outimage;

            } catch (Exception exp) {

                log.Error(exp);
                return null;
            }

        }

        public override string ToString() {

            if (this.CameraName == "Capture from IC Imaging Camera") {
                return this.CameraName;
            }


            return "ICS:" + this.CameraName;
        }

        public ICSCameraCapture(VisionProject selectedProject) {
            base.SelectedProject = selectedProject;
            _CameraName.OnMemberRedo += new UndoRedo<string>.MemberRedo(_CameraName_OnMemberRedo);
            _CameraName.OnMemberUndo += new UndoRedo<string>.MemberUndo(_CameraName_OnMemberRedo);
            Camtype = CameraTypes.ICS;
        }

        public ICSCameraCapture() {




            _CameraName.OnMemberRedo += new UndoRedo<string>.MemberRedo(_CameraName_OnMemberRedo);
            _CameraName.OnMemberUndo += new UndoRedo<string>.MemberUndo(_CameraName_OnMemberRedo);
            Camtype = CameraTypes.ICS;
            

        }

        void _CameraName_OnMemberRedo(string UndoObject) {
            SetCamera(UndoObject);
        }

        public ICSCameraCapture(String DeviceName) {

            
            CameraName = DeviceName;

        }

        public override void Dispose() {
            if (_ICSCamera!=null) {
                _ICSCamera.Dispose();
                _ICSCamera = null;
            }
            base.Dispose();
        }

    }



    public class FileCapture : BaseCapture {
        


        String _CameraName = "";
        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(false)]
        public override String CameraName {
            get {
                return _CameraName;
            }
            set {

                _CameraName = value;
            }
        }


        readonly UndoRedo<String> _FileLoc = new UndoRedo<string>("");
        [XmlAttribute]
        [Category("Aquisition Settings"), Description("Select file location"), DisplayName("File Name"), Browsable(true)]
        [EditorAttribute(typeof(MyFileNameEditor), typeof(UITypeEditor))]
        public String FileLoc {
            get {
                return _FileLoc.Value;
            }
            set {
                if (value != _FileLoc.Value) {


                    if (!UndoRedoManager.IsCommandStarted) {

                        using (UndoRedoManager.Start(this.CameraName + " File location changed to: " + value)) {
                            _FileLoc.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _FileLoc.Value = value;
                    }



                }
            }
        }


        public override Image<Bgr, Byte> GetImage() {         

            return new Image<Bgr, byte>(FileLoc);

        }

        public override string ToString() {
            return FileLoc;
        }


        public FileCapture(VisionProject selectedProject) {
            base.SelectedProject = selectedProject;            
            FileLoc = "Select file location";
            CameraName = FileLoc;
            this.Camtype = CameraTypes.File;
            


        }

        public FileCapture() {
            CameraName = "Add file location";
            FileLoc = "Select location";
            this.Camtype = CameraTypes.File;
            


        }

        public override void Dispose() {
            base.Dispose();
        }



    }

    public class RequestSourceInspections {



        private Image<Bgr, Byte> _InspectionImage;
        [XmlIgnore]
        public Image<Bgr, Byte> InspectionImage {
            get {

                return _InspectionImage;
            }

        }

        private String _CallingRequest;

        public String CallingRequest {
            get { return _CallingRequest; }
            set { _CallingRequest = value; }
        }

        private Inspection _RequestSourceInspection;

        public Inspection RequestSourceInspection {
            get { return _RequestSourceInspection; }
            set {
                _RequestSourceInspection = value;
                if (_RequestSourceInspection != null) {

                } else {

                }
            }
        }



        public RequestSourceInspections(String Callingrequest, Inspection inspection) {
            CallingRequest = Callingrequest;
            RequestSourceInspection = inspection;
        }

        public RequestSourceInspections() {
        }
    }


    public class ZoneInfo {


        private String m_ZoneName = "Not set";
        [XmlAttribute, DisplayName("Zone Name")]
        public String ZoneName {
            get { return m_ZoneName; }
            set { m_ZoneName = value; }
        }

        private Rectangle m_Zone = Rectangle.Empty;

        public Rectangle Zone {
            get { return m_Zone; }
            set { m_Zone = value; }
        }

        public override string ToString() {
            return ZoneName + " - " + Zone.ToString();
        }

        public ZoneInfo(String name, Rectangle zone) {
            ZoneName = name;
            Zone = zone;
        }
        public ZoneInfo() {
        }
    }

    [Serializable]
    public class InspectionCapture : BaseCapture {
        
        

        public delegate void CaptureInspectionNameChanged(InspectionCapture Sender, String newName);



        String _CameraName = "";
        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(true)]
        public override String CameraName {
            get {
                return _CameraName;
            }
            set {
                _CameraName = value;
            }
        }

        readonly UndoRedo<Inspection> _InspectionSource = new UndoRedo<Inspection>();
        [EditorAttribute(typeof(InspNameSelector), typeof(System.Drawing.Design.UITypeEditor))]
        [XmlIgnore]
        public Inspection InspectionSource {
            get {
                return _InspectionSource.Value;
            }
            set {
                if (_InspectionSource.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Inspection source changed to:" + value)) {
                            _InspectionSource.Value = value;
                            UndoRedoManager.Commit();
                        }


                    } else {

                        _InspectionSource.Value = value;

                    }

                    if (InspectionSource != null) {
                        _CameraName = InspectionSource.Name + "." + InspectionSource.RequestID;
                        InspectionName = InspectionSource.Name + "." + InspectionSource.RequestID;
                    }
                }
            }
        }

        private Boolean _ShowAuxROIS = false;
        [XmlAttribute, DisplayName("Show Aux ROIs")]
        public Boolean ShowAuxROIS {
            get { return _ShowAuxROIS; }
            set { _ShowAuxROIS = value; }
        }


        String _InspectionName = "";
        [XmlAttribute]
        [Browsable(false)]
        public String InspectionName {
            get {

                return _InspectionName;
            }
            set {
                if (_InspectionName != value) {
                    _InspectionName = value;

                }

            }
        }


        private ZoneInfo m_CaptureZone = new ZoneInfo();
        [EditorAttribute(typeof(ZoneSelector), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ZoneInfo CaptureZone {
            get { return m_CaptureZone; }
            set {
                if (m_CaptureZone != value) {
                    m_CaptureZone = value;
                }
            }
        }


        public override Image<Bgr, Byte> GetImage(Boolean FullImage) {

            if (InspectionSource == null) {
                return null;
            }
            InspectionSource.Execute(this, false);

            if (InspectionSource.OriginalImageBgr == null) {
                return new Image<Bgr, byte>(new Size(800, 600));
            }


            InspectionSource.OriginalImageBgr.ROI = Rectangle.Empty;

            return InspectionSource.OriginalImageBgr;

        }



        public override Image<Bgr, Byte> GetImage() {


            if (InspectionSource == null) {
                return null;
            }
            InspectionSource.Execute(this, false);
            if (InspectionSource.OriginalImageBgr == null) {
                return new Image<Bgr, byte>(new Size(800, 600));
            }
            if (CaptureZone.Zone != Rectangle.Empty) {
                InspectionSource.OriginalImageBgr.ROI = CaptureZone.Zone;
                return InspectionSource.OriginalImageBgr;
            } else {
                return InspectionSource.OriginalImageBgr;
            }


        }

        public override string ToString() {
            if (InspectionSource != null) {

                return InspectionSource.RequestName + " - " + InspectionSource.Name;
            } else {
                return "No Inspection Source";
            }

        }

        public InspectionCapture( VisionProject selectedproject){
            base.SelectedProject = selectedproject;            
            CameraName ="Select Inspection";

            this.Camtype = CameraTypes.Inspection;
            //SpecialFunctions.ChageAttributeValue<ReadOnlyAttribute>(this, "CameraName", "isReadOnly", false);


        }

        


        public InspectionCapture() {
            CameraName = "Select Inspection";
            this.Camtype = CameraTypes.Inspection;
            //SpecialFunctions.ChageAttributeValue<ReadOnlyAttribute>(this, "CameraName", "isReadOnly", false);


        }

        public override void Dispose() {
            base.Dispose();
        }


    }


    public class DirectShowCamera {
        

        
    }

    public class DirectShowCameraCapture : BaseCapture {
        private static FilterInfoCollection _DevicesAvaible = null;

        public static FilterInfoCollection DevicesAvaible {
            get { return _DevicesAvaible; }         
        }


        public static void StopCameras() {
            
            for (int i = 0; i < DirectShowCameras.Count-1; i++) {
                DirectShowCameras[i].Stop();
                DirectShowCameras[i] = null;
            }
            DirectShowCameras.Clear();
        }

        public static void InitCameras() {
            if (DirectShowCameras.Count==0) {
                _DevicesAvaible = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (FilterInfo deviceinfo in DevicesAvaible) {
                    DirectShowCameras.Add(new VideoCaptureDevice(deviceinfo.MonikerString));
                }    
            }
            
        }



        private static List<VideoCaptureDevice> _DirectShowCameras = new List<VideoCaptureDevice>();

        public static List<VideoCaptureDevice> DirectShowCameras {
            get { return DirectShowCameraCapture._DirectShowCameras; }            
        }

        VideoCaptureDevice _Camera = null;
        [XmlIgnore, Browsable(false)]
        public VideoCaptureDevice Camera {
            get { return _Camera; }
            set {

                if (_Camera != value) {
                    if (_Camera != null) {
                        waitImage.Set();
                        _Camera.Stop();
                        _Camera.WaitForStop();
                        _Camera = null;
                    }
                    if (value != null) {
                        _Camera = value;
                        _Camera.VideoResolution = _Camera.VideoCapabilities[_Camera.VideoCapabilities.Count() - 1];
                        _Camera.NewFrame += new NewFrameEventHandler(_Camera_NewFrame);
                        _Camera.Start();
                    }
                }
            }
        }





        void SetCamera(String value) {
            try {
                Camera = DirectShowCameras.Find(moniker => moniker.Source == value);
              
            } catch (Exception exp) {

            }
        }

        private String _CameraID = "";
        [XmlAttribute("Camera ID"), Browsable(true), EditorAttribute(typeof(DirectShowSelector), typeof(UITypeEditor))]
        public String CameraID {
            get { return _CameraID; }
            set {

                if (_CameraID != value) {                    
                    _CameraID = value;
                    FilterInfo caminf = DevicesAvaible.Cast<FilterInfo>().ToList().Find(moniker => moniker.MonikerString == value);
                    _CameraName = caminf.Name;
                    SetCamera(value);
                    //if (CameraInfo == null) {
                    //    CameraInfo = DirectShowCameraCapture.DevicesAvaible.Cast<FilterInfo>().ToList().
                    //}
                }

            }
        }



        //private FilterInfo _CameraInfo = null;
        //[EditorAttribute(typeof(DirectShowSelector), typeof(UITypeEditor)), XmlIgnore]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public FilterInfo CameraInfo {
        //    get { return _CameraInfo; }
        //    set {
        //        if (_CameraInfo != value) {
        //            _CameraInfo = value;
        //            CameraName = value.Name;
        //            CameraID = value.MonikerString;
        //            SetCamera(CameraID);
        //        }
        //    }
        //}

        private String _CameraName ="";
        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(true), Browsable(true)]
        public override String CameraName {
            get {
                return _CameraName;
            }

            set {
                if (_CameraName!=value) {
                    _CameraName = value;
                }
            }
           
       }


        readonly UndoRedo<double> _exposure = new UndoRedo<double>();
        [Browsable(false),XmlIgnore]
        public double Exposure {
            get {

                return _exposure.Value;
            }
            set {
                if (_exposure.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Exposure changed to:" + value)) {
                            _exposure.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }

                } else {

                    _exposure.Value = value;
                }
            }
        }



        private Image<Bgr, Byte> FrameImage = null;


        Boolean _GotFrame = false;
        [XmlIgnore, Browsable(false)]
        public Boolean GotFrame {
            get { return _GotFrame; }
            set {
                _GotFrame = value;
            }
        }


        private int framecount = 0;
        private int _ValidFrameCount = 0;
        [XmlAttribute,DisplayName("Frame Count"),Description("Number of frames before capture image")]
        public int ValidFrameCount {
            get { return _ValidFrameCount; }
            set { _ValidFrameCount = value; }
        }

        void _Camera_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            try {
                if (GotFrame == false) {
                    if (framecount >= ValidFrameCount) {
                        framecount = 0;
                        if (FrameImage != null) {
                            FrameImage.Dispose();
                        }
                        FrameImage = new Image<Bgr, byte>(eventArgs.Frame);
                        GotFrame = true;
                        waitImage.Set(); 
                    } else {
                        framecount++;
                    }
                }
            } catch (Exception exp) {

                FrameImage = null;
            }
        }


        private AutoResetEvent waitImage = new AutoResetEvent(false);
        private object lockobject = new object();
        //       private Boolean capture = false;
        public override Image<Bgr, Byte> GetImage() {

            if (Camera==null) {
                return null;
            }
            waitImage.Reset();
            GotFrame = false;
            waitImage.WaitOne(3000);
            Image<Bgr, Byte> newimage = null;


            if (FrameImage != null) {
                newimage = new Image<Bgr, byte>(FrameImage.Size);               
                FrameImage.CopyTo(newimage);
            }

            Console.WriteLine("Capture Done");
            // _Camera.Stop();

            return newimage;

        }


      

        public override string ToString() {
            return "Direct Show Capture";
        }

        public DirectShowCameraCapture(VisionProject selectedproject) {
            base.SelectedProject = selectedproject;
            this.CameraName = "Direct Show input";
            this.Camtype = CameraTypes.DirectShow;
        }

        public DirectShowCameraCapture() {

            InitCameras();
            this.CameraName = "Direct Show input";
            this.Camtype = CameraTypes.DirectShow;
            
        }


        public override void Dispose() {
            
        }

    }

    public class CVCameraCapture : BaseCapture {

        private static KPPLogger log = new KPPLogger(typeof(CVCameraCapture));
        decimal _CamIndex = -1;
        [XmlAttribute("Camera Index")]
        public decimal CamIndex {
            get {
                return _CamIndex;
            }
            set {
                if (_CamIndex != value) {
                    try {
                        if (Camera != null) {
                            Camera.Dispose();
                            Camera = null;
                        }
                        _CamIndex = value;
                    } catch (Exception exp) {
                        _CamIndex = -1;
                        log.Error(exp);
                    }


                }
            }
        }

        readonly UndoRedo<double> _exposure = new UndoRedo<double>();
        [XmlAttribute]
        public double Exposure {
            get {

                return _exposure.Value;
            }
            set {
                if (_exposure.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.CameraName + " Exposure changed to:" + value)) {
                            _exposure.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }

                } else {

                    _exposure.Value = value;
                }
            }
        }


        Capture _Camera = null;
        [XmlIgnore]
        [Browsable(false)]
        public Capture Camera {
            get {
                return _Camera;
            }
            set {
                if (_Camera != value) {
                    _Camera = value;
                }
            }
        }



        private object lockobject = new object();
        private Image<Bgr, Byte> frameImage = null;
        private Boolean capture = false;
        public override Image<Bgr, Byte> GetImage() {


            if (Camera == null) {



                Camera = new Capture(0);
                Camera.ImageGrabbed += new Capture.GrabEventHandler(Camera_ImageGrabbed);
                Camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 320);
                Camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 240);

                //Camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640);
                //Camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480);
                //Camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FPS, 1); 
                //Camera.Start();
                Camera.Grab();


            }

            Image<Bgr, Byte> newimage = null;
            //frameImage = null;
            //Console.WriteLine("Capture Start");
            //capture = true;
            //do {

            //    lock (lockobject) {
            //        if (frameImage!=null) {
            //            newimage = frameImage;
            //            break;
            //        }
            //    }
            //    CvInvoke.cvWaitKey(1);

            //} while (true);

            //capture = false;

            //Image<Bgr, Byte> newimage = Camera.QueryFrame().Convert<Bgr,Byte>();

            for (int i = 0; i < 5; i++) {
                Camera.Grab();
                CvInvoke.cvWaitKey(30);
                //Camera.RetrieveBgrFrame();
                //CvInvoke.cvWaitKey(5);
            }

            newimage = Camera.RetrieveBgrFrame();

            //    string _appfile = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "temp.bmp");
            //   newimage.Save(_appfile);
            Console.WriteLine("Capture Done");
            return newimage;

        }

        private int framecounter = 0;
        void Camera_ImageGrabbed(object sender, EventArgs e) {

        }

        public override string ToString() {
            return "OpenCV Capture";
        }

        public CVCameraCapture(VisionProject selectedproject) {
            base.SelectedProject = selectedproject;
        }

        public CVCameraCapture() {
            CamIndex = -1;
            this.CameraName = "Opencv input";
            this.Camtype = CameraTypes.CV;
            
        }

        public override void Dispose() {
            if (Camera != null) {
                Camera.Stop();
                Camera = null;

                Console.WriteLine("Camera Stopped");
            }
        }

    }




    
    [XmlInclude(typeof(DirectShowCameraCapture))]
    [XmlInclude(typeof(CVCameraCapture))]
    [XmlInclude(typeof(ICSCameraCapture))]
    [XmlInclude(typeof(FileCapture))]
    [XmlInclude(typeof(InspectionCapture))]
    [XmlInclude(typeof(uEyeCamera))]
    [Serializable]
    public class BaseCapture {




        public enum CameraTypes { Remote, DirectShow, CV, ICS, File, Inspection, Request, uEye, Undef }


        private static KPPLogger log = new KPPLogger(typeof(BaseCapture));
        

        private String m_ModuleName;
        [XmlAttribute, Browsable(false)]
        public virtual String ModuleName {
            get { return m_ModuleName; }
            set {
                if (m_ModuleName != value) {
                    m_ModuleName = value;
                    
                }
            }
        }


        CameraTypes _camtype = CameraTypes.Undef;
        [XmlAttribute, ReadOnly(true)]
        public CameraTypes Camtype {
            get { return _camtype; }
            set {
                if (_camtype != value) {
                    _camtype = value;



                }
            }
        }

        [XmlIgnore,Browsable(false)]
        public VisionProject SelectedProject;

        public virtual void UpdateSource() {

        }

        //private Channel _UseChannel = Channel.Bgr;
        //[XmlAttribute, DisplayName("Output channel")]
        //public virtual Channel UseChannel {
        //    get { return _UseChannel; }
        //    set { _UseChannel = value; }
        //}



        String _CameraName = "";
        [XmlAttribute, DisplayName("Camera Name"), ReadOnly(false), Browsable(false)]
        public virtual String CameraName {
            get {
                return _CameraName;
            }
            set {
                if (_CameraName != value) {

                    String oldname = _CameraName;

                    _CameraName = value;





                }
            }
        }

        internal delegate void AcquisitionAttributesChanged();
        internal event AcquisitionAttributesChanged OnAcquisitionAttributesChanged;

        public virtual void UpdateAttributes() {
            if (OnAcquisitionAttributesChanged != null) {
                OnAcquisitionAttributesChanged();
            }
        }

        public virtual Image<Bgr, Byte> GetImage(Boolean FullImage) {
            return null;
        }


        public virtual Image<Bgr, Byte> GetImage(int cam, Boolean Capture) {
            return null;
        }


        public virtual Image<Bgr, Byte> GetImage() {

            return null;
        }

        



        public BaseCapture() {
            try {


            } catch (Exception exp) {

                log.Error(exp);
            }


        }

        public override string ToString() {
            return CameraName;
        }

        public virtual void Dispose() {
            OnAcquisitionAttributesChanged = null;
        }
    }

}
