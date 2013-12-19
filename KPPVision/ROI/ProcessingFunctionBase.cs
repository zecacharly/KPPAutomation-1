using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Reflection;
using KPP.Controls.Winforms.ImageEditorObjs;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using DejaVu;
using System.Windows.Forms.DataVisualization.Charting;
using KPP.Core.Debug;
using KPPAutomationCore;



namespace VisionModule {



    public enum Channel { Bgr, Red, Green, Blue,Mono}




    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class UseInProperties : System.Attribute {

        public Boolean Use;

        public UseInProperties(Boolean use) {
            this.Use = use;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class UseInRef : System.Attribute {
        
        public Boolean Use;

        public UseInRef(Boolean use) {
            this.Use = use;            
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class UseInResultInput : System.Attribute {

        public Boolean Use;

        public UseInResultInput(Boolean use) {
            this.Use = use;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AcceptType : System.Attribute {

        public Type acceptType;

        public AcceptType(Type accepttype) {
            this.acceptType = accepttype;
            
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AllowDrag : System.Attribute {

        public Boolean Use;

        public AllowDrag(Boolean use) {
            this.Use = use;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AcceptDrop : System.Attribute {

        public Boolean Use;

        public AcceptDrop(Boolean use) {
            this.Use = use;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class UseInPyton : System.Attribute {

        public Boolean Use;

        public UseInPyton(Boolean use) {
            this.Use = use;
        }
    }

    /*
   
    */
    [ProcessingFunction("Coordinate System", "Coordinate System")]
    public class CoordinateSystems{

        private static KPPLogger log = new KPPLogger(typeof(CoordinateSystems));



        #region Pre-Processing

        ResultReference _Center;
        [AcceptDrop(true)]
        [Description("Center point of Coordinate System"), DisplayName("Center"), AcceptType(typeof(Point))]
        public ResultReference Center {
            get {
                return _Center;
            }
            set {
                _Center = value;
            }
        }


        ResultReference _Angle;
        [AcceptDrop(true)]
        [Description("Angle of Coordinate System"), DisplayName("Angle"),AcceptType(typeof(double))]
        public ResultReference Angle {
            get {
                //if (_Angle!=null) {

                //    if (_Angle.ResultOutput!=null) {

                //    }
                //}
                return _Angle;
            }
            set {
                _Angle = value;
            }
        }





        #endregion


        public CoordinateSystems() {

        }



    }


    [ProcessingFunction("Set Value", "Other")]
    public class SetValue : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(SetValue));
        public SetValue() {

            
        }

        



        #region Pre-Processing

        [XmlIgnore, Browsable(false)]
        public override ContourPreProc ContourPreProc1 { get; set; }

        [XmlIgnore, Browsable(false)]
        public override ImagePreProc ImagePreProc1 { get; set; }

        

        private ResultReference _Input;
        [Category("Pre-Processing"), TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Input"), AcceptDrop(true), AcceptType(typeof(CustomCollection<>))]
        public ResultReference Input {
            get { return _Input; }
            set {
                _Input = value;
            }
        }

        #endregion



        #region Post Processing

        private ResultReference _Output = null;
        [Category("Post-Processing"), AllowDrag(true), UseInRef(true), UseInResultInput(true), DisplayName("Output"),AcceptType(typeof(object))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ResultReference Output {
            get {
                return _Output;
            }
            set {
                _Output = value;
            }
        }



        #endregion

        public override void UpdateRegionToHighligth(object Region) {
            if (Region is BlobInfo) {
                BlobInfo blobinf = Region as BlobInfo;
                base.IdentRegion = new Image<Gray, byte>(base.BaseRoi.Size);
                base.IdentRegion.Draw(blobinf.BoundingBox, new Gray(255), 2);
            }
        }

        public override Boolean Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                base.Process(ImageIn, ImageOut, RoiRegion);
                
                Output = null;

                if (Input == null) {
                    return false;
                }

                Input.UpdateValue();

                if (Output == null) {
                    return false;
                }
                Output.UpdateValue();

                Output.ResultOutput = Input.ResultOutput;
                

            } catch (Exception exp) {
                log.Error(exp);
                return false;
            }




            return true;


        }


    }



    


    public class Customconverter<T> : ExpandableObjectConverter {

        public override object ConvertTo(ITypeDescriptorContext context,
                                 System.Globalization.CultureInfo culture,
                                 object value, Type destType) {
            if (destType == typeof(string) && value is BlobInfo) {
                T blob = (T)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destType);
        }


    }


    public class ConditionInputConverter : ExpandableObjectConverter {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            //StaticObjects.SelectedProject.SelectedRequest.SelectedInspection.SelectedROI.
            if (context!=null) {
                
            }
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            //if (true) {

            //}
            return true;
            // return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            ResultReference resrefinst = context.Instance as ResultReference;


            var converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertFrom(value.GetType())) {
                object newObject = converter.ConvertFrom(value);
                value = newObject;
            }


            return value;
        }
    }

    public class ImagePreProc {



        readonly UndoRedo<int> _erode = new UndoRedo<int>(1);

        [XmlAttribute]
        [EditorAttribute(typeof(PropertySetEditor), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), Category("Pre-Processing")]
        public virtual int Erode {
            get {
                return _erode.Value;
            }
            set {

                if (_erode.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Erode value changed to: " + value.ToString())) {
                            _erode.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _erode.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _dilate = new UndoRedo<int>(1);

        [XmlAttribute]
        [EditorAttribute(typeof(PropertySetEditor), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), Category("Pre-Processing")]
        public virtual int Dilate {
            get {
                return _dilate.Value;
            }
            set {

                if (_dilate.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Dilate value changed to: " + value.ToString())) {
                            _dilate.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _dilate.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _threshold = new UndoRedo<int>(120);

        [XmlAttribute]
        [DescriptionAttribute("Threshold value")]
        [EditorAttribute(typeof(PropertySetEditor), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), Category("Pre-Processing")]
        public virtual int Threshold {
            get {
                return _threshold.Value;
            }
            set {

                if (_threshold.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Threshold value changed to: " + value.ToString())) {
                            _threshold.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _threshold.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<int> _thresholdLink = new UndoRedo<int>(120);
        [XmlAttribute]
        [DescriptionAttribute("Threshold Link value")]
        [EditorAttribute(typeof(PropertySetEditor), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), Category("Pre-Processing"), DisplayName("Threshold Link")]
        public virtual int ThresholdLink {
            get {
                return _thresholdLink.Value;
            }
            set {

                if (_thresholdLink.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Threshold Link value changed to: " + value.ToString())) {
                            _thresholdLink.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _thresholdLink.Value = value;
                    }
                }
            }
        }


        readonly UndoRedo<int> _ApertureSize = new UndoRedo<int>(3);
        [XmlAttribute]
        //[DescriptionAttribute("Threshold Link value")]
        [EditorAttribute(typeof(PropertySetEditor), typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), Category("Pre-Processing"), DisplayName("Aperture Size")]
        public virtual int ApertureSize {
            get {
                return _ApertureSize.Value;
            }
            set {

                if (_ApertureSize.Value != value) {

                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Aperture Size value changed to: " + value.ToString())) {
                            _ApertureSize.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _ApertureSize.Value = value;
                    }
                }
            }
        }




        readonly UndoRedo<Channel> _usechannel = new UndoRedo<Channel>(Channel.Mono);

        [XmlAttribute]
        [Description("Use Channel for processing"), Browsable(true), Category("Pre-Processing")]
        public virtual Channel UseChannel {
            get { return _usechannel.Value; }
            set {
                if (_usechannel.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Threshold value changed to: " + value.ToString())) {
                            _usechannel.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _usechannel.Value = value;
                    }

                }

            }
        }

        readonly UndoRedo<TypeOfThreshold> _thresholdtype = new UndoRedo<TypeOfThreshold>(TypeOfThreshold.Normal);

        [XmlAttribute]
        [Description("Type of threshold"), Browsable(true), Category("Pre-Processing")]
        public virtual TypeOfThreshold ThresholdType {
            get {
                return _thresholdtype.Value;
            }
            set {
                if (_thresholdtype.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Threshold Type value changed to: " + value.ToString())) {
                            _thresholdtype.Value = value;
                            UndoRedoManager.Commit();
                        }

                    }
                    else {
                        _thresholdtype.Value = value;
                    }
                }

            }
        }



        public override string ToString() {
            return "Image settings";
        }
    }

    public class ContourPreProc {

        readonly UndoRedo<Double> _MinContourLength = new UndoRedo<double>();
        [XmlAttribute]
        [DisplayName("Min Length"), Description("Minimum length to consider a valid contour"), Category("Pre-Processing")]
        public Double MinContourLength {
            get { return _MinContourLength.Value; }
            set {
                if (_MinContourLength.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min contour value changed to: " + value.ToString())) {
                            _MinContourLength.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _MinContourLength.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<Double> _MaxContourLength = new UndoRedo<double>(1000);
        [XmlAttribute]
        [DisplayName("Max Length"), Description("Maximum length to consider a valid contour"), Category("Pre-Processing")]
        public Double MaxContourLength {
            get { return _MaxContourLength.Value; }
            set {
                if (_MaxContourLength.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Max contour value changed to: " + value.ToString())) {
                            _MaxContourLength.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _MaxContourLength.Value = value;
                    }
                }
            }
        }

        readonly UndoRedo<Double> _MinArea = new UndoRedo<double>();
        [XmlAttribute]
        [DisplayName("\tMinimum Area"), Description("Minimum area to consider a valid contour"), Category("Pre-Processing")]
        public Double MinArea {
            get { return _MinArea.Value; }
            set {
                if (_MinArea.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Min Area value changed to: " + value.ToString())) {
                            _MinArea.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _MinArea.Value = value;
                    }
                }
            }
        }



        readonly UndoRedo<Double> _MaxArea = new UndoRedo<double>(20000);
        [XmlAttribute]
        [DisplayName("\t\tMaximum Area"), Description("Maximum area to consider a valid contour"), Category("Pre-Processing")]
        public Double MaxArea {
            get { return _MaxArea.Value; }
            set {
                if (_MaxArea.Value != value) {
                    if (!UndoRedoManager.IsCommandStarted) {
                        using (UndoRedoManager.Start("Max Area value changed to: " + value.ToString())) {
                            _MaxArea.Value = value;
                            UndoRedoManager.Commit();
                        }
                    }
                    else {
                        _MaxArea.Value = value;
                    }
                }
            }

        }


        [XmlAttribute]
        [Description("Remove contours that are touching the ROI edges"), DisplayName("Remove Touching Borders"), Category("Pre-Processing")]
        public Boolean RemoveTouchingROIEdges { get; set; }

        public override string ToString() {
            return "Contour settings";
        }
    }



    public enum OutputResultType { orContours, orResults, orNone, orPreProcessing }

    public enum TypeOfThreshold { Normal, Inverted, Adaptive, None }

    public class FilterdShape{
        


        public FilterdShape(){

        }
    }

    #region Includes
    [XmlInclude(typeof(ProcessingFunctionConstant))]    
    [XmlInclude(typeof(ProcessingFunctionBoundingRectangle))]
    [XmlInclude(typeof(ProcessingFunctionPixelanalysis))]   
    [XmlInclude(typeof(ProcessingFunctionPrePos))]
    [XmlInclude(typeof(ProcessingFunctionLines))]
    [XmlInclude(typeof(ProcessingFunctionContourAnalysis))]
    [XmlInclude(typeof(ProcessingFunctionCircleEdges))]
    [XmlInclude(typeof(ProcessingFunctionCircleAnalysis))]
    [XmlInclude(typeof(ProcessingFunctionLineEdge))]
    [XmlInclude(typeof(ProcessingFunctionPoint2PointDist))]
    [XmlInclude(typeof(ProcessingFunctionAreaAnalysis))]
    [XmlInclude(typeof(BlobAnalysis))]
    [XmlInclude(typeof(ProcessingFunctionPoints2Line))]
    [XmlInclude(typeof(ProcessingFunctionPoints2Circle))]
    [XmlInclude(typeof(ProcessingFunctionLine2LineAngle))]
    [XmlInclude(typeof(ProcessingFunctionSum))]
    [XmlInclude(typeof(ProcessingFunctionSub))]
    [XmlInclude(typeof(ProcessingFunctionDivision))]
    [XmlInclude(typeof(ProcessingFunctionRange))]
    [XmlInclude(typeof(GetObject))]
    [XmlInclude(typeof(ProcessingFunctionEllipseFitter))]
    [XmlInclude(typeof(ProcessingFunctionTest))]
    [XmlInclude(typeof(SetValue))]
    [XmlInclude(typeof(CircleFitter))]
    #endregion    
    [Serializable()]
    public abstract class ProcessingFunctionBase : ICloneable, IDisposable {

        

        static public double ConvertToRadians(double angle) {
            return (Math.PI / 180) * angle;
        }

        public ProcessingFunctionBase() {
            
            SetFunctionParams();
        }


        private static KPPLogger log = new KPPLogger(typeof(ProcessingFunctionBase));
       

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

        private void UpdateReferences(Object _theobject,VisionProject selectedproject) {
            try {
                Type myType = _theobject.GetType();
                var systemTypes = myType.GetProperties().Select(t => t.PropertyType).Where(t => t.Namespace.Contains("System"));
                List<Type> customtypes = myType.GetProperties().Select(t => t.PropertyType).Except(systemTypes).ToArray().ToList();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                foreach (PropertyInfo prop in props) {
                    object propValue = prop.GetValue(_theobject, null);
                    if (propValue != null) {
                        if (customtypes.Contains(propValue.GetType())) {
                            if (propValue is ResultReference) {

                                ((ResultReference)propValue).SelectedVisionProject = selectedproject;

                            } else if (propValue is ProcessingFunctionBase) {
                                UpdateReferences(propValue, selectedproject);
                            }
                        }

                    }
                    // Do something with propValue
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        private VisionProject _SelectedVisionProject;
        [XmlIgnore,Browsable(false)]
        public virtual VisionProject SelectedVisionProject {
            get { return _SelectedVisionProject; }
            set {
                if (_SelectedVisionProject!=value) {
                    _SelectedVisionProject = value;

                    UpdateReferences(this, value);


                    if (value!=null) {
                        ModuleName = _SelectedVisionProject.Name;
                    }
                }
            }
        }


        



        ContourPreProc _ContourPreProc = new ContourPreProc();
        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Contour"), Category("Pre-Processing"), Browsable(true)]
        public virtual ContourPreProc ContourPreProc1 {
            get { return _ContourPreProc; }
            set { _ContourPreProc = value; }
        }


        CoordinateSystems _CoordinateSystem = new CoordinateSystems();
        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Coordinate System"), Category("Coordinate System"), Browsable(true), ReadOnly(true)]
        public virtual CoordinateSystems CoordinateSystem {
            get { return _CoordinateSystem; }
            set { _CoordinateSystem = value; }
        }

        ImagePreProc _ImagePreProc = new ImagePreProc();
        [TypeConverter(typeof(ExpandableObjectConverter)), DisplayName("Image"), Category("Pre-Processing"), Browsable(true)]
        public virtual ImagePreProc ImagePreProc1 {
            get { return _ImagePreProc; }
            set { _ImagePreProc = value; }
        }


        public delegate void PrePosisionOffsetChanged();     

        public delegate void FunctionNameChanged(String OldValue,ref String NewName);
        public delegate void PropertyChanged();

        public event FunctionNameChanged OnFunctionNameChanged;
        public event PropertyChanged OnPropertyChanged;

        public delegate void UpdateResultImage(Image<Bgr,byte> imgout);

        public virtual event UpdateResultImage OnUpdateResultImage;



        private Boolean _showresultinroi = true;

        private String _name = "";
        private String _functype = "";
        
        
        
        private Bitmap _ROIBitmapOut = null;

        [XmlIgnore]
        [CategoryAttribute("Common settings"), DescriptionAttribute("Show results in the ROI"), Browsable(false)]
        public virtual bool ShowResultsInROI { get { return _showresultinroi; } set { _showresultinroi = value; } }

        
        private Boolean _Active = true;
        [XmlAttribute, CategoryAttribute("Common settings"), DescriptionAttribute("Image Overaly annotations size"), Browsable(true)]
        public Boolean Active {
            get { return _Active; }
            set { _Active = value; }
        }


        private int _AnnotationSize = 1;
        [XmlAttribute,CategoryAttribute("Common settings"), DescriptionAttribute("Image Overaly annotations size"), Browsable(true)]
        public int AnnotationSize {
            get { return _AnnotationSize; }
            set { _AnnotationSize = value; }
        }


        private Rectangle _BaseRoi = Rectangle.Empty;
        [XmlIgnore,Browsable(false)]
        public Rectangle BaseRoi {
            get { return _BaseRoi; }
            set { _BaseRoi = value; }
        }

        Image<Gray, byte> _IdentRegion = new Image<Gray, byte>(new Size(0, 0));
        [XmlIgnore]
        [Browsable(false)]
        public virtual Image<Gray, byte> IdentRegion {
            get { return _IdentRegion; }
            internal set {
                if (_IdentRegion!=value) {
                    _IdentRegion = value;  
                    
                }
            }
        }

        [XmlIgnore,Browsable(false)]                
        public virtual List<Series> FunctionSeries {get;set;}
        


        int _ProcPos = 1;
        [XmlAttribute]
        [Browsable(false)]
        public int ProcPos {
            get { return _ProcPos; }
            set {                 
                _ProcPos = value; 
            }
        }



        private OutputResultType _ResultsInROI = OutputResultType.orResults;        
        [Category("Post-Processing"), Description("Type of displayed results in ROI"),Browsable(true)]        
        public virtual OutputResultType ResultsInROI { get { return _ResultsInROI; } set { _ResultsInROI = value; } }

        [XmlAttribute]
        [CategoryAttribute("Common settings"), DescriptionAttribute("Function name")]
        //[ReadOnly(true),Browsable(false)]
        public string FunctionName { 
            get { 
                return _name; 
            } 
            set {
                if (_name != value) {
                    
                    //if (OnFunctionNameChanged != null) {

                    //    OnFunctionNameChanged(_name,ref value);
                    //}
                    _name = value;
                }
            } 
        }

        [XmlAttribute]        
        [Browsable(false)]
        public string FunctionType {
            get {
                return _functype;
            }
            set {
                _functype = value;
            }
        }

        
        private string _funcgroup;        
        [Browsable(false),XmlAttribute]
        public string FunctionGroup {
            get {
                
                return _funcgroup;
            }
            set {
                if (_funcgroup!= value) {
                    _funcgroup= value;
                    
                }
                
            }
        }

        public virtual void SetDraggedPpoint(Point point, String property) {
        }

        public void SetFunctionParams() {
            List<object> atts;
            atts=this.GetType().GetCustomAttributes(typeof(ProcessingFunctionAttribute), true).ToList();
            if (atts!=null) {



                ProcessingFunctionAttribute procf = (ProcessingFunctionAttribute)atts.Find(procattr => ((ProcessingFunctionAttribute)procattr).FunctionType!="");
                if (procf!=null) {
                    this.FunctionType = procf.FunctionType;
                    this.FunctionGroup = procf.FunctionGroup;
                }
                
            }
            
        }




        [NonSerialized]
        private PixelFormat _ROIImageFormat;

        

        [XmlIgnore]
        [Browsable(false)]
        public PixelFormat ROIImageFormat {
            get {
                return _ROIImageFormat;
            }
            set {
                _ROIImageFormat = value;
            }
        }



        
        

        
        [field:NonSerialized]      
        public class MyConverter2 : StringConverter {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext
            context) {
                //true means show a combobox
                return true;
            }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
                //true will limit to list. false will show the list, but allow free-form entry
                return true;
            }


            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {


                ProcessingFunctionBase _procfunc = (context.Instance as ProcessingFunctionBase);
                PixelFormat _imageformat = _procfunc.ROIImageFormat;
                List<String> Channels = new List<string>();

                switch (_imageformat) {

                    case PixelFormat.Format24bppRgb:
                        Channels.Add("Bgr");
                        Channels.Add("Red");
                        Channels.Add("Green");
                        Channels.Add("Blue");
                        Channels.Add("Mono");
                        break;

                    case PixelFormat.Format8bppIndexed:
                        Channels.Add("Mono");
                        break;

                    default:
                        break;
                }

                return new StandardValuesCollection(Channels);
            }

        }
               

        public virtual void UpdateRegionToHighligth(Object Region) {

        }

        private Boolean _ValidCoordinateSystem = false;
        [XmlIgnore]
        public Boolean ValidCoordinateSystem {
            get { return _ValidCoordinateSystem; }
            set { _ValidCoordinateSystem = value; }
        }

        public virtual Boolean Process(Image<Bgr, byte> ImageIn, Image<Bgr, byte> ImageOut, Rectangle RoiRegion) {
            try {
                BaseRoi = RoiRegion;
                ImageOut.ROI = Rectangle.Empty;
                _ValidCoordinateSystem = false;
                if (CoordinateSystem!=null) {
                    if (CoordinateSystem.Angle!=null) {
                        CoordinateSystem.Angle.UpdateValue();
                        if (CoordinateSystem.Angle.ResultOutput!=null) {
                            if (CoordinateSystem.Center != null) {
                                CoordinateSystem.Center.UpdateValue();
                                if (CoordinateSystem.Center.ResultOutput != null) {
                                    if (((Point)CoordinateSystem.Center.ResultOutput)!=(new Point(0,0))) {
                                        _ValidCoordinateSystem = true;
                                    }
                                    
                                }
                            }
                        }
                        
                    }

                    
                }

              
            } catch (Exception exp) {
                log.Error(exp);
                return false;
                
            }

            return true;
        }

      



        internal Point OriginPointConvert(int thevalue, ShapeRectangle Roi) {
            return new Point(thevalue + Roi.ShapeEfectiveBounds.X, thevalue + Roi.ShapeEfectiveBounds.Y);

        }

        internal Point OriginPointConvert(Point thepoint, ShapeRectangle Roi) {
            return new Point(thepoint.X + Roi.ShapeEfectiveBounds.X, thepoint.Y + Roi.ShapeEfectiveBounds.Y);
            
        }


        void ClearEvents() {
            OnFunctionNameChanged = null;
            OnUpdateResultImage = null;
            OnPropertyChanged = null;
        }

        public void Dispose() {
            ClearEvents();
        }


        internal static IEnumerable<FieldInfo> GetAllFields(Type t) {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

          //  BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            BindingFlags flags = BindingFlags.NonPublic|BindingFlags.Instance;

            return t.GetFields(flags).Union(GetAllFields(t.BaseType));
        }

       
        public virtual object Clone() {
            //First we create an instance of this specific type.

            Type t = this.GetType();

            object newObjectInstance = Activator.CreateInstance(t);

            //We get the array of fields for the new type instance.




            FieldInfo[] fields = GetAllFields(t).ToArray();
            
            FieldInfo[] fields2 = GetAllFields(t).ToArray();

            int i = 0;

            foreach (FieldInfo fi in fields2) {
                //We query if the fiels support the ICloneable interface.
                try {

                    Type ICloneType = fi.FieldType.
                                GetInterface("ICloneable", true);

                    if (ICloneType != null) {
                        //Getting the ICloneable interface from the object.

                        ICloneable IClone = (ICloneable)fi.GetValue(this);

                        //We use the clone method to set the new value to the field.

                        fields[i].SetValue(newObjectInstance, IClone.Clone());
                    } else {
                        // If the field doesn't support the ICloneable 

                        // interface then just set it.

                        fields[i].SetValue(newObjectInstance, fi.GetValue(this));
                    }

                    //Now we check if the object support the 

                    //IEnumerable interface, so if it does

                    //we need to enumerate all its items and check if 

                    //they support the ICloneable interface.

                    Type IEnumerableType = fi.FieldType.GetInterface
                                    ("IEnumerable", true);
                    if (IEnumerableType != null) {
                        //Get the IEnumerable interface from the field.

                        IEnumerable IEnum = (IEnumerable)fi.GetValue(this);

                        //This version support the IList and the 

                        //IDictionary interfaces to iterate on collections.

                        Type IListType = fields[i].FieldType.GetInterface
                                            ("IList", true);
                        Type IDicType = fields[i].FieldType.GetInterface
                                            ("IDictionary", true);

                        int j = 0;
                        if (IListType != null) {
                            //Getting the IList interface.

                            IList list = (IList)fields[i].GetValue(newObjectInstance);

                            foreach (object obj in IEnum) {
                                //Checking to see if the current item 

                                //support the ICloneable interface.

                                ICloneType = obj.GetType().
                                    GetInterface("ICloneable", true);

                                if (ICloneType != null) {
                                    //If it does support the ICloneable interface, 

                                    //we use it to set the clone of

                                    //the object in the list.

                                    ICloneable clone = (ICloneable)obj;

                                    list[j] = clone.Clone();
                                }

                                //NOTE: If the item in the list is not 

                                //support the ICloneable interface then in the 

                                //cloned list this item will be the same 

                                //item as in the original list

                                //(as long as this type is a reference type).


                                j++;
                            }
                        } else if (IDicType != null) {
                            //Getting the dictionary interface.

                            IDictionary dic = (IDictionary)fields[i].
                                                GetValue(newObjectInstance);
                            j = 0;

                            foreach (DictionaryEntry de in IEnum) {
                                //Checking to see if the item 

                                //support the ICloneable interface.

                                ICloneType = de.Value.GetType().
                                    GetInterface("ICloneable", true);

                                if (ICloneType != null) {
                                    ICloneable clone = (ICloneable)de.Value;

                                    dic[de.Key] = clone.Clone();
                                }
                                j++;
                            }
                        }
                    }
                    i++;
                } catch (Exception) {
                    i++;
                    
                }
            }
            return newObjectInstance;
        }


    }
}
