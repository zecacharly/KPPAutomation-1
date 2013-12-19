using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KPP.Core.Debug;
using System.IO;
using System.Windows.Forms;
using VisionModule;
using System.ComponentModel;
using KPPAutomationCore;
using System.Drawing.Design;
using VisionModule.Forms;
using System.Windows.Forms.Design;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;
using EpsonModule;


namespace KPPAutomation {


    public class ModuleAddRemoveSelector : UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        //UserControl contextcontrol = StaticObjects.InputItemSelectorControl;
        private static KPPLogger log = new KPPLogger(typeof(ModuleAddRemoveSelector));
        public ModuleAddRemoveForm form = new ModuleAddRemoveForm();

        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();





        public ModuleAddRemoveSelector() {
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




            ApplicationSettings settings = context.Instance as ApplicationSettings;
            

            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {

                // form.SelectedProject = StaticObjects.SelectedProject;

                form.AppSettings = settings;
                form.__comboModuleTypes.Items.Clear();
                var lListOfBs = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from lType in lAssembly.GetTypes()
                                 where lType.IsSubclassOf(typeof(KPPModule))
                                 select lType).ToArray();
                form.__comboModuleTypes.Items.AddRange(lListOfBs);
                if (edSvc.ShowDialog(form) == DialogResult.OK) {

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

    #region Custom editor
    public class AppFileFolderSelector : UITypeEditor {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {

            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            openFolderDialog.Description = "Application Settings File Location";

            String currentloc = ((KPPModule)context.Instance).ModuleFilesLocation;
            if (String.IsNullOrEmpty(currentloc)) {
                currentloc = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            }
            openFolderDialog.SelectedPath = currentloc;
            if (openFolderDialog.ShowDialog() == DialogResult.OK) {

                openFolderDialog.Dispose();
                //base.InitializeDialog(openFolderDialog);
                Uri fullPath = new Uri(openFolderDialog.SelectedPath, UriKind.Absolute);
                Uri relRoot = new Uri(AppDomain.CurrentDomain.BaseDirectory, UriKind.Absolute);

                String relative = relRoot.MakeRelativeUri(fullPath).ToString();
                relative = relative.Replace("%20", " ");

                //openFolderDialog.DirectoryPath = relative;
                return base.EditValue(context, provider, relative);
            }
            return base.EditValue(context, provider, value);
        }
    }

    #endregion



    public class KPPEpsonModule : KPPModule {

        public override event ModuleNameChanged OnModuleNameChanged;


        private static KPPLogger log = new KPPLogger(typeof(KPPEpsonModule));



        private String m_ModuleSettingsFile;
        [XmlIgnore, Browsable(false)]
        public override String ModuleSettingsFile {
            get { return m_ModuleSettingsFile; }
            set { m_ModuleSettingsFile = value; }
        }

        private string m_ModuleFilesLocation;
        [XmlAttribute, DisplayName("Module Settings location")]
        [EditorAttribute(typeof(AppFileFolderSelector), typeof(UITypeEditor))]
        public override string ModuleFilesLocation {
            get { return m_ModuleFilesLocation; }
            set {


                UpdateModuleFilesLocation(m_ModuleFilesLocation, value);

                m_ModuleFilesLocation = value;




            }
        }




        private String m_ModuleName = "New Epson module";
        [XmlAttribute, DisplayName("Module Name")]
        public override String ModuleName {
            get { return m_ModuleName; }
            set {
                if (m_ModuleName != value) {
                    String oldvalue = m_ModuleName;
                    m_ModuleName = value;

                    if (OnModuleNameChanged != null) {
                        OnModuleNameChanged(this, oldvalue);
                    }

                }
            }
        }


        [XmlIgnore]
        public override String ModuleType {
            get {
                return this.GetType().ToString();
            }

        }


        private DockPanel MainDock;

        [XmlIgnore, Browsable(false)]
        public EpsonMainForm epsonForm;


        [XmlIgnore, Browsable(false)]
        public override DockContent ModuleForm {
            get { return epsonForm; }
        }


        public override void UpdateModuleNameFiles(String OldModuleName, String NewModuleName) {

            if (!String.IsNullOrEmpty(NewModuleName)) {
                if (!String.IsNullOrEmpty(OldModuleName)) {

                    if (File.Exists(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"))) {
                        File.Copy(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"), Path.Combine(ModuleFilesLocation, NewModuleName + ".dock"), true);
                        File.Delete(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"));
                    }

                    if (File.Exists(Path.Combine(ModuleFilesLocation, OldModuleName + ".EpsonModule"))) {
                        File.Copy(Path.Combine(ModuleFilesLocation, OldModuleName + ".EpsonModule"), Path.Combine(ModuleFilesLocation, NewModuleName + ".EpsonModule"), true);
                        File.Delete(Path.Combine(ModuleFilesLocation, OldModuleName + ".EpsonModule"));
                    }
                    else {
                        EpsonSettings.WriteConfiguration(new EpsonSettings(), Path.Combine(NewModuleName, ModuleName + ".EpsonModule"));
                    }

                }

                if (!File.Exists(Path.Combine(ModuleFilesLocation, ModuleName + ".EpsonModule"))) {

                    EpsonSettings.WriteConfiguration(new EpsonSettings(), Path.Combine(ModuleFilesLocation, ModuleName + ".EpsonModule"));
                }
                ModuleSettingsFile = Path.Combine(ModuleFilesLocation, ModuleName + ".EpsonModule");
            }
        }

        public override void UpdateModuleFilesLocation(String OldLocation, String NewLocation) {
            if (!String.IsNullOrEmpty(NewLocation)) {
                if (!Directory.Exists(NewLocation)) {
                    Directory.CreateDirectory(Path.GetDirectoryName(NewLocation));
                }
                if (!String.IsNullOrEmpty(OldLocation)) {

                    if (File.Exists(Path.Combine(OldLocation, ModuleName + ".dock"))) {
                        File.Copy(Path.Combine(OldLocation, ModuleName + ".dock"), Path.Combine(NewLocation, ModuleName + ".dock"), true);
                        File.Delete(Path.Combine(OldLocation, ModuleName + ".dock"));
                    }

                    if (File.Exists(Path.Combine(OldLocation, ModuleName + ".EpsonModule"))) {
                        File.Copy(Path.Combine(OldLocation, ModuleName + ".EpsonModule"), Path.Combine(NewLocation, ModuleName + ".EpsonModule"), true);
                        File.Delete(Path.Combine(OldLocation, ModuleName + ".EpsonModule"));
                    }
                    else {
                        EpsonSettings.WriteConfiguration(new EpsonSettings(), Path.Combine(NewLocation, ModuleName + ".EpsonModule"));
                    }

                }

                if (!File.Exists(Path.Combine(NewLocation, ModuleName + ".EpsonModule"))) {

                    EpsonSettings.WriteConfiguration(new EpsonSettings(), Path.Combine(NewLocation, ModuleName + ".EpsonModule"));
                }
                ModuleSettingsFile = Path.Combine(NewLocation, ModuleName + ".EpsonModule");
            }
        }

        public override void StartModule(DockPanel mainDock, Boolean show = false) {
            if (mainDock.InvokeRequired) {
                mainDock.BeginInvoke(new MethodInvoker(delegate {
                    StartModule(mainDock,show);

                }
                ));
                return;
            }
            MainDock = mainDock;
            if (!ModuleStarted) {
                epsonForm = new EpsonMainForm();

                epsonForm.FormClosed += new FormClosedEventHandler(KPPVisionModule_FormClosed);
                

                if (String.IsNullOrEmpty(ModuleFilesLocation)) {
                    ModuleFilesLocation = AppDomain.CurrentDomain.BaseDirectory;
                }

                epsonForm.InitModule(ModuleName, ModuleSettingsFile);


                epsonForm.EpsonSettings.OnSelectedProjectChanged += new SelectedProjectChanged(EpsonModule_OnSelectedProjectChanged);
                EpsonModule_OnSelectedProjectChanged(epsonForm.EpsonSettings.SelectedProject);
                if (show) {
                    epsonForm.Show(MainDock);
                }
                ModuleStarted = true;
            }
        }

        void KPPVisionModule_FormClosed(object sender, FormClosedEventArgs e) {
            ModuleStarted = false;

            if (epsonForm.Restart) {
                Thread th = new Thread(new ThreadStart(StartModule));
                th.Start();
            }
        }
        [XmlIgnore, Browsable(false)]
        public EpsonProject ProjectSelected;

        void EpsonModule_OnSelectedProjectChanged(ModuleProject projectSelected) {
            ProjectSelected = (EpsonProject)projectSelected;
        }

        void StartModule() {
            Thread.Sleep(100);

            Control ctr = new Control();


            StartModule(MainDock);


        }







        public override void StopModule() {
            if (ModuleStarted) {
                epsonForm.Restart = false;
                epsonForm.Close();
                ModuleStarted = false;

            }

        }


        public override string ToString() {
            return ModuleName;
        }


    }


    public class KPPVisionModule : KPPModule {
        
        public override event ModuleNameChanged OnModuleNameChanged;
        

        private static KPPLogger log = new KPPLogger(typeof(KPPVisionModule));
        

        
        private String m_ModuleSettingsFile;
        [XmlIgnore, Browsable(false)]
        public override String ModuleSettingsFile {
            get { return m_ModuleSettingsFile; }
            set { m_ModuleSettingsFile = value; }
        }

        private string m_ModuleFilesLocation;
        [XmlAttribute, DisplayName("Module Settings location")]
        [EditorAttribute(typeof(AppFileFolderSelector), typeof(UITypeEditor))]
        public override string ModuleFilesLocation {
            get { return m_ModuleFilesLocation; }
            set {


                UpdateModuleFilesLocation(m_ModuleFilesLocation, value);

                m_ModuleFilesLocation = value;




            }
        }

        


        private String m_ModuleName = "New vision module";
        [XmlAttribute, DisplayName("Module Name")]
        public override String ModuleName {
            get { return m_ModuleName; }
            set {
                if (m_ModuleName != value) {
                    String oldvalue = m_ModuleName;
                    m_ModuleName = value;

                    if (OnModuleNameChanged!=null) {
                        OnModuleNameChanged(this,oldvalue);
                    }

                }
            }
        }

       
        [XmlIgnore]
        public override String ModuleType {
            get {
                return this.GetType().ToString();
            }
           
        }


        private DockPanel MainDock;

        [XmlIgnore, Browsable(false)]
        public VisionForm visionForm;

        
        [XmlIgnore, Browsable(false)]
        public override DockContent ModuleForm {
            get { return visionForm; }            
        }


        public override void UpdateModuleNameFiles(String OldModuleName,String NewModuleName) {

            if (!String.IsNullOrEmpty(NewModuleName)) {                
                if (!String.IsNullOrEmpty(OldModuleName)) {

                    if (File.Exists(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"))) {
                        File.Copy(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"), Path.Combine(ModuleFilesLocation, NewModuleName + ".dock"),true);
                        File.Delete(Path.Combine(ModuleFilesLocation, OldModuleName + ".dock"));
                    }

                    if (File.Exists(Path.Combine(ModuleFilesLocation, OldModuleName + ".VisionModule"))) {
                        File.Copy(Path.Combine(ModuleFilesLocation, OldModuleName + ".VisionModule"), Path.Combine(ModuleFilesLocation, NewModuleName + ".VisionModule"),true);
                        File.Delete(Path.Combine(ModuleFilesLocation, OldModuleName + ".VisionModule"));
                    } else {
                        VisionSettings.WriteConfiguration(new VisionSettings(), Path.Combine(NewModuleName, ModuleName + ".VisionModule"));
                    }

                }

                if (!File.Exists(Path.Combine(ModuleFilesLocation, ModuleName + ".VisionModule"))) {

                    VisionSettings.WriteConfiguration(new VisionSettings(), Path.Combine(ModuleFilesLocation, ModuleName + ".VisionModule"));
                }
                ModuleSettingsFile = Path.Combine(ModuleFilesLocation, ModuleName + ".VisionModule");
            }
        }

        public override void UpdateModuleFilesLocation(String OldLocation, String NewLocation) {
            if (!String.IsNullOrEmpty(NewLocation)) {
                if (!Directory.Exists(NewLocation)) {
                    Directory.CreateDirectory(Path.GetDirectoryName(NewLocation));
                }
                if (!String.IsNullOrEmpty(OldLocation)) {

                    if (File.Exists(Path.Combine(OldLocation, ModuleName + ".dock"))) {
                        File.Copy(Path.Combine(OldLocation, ModuleName + ".dock"), Path.Combine(NewLocation, ModuleName + ".dock"),true);
                        File.Delete(Path.Combine(OldLocation, ModuleName + ".dock"));
                    }

                    if (File.Exists(Path.Combine(OldLocation, ModuleName + ".VisionModule"))) {
                        File.Copy(Path.Combine(OldLocation, ModuleName + ".VisionModule"), Path.Combine(NewLocation, ModuleName + ".VisionModule"),true);
                        File.Delete(Path.Combine(OldLocation, ModuleName + ".VisionModule"));
                    } else {
                        VisionSettings.WriteConfiguration(new VisionSettings(), Path.Combine(NewLocation, ModuleName + ".VisionModule"));
                    }

                }

                if (!File.Exists(Path.Combine(NewLocation, ModuleName + ".VisionModule"))) {

                    VisionSettings.WriteConfiguration(new VisionSettings(), Path.Combine(NewLocation, ModuleName + ".VisionModule"));
                }
                ModuleSettingsFile = Path.Combine(NewLocation, ModuleName + ".VisionModule");
            }
        }

        public override void StartModule(DockPanel mainDock,Boolean show=false) {
            if (mainDock.InvokeRequired) {
                mainDock.BeginInvoke(new MethodInvoker(delegate {
                    
                    StartModule(mainDock,show);
                    
                }
                ));
                return;
            }
            MainDock = mainDock;
            if (!ModuleStarted) {

                visionForm = new VisionForm();
                 visionForm.FormClosed += new FormClosedEventHandler(KPPVisionModule_FormClosed);
                

                if (String.IsNullOrEmpty(ModuleFilesLocation)) {
                    ModuleFilesLocation = AppDomain.CurrentDomain.BaseDirectory;
                }

                visionForm.InitModule(ModuleName, ModuleSettingsFile);
                
                visionForm.VisionConfig.OnSelectedProjectChanged += new SelectedProjectChanged(KPPVisionModule_OnSelectedProjectChanged);
                KPPVisionModule_OnSelectedProjectChanged(visionForm.VisionConfig.SelectedProject);

                if (show) {
                    visionForm.Show(MainDock);
                }
                ModuleStarted = true;
            }
        }

        void KPPVisionModule_FormClosed(object sender, FormClosedEventArgs e) {
            ModuleStarted = false;

            if (visionForm.Restart) {
                visionForm = null;

                Thread th = new Thread(new ThreadStart(StartModule));
                th.Start();
            }
            else {
                visionForm = null;
            }
        }



        private ModuleProject _ProjectSelected;
        [XmlIgnore, Browsable(false)]
        public override ModuleProject ProjectSelected {
            get { return _ProjectSelected; }
            set { _ProjectSelected = value; }
        }
        
        

        void KPPVisionModule_OnSelectedProjectChanged(ModuleProject projectSelected) {
            ProjectSelected = projectSelected;
        }

        void StartModule() {
            Thread.Sleep(100);

            StartModule(MainDock, true);


        }


        

        


        public override void StopModule() {
            if (ModuleStarted) {
                visionForm.Restart = false;
                visionForm.Close();
                ModuleStarted = false;
                
            }

        }


        public override string ToString() {
            return ModuleName;
        }

        
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [XmlInclude(typeof(KPPVisionModule))]
    [XmlInclude(typeof(KPPEpsonModule))]
    public class KPPModule {

        public delegate void ModuleNameChanged(KPPModule module,String OldName);
        

        public virtual event ModuleNameChanged OnModuleNameChanged;

        private ModuleProject _ProjectSelected;
        [XmlIgnore, Browsable(false)]
        public virtual ModuleProject ProjectSelected {
            get { return _ProjectSelected; }
            set { _ProjectSelected = value; }
        }
        
     

        [XmlAttribute, DisplayName("Module Name")]
        public virtual String ModuleName {
            get;
            set;
        }

        private Boolean m_Enabled = false;
        [XmlAttribute]
        public virtual Boolean Enabled {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        private String m_ModuleType = "KPP Module";
        [XmlIgnore]
        public virtual String ModuleType {
            get { return m_ModuleType; }
            private set { m_ModuleType = value; }
        }


        private Boolean m_ModuleStarted = false;
        [XmlIgnore]
        public virtual Boolean ModuleStarted {
            get { return m_ModuleStarted; }
            internal set { m_ModuleStarted = value; }
        }

        private String m_ModuleSettingsFile;
        [XmlIgnore, Browsable(false)]
        public virtual String ModuleSettingsFile {
            get { return m_ModuleSettingsFile; }
            set { m_ModuleSettingsFile = value; }
        }

        [XmlAttribute, DisplayName("Module Settings location")]
        [EditorAttribute(typeof(AppFileFolderSelector), typeof(UITypeEditor))]
        public virtual string ModuleFilesLocation {
            get;
            set;
        }
        public virtual void UpdateModuleFilesLocation(String OldLocation, String NewLocation) {
        }
        public virtual void UpdateModuleNameFiles(String OldModuleName, String NewModuleName) {
        }

        private DockContent m_ModuleForm;
        [XmlIgnore,Browsable(false)]
        public virtual DockContent ModuleForm {
            get { return m_ModuleForm; }
            set { m_ModuleForm = value; }
        }
            


        public virtual void StopModule() {


        }


        public KPPModule() {
            




        }

        public KPPModule(String LoadProjectName)
            : this() {
        }

        public KPPModule(int LoadProjectID)
            : this() {

        }


        public virtual void StartModule(DockPanel mainDock, Boolean show = false) {



        }
       

        public override string ToString() {
            return ModuleName;
        }
    }


    public sealed class ApplicationSettings {


         private List<UserDef> m_Users = new List<UserDef>();

        public List<UserDef> Users {
            get { return m_Users; }
            set { m_Users = value; }
        }

        public static UserDef SelectedUser = null;


        #region -  Serialization attributes  -

        public static Int32 S_BackupFilesToKeep = 5;
        public static String S_BackupFolderName = "backup";
        public static String S_BackupExtention = "bkp";
        public static String S_DefaulFileExtention = "xml";

        private String _filePath = null;
        private String _defaultPath = null;

        [XmlIgnore]
        [Browsable(false)]
        public Int32 BackupFilesToKeep { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public String BackupFolderName { get; set; }
        [XmlIgnore]
        [Browsable(false)]
        public String BackupExtention { get; set; }

        #endregion
        private static KPPLogger log = new KPPLogger(typeof(ApplicationSettings));

        [XmlAttribute]
        [ReadOnly(true)]
        public String Name { get; set; }



        private CustomCollection<KPPModule> m_Modules = new CustomCollection<KPPModule>();
        //[XmlIgnore]
        [Category("Modules Definition")]
        [TypeConverter(typeof(ExpandableObjectConverter)), EditorAttribute(typeof(ModuleAddRemoveSelector), typeof(UITypeEditor))]
        public CustomCollection<KPPModule> Modules {
            get { return m_Modules; }
            set { m_Modules = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public ApplicationSettings() {
            Name = "Application Settings";
            //Visions.Add

        }

        #region Read Operations

        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static ApplicationSettings ReadConfigurationFile(string path) {
            //log.Debug(String.Format("Load Xml file://{0}", path));
            if (File.Exists(path)) {
                ApplicationSettings result = null;
                TextReader reader = null;

                try {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    reader = new StreamReader(path);
                    ApplicationSettings config = serializer.Deserialize(reader) as ApplicationSettings;
                    config._filePath = path;

                    result = config;
                }
                catch (Exception exp) {
                    log.Error("ApplicationSettings", exp);
                }
                finally {
                    if (reader != null) {
                        reader.Close();
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <param name="childtype">The childtype.</param>
        /// <param name="xmlString">The XML string.</param>
        /// <returns></returns>
        public static ApplicationSettings ReadConfigurationString(string xmlString) {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                ApplicationSettings config = serializer.Deserialize(new StringReader(xmlString)) as ApplicationSettings;

                return config;
            }
            catch (Exception exp) {
                log.Error("ApplicationSettings", exp);
            }
            return null;
        }

        #endregion

        #region Write Operations

        /// <summary>
        /// Writes the configuration.
        /// </summary>
        public void WriteConfigurationFile() {
            if (_filePath != null) {
                WriteConfigurationFile(_filePath);
            }
        }

        /// <summary>
        /// Writes the configuration.
        /// </summary>
        /// <param name="path">The path.</param>
        public void WriteConfigurationFile(string path) {
            WriteConfiguration(this, path, BackupFolderName, BackupExtention, BackupFilesToKeep);
        }

        /// <summary>
        /// Writes the configuration string.
        /// </summary>
        /// <returns></returns>
        public String WriteConfigurationToString() {
            return WriteConfigurationToString(this);
        }

        /// <summary>
        /// Writes the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="path">The path.</param>
        public static void WriteConfiguration(ApplicationSettings config, string path) {
            WriteConfiguration(config, path, S_BackupFolderName, S_BackupExtention, S_BackupFilesToKeep);
        }

        /// <summary>
        /// Writes the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="path">The path.</param>
        public static void WriteConfiguration(ApplicationSettings config, string path, string backupFolderName, String backupExtention, Int32 backupFilesToKeep) {
            if (File.Exists(path) && backupFilesToKeep > 0) {
                //Do a file backup prior to overwrite
                try {
                    //Check if valid backup folder name
                    if (backupFolderName == null || backupFolderName.Length == 0) {
                        backupFolderName = "backup";
                    }

                    //Check Backup folder
                    String bkpFolder = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Config"), backupFolderName);
                    if (!Directory.Exists(bkpFolder)) {
                        Directory.CreateDirectory(bkpFolder);
                    }

                    //Check extention
                    String ext = backupExtention != null && backupExtention.Length > 0 ? backupExtention : Path.GetExtension(path);
                    if (!ext.StartsWith(".")) { ext = String.Format(".{0}", ext); }

                    //Delete existing backup file (This should not exist)
                    String bkpFile = Path.Combine(bkpFolder, String.Format("{0}_{1:yyyyMMddHHmmss}{2}", Path.GetFileNameWithoutExtension(path), DateTime.Now, ext));
                    if (File.Exists(bkpFile)) { File.Delete(bkpFile); }

                    //Delete excess backup files
                    String fileSearchPattern = String.Format("{0}_*{1}", Path.GetFileNameWithoutExtension(path), ext);
                    String[] bkpFilesList = Directory.GetFiles(bkpFolder, fileSearchPattern, SearchOption.TopDirectoryOnly);
                    if (bkpFilesList != null && bkpFilesList.Length > (backupFilesToKeep - 1)) {
                        bkpFilesList = bkpFilesList.OrderByDescending(f => f.ToString()).ToArray();
                        for (int i = (backupFilesToKeep - 1); i < bkpFilesList.Length; i++) {
                            File.Delete(bkpFilesList[i]);
                        }
                    }

                    //Backup current file
                    File.Copy(path, bkpFile);
                    //log.Debug(String.Format("Backup file://{0} to file://{1}", path, bkpFile));
                }
                catch (Exception exp) {
                    //log.Error(String.Format("Error copying file {0} to backup.", path), exp);
                }
            }
            try {
                XmlSerializer serializer = new XmlSerializer(config.GetType());

                //serializer.

                TextWriter textWriter = new StreamWriter(path);
                serializer.Serialize(textWriter, config);
                textWriter.Close();
                //log.Debug(String.Format("Write Xml file://{0}", path));
            }
            catch (Exception exp) {
                //log.Error("Error writing configuration. ", exp);
                Console.WriteLine(exp.ToString());
            }
        }

        /// <summary>
        /// Writes the configuration to string.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static String WriteConfigurationToString(ApplicationSettings config) {
            try {
                XmlSerializer serializer = new XmlSerializer(config.GetType());
                StringWriter stOut = new StringWriter();
                serializer.Serialize(stOut, config);
                return stOut.ToString();
            }
            catch (Exception exp) {
                //log.Error("Error writing configuration. ", exp);
            }
            return null;
        }

        #endregion
    }
    

}
