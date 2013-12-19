using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPP.Core.Debug;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using TIS.Imaging;
using System.Reflection;
using VisionModule.Forms;
using AForge.Video.DirectShow;
using KPPAutomationCore;
using System.Collections;

namespace VisionModule {



    public class PropertiesSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();

        
        public PropertiesSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {
            Box1.Items.Clear();


            object inst = context.Instance;

            PropertyInfo[] propinfos = inst.GetType().GetProperties();

            foreach (PropertyInfo item in propinfos) {
                var attributes = item.GetCustomAttributes(typeof(UseInProperties), false);
                if (attributes.Count()>0) {
                    var attribute = (UseInProperties)attributes.First();
                    if (attribute != null) {
                        if (attribute.Use) {
                            object itemvalue = item.GetValue(inst, null);
                            if (itemvalue is ResultReference) {
                                ResultReference resref = itemvalue as ResultReference;
                                if (resref!=null) {
                                    if (resref.ResultOutput is ICollection) {
                                        Type itemlisttype =resref.ResultOutput.GetType().GetProperty("Item").PropertyType;
                                        PropertyInfo[] itempropinfos=itemlisttype.GetProperties();
                                        Box1.Items.AddRange(itempropinfos.Select(n => n.Name).ToArray());
                                    } else {

                                    }
                                }
                            }
                        }
                    } 
                }
                
            }

            //Box1.Items.AddRange(captures);
          
            Box1.Height = Box1.PreferredHeight;

           
           
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {



                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem != null) {
                    return Box1.SelectedItem;
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }



    public class InputReferenceSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        //UserControl contextcontrol = StaticObjects.InputItemSelectorControl;
        private static KPPLogger log = new KPPLogger(typeof(InputReferenceSelector));
        internal OutputResultConfForm _outputresultform = new OutputResultConfForm();

        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();





        public InputReferenceSelector() {
            //menu.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            //  textbox1.KeyDown += new KeyEventHandler(textebox1_KeyDown);

            //contextcontrol.VisibleChanged += new EventHandler(contextcontrol_VisibleChanged);
        }




        void textebox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                edSvc.CloseDropDown();
            }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

            VisionProject selected = (VisionProject)context.Instance.GetPropertyValue("SelectedVisionProject");

            AttributeCollection attrs = context.PropertyDescriptor.Attributes;

            Type resultType = null;
            for (int i = 0; i < attrs.Count; i++) {
                if (attrs[i] is AcceptType) {
                    resultType = (attrs[i] as AcceptType).acceptType;
                    break;
                }
            }

            if (resultType == null) {
                //log.Error("Result Type not set in Function: " + procbase.FunctionName+"("+procbase.FunctionType+")");
                log.Error("Result Type not set in Function: " + context.Instance.ToString());
                return value;
            }



            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {

                _outputresultform.SelectedProject = selected;
                _outputresultform.acceptType = resultType;
                _outputresultform.ResultRef = (value as ResultReference);
                if (edSvc.ShowDialog(_outputresultform) == DialogResult.OK) {
                    value = _outputresultform.ResultRef;
                }


                //if (textbox1.Text != "") {
                //    newresref.ResultReferenceID = textbox1.Text;
                //    newresref.ResultOutput = double.Parse(textbox1.Text);
                //    return newresref;
                //}

            }
            return value;
        }



    }


    public class ZoneSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        //ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public ZoneSelector() {


        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {


            if (!(context.Instance is InspectionCapture)) {
                return value;
            }

            InspectionCapture inspcap = (InspectionCapture)(context.Instance);

            //inspcap.InspectionSource.

            ZoneSelectorForm zoneform = new ZoneSelectorForm(inspcap.InspectionSource.SelectedProject);

            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {
                zoneform.__image.Image = inspcap.GetImage(true).ToBitmap();
                zoneform.__numericCol.Value = inspcap.InspectionSource.SelectedProject.SelectedRequest.ImageCol;
                zoneform.__numericLines.Value = inspcap.InspectionSource.SelectedProject.SelectedRequest.ImageLine;
                zoneform.SelectedZone = inspcap.CaptureZone;

                if (edSvc.ShowDialog(zoneform) == DialogResult.OK) {
                    return zoneform.SelectedZone;
                }


            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }


    public class RefPointSelect : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox ListRefPoints = new ListBox();
        IWindowsFormsEditorService edSvc;



        internal static ROI SelROI = null;

        public RefPointSelect() {
            ListRefPoints.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            ListRefPoints.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {
            try {
                ListRefPoints.Items.Clear();
                // ListRefPoints.Items.AddRange(StaticObjects.ReferencePoints.ToArray());
                ListRefPoints.Height = ListRefPoints.PreferredHeight;

                // Uses the IWindowsFormsEditorService to 
                // display a drop-down UI in the Properties 
                // window.
                SelROI = (ROI)context.Instance;
                edSvc =
                   (IWindowsFormsEditorService)provider.
                   GetService(typeof
                   (IWindowsFormsEditorService));
                if (edSvc != null) {
                    edSvc.DropDownControl(ListRefPoints);
                    if (ListRefPoints.SelectedItem == null) {
                        return SelROI.referencePoint;
                    }
                    return ListRefPoints.SelectedItem;

                }
                return value;
            } catch (Exception exp) {

                return null;
            }
        }

        private void Box1_Click(object sender, EventArgs e) {
            //if (Box1.SelectedItem !=null) {
            //    if (SelectedInsp!=null) {
            //        SelectedInsp.CaptureSource = (CameraCapture)Box1.SelectedItem;   
            //    }
            //}
            edSvc.CloseDropDown();
        }
    }


    #region Capture



    public class InputSourceSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();

        internal static BaseCapture CamSource;



        public InputSourceSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {
            Box1.Items.Clear();

            // TODO dynamic captures selection

            String[] captures = new String[] { "File", "Inspection", "ICSCamera", "CVCamera", "DirectShowCamera", "uEyeCamera" };
            Box1.Items.AddRange(captures);
            //Box1.Items.Add(new CameraInfo("Select File", CameraInfo.CameraTypes.File));
            Box1.Height = Box1.PreferredHeight;

            //if (context.Instance is RemoteCameraCapture) {
            //    CamSource = (BaseCapture)context.Instance;
            //    Box1.Items.Remove((StaticObjects.CaptureSources.Find(cap => cap.GetType() == typeof(RemoteCameraCapture))));
            //}
            //else {
            CamSource = (BaseCapture)((Inspection)context.Instance).CaptureSource;
            VisionProject selectedproject = ((Inspection)context.Instance).SelectedProject;
            // }


            //if (!StaticObjects.CaptureSources.Contains(CamSource)) {
            //    StaticObjects.CaptureSources.Add(CamSource);
            //}



            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {



                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {
                    return value;
                } else {
                    String caminfo = (String)Box1.SelectedItem;

                    if (caminfo == "File") {


                        FileCapture newfilecap = new FileCapture(selectedproject);

                        return newfilecap;


                    } else if (caminfo == "Inspection") {


                        InspectionCapture newfilecap = new InspectionCapture(selectedproject);
                        //newfilecap.OnCaptureInspectionNameChanged += new InspectionCapture.CaptureInspectionNameChanged(newInspcap_OnCaptureInspectionNameChanged); ;

                        return newfilecap;


                    } else if (caminfo == "ICSCamera") {

                        return new ICSCameraCapture(selectedproject);

                    } else if (caminfo == "CVCamera") {
                        return new CVCameraCapture(selectedproject);
                    }
                        //else if (caminfo is PythonRemoteCapture) {
                        //    if (CamSource != null) {
                        //        return (BaseCapture)Box1.SelectedItem;
                        //    }
                        //    else {
                        //        return new PythonRemoteCapture(caminfo.SelectedProject);
                        //    }
                        //}
                      else if (caminfo == "DirectShowCamera") {
                        return new DirectShowCameraCapture(selectedproject);
                    } else if (caminfo == "uEyeCamera") {

                        return new uEyeCamera(selectedproject);

                    } else {
                        return value;
                    }
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }




    public class DirectShowSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public DirectShowSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

            if (!(context.Instance is DirectShowCameraCapture)) {
                return "";
            }

            DirectShowCameraCapture DirectShowCamSource = (DirectShowCameraCapture)(context.Instance);


            Box1.Items.Clear();
            Box1.DisplayMember = "Name";
            Box1.ValueMember = "MonikerString";
            foreach (FilterInfo item in DirectShowCameraCapture.DevicesAvaible) {
                Box1.Items.Add(item);

            }


            Box1.Height = Box1.PreferredHeight;


            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {


                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {
                    return value;
                } else {
                    return Box1.SelectedItem;
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }



    public class InspNameSelector : System.Drawing.Design.UITypeEditor {

        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;

        //void Update_result_requests(String name, int id) {
        //    StaticObjects.SelectedProject.RequestList.ForEach(req => req.Inspections.ForEach(insp => insp.Results.ForEach(resinf => resinf.AddOverrideID(name, id))));
        //}


        public InspNameSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {


            InspectionCapture selectedcap = (InspectionCapture)context.Instance;


            Box1.Items.Clear();

            foreach (Request req in selectedcap.SelectedProject.RequestList) {
                foreach (Inspection insp in req.Inspections) {
                    if (insp.ID != selectedcap.SelectedProject.SelectedRequest.ID) {
                        Box1.Items.Add(insp);
                    } else {
                        if (req.ID != selectedcap.SelectedProject.SelectedRequest.ID) {
                            Box1.Items.Add(insp);
                        }
                    }

                }
            }



            Box1.Height = Box1.PreferredHeight;


            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {


                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {



                    return value;
                } else {
                    Inspection ins = Box1.SelectedItem as Inspection;

                    if (ins != null) {
                        ins.SelectedProject.SelectedRequest.SelectedInspection.ROIList.AuxROIS = ins.ROIList.ToList();
                    }


                    return (Box1.SelectedItem);
                }

            }
            return value;
        }

        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }


    public class InterfaceValueSelector : System.Drawing.Design.UITypeEditor {



        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list


        internal static ICSCameraCapture SelectedCamera = null;

        public InterfaceValueSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }
        private static KPPLogger log = new KPPLogger(typeof(InterfaceValueSelector));
        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {


            if (!(context.Instance is ICSCameraCapture)) {
                return value;
            }
            SelectedCamera = (ICSCameraCapture)context.Instance;

            try {

                Box1.Items.Clear();

                //Box1.Height = Box1.PreferredHeight;



                if (SelectedCamera == null) {
                    return value;
                }

                VCDRangeProperty selectedinterface = SelectedCamera._ICSCamera.Zoominterface;

                object returnval = -1;
                if (context.PropertyDescriptor.DisplayName == "Zoom") {
                    if (SelectedCamera._ICSCamera.Zoominterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Zoominterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.Zoom;
                } else if (context.PropertyDescriptor.DisplayName == "Exposure") {
                    if (SelectedCamera._ICSCamera.Exposureinterface == null) {
                        return value;
                    }
                    Box1.Items.Add(SelectedCamera.GetIntValue());
                    returnval = SelectedCamera.Exposure;
                } else if (context.PropertyDescriptor.DisplayName == "Focus") {
                    if (SelectedCamera._ICSCamera.Focusinterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Focusinterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.Focus;
                } else if (context.PropertyDescriptor.DisplayName == "Iris") {
                    if (SelectedCamera._ICSCamera.Irisinterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Irisinterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.Iris;
                } else if (context.PropertyDescriptor.DisplayName == "Gain") {
                    if (SelectedCamera._ICSCamera.Gaininterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Gaininterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.Gain;
                } else if (context.PropertyDescriptor.DisplayName == "Blue Balance") {
                    if (SelectedCamera._ICSCamera.Bluebalanceinterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Bluebalanceinterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.BlueBalance;
                } else if (context.PropertyDescriptor.DisplayName == "Green Balance") {
                    if (SelectedCamera._ICSCamera.Greenbalanceinterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Greenbalanceinterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.GreenBalance;
                } else if (context.PropertyDescriptor.DisplayName == "Red Balance") {
                    if (SelectedCamera._ICSCamera.Redbalanceinterface == null) {
                        return value;
                    }
                    selectedinterface = SelectedCamera._ICSCamera.Redbalanceinterface;
                    Box1.Items.Add(selectedinterface.Value);
                    returnval = SelectedCamera.RedBalance;
                } else if (context.PropertyDescriptor.DisplayName == "PixelFormat") {
                    Box1.Items.AddRange(SelectedCamera._ICSCamera.Camera.VideoFormats.Select(format => format.Name).ToArray());
                    Box1.SelectedItem = SelectedCamera._ICSCamera.Camera.VideoFormatCurrent.Name;
                    returnval = SelectedCamera._ICSCamera.Camera.VideoFormatCurrent.Name;
                }


                edSvc =
                   (IWindowsFormsEditorService)provider.
                   GetService(typeof
                   (IWindowsFormsEditorService));
                if (edSvc != null) {
                    edSvc.DropDownControl(Box1);
                    if (Box1.SelectedItem == null) {
                        return returnval;
                    }

                    return Box1.SelectedItem;

                }
                return value;
            } catch (Exception exp) {
                log.Error(exp);

                return value;

            }
        }

        private void Box1_Click(object sender, EventArgs e) {
            //if (Box1.SelectedItem !=null) {
            //    if (SelectedInsp!=null) {
            //        SelectedInsp.CaptureSource = (CameraCapture)Box1.SelectedItem;   
            //    }
            //}
            edSvc.CloseDropDown();
        }
    }

    public class uEyeCameraSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public uEyeCameraSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

            if (!(context.Instance is uEyeCamera)) {
                return "";
            }

            uEyeCamera uEyeCamSource = (uEyeCamera)(context.Instance);


            Box1.Items.Clear();



            uEye.Types.CameraInformation[] cameraList;
            uEye.Info.Camera.GetCameraList(out cameraList);

            foreach (uEye.Types.CameraInformation info in cameraList) {

                Box1.Items.Add(info.Model.Replace("\0", "") + "#" + info.SerialNumber.Replace("\0", ""));
            }

            Box1.Height = Box1.PreferredHeight;


            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {


                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {
                    return value;
                } else {
                    return Box1.SelectedItem;
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }


    public class uEyeCameraConfigSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        //ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public uEyeCameraConfigSelector() {


        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

            if (!(context.Instance is uEyeCamera)) {
                return value;
            }

            uEyeCamera uEyeCamSource = (uEyeCamera)(context.Instance);



            CameraControlForm camctr = new CameraControlForm(uEyeCamSource.Camera);

            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {


                if (edSvc.ShowDialog(camctr) == DialogResult.OK) {


                    if (context.PropertyDescriptor.DisplayName == "Exposure") {
                        double outval;
                        uEyeCamSource.Camera.Timing.Exposure.Get(out outval);
                        return outval;
                    } else if (context.PropertyDescriptor.DisplayName == "PixelClock") {
                        int outval;
                        uEyeCamSource.Camera.Timing.PixelClock.Get(out outval);
                        return outval;
                    } else if (context.PropertyDescriptor.DisplayName == "Framerate") {
                        double outval;
                        uEyeCamSource.Camera.Timing.Framerate.Get(out outval);
                        return outval;
                    }

                }


            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }


    public class ICSCameraSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public ICSCameraSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

            if (!(context.Instance is ICSCameraCapture)) {
                return "";
            }

            ICSCameraCapture ICSCamSource = (ICSCameraCapture)(context.Instance);


            Box1.Items.Clear();



            foreach (ICImagingControl item in ICScamera.ICSDevices) {
                Box1.Items.Add(item.Device);
            }


            Box1.Height = Box1.PreferredHeight;


            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {


                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {
                    return ICSCamSource.CameraName;
                } else {
                    return Box1.SelectedItem;
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    } 
    #endregion

}
