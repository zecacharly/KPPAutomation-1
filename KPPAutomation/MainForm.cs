using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using KPP.Core.Debug;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using VisionModule;
using KPPAutomationCore;
using IOModule;

namespace KPPAutomation {
    public partial class MainForm : Form {

        public Boolean SetAdmin = false;
        public String ConfPath = "";
        private static KPPLogger log = new KPPLogger(typeof(MainForm));
        private ConfigForm _ConfigForm = new ConfigForm();
        
        private String m_AppFile = "";
        public String AppFile {
            get { return m_AppFile; }
            set { m_AppFile = value; }
        }

        private ApplicationSettings m_ApplicationConfig = null;
        public ApplicationSettings ApplicationConfig {
            get { return m_ApplicationConfig; }
            set { m_ApplicationConfig = value; }
        }

        private string MainDockFile = "";
        DeserializeDockContent m_deserializeDockContent;

        public MainForm() {

            switch (Program.Language) {
                case Program.LanguageName.Unk:
                    break;
                case Program.LanguageName.PT:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");

                    break;
                case Program.LanguageName.EN:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

                    break;
                default:
                    break;
            }
          
            InitializeComponent();

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
          
            
        }

        
      
        private void MainForm_Load(object sender, EventArgs e) {

            try {

                DebugController.ActiveDebugController.OnDebugMessage += new OnDebugMessageHandler(ActiveDebugController_OnDebugMessage);
                

                __MainDock.ActiveContentChanged += new EventHandler(__MainDock_ActiveContentChanged);
                switch (Program.Language) {
                    case Program.LanguageName.Unk:
                        break;
                    case Program.LanguageName.PT:
                        portugueseToolStripMenuItem.Checked = true;
                        englishToolStripMenuItem.Checked = false;
                        break;
                    case Program.LanguageName.EN:
                        portugueseToolStripMenuItem.Checked = false;
                        englishToolStripMenuItem.Checked = true;
                        break;
                    default:
                        break;
                }

                TextBox __logpasstb = this.__logpass.Control as TextBox;                
                __logpasstb.PasswordChar = '*'; 

                if (!Directory.Exists(ConfPath)) {
                    try {
                        Directory.CreateDirectory(ConfPath);
                    } catch (Exception exp) {

                        ConfPath = "config";
                    }
                }


                AppFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfPath+"\\KPPAutomationSettings.app");

                if (!File.Exists(AppFile)) {
                    if (!Directory.Exists(AppFile)) {
                        Directory.CreateDirectory(Path.GetDirectoryName(AppFile));
                    }
                    ApplicationConfig = new ApplicationSettings();
                    ApplicationConfig.WriteConfigurationFile(AppFile);
                }
                ApplicationConfig = ApplicationSettings.ReadConfigurationFile(AppFile);
                ApplicationConfig.BackupExtention = ".bkp";
                ApplicationConfig.BackupFilesToKeep = 5;
                ApplicationConfig.BackupFolderName = "Backup";
                if (ApplicationConfig.Users.Count == 0) {
                    ApplicationConfig.Users.Add(new UserDef("auto123", Acesslevel.Admin));
                    ApplicationConfig.Users.Add(new UserDef("man", Acesslevel.Man));
                }


                MainDockFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfPath+"\\MainDockPanel.dock");


                if (!Directory.Exists(Path.GetDirectoryName(MainDockFile))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(MainDockFile));
                }

                _ConfigForm.__btsaveConf.Click += new EventHandler(__btsaveConf_Click);
                _ConfigForm.__PropertySettings.SelectedObject = ApplicationConfig;



                

                AcessManagement.OnAcesslevelChanged += new AcessManagement.AcesslevelChanged(AcessManagement_OnAcesslevelChanged);


                if (SetAdmin) {
                    AcessManagement.AcessLevel = Acesslevel.Admin;
                }
                else {
                    AcessManagement.AcessLevel = Acesslevel.User;
                }
                foreach (KPPModule item in ApplicationConfig.Modules) {
                    try {

                        if (item.Enabled) {
                            item.StartModule(__MainDock);
                            item.OnModuleNameChanged += new KPPModule.ModuleNameChanged(item_OnModuleNameChanged);
                           
                        }
                    }
                    catch (Exception exp) {

                        log.Error(exp);
                    }
                }



                //foreach (KPPModule item in ApplicationConfig.Modules) {
                //    item.ShowModule(__MainDock);


                //}

                ////__MainDock.
                if (File.Exists(MainDockFile))
                    try {
                        __MainDock.LoadFromXml(MainDockFile, m_deserializeDockContent);
                    }
                    catch (Exception exp) {

                       __MainDock.SaveAsXml(MainDockFile);

                    }
                else {


                }


                if (!_LogForm.Visible) {
                    _LogForm.Show(__MainDock);
                }

                foreach (KPPModule item in ApplicationConfig.Modules) {
                    try {

                        if (item.Enabled) {
                            if (!item.ModuleForm.Visible) {
                                item.ModuleForm.Show(__MainDock);

                            }
                            
                        }
                    } catch (Exception exp) {

                        log.Error(exp);
                    }
                }


                //__MainDock.Update();
                //LoadModulesThread = new Thread(new ThreadStart(DoLoadModules));
                //LoadModulesThread.IsBackground = true;
                //LoadModulesThread.Start();              

            
                __MainDock.ActiveContentChanged+=new EventHandler(__MainDock_ActiveContentChanged);

                PhidgetsIO.InitializePhidgets();

                

                
            }
            catch (Exception exp) {

                log.Error(exp);
            }
           
        }

        void PhidgetsIO_OnBordAvaible(int boardSerial) {
            
        }


        void item_OnModuleNameChanged(KPPModule module, string OldName) {
            if (MessageBox.Show(this.GetResourceText("Change_Module_Name"), this.GetResourceText("Confirm_option"), MessageBoxButtons.YesNo) == DialogResult.Yes) {
                _ConfigForm.Close();
                Thread.Sleep(100);
                module.StopModule();
                module.UpdateModuleNameFiles(OldName,module.ModuleName);
                ApplicationConfig.WriteConfigurationFile();
                module.StartModule(__MainDock);
            }
        }



        KPPModule _activemodule = null;
        void __MainDock_ActiveContentChanged(object sender, EventArgs e) {
            //try {
            //    foreach (KPPModule item in ApplicationConfig.Modules) {
            //        DockPanel dock = (DockPanel)sender;
            //        if (item.GetModuleForm().Equals(dock.ActiveContent)) {
            //            _activemodule=item;
            //            break;
            //        }
            //    }
            //} catch (Exception exp) {

            //    log.Error(exp);
            //}
        }

        LogForm _LogForm = new LogForm();
        void ActiveDebugController_OnDebugMessage(object sender, DebugMessageArgs e) {
            try {
                
                if (InvokeRequired) {
                    BeginInvoke(new MethodInvoker(delegate { ActiveDebugController_OnDebugMessage(sender, e); }));
                }
                else {
                    if (e.MessageType == MessageType.Error || e.MessageType == MessageType.Fatal) {


                        //_LogForm.__textBoxExceptions.Text += "["+e.IdName+"]"+e.Message;
                        _LogForm.__textBoxExceptions.Text += e.Message;


                        if (_LogForm.__textBoxExceptions.Text.Length > 0) {
                            _LogForm.__textBoxExceptions.AppendText(Environment.NewLine);
                        }

                        _LogForm.__textBoxExceptions.Text += Environment.NewLine;

                    }
                    else {
                        //_LogForm.__textBoxWarnings.Text += "[" + e.IdName + "]"+e.Message;
                        _LogForm.__textBoxWarnings.Text += e.Message;

                        if (_LogForm.__textBoxWarnings.Text.Length > 0) {
                            _LogForm.__textBoxWarnings.AppendText(Environment.NewLine);
                        }

                    }
                    if (_LogForm.__textBoxExceptions.Text.Length > 0) {

                        _LogForm.__textBoxExceptions.SelectionStart = _LogForm.__textBoxExceptions.Text.Length - 1;
                        _LogForm.__textBoxExceptions.SelectionLength = 0;
                        _LogForm.__textBoxExceptions.ScrollToCaret();
                    }

                }
            }
            catch (Exception exp) {


            }
        }

        void AcessManagement_OnAcesslevelChanged(Acesslevel NewLevel) {
            Boolean state = (NewLevel == Acesslevel.Admin || NewLevel == Acesslevel.Man);
            
            __btConfig.Visible = state;

            switch (NewLevel) {
                case Acesslevel.Admin:
                    __dropLogin.Text = this.GetResourceText("Admin_Mode");
                    __dropLogin.Image = new Bitmap(Properties.Resources.AcessUnlock);
                    __dropLogin.BackColor = Color.LightGreen;
                    __btlogin.Text = this.GetResourceText("Logout");
                    break;
                case Acesslevel.Man:
                    __dropLogin.Text = this.GetResourceText("Man_mode");
                    __dropLogin.Image = new Bitmap(Properties.Resources.AcessUnlock);
                    __dropLogin.BackColor = Color.LightGreen;
                    break;
                case Acesslevel.User:
                    __dropLogin.BackColor = SystemColors.Control;
                    __dropLogin.Image = new Bitmap(Properties.Resources.Acesslock);
                    __dropLogin.Text = this.GetResourceText("User_Mode");
                    __btlogin.Text = this.GetResourceText("Login");
                    break;
                default:
                    break;
            }
        }
        void __btsaveConf_Click(object sender, EventArgs e) {
            ApplicationConfig.WriteConfigurationFile();
        }

        private IDockContent GetContentFromPersistString(string persistString) {

            if (persistString == typeof(LogForm).ToString())
                return _LogForm;



            foreach (KPPModule item in ApplicationConfig.Modules) {
                if (persistString == item.ModuleName) {
                    return item.ModuleForm;
                }

            }

         
                return null;
           
        }

        private Boolean restartapp(Program.LanguageName lang) {
            try {

                String cap = this.GetResourceText("MessageBox_Language_caption");
                String text = this.GetResourceText("MessageBox_Language_text");
                //String text = res_man.GetString("", Thread.CurrentThread.CurrentUICulture);
                if (MessageBox.Show(text, cap, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK) {
                    Program.Language = lang;
                    Program.Restart = true;
                    this.Close();
                }
            }
            catch (Exception exp) {

                log.Error(exp);
            }
            return false;
        }

        private void portugueseToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Program.Language != Program.LanguageName.PT) {
                if (!restartapp(Program.LanguageName.PT)) {
                    portugueseToolStripMenuItem.Checked = false;
                    englishToolStripMenuItem.Checked = true;
                }
            }
            
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Program.Language != Program.LanguageName.EN) {
                if (!restartapp(Program.LanguageName.EN)) {
                    portugueseToolStripMenuItem.Checked = true;
                    englishToolStripMenuItem.Checked = false;
                }
            }
        }

        //private void LoadVisionModule() {
        //    try {

        //        ApplicationConfig.Vision.StartModule(__MainDock);
        //        __MainDock.Refresh();
        //    }
        //    catch (Exception exp) {

        //        log.Error(exp);
        //    }
        //}

        private void UnLoadVisionModules() {
            foreach (KPPModule item in ApplicationConfig.Modules) {
                if (item.ModuleStarted) {
                    item.ModuleForm.Close();
                    //TODDO dispose vision
                }
            }

        }


        private void __btConfig_Click(object sender, EventArgs e) {
            _ConfigForm.ShowDialog();
            //if (ApplicationConfig.Vision.Enabled) {
            //    LoadVisionModule();
            //}
            //else {
            //    UnLoadVisionModule();
            //}
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                __MainDock.SaveAsXml(MainDockFile);
                UnLoadVisionModules();
            }
            catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __btlogin_Click(object sender, EventArgs e) {
            if (__btlogin.Text == this.GetResourceText("Login")) {
                ApplicationSettings.SelectedUser = ApplicationConfig.Users.Find(bypass => bypass.Pass == __logpass.Text);
                if (ApplicationSettings.SelectedUser != null) {
                    AcessManagement.AcessLevel = ApplicationSettings.SelectedUser.Level;
                    __dropLogin.HideDropDown();


                }
            }
            else {

                __logpass.Text = "";

                AcessManagement.AcessLevel= Acesslevel.User;
                __dropLogin.HideDropDown();


            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if ((keyData == Keys.F9 || keyData == Keys.F8 || keyData == Keys.F7 || keyData == Keys.F6) || (keyData == (Keys.Control | Keys.L)) || (keyData == (Keys.Control | Keys.S))) {
                try {
                    foreach (KPPModule item in ApplicationConfig.Modules) {

                        if (item.ModuleForm.Equals(__MainDock.ActiveContent)) {
                            if (item is KPPVisionModule) {
                                VisionProject project = (VisionProject)((KPPVisionModule)item).ProjectSelected;
                                if (project != null) {
                                    if (keyData == Keys.F9) {
                                        project.SelectedRequest.ProcessRequest(null, true, null);
                                    } else if (keyData == Keys.F8) {
                                        project.SelectedRequest.ProcessRequest(null, false, null);
                                    } else if (keyData == Keys.F7) {
                                        ((VisionForm)item.ModuleForm).captureAndProcessToolStripMenuItem_Click(null, null);                                        
                                    } else if (keyData == Keys.F6) {
                                        ((VisionForm)item.ModuleForm).processToolStripMenuItem_Click(null, null);                                        
                                    }else if ((keyData == (Keys.Control | Keys.L))) {
                                        ((VisionForm)item.ModuleForm).OpenToolStripMenuItem_Click(null, null);
                                    } else if ((keyData == (Keys.Control | Keys.S))) {
                                        ((VisionForm)item.ModuleForm).__toolSaveproj_Click(null, null);
                                    }


                                }
                            }
                            break;
                        }
                    }
                } catch (Exception exp) {

                    log.Error(exp);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e) {
            
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
           
        }

    }
}
