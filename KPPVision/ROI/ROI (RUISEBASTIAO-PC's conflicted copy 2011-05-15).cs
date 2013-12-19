using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using KPP.Controls.Winforms.ImageEditorObjs;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Drawing.Imaging;
using System.Collections;




namespace ClassInspection {


    public delegate void ROINameChanging(ROI roi,String OldValue,String NewValue);
    public delegate void ROINameChanged(ROI roi, String NewValue);
    

    public class ROI : IDisposable {

        #region class ROI


        public List<ProcessingFunctionBase> ProcessingFunctions { get; set; }

        [XmlIgnore]
        public  ShapeRaster _ROIShape;
        private Layer _ROILayer;
        private Image<Rgb, Byte> _roiimageRGB;
        private String _name="";
        private Boolean _pass = false;
        private String _useresult;
        private PixelFormat _ROIImageFormat;

        public event ROINameChanging OnROINameChanging;
        public event ROINameChanged OnROINameChanged;    


        public delegate void UpdateROIImageHandler();

        
        public event PropertyChangedEventHandler PropertyChanged;



        [XmlIgnore]
        [ReadOnly(true)]
        public PixelFormat ROIImageFormat {
            get { return _ROIImageFormat; }
            set { if (_ROIImageFormat != value) _ROIImageFormat = value; }
        }

       // [XmlIgnore]
        //public ResultsClass Results { get; set; }

        [XmlAttribute]
        public String UseResult {
            get { return _useresult; }
            set {
                if (_useresult != value) {
                    _useresult = value;


                }
            }
        }



        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }

        [XmlIgnore]
        public Boolean Pass {
            get { return _pass; }
        }
        
        [XmlIgnore]
        public Image<Rgb, Byte> ROIImageInRGB {
            get { return _roiimageRGB; }
            set { _roiimageRGB = value; }
        }

        

        [XmlAttribute]
        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    
                    if (OnROINameChanging != null)
                        OnROINameChanging(this,_name,value);

                    _name = value;

                    if (OnROINameChanged != null)
                        OnROINameChanged(this, _name);


                    if (_ROIShape != null) {
                        _ROIShape.Name = value;
                    }
                    OnPropertyChanged("ROIName");
                }
            }
        }
                
        public Rectangle ROIRectangle {
            get { return _ROIShape.ShapeEfectiveBounds; }
            set {
                _ROIShape.ShapeEfectiveBounds = value;
                OnPropertyChanged("ROIRectangle");

            }
        }

        public void AddProcessingFunction(ProcessingFunctionBase _proc) {
            //_proc.PropertyChanged += new PropertyChangedEventHandler(_proc_PropertyChanged);
            _proc.ROIImageFormat = this.ROIImageFormat;
            ProcessingFunctions.Add(_proc);
        }



        public void UpdateROIShape(Layer ROILayer) {

            _ROIShape.Name = this.Name;
            _ROIShape.ShowRasterBorder = true;
            _ROIShape.ShapeEfectiveBounds = ROIRectangle;

            ROILayer.AddShape(_ROIShape);

            _ROILayer = ROILayer;

            
            foreach (ProcessingFunctionBase _proc in ProcessingFunctions) {
                _proc.ROIImageFormat = this.ROIImageFormat;
                
            }


        }
        

        

        public void ProcessRoi() {

            _pass = true;
            foreach (ProcessingFunctionBase _procfunc in ProcessingFunctions) {

                if (ROIImageInRGB != null) {
                    _procfunc.Process(ROIImageInRGB, _ROIShape);
                    if (!_procfunc.Pass) _pass = false;
                }           

            }
        }


        public ROI(ShapeRaster ROIShape, Layer ROILayer) {

            ROIShape.Image=null;
            _roiimageRGB = new Image<Rgb, byte>(new Size());
            ROILayer.AddShape(ROIShape);
            Name = ROIShape.Name;
            _ROIShape = ROIShape;
             
            _ROIShape.ShowRasterBorder = true;
            _ROIShape.ClearImageOnLocationChange = true;
            _ROIShape.ClearImageOnSizeChange = true;
            _ROIShape.BorderThickness = 2;
            

            _ROILayer = ROILayer;
            if (ProcessingFunctions == null)
                ProcessingFunctions = new List<ProcessingFunctionBase>();

        }

        public ROI() {

            //AddProcessingFunctions();

            _ROIShape = new ShapeRaster();
            _ROIShape.BorderColor = Color.Green;            
            _ROIShape.ShowRasterBorder = true;
            _ROIShape.ClearImageOnLocationChange = true;
            _ROIShape.ClearImageOnSizeChange = true;
            _ROIShape.ShapeEfectiveBounds = new Rectangle();
            ProcessingFunctions = new List<ProcessingFunctionBase>();

        }


        public void Dispose() {
            foreach (ProcessingFunctionBase _proc in ProcessingFunctions) {
                _proc.Dispose();   
            }
        }

        #endregion
    }



    public class PropertySetEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(
            ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value) {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (wfes != null) {
                PropertySetForm _PropertySetForm = new PropertySetForm();

                _PropertySetForm.trackBar1.Value = (int)value;
                _PropertySetForm.BarValue = _PropertySetForm.trackBar1.Value;
                _PropertySetForm._wfes = wfes;


                wfes.DropDownControl(_PropertySetForm);

                if (_PropertySetForm._BtPressed) {
                    value = _PropertySetForm.BarValue;
                }


            }
            return value;
        }
    }

    public class BitmapOutPropertyViewer : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(
            ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value) {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (wfes != null) {
                BitmapOutProperty _BitmapOutProperty = new BitmapOutProperty();

                _BitmapOutProperty.__pictureBox1.Image = (Bitmap)value;
                _BitmapOutProperty._wfes = wfes;
                wfes.DropDownControl(_BitmapOutProperty);


            }
            return value;
        }
    }

}
