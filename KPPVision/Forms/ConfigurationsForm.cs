using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KPP.Core.Debug;
using System.IO;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;
using IOModule;

namespace VisionModule {
    internal partial class ConfigurationsForm : Form {

        private static KPPLogger log = new KPPLogger(typeof(ConfigurationsForm));

        private VisionSettings _appsettings;


        public VisionSettings Appsettings {
            get { return _appsettings; }
            set {
                if (_appsettings != value) {
                    _appsettings = value;
                    if (_appsettings != null) {
                        __textFiles.Text = _appsettings.ProjectFile;
                    }
                }
            }
        }
      

        public ConfigurationsForm() {
            switch (LanguageSettings.Language) {
                case LanguageName.Unk:
                    break;
                case LanguageName.PT:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");

                    break;
                case LanguageName.EN:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

                    break;
                default:
                    break;
            }
            InitializeComponent();
        }





        private void UpdateAvaibleFiles(String path) {

            try {
                __listfiles.Items.Clear();
                Uri fullPath = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), path);

                String newpath = fullPath.LocalPath;
                if (Directory.Exists(newpath)) {

                    String[] files = Directory.GetFiles(newpath, "*.proj");

                    foreach (String item in files) {
                        __listfiles.Items.Add(Path.GetFileName(item));
                    } 
                }
                else {
                    log.Warn("Directory ("+newpath+") does not exists");
                }

            } catch (Exception exp) {
                log.Error(exp);

            }

        }

        private void __ConfigurationsForm_Load(object sender, EventArgs e) {

            __listdirs.Items.Clear();
            if (_appsettings != null) {
                __listdirs.Items.AddRange(_appsettings.ProjectDirectories.ToArray());

            }


            //Uri fullPath = new Uri(, UriKind.Absolute);
            //Uri relRoot = new Uri(Path.GetDirectoryName(Application.ExecutablePath), UriKind.Absolute);

            //String relative = relRoot.MakeRelativeUri(fullPath).ToString();



            __listdirs.SelectedItem = _appsettings.ProjectDirectories.Find(s => _appsettings.ProjectFile.Contains(s));
            __listfiles.SelectedItem = Path.GetFileName(_appsettings.ProjectFile);
            

        }

        private void __btExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void ConfigurationsForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        private void __iolist_ItemsChanged(object sender, ItemsChangedEventArgs e) {
            
        }

        private void __iolist_CellEditFinishing(object sender, CellEditEventArgs e) {
          
        }

        private void __btCreateEmpty_Click(object sender, EventArgs e) {
            
        }

        private void __comboListProjects_DropDown(object sender, EventArgs e) {
            
          
        }

        private void __btAddDir_Click(object sender, EventArgs e) {
            try {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (__listdirs.SelectedIndex >= 0) {
                    fbd.SelectedPath = (String)__listdirs.SelectedItem;
                }
                else {
                    fbd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    if (!__listdirs.Items.Cast<String>().ToList().Contains(fbd.SelectedPath)) {

                        Uri fullPath = new Uri(fbd.SelectedPath, UriKind.Absolute);
                        Uri relRoot = new Uri(AppDomain.CurrentDomain.BaseDirectory, UriKind.Absolute);

                        String relative = relRoot.MakeRelativeUri(fullPath).ToString();
                        relative = relative.Replace("%20", " ");

                        _appsettings.ProjectDirectories.Add(relative);

                        __listdirs.Items.Add(relative);

                        UpdateAvaibleFiles(relative);
                    }
                }
            }
            catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __btRemove_Click(object sender, EventArgs e) {
            if (__listdirs.SelectedIndex>=0) {
               _appsettings.ProjectDirectories.Remove((String)__listdirs.SelectedItem);
                __listdirs.Items.RemoveAt(__listdirs.SelectedIndex);
            }
        }

        private void __listdirs_SelectedIndexChanged(object sender, EventArgs e) {
            if (__listdirs.SelectedIndex>=0) {
                __listfiles.Enabled = true;
                __btCreateEmpty.Enabled = true;
                UpdateAvaibleFiles((String)__listdirs.SelectedItem);
            }
            else {
                __listfiles.Enabled = false;
                __btCreateEmpty.Enabled = false;
                __listfiles.Items.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (__listfiles.SelectedItem!=null) {
               _appsettings.ProjectFile = (String)__listdirs.SelectedItem +"/"+(String)__listfiles.SelectedItem;
                __textFiles.Text = _appsettings.ProjectFile;
            }
        }

        private void __btExit_Click_1(object sender, EventArgs e) {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            if (__listfiles.SelectedItem != null) {
                ReloadFile= (String)__listdirs.SelectedItem + "/" + (String)__listfiles.SelectedItem;
                this.Close();
            }
        }






        public string ReloadFile { get; set; }

        private void button3_Click(object sender, EventArgs e) {
            _appsettings.ProjectFile = "";
            __textFiles.Text = "";
        }

        private void __btCreateEmpty_Click_1(object sender, EventArgs e) {
            VisionProjects newprojects = new VisionProjects();
            if (!__EditEmptyFile.Text.Contains(".proj")) {
                __EditEmptyFile.Text = __EditEmptyFile.Text + ".proj";
            }
            newprojects.WriteConfigurationFile(__listdirs.SelectedItem.ToString() + "/" + __EditEmptyFile.Text);
            UpdateAvaibleFiles((String)__listdirs.SelectedItem);
        }

        private void __tooladdServer_Click(object sender, EventArgs e) {
            _appsettings.Servers.Add(new TCPServer("New Server", 0));
            __serverconflist.Objects=_appsettings.Servers;
        }

        private void __toolremoveServer_Click(object sender, EventArgs e) {
            _appsettings.Servers.Remove(__serverconflist.SelectedObject as TCPServer);
            __serverconflist.Objects = _appsettings.Servers;
        }

        private void __btSave_Click(object sender, EventArgs e) {

        }
    }
}
