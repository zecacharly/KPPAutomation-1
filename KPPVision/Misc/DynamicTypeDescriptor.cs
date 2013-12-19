using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace VisionModule {
    /// <summary>
    /// The .NETs PropertyDescriptorCollection is a read-only collection and is not suitable for list minipulation.
    /// So ended up creating a list, works lik a champ :-).
    /// </summary>
    public class PropertyDescriptorList : List<PropertyDescriptor> {
        public PropertyDescriptor Find(string propertyName, bool ignoreCase) {
            foreach (PropertyDescriptor pd in this) {
                if (String.Compare(pd.Name, propertyName, ignoreCase) == 0) {
                    return pd;
                }
            }
            return null;

        }
        public void SortUsingSortAttribute() {
            if (ExistSortOrderAttribute()) {
                this.Sort(new PropertyDescriptorListSorter());
            }
        }
        private bool ExistSortOrderAttribute() {
            foreach (PropertyDescriptor pd in this) {
                foreach (Attribute att in pd.Attributes) {
                    if (att is SortOrderAttribute) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumItemAttribute : Attribute {
        public EnumItemAttribute() {
        }
        public EnumItemAttribute(string displayName) {
            m_DisplayName = displayName;
        }
        public EnumItemAttribute(string displayName, bool browsable) {
            m_DisplayName = displayName;
            m_Browsable = browsable;
        }
        public EnumItemAttribute(string displayName, string description) {
            m_DisplayName = displayName;
            m_Description = description;
        }
        public EnumItemAttribute(bool browsable, string description) {
            m_Browsable = browsable;
            m_Description = description;
        }
        public EnumItemAttribute(string displayName, bool browsable, string description) {
            m_DisplayName = displayName;
            m_Browsable = browsable;
            m_Description = description;
        }

        public EnumItemAttribute(bool browsable) {
            m_Browsable = browsable;
        }
        private string m_DisplayName = String.Empty;
        public string DisplayName {
            get {
                return m_DisplayName;
            }
            set {
                m_DisplayName = value;
            }
        }
        private bool m_Browsable = true;

        public bool Browsable {
            get {
                return m_Browsable;
            }
            set {
                m_Browsable = value;
            }
        }
        private string m_Description = String.Empty;

        public string Description {
            get {
                return m_Description;
            }
            set {
                m_Description = value;
            }
        }

    }

    /*
  public class EnumTypeEditor : UITypeEditor
  {
    private EnumEditorUI m_ui = new EnumEditorUI();
    public EnumTypeEditor()
    {

    }

    public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
    {
      return false;
    }

    public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.DropDown;
    }
    public override bool IsDropDownResizable
    {
      get
      {
        return true; // base.IsDropDownResizable;
      }
    }


    public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      if (provider != null)
      {
        if (!context.PropertyDescriptor.PropertyType.IsEnum)
        {
          throw new Exception("Property must be a typeof enum.");
        }

        IWindowsFormsEditorService editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
        if (editorService == null)
          return value;


        m_ui.SetData(context, editorService, (int)value);

        editorService.DropDownControl(m_ui);

        value = m_ui.GetValue();

      }

      return value;
    }
  }
    */

    /// <summary>
    /// In this class we will avoid all bitwize operations ourselves.
    /// we will use the System.Enum class to do that work for us
    /// </summary>
    public class EnumTypeConverter : EnumConverter {
        private bool m_bFlag = false;
        // a map between name (what is in the source code) and the display name
        private Hashtable m_mapNameDispName = new Hashtable();  // Name is stored as key and display name is stored as value
        // Note: we do not use the values of the enum
        public EnumTypeConverter(Type type)
            : base(type) {
            InitializeMaps();
        }

        private void InitializeMaps() {
            m_bFlag = EnumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;

            FieldInfo[] fields = EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fi in fields) {
                string sName = fi.Name;
                string sDispName = sName;  // initially display name is same as the name itself

                EnumItemAttribute[] attr = fi.GetCustomAttributes(typeof(EnumItemAttribute), false) as EnumItemAttribute[];
                if (attr != null && attr.Length > 0) {
                    if (attr[0].Browsable == false) {
                        continue;
                    }
                    if (attr[0].DisplayName != null && attr[0].DisplayName.Trim().Length > 0) {
                        sDispName = attr[0].DisplayName.Trim();
                    }
                }

                m_mapNameDispName.Add(sName, sDispName);

            }

        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            if (value.GetType() != typeof(string)) {
                return base.ConvertFrom(context, culture, value);
            }

            string sInpuValue = (string)value;

            if (sInpuValue.Length == 0) {
                throw new Exception("Value cannot not be an empty string.");
            }

            string sValue = String.Empty;
            if (m_bFlag) {
                string[] arrDispName = sInpuValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder sb = new StringBuilder(1000);
                foreach (string sDispName in arrDispName) {
                    if ((sValue = GetNameByDisplayName(sDispName)) == null) {
                        throw new Exception("'" + sDispName + "' is not valid enumeration field name.");
                    }

                    if (sb.Length > 0) {
                        sb.Append(",");
                    }
                    sb.Append(sValue);
                }  // end of foreach..loop


                // Let the Enum class do the hard work.  following line may throw exception - PropertyGrid will handle it gracefuly
                return Enum.Parse(EnumType, sb.ToString(), true);

            } else {
                if ((sValue = GetNameByDisplayName(sInpuValue)) == null) {
                    throw new Exception("'" + sInpuValue + "' is not valid enumeration field name.");
                }

                // Let the Enum class do the hard work . following line may throw exception - PropertyGrid will handle it gracefuly
                return Enum.Parse(EnumType, sValue, true);

            }

        }


        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (destinationType != typeof(string)) {
                return base.ConvertTo(context, culture, value, destinationType);
            }


            object objEnumValue = null;

            if (value is string) {
                objEnumValue = ConvertFrom(context, culture, value);
            } else {
                objEnumValue = value;
            }

            if (objEnumValue == null) {
                throw new Exception("Null is not a valid enumeration value.");
            }


            if (m_bFlag) {
                // We are using Enum class again to do our work
                string sDelimitedValues = Enum.Format(EnumType, objEnumValue, "G");
                string[] arrValue = sDelimitedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder sb = new StringBuilder(1000);
                foreach (string sValue in arrValue) {
                    string sDispName = (string)m_mapNameDispName[sValue.Trim()];
                    if (sb.Length > 0) {
                        sb.Append(", ");
                    }
                    sb.Append(sDispName);
                }

                return sb.ToString();
            } else {
                string sValue = String.Empty;
                if ((sValue = GetNameByDisplayName(objEnumValue.ToString())) == null) {
                    throw new Exception("'" + objEnumValue.ToString() + "' is not valid enumeration field name.");
                }
                return m_mapNameDispName[objEnumValue.ToString()];
            }

        }



        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return !m_bFlag;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            ArrayList list = new ArrayList();

            foreach (DictionaryEntry de in m_mapNameDispName) {
                list.Add(Enum.Parse(EnumType, de.Key.ToString()));
            }

            StandardValuesCollection svc = new StandardValuesCollection(list);
            return svc;

        }
        private string GetNameByDisplayName(string sDispName) {
            string sTrimDispName = sDispName.Trim();
            foreach (DictionaryEntry de in m_mapNameDispName) {
                if (String.Compare(de.Value.ToString(), sTrimDispName, true) == 0 ||
                    String.Compare(de.Key.ToString(), sTrimDispName, true) == 0) {
                    return de.Key.ToString();
                }
            }
            return null;
        }

    }

    public class DynamicCustomTypeDescriptor : CustomTypeDescriptor {
        private PropertyDescriptorList m_pdlFiltered = null;
        private PropertyDescriptorList m_pdlUnFiltered = null;
        private object m_instance = null;
        private MethodInfo m_mi = null;

        public DynamicCustomTypeDescriptor(ICustomTypeDescriptor ctd, object instance)
            : base(ctd) {
            m_instance = instance;

            // define the parameter data type of the method
            Type[] arrType = new Type[1];
            arrType.SetValue(typeof(PropertyDescriptorList), 0);

            // get method info
            m_mi = m_instance.GetType().GetMethod("ModifyDynamicProperties", arrType);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            if (m_pdlFiltered == null) {
                m_pdlFiltered = new PropertyDescriptorList();
                PropertyDescriptorCollection pdc = base.GetProperties(attributes);  // this gives us a readonly collection, no good    
                foreach (PropertyDescriptor pd in pdc) {
                    m_pdlFiltered.Add(pd);
                }
            }

            if (m_mi != null) {
                // invoke the method
                Object[] arrObj = { m_pdlFiltered };
                object obj = m_mi.Invoke(m_instance, arrObj);
                m_pdlFiltered.SortUsingSortAttribute();

            }
            PropertyDescriptorCollection pdcReturn = new PropertyDescriptorCollection(m_pdlFiltered.ToArray());
            return pdcReturn;
        }

        public override PropertyDescriptorCollection GetProperties() {
            if (m_pdlUnFiltered == null) {
                m_pdlUnFiltered = new PropertyDescriptorList();
                PropertyDescriptorCollection pdc = base.GetProperties();  // this gives us a readonly collection, no good    
                foreach (PropertyDescriptor pd in pdc) {
                    m_pdlUnFiltered.Add(pd);
                }
            }

            if (m_mi != null) {
                // invoke the method
                Object[] arrObj = { m_pdlUnFiltered };
                object obj = m_mi.Invoke(m_instance, arrObj);
                m_pdlUnFiltered.SortUsingSortAttribute();

            }
            PropertyDescriptorCollection pdcReturn = new PropertyDescriptorCollection(m_pdlUnFiltered.ToArray());
            return pdcReturn;


        }

        private void PrintIndexes(PropertyDescriptorCollection pdc) {
            Console.Write("Count: " + pdc.Count);
            foreach (PropertyDescriptor pd in pdc) {
                Console.Write(", ");
                if (pd != null) {
                    Console.Write(pdc.IndexOf(pd).ToString() + "(" + pd.Name + ")");
                } else {
                    Console.Write("(null)");
                }
            }
        }

        public override object GetPropertyOwner(PropertyDescriptor pd) {
            return m_instance;
            ;
        }

    }

    public class DynamicTypeDescriptionProvider : TypeDescriptionProvider {
        private TypeDescriptionProvider m_parent = null;

        /// <summary>
        /// A user of TypeDescriptor will usually invoke this constructor.
        /// But this constructor has no way of knowing the parent TypeDescriptor or 
        /// object it is attached to.  To overcome this problem, we do some tricks 
        /// in GetTypeDescriptor method
        /// </summary>
        public DynamicTypeDescriptionProvider()
            : base() {

        }

        /// <summary>
        /// This constructor is the one gives use enough information to 
        /// get type and property descriport data
        /// </summary>
        /// <param name="parent"></param>
        public DynamicTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent) {
            m_parent = parent;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
            if (m_parent == null)  // we need to get the parent now
      {
                //NOTE:  we must use objectType to get the parent, and not the instance

                // we pop ourself from the stack
                TypeDescriptor.RemoveProvider(this, objectType);  // this will allow us to access the previous provider

                // we get the previous provider provided by somebody else (likely by .NET framework as default provider)
                m_parent = TypeDescriptor.GetProvider(objectType);

                // now push ourself back onto the stack
                TypeDescriptor.AddProvider(this, objectType);

            }

            if (instance != null) {
                return new DynamicCustomTypeDescriptor(m_parent.GetTypeDescriptor(instance), instance);
            } else {
                return m_parent.GetTypeDescriptor(objectType);
            }

        }


    }

    /// <summary>
    /// This PropertyDescriptor handles setting of a given vertex
    /// for the FunkyButton.  It specifies our custom UI editor for Points
    /// as well as handles determining how to draw each vertex.
    /// </summary>
    public class PseudoPropertyDescriptor : PropertyDescriptor {
        private object m_owner = null;
        private object m_value = null;
        private Type m_type = Type.Missing.GetType();
        private AttributeCollection m_ac = new AttributeCollection();

        public PseudoPropertyDescriptor(object owner, object value, string sName, Type type, params Attribute[] attributes)
            : base(sName, attributes) {
            this.m_owner = owner;
            this.m_value = value;
            m_type = type;

        }

        /// <summary>
        /// The type of component the framework expects for this property.  Notice
        /// This returns FunkyButton.  That is because the object that is being browsed
        /// when this property is shown is a FunkyButton.  So we are faking the PropertyGrid
        /// into thinking this is a property on that type, even though it isn't.
        /// </summary>	
        public override Type ComponentType {
            get {
                return m_owner.GetType();
            }
        }

        /// <summary>
        /// Must override abstract properties.
        /// </summary>
        public override bool IsReadOnly {
            get {
                return false;
            }
        }

        public override Type PropertyType {
            get {
                return m_type;
            }
        }

        public override object GetValue(object o) {
            return m_value;
        }

        /// <summary>
        /// When the value is set, we just push that value
        /// back up into the buttons Points array.
        /// </summary>
        public override void SetValue(object o, object value) {
            m_value = value;
            this.GetValueChangedHandler(o).Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Abstract base members
        /// </summary>			
        public override void ResetValue(object o) {

        }

        public override bool CanResetValue(object o) {
            return false;
        }

        public override bool ShouldSerializeValue(object o) {
            return false;
        }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SortOrderAttribute : Attribute {
        public SortOrderAttribute() {
        }
        private int m_SortOrder = 0;
        public SortOrderAttribute(int sortOrder) {
            m_SortOrder = sortOrder;
        }


        public int SortOrder {
            get {
                return m_SortOrder;
            }
            set {
                m_SortOrder = value;
            }
        }

    }


    public class PropertyDescriptorListSorter : IComparer<PropertyDescriptor> {
        #region IComparer<PropertyDescriptor> Members

        public int Compare(PropertyDescriptor x, PropertyDescriptor y) {
            if (x == null && y != null) {
                return -1;
            }
            if (x != null && y == null) {
                return 1;
            }
            if (x == null && y == null) {
                return 0;
            }

            int orderX = -1;
            int orderY = -1;

            foreach (Attribute attr in x.Attributes) {
                if (attr is SortOrderAttribute) {
                    orderX = ((SortOrderAttribute)attr).SortOrder;
                    break;
                }
            }

            foreach (Attribute attr in y.Attributes) {
                if (attr is SortOrderAttribute) {
                    orderY = ((SortOrderAttribute)attr).SortOrder;
                    break;
                }
            }

            if (orderX == -1 || orderY == -1) {
                return x.Name.CompareTo(y.Name);
            }
            if (orderX > orderY) {
                return 1;
            }
            if (orderX < orderY) {
                return -1;
            }
            return 0;

        }

        #endregion
    }


}  // end of namespace

