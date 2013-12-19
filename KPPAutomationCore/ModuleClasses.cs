using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml.Serialization;
using KPP.Core.Debug;
using IOModule;
using System.ComponentModel;


namespace KPPAutomationCore {

    public delegate void SelectedProjectChanged(ModuleProject ProjectSelected);

    //[XmlInclude(typeof(VisionProject))]
    public class ModuleProject : ICloneable {


        private static KPPLogger log = new KPPLogger(typeof(ModuleProject));
        
        
        
        [XmlAttribute, Browsable(false)]
        public virtual String ModuleName {
            get;
            set;
        }

        [XmlAttribute]
        public virtual String Name { get; set; }

        [XmlAttribute("ID")]
        public virtual int ProjectID { get; set; }


        private bool _loadonstart = false;
        [XmlAttribute]
        public virtual bool Loadonstart {
            get { return _loadonstart; }
            set {
                _loadonstart = value;
            }
        }

        public virtual object Clone() {
            return this.MemberwiseClone();
        }

        public virtual void Dispose() {

        }
    }

    public class ModuleSettings {


        public event SelectedProjectChanged OnSelectedProjectChanged;


        #region -  Serialization attributes  -

        public static Int32 S_BackupFilesToKeep = 5;
        public static String S_BackupFolderName = "backup";
        public static String S_BackupExtention = "bkp";
        public static String S_DefaulFileExtention = "xml";

        
        private String _FilePath = null;
        [XmlIgnore, Browsable(false)]
        public virtual String FilePath {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        private String _defaultPath = null;

        [XmlIgnore]
        public Int32 BackupFilesToKeep { get; set; }
        [XmlIgnore]
        public String BackupFolderName { get; set; }
        [XmlIgnore]
        public String BackupExtention { get; set; }

        #endregion
                

        [XmlAttribute]
        public virtual String Name { get; set; }


        public virtual List<String> ProjectDirectories { get; set; }

       

        [XmlAttribute]
        public virtual String ProjectFile { get; set; }

        private String m_ModuleName;
        [XmlAttribute]
        public virtual String ModuleName {
            get { return m_ModuleName; }
            set { m_ModuleName = value; }
        }


        private ModuleProject _SelectedProject = null;
        [XmlIgnore,Browsable(false)]
        public virtual ModuleProject SelectedProject {
            get { return _SelectedProject; }
            set {
                _SelectedProject = value;
                if (OnSelectedProjectChanged != null) {
                    OnSelectedProjectChanged(value);
                }

            }
        }
     
        private string m_DockFile;// = Path.Combine(FilesLocation, ModuleName + "DockPanel.dock");
        [XmlIgnore, Browsable(false)]
        public virtual string DockFile {
            get { return m_DockFile; }
            set { m_DockFile = value; }
        }
        




        /// <summary>
        /// 
        /// </summary>
        public ModuleSettings() {
           
        }

    }

    public interface IModuleForm {

        void InitModule(String moduleName, String visionSettingsFile);


        
        Boolean Restart {
            get;
            set;
        }
        //event SelectedProjectChanged OnSelectedProjectChanged;
        
    }
}
