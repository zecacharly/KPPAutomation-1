using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KPP.Core.Debug;
using System.Threading;
using System.IO;
using System.Resources;
using System.Reflection;

namespace KPPAutomation {
    static class Program {

        public enum LanguageName { Unk, PT, EN }
        public static Boolean Restart = true;



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

       

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params String[] prms) {
            bool ok;
            Mutex m = new Mutex(true, "KPP.Automation.Software.v1.0", out ok);
            
            
            //TODO check folder logs
            DebugController.ActiveDebugController = new DebugController(Path.Combine(Application.StartupPath,"logs"),fileExtention:".log",enableTelnet:false);
            
            //String path = Path.GetDirectoryName(Application.ExecutablePath);
           // DebugController.ActiveDebugController = new DebugController(path);
            //log = new KPPLogger(typeof(t));
            Boolean startadmin = false;
            String confpath = "";
            if (prms != null && prms.Length > 0) {

                for (int i = 0; i < prms.Length; i++) {
                    if (prms[i] == "-SetAdmin") {
                        startadmin = true;
                    } else if (prms[i].Contains("-Conf")) {
                        String[] strs = prms[i].Split(new String[]{"="},StringSplitOptions.None);
                        if (strs.Count()==2) {
                            confpath = strs[1];
                        }
                    }


                }

            }

            if (ok) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                while (Restart) {
                    MainForm runningform = new MainForm();
                    try {

                        runningform.SetAdmin = startadmin;
                        runningform.ConfPath = confpath;

                        Restart = false;
                        Application.Run(runningform);




                    }
                    catch (Exception exp) {
                        
                        Restart = false;


                      


                    }
                }
            }
        }
    }
}
