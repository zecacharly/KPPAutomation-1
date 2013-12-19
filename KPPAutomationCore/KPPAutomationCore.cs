using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Resources;
using System.Threading;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using KPP.Core.Debug;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml.Serialization;

namespace KPPAutomationCore {

    public delegate void AttributeValueChanged(Object obj);


    #region Custome types

    //public class ModuleDockingSettings {

    //    Double m_DockBottomPortion;
    //    [XmlAttribute]
    //    public Double DockBottomPortion {
    //        get { return m_DockBottomPortion; }
    //        set { m_DockBottomPortion = value; }
    //    }

    //    Double m_DockLeftPortion;
    //    [XmlAttribute]
    //    public Double DockLeftPortion {
    //        get { return m_DockLeftPortion; }
    //        set { m_DockLeftPortion = value; }
    //    }

    //    public DockPanel MainDock;

    //    public ModuleDockingSettings(DockPanel mainDock,Double dockBottomPortion,dockLeftPortion) {
    //        MainDock = mainDock;
    //        DockBottomPortion=dockBottomPortion;
    //        DockLeftPortion = dockLeftPortion;
    //        MainDock.
    //    }

    //    public ModuleDockingSettings() {
    //    }
    //}

    public class CustomCollection<T> : CollectionBase, ICustomTypeDescriptor {

        public CustomCollection() {

        }

        private String _name = "List";

        public void SetName(String Name) {
            _name = Name;
        }

        public Object GetList() {
            return List;
        }



        #region  Collection methods implementation

        public void Add(T source) {
            this.List.Add(source);
        }
        public void Remove(T source) {
            this.List.Remove(source);
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public T this[int index] {
            get {
                try {
                    if (index < this.List.Count) {
                        //index--;

                        return (T)this.List[index];
                    } else {
                        return default(T);
                    }
                } catch (Exception exp) {

                    return default(T);

                }
            }
        }

        #endregion

        public List<T> ToList() {
            List<T> list = new List<T>();
            foreach (T item in this.List) {
                list.Add(item);
            }

            return list;
        }

        #region Implementation of ICustomTypeDescriptor:

        public String GetClassName() {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes() {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName() {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter() {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent() {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty() {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType) {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes) {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents() {
            return TypeDescriptor.GetEvents(this, true);
        }



        public object GetPropertyOwner(PropertyDescriptor pd) {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties() {
            // Create a new collection object PropertyDescriptorCollection
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            // Iterate the list of employees
            for (int i = 0; i < this.List.Count; i++) {
                // For each employee create a property descriptor 
                // and add it to the 
                // PropertyDescriptorCollection instance
                CustomCollectionPropertyDescriptor<T> pd = new
                              CustomCollectionPropertyDescriptor<T>(this, i);
                pds.Add(pd);
            }
            return pds;
        }

        #endregion

        public override string ToString() {

            return this._name;
        }
    }

    public class CustomCollectionPropertyDescriptor<T> : PropertyDescriptor {
        private CustomCollection<T> collection = null;
        private int index = -1;

        public CustomCollectionPropertyDescriptor(CustomCollection<T> coll,
                           int idx)
            : base("#" + idx.ToString(), null) {
            this.collection = coll;
            this.index = idx;
        }

        public override AttributeCollection Attributes {
            get {
                return new AttributeCollection(null);
            }
        }


        public override bool CanResetValue(object component) {
            return true;
        }

        public override Type ComponentType {
            get {
                return this.collection.GetType();
            }
        }

        public override string DisplayName {
            get {
                try {
                    T source = this.collection[index];
                    if (source != null) {


                        String Name = source.ToString();//(String)source.GetType().GetProperty("Name").GetValue(source, null);
                        if (Name != "") {
                            return Name;
                        }
                        return source.ToString();
                    } else {
                        return "";
                    }
                } catch (Exception exp) {
                    return null;
                }
            }
        }

        public override string Description {
            get {

                T source = this.collection[index];
                if (source == null) {
                    return "";
                }
                try {
                    String Name = (String)source.GetType().GetProperty("Name").GetValue(source, null);
                    if (Name != "") {
                        return Name;
                    }
                } catch (Exception exp) {


                }
                return source.ToString();
            }
        }

        public override object GetValue(object component) {
            return this.collection[index];
        }

        public override bool IsReadOnly {
            get { return false; }
        }

        public override string Name {
            get { return "#" + index.ToString(); }
        }

        public override Type PropertyType {
            get {
                if (this.collection[index] == null) {
                    return typeof(Nullable);
                }
                return this.collection[index].GetType();
            }
        }

        public override void ResetValue(object component) { }

        public override bool ShouldSerializeValue(object component) {
            return true;
        }



        public override void SetValue(object component, object value) {
            //this.collection[index] = value;
        }
    }



    #endregion

    #region Acess Manegement Region

    public class UserDef {

        private Acesslevel m_Level = Acesslevel.NotSet;

        public Acesslevel Level {
            get { return m_Level; }
            set { m_Level = value; }
        }

        private String m_Pass = "";

        public String Pass {
            get { return m_Pass; }
            set { m_Pass = value; }
        }


        public UserDef(String pass, Acesslevel level) {
            Pass = pass;
            Level = level;
        }

        public UserDef() {

        }
    }


    public enum Acesslevel { Admin, User, NotSet, Man }

    public static class AcessManagement {




        public delegate void AcesslevelChanged(Acesslevel NewLevel);
        public static event AcesslevelChanged OnAcesslevelChanged;




        private static Acesslevel _acessLevel = Acesslevel.NotSet;

        public static Acesslevel AcessLevel {
            get { return _acessLevel; }
            set {
                if (_acessLevel != value) {
                    _acessLevel = value;
                    if (OnAcesslevelChanged != null) {
                        OnAcesslevelChanged(value);
                    }
                }
            }
        }
    }
    #endregion

    public static class KPPExtensions {

        //public static string GetResourceText(this Object from, String ResVar) {
        //    return GetResourceText(from, "VisionModule.Resources.Language.Res", ResVar);
        //}

        //public static string GetResourceText(this Object from, String ResLocation, String ResVar) {
        //    try {
        //        //ComponentResourceManager resources = new ComponentResourceManager();
        //        ResourceManager res_man = new ResourceManager(ResLocation, from.GetType().Assembly);
        //        return res_man.GetString(ResVar, Thread.CurrentThread.CurrentUICulture);
        //    }
        //    catch (Exception exp) {

        //        return "Error getting resource";
        //    }
        //}
        public static object GetPropertyValue(this object obj, string propertyName) {
            return obj.GetType().GetProperties()
               .Single(pi => pi.Name.ToLower().Contains(propertyName.ToLower()))
               .GetValue(obj, null);
        }

        public static string GetResourceText(this Object from, String ResVar) {
            return GetResourceText(from, from.GetType().Assembly.GetName().Name + ".Resources.Language.Res", ResVar);
        }

        public static string GetResourceText(this Object from, String ResLocation, String ResVar) {
            try {

                //ComponentResourceManager resources = new ComponentResourceManager(Program.);
                ResourceManager res_man = new ResourceManager(ResLocation, from.GetType().Assembly);
                return res_man.GetString(ResVar, Thread.CurrentThread.CurrentUICulture);
            } catch (Exception exp) {

                return "Error getting resource";
            }
        }


        //public static String GetModuleName(this object obj) {

        //    PropertyInfo propinf = obj.GetType().GetProperty("ModuleName");
        //    if (propinf!=null) {
        //        return (String)propinf.GetValue(obj, null);
        //    }

        //    return "N/A";

        //}
        public static object GetDefaultValue(this Object obj) {
            if (obj == null) {
                return null;
            }

            Type t = obj.GetType();

            //return t.GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(obj, null);

            if (t.IsValueType) {
                return Activator.CreateInstance(t);
            } else {
                return null;
            }
        }

        public static int RoundUp(this int num, int multiple) {
            if (multiple == 0)
                return 0;
            int add = multiple / Math.Abs(multiple);
            return ((num + multiple - add) / multiple) * multiple;
        }


        public static int NextEven(this int num) {
            if ((num & 1) == 0) {
                // It's even
            } else {
                num++;
            }

            return num;
        }


        public static string base64Encode(this string data) {
            try {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            } catch (Exception e) {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public static string base64Decode(this string data) {
            try {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            } catch (Exception e) {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }


        //public static KPPLogger SetNewLogger(this KPPLogger thelogger, Type logtype, string logname) {


        //    return new KPPLogger(logtype, name: logname);





        //}



        private static AttributeValueChanged _OnAttributeValueChanged;
        private static object _eventLock = new object();
        public static event AttributeValueChanged OnAttributeValueChanged {
            add {
                lock (_eventLock) {
                    _OnAttributeValueChanged -= value;
                    _OnAttributeValueChanged += value;
                }
            }
            remove {
                lock (_eventLock) {
                    _OnAttributeValueChanged -= value;
                }
            }
        }



        public static void ChangeAttributeValue<T>(this object selectedObject, string propertyName, string field, bool newval) {

            try {




                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(selectedObject.GetType())[propertyName];
                //ReadOnlyAttribute attribute = (ReadOnlyAttribute)
                //                              descriptor.Attributes[typeof(ReadOnlyAttribute)];

                if (descriptor == null) {
                    return;
                }
                object attribute = descriptor.Attributes[typeof(T)];

                if (attribute == null) {
                    return;
                }
                T attrval = (T)attribute;

                FieldInfo fieldToChange = attrval.GetType().GetField(field,
                                                 System.Reflection.BindingFlags.NonPublic
                                                 | System.Reflection.BindingFlags.Instance
                                                 );

                if (fieldToChange == null) {
                    return;
                }
                fieldToChange.SetValue(attrval, newval);

                if (_OnAttributeValueChanged != null) {
                    _OnAttributeValueChanged(selectedObject);
                }
            } catch (Exception exp) {


            }

        }

    }

    public enum LanguageName { Unk, PT, EN }

    public static class LanguageSettings {
        private static LanguageName _Language = LanguageName.PT;

        public static LanguageName Language {
            get { return _Language; }
            set {
                if (_Language != value) {
                    _Language = value;
                    switch (value) {
                        case LanguageName.Unk:
                            break;
                        case LanguageName.PT:
                            break;
                        case LanguageName.EN:

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

     

    public class KPPFuctions {

        public static string ImageToBase64String(Image image) {
            using (MemoryStream stream = new MemoryStream()) {
                image.Save(stream, image.RawFormat);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        /// Creates a new image from the given base64 encoded string.
        /// </summary>
        /// <param name="base64String">The encoded image data as a string.</param>
        public static Image ImageFromBase64String(string base64String) {
            using (MemoryStream stream = new MemoryStream(
                Convert.FromBase64String(base64String)))
            using (Image sourceImage = Image.FromStream(stream)) {
                return new Bitmap(sourceImage);
            }
        }





       






        public static T Clone<T>(T source) {
            if (!typeof(T).IsSerializable) {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null)) {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream) {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }



    }

    //public interface IModuleName {

    //    String ModuleName { get; set; }
    //}
}
