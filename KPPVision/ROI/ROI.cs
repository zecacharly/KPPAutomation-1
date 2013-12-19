using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Drawing.Imaging;
using System.Collections;
using KPP.Core.Debug;
using KPP.Controls.Winforms.ImageEditorObjs;
using DejaVu;
using DejaVu.Collections.Generic;
using KPPAutomationCore;




namespace VisionModule {


    
    public delegate void ROINameChanged(ROI roi, String NewValue);

    

    public class ROI : IDisposable {



        private static KPPLogger log = new KPPLogger(typeof(Inspection));


        private String m_ModuleName;
        [XmlAttribute,Browsable(false)]
        public String ModuleName {
            get { return m_ModuleName; }
            set {
                if (m_ModuleName != value) {                  

                    m_ModuleName = value;

                }
            }
        }

        #region class ROI

        

      //  public delegate void ROIPositionChanged(ROI roi,int OldPos, int NewPos);
       // public event ROIPositionChanged OnROIPositionChanged;

        public class ProcessingFunctionsList : UndoRedoList<ProcessingFunctionBase> {

        }

        public ProcessingFunctionsList ProcessingFunctions { get; set; }

        

        ShapeRectangle _ROIShape = new ShapeRectangle();
        [XmlIgnore]
        public ShapeRectangle ROIShape {
            get { return _ROIShape; 
            }
            set {
               
                _ROIShape = value; 
            }
        }


        readonly UndoRedo<Rectangle> _UndoRedoRectangle = new UndoRedo<Rectangle>();

        [XmlIgnore]
        [Browsable(false)]
        public Rectangle UndoRedoRectangle {
            get {
                return _UndoRedoRectangle.Value;
            }

            set {
              
                    _UndoRedoRectangle.Value = value;
                     
                
            }
        }
        [NonSerialized]
        private Layer _ROILayer;
        [NonSerialized]
        private Image<Bgr, Byte> _roiimageBgr;
        [NonSerialized]
        private String _useresult;
        [NonSerialized]
        private PixelFormat _ROIImageFormat;

        //public event ROINameChanging OnROINameChanging;
        public event ROINameChanged OnROINameChanged;
        

        public delegate void UpdateROIImageHandler();

        
        public event PropertyChangedEventHandler PropertyChanged;
       

        readonly UndoRedo<int> _ROIPos= new UndoRedo<int>(1);

        [XmlAttribute]
        [DisplayName("Position"),Browsable(false)]
        public int ROIPos {
            get {
                return _ROIPos.Value;
            }
            set {
                if (_ROIPos.Value != value && value > 0) {
                    int oldval=_ROIPos.Value;

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.Name + " Position changed to:" + value)) {
                            _ROIPos.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _ROIPos.Value = value;
                    }
                    //if (OnROIPositionChanged!=null) {
                    //    OnROIPositionChanged(this,oldval, value);
                    //}
                }

            }
        }


        //internal void UpdatePosition(int position) {
        //    using (UndoRedoManager.StartInvisible("Init")) {
        //        this._ROIPos.Value = position;
        //        UndoRedoManager.Commit();
        //    }
        //}

        [XmlIgnore]
        [ReadOnly(true)]        
        public PixelFormat ROIImageFormat {
            get { return _ROIImageFormat; }
            set { if (_ROIImageFormat != value) _ROIImageFormat = value; }
        }



        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }

        

        //private Boolean _noPartCheck = false;

        //public Boolean NoPartCheck {
        //    get { return _noPartCheck; }
        //    set { _noPartCheck = value; }
        //}


    
        Boolean _Locked = false;

        [XmlAttribute]
        public Boolean Locked{
            get { 
                return _Locked; }
            set {
                if (_Locked != value) {
                    try {

                        _Locked = value;
                        this.ROIShape.Locked = _Locked;
                     
                    } catch (Exception exp) {      
                        log.Error(exp);
                    }
                }
            }
        }

        readonly UndoRedo<String> _name = new UndoRedo<string>();

        [XmlAttribute]
        public string Name {
            get { return _name.Value; }
            set {
                if (_name.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start(this.Name + " Name changed to:" + value)) {
                            _name.Value = value;
                            UndoRedoManager.Commit();
                        }
                    } else {
                        _name.Value = value;
                    }

                    if (OnROINameChanged != null)
                        OnROINameChanged(this, value);


                    if (ROIShape != null) {
                        ROIShape.Name = value;
                    }
                    OnPropertyChanged("ROIName");
                }
            }
        }
                
        public Rectangle ROIRectangle {
            get { return ROIShape.ShapeEfectiveBounds; }
            set {
                ROIShape.ShapeEfectiveBounds = value;
                OnPropertyChanged("ROIRectangle");

            }
        }

        readonly UndoRedo<ReferencePoint> _ReferencePoint = new UndoRedo<ReferencePoint>(new ReferencePoint());
        [CategoryAttribute("\tPre-Positioning"), DisplayName("Reference Point")]
        public ReferencePoint referencePoint {
            get {
                return _ReferencePoint.Value;
            }
            set {
                if (_ReferencePoint.Value != value) {
                    _ReferencePoint.Value = value;
                }
            }
        }

        public override string ToString() {
            return this.Name;
        }

        public void AddProcessingFunction(ProcessingFunctionBase _proc) {
            _proc.ROIImageFormat = this.ROIImageFormat;
            using (UndoRedoManager.Start(Name + ": Processing Function added: " + _proc.FunctionName)) {
                ProcessingFunctions.Add(_proc);
                UndoRedoManager.Commit();
            }
            
        }


        

        public void UpdateROIShape(Layer ROILayer) {

            ROIShape.Name = this.Name;
            //_ROIShape.ShowRasterBorder = true;
            ROIShape.ShapeEfectiveBounds = ROIRectangle;

            ROILayer.AddShape(ROIShape);


            using (UndoRedoManager.StartInvisible("Init")) {
                UndoRedoRectangle = ROIShape.ShapeEfectiveBounds;
                UndoRedoManager.Commit();
            }
                   
            
            foreach (ProcessingFunctionBase _proc in ProcessingFunctions) {
                _proc.ROIImageFormat = this.ROIImageFormat;
                
            }


        }


        public void ClearEvents() {            
            OnROINameChanged=null;            
        }

        private Rectangle RefLoc = Rectangle.Empty;

        public Boolean ProcessRoi(Image<Bgr, Byte> ROIImageIn, Image<Bgr, Byte> ROIImageOut) {
            Boolean _pass = false;
            try {

                

                    var SortedProcessingFunctions = ProcessingFunctions.OrderBy(c => c.ProcPos);




                    //foreach (ProcessingFunctionBase _procfunc in SortedProcessingFunctions) {

                    for (int i = 0; i < SortedProcessingFunctions.Count(); i++) {

                        ProcessingFunctionBase _procfunc = SortedProcessingFunctions.ElementAt(i);
                        if (!_procfunc.Active) {
                            continue;
                        }
                        if (ROIImageIn != null && ROIImageOut != null) {
                            
                            Rectangle RoiRect;
                            if (this.referencePoint.ReferencePointName != "N/A") {
                                if (RefLoc== Rectangle.Empty) {
                                    RefLoc = ROIShape.ShapeEfectiveBounds;
                                }
                                RoiRect = new Rectangle((int)(RefLoc.X + this.referencePoint.OffsetX), (int)(RefLoc.Y + this.referencePoint.OffsetY), ROIShape.ShapeEfectiveBounds.Width, ROIShape.ShapeEfectiveBounds.Height);
                                ROIShape.ShapeEfectiveBounds = RoiRect;
                            }
                            else {
                                RefLoc = Rectangle.Empty;
                                RoiRect = ROIShape.ShapeEfectiveBounds;
                            }
                            ROIImageIn.ROI = Rectangle.Empty;
                            if (RoiRect.X < 0 || RoiRect.Y < 0 || ((RoiRect.X + RoiRect.Width) > ROIImageIn.Width) || ((RoiRect.Y + RoiRect.Height) > ROIImageIn.Height)) {
                                return false;
                            }
                       
                            _pass=_procfunc.Process(ROIImageIn, ROIImageOut, RoiRect);


                        }

                    }



                }
            
            catch (Exception exp) {
                Console.WriteLine(exp);
                log.Error(exp);
                return false;
            }

            return _pass;
        }


        public ROI(ShapeRectangle _roishape, Layer ROILayer, int Pos) {

            _roiimageBgr = new Image<Bgr, byte>(new Size());
            ROILayer.AddShape(_roishape);

            Name = _roishape.Name;


            ROIPos = Pos;




            ROIShape = _roishape;
            // ROIShape.Text = Name;
            ROIShape.TextColor = Color.Red;
            ROIShape.Text = "";

            using (UndoRedoManager.StartInvisible("init")) {
                UndoRedoRectangle = ROIShape.ShapeEfectiveBounds;
                UndoRedoManager.Commit();
            }

            _UndoRedoRectangle.OnMemberUndo += new UndoRedo<Rectangle>.MemberUndo(_UndoRedoRectangle_OnMemberUndo);
            _UndoRedoRectangle.OnMemberRedo += new UndoRedo<Rectangle>.MemberRedo(_UndoRedoRectangle_OnMemberUndo);

            ROIShape.BorderThickness = 2;
            _ReferencePoint.OnMemberUndo += new UndoRedo<ReferencePoint>.MemberUndo(_ReferencePoint_OnMemberUndo);

            _ROILayer = ROILayer;
            if (ProcessingFunctions == null)
                using (UndoRedoManager.StartInvisible("Init")) {
                    ProcessingFunctions = new ProcessingFunctionsList();
                    UndoRedoManager.Commit();
                }

        }


        

      

        public ROI() {

            //AddProcessingFunctions();

        

            if (ROIShape!=null) {

            } else {
                _ROIShape = new ShapeRectangle();
                _ROIShape.ShapeEfectiveBounds = new Rectangle();
            }
            
            _ROIShape.Text = _ROIShape.ToString();
            _ROIShape.BorderColor = Color.Green;
            _ROIShape.Text = "";
            

            ProcessingFunctions = new ProcessingFunctionsList();
            _UndoRedoRectangle.OnMemberUndo += new UndoRedo<Rectangle>.MemberUndo(_UndoRedoRectangle_OnMemberUndo);
            _UndoRedoRectangle.OnMemberRedo += new UndoRedo<Rectangle>.MemberRedo(_UndoRedoRectangle_OnMemberUndo);
            _name.OnMemberRedo += new UndoRedo<string>.MemberRedo(_name_OnMemberRedo);
            _name.OnMemberUndo += new UndoRedo<string>.MemberUndo(_name_OnMemberRedo);
            _ReferencePoint.OnMemberUndo += new UndoRedo<ReferencePoint>.MemberUndo(_ReferencePoint_OnMemberUndo);
        }

        void _ReferencePoint_OnMemberUndo(ReferencePoint UndoObject) {
            if (true) {
                
            }
        }

        void _name_OnMemberRedo(string UndoObject) {

            if (OnROINameChanged != null)
                OnROINameChanged(this, UndoObject);


            if (ROIShape != null) {
                ROIShape.Name = UndoObject;
            }
            OnPropertyChanged("ROIName");

        }

        void _UndoRedoRectangle_OnMemberUndo(Rectangle UndoObject) {
            ROIShape.ShapeEfectiveBounds = UndoObject;            
        }

        void _ROIShape_OnMemberUndo(ShapeRectangle UndoObject) {
        }


        public void Dispose() {
            ClearEvents();
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
