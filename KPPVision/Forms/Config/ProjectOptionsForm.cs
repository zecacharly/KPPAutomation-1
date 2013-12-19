using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisionModule;
using KPP.Core.Debug;
using BrightIdeasSoftware;
using System.Threading;
using System.Globalization;
using KPPAutomationCore;

namespace VisionModule {
    internal partial class ProjectOptionsForm : Form {

        public VisionProjects _projsconf = null;
        public String _projsfile = "";
        private static KPPLogger log = new KPPLogger(typeof(ProjectOptionsForm));

        public ProjectOptionsForm() {
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

        private void ProjectOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
            this.Hide();
            e.Cancel=true;
        }


        private void _btNewProj_Click(object sender, EventArgs e) {
            

        }

        private void __btsave_Click(object sender, EventArgs e) {
            _projsconf.WriteConfigurationFile();
        }

        private void __listprojects_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e) {
            try {
                if (e.Cancel) {
                    return;
                }
                
                if (e.Column == olvLoadOnStart && e.RowObject != null) {

                    Boolean cellvalue = (Boolean)e.NewValue;
                    String projname = ((VisionProject)e.RowObject).Name;

                    foreach (VisionProject item in _projsconf.Projects) {
                        if (cellvalue == true) {
                            if (item.Name != projname) {
                                item.Loadonstart = false;
                            }
                        }
                    }

                    //e.Value
                }
                else if (e.Column == olvProjName) {
                    ((VisionProject)e.RowObject).Name = (String)e.NewValue;                
                    
                } 
                __listprojects.RefreshObjects(_projsconf.Projects);
                List<VisionProject> projects=__listprojects.Objects.Cast<VisionProject>().ToList();
                _projsconf.Projects = projects;
                _projsconf.WriteConfigurationFile(_projsfile);
            }
            catch (Exception exp) {

                log.Error(exp);
            }
        }

        private void __btduplicate_Click(object sender, EventArgs e) {
            VisionProject source=__listprojects.SelectedObject as VisionProject;

            if (source!=null) {
                VisionProject newproj = source.Clone() as VisionProject;
                if (newproj!=null) {
                    newproj.Name = newproj.Name + "_copy";
                    _projsconf.Projects.Add(newproj);
                    __listprojects.Objects = _projsconf.Projects;
                }
            }
        }

        private void __listprojects_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode== Keys.Enter) {
                if (__listprojects.SelectedIndex>0) {
                    __btLoadProj.PerformClick();
                }
            }
        }

       

        private void ProjectOptionsForm_Load(object sender, EventArgs e) {
            AcessManagement.OnAcesslevelChanged += new AcessManagement.AcesslevelChanged(StaticObjects_OnAcesslevelChanged);
            
        }

        void StaticObjects_OnAcesslevelChanged(Acesslevel NewLevel) {
            Boolean state = NewLevel ==Acesslevel.Admin;

            olvLoadOnStart.IsVisible = state;
            olvProjID.IsVisible = state;
         
            __btduplicate.Visible = state;
            __btNewProj.Visible = state;

            __listprojects.RebuildColumns();

        }

       

        private void button1_Click(object sender, EventArgs e) {
            if (__listprojects.SelectedIndex==0) {
                __listprojects.SelectedIndex = __listprojects.Items.Count - 1;
            } else {
                __listprojects.SelectedIndex--;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (__listprojects.SelectedIndex == __listprojects.Items.Count - 1) {
                __listprojects.SelectedIndex = 0;
            } else {
                __listprojects.SelectedIndex++;
            }
        }

        private void __btNewProj_Click(object sender, EventArgs e) {
            try {
                
                _projsconf.Projects.Add(new VisionProject());
                __listprojects.Objects=_projsconf.Projects;
                _projsconf.WriteConfigurationFile(_projsfile);

            } catch (Exception exp) {

                log.Error(exp);
            }
        }

      
    }
}
