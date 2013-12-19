using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPP.Core.Debug;
using System.Xml.Serialization;
using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Collections;
using KPPAutomationCore;

namespace VisionModule {
    [ProcessingFunction("Get Object from Array", "Array")]
    public class GetObject : ProcessingFunctionBase {
        private static KPPLogger log = new KPPLogger(typeof(GetObject));

        public GetObject() {


        }

        #region Pre-Processing

        [XmlIgnore, Browsable(false)]
        public override ContourPreProc ContourPreProc1 { get; set; }

        [XmlIgnore, Browsable(false)]
        public override ImagePreProc ImagePreProc1 { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("return the Object info from index"), DisplayName("Object Index")]
        public int Index { get; set; }

        [XmlAttribute]
        [Category("Pre-Processing"), Description("Set the name of the object property to be returned"), DisplayName("Property")]
        [EditorAttribute(typeof(PropertiesSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public String ObjectProp { get; set; }


        private ResultReference _InputList;
        [Category("Pre-Processing"), TypeConverter(typeof(ExpandableObjectConverter)), UseInProperties(true), DisplayName("Blobs List"), AcceptDrop(true), AcceptType(typeof(CustomCollection<object>))]
        public ResultReference InputList {
            get { return _InputList; }
            set {
                _InputList = value;
            }
        }

        #endregion



        #region Post Processing

        private Object _OutputObject = null;
        [Category("Post-Processing"), XmlIgnore, AllowDrag(true), UseInRef(true), UseInResultInput(true), DisplayName("Output"), ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Object OutputObject {
            get {
                return _OutputObject;
            }
            private set {
                _OutputObject = value;
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

                OutputObject = null;

                if (InputList == null) {
                    return false;
                }

                InputList.UpdateValue();



                if (!(InputList.ResultOutput is CollectionBase)) {

                    return false;
                }
                try {
                    CollectionBase collection = InputList.ResultOutput as CollectionBase;
                    if (collection != null) {
                        List<Object> objectcollection = collection.Cast<object>().ToList();
                        if (objectcollection != null) {
                            if (Index < objectcollection.Count) {
                                Object theobject = objectcollection[Index];
                                OutputObject = theobject.GetType().GetProperty(ObjectProp).GetValue(theobject, null);
                            }
                        }
                    }


                } catch (Exception exp) {
                    log.Error(exp);
                    return false;


                }



            } catch (Exception exp) {
                log.Error(exp);
                return false;
            }




            return true;


        }


    }
}
