using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KPP.Core.Debug;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using KPPAutomationCore;

namespace VisionModule {

    internal partial class OutputResultConfForm : Form {
    
        public OutputResultConfForm() {                      
            InitializeComponent();
        }

        private static KPPLogger log = new KPPLogger(typeof(OutputResultConfForm));

        public VisionProject SelectedProject = null;

        private Type _acceptType = null;

        public Type acceptType {
            get { return _acceptType; }
            set { 
                _acceptType = value; 
            }
        }

        public ResultReference ResultRef= null;
        private void OutputResultConfForm_Load(object sender, EventArgs e) {
            if (SelectedProject != null) {
                __comboRequest.Items.Clear();
                __comboRequest.Items.AddRange(SelectedProject.RequestList.ToArray());

                __comboRequest.SelectedItem = SelectedProject.SelectedRequest;
                if (SelectedProject.SelectedRequest != null) {
                    __comboInsp.SelectedItem = SelectedProject.SelectedRequest.SelectedInspection;
                    if (__comboInsp.SelectedItem != null) {
                        __comboRois.SelectedItem = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI;

                    }
                }
                Object newid = null;

                try {
                    newid = Activator.CreateInstance(acceptType);
                } catch (Exception exp) {

                    
                }

                if (ResultRef == null) {
                    ResultRef = new ResultReference(SelectedProject,newid);
                } else {
                    if (ResultRef.ResultReferenceID != null) {
                        if (ResultRef.ResultReferenceID.GetType() != acceptType) {
                            if (ResultRef.ResultReferenceID==null) {
                                ResultRef.ResultReferenceID = newid; 
                            }
                        }
                    } else {
                        ResultRef.ResultReferenceID = newid;
                    }
                }

                propertyGrid1.SelectedObject = ResultRef;

            }
        }

        private void __comboRequest_SelectedValueChanged(object sender, EventArgs e) {
            __comboInsp.SelectedItem = null;
            __comboInsp.Items.Clear();
            if (__comboRequest.SelectedItem!=null) {

                __comboInsp.Items.Clear();
                __comboInsp.Items.AddRange(((Request)__comboRequest.SelectedItem).Inspections.ToArray());
                //if (SelectedProject.SelectedRequest!=null) {
                //    __comboInsp.SelectedItem = SelectedProject.SelectedRequest.SelectedInspection;
                //}
                
            }
            
        }

        private void __comboInsp_SelectedIndexChanged(object sender, EventArgs e) {
            __comboRois.SelectedItem = null;
            __comboRois.Items.Clear();
            if (__comboInsp.SelectedItem != null) {
                __comboRois.Items.Clear();
                __comboRois.Items.AddRange(((Inspection)__comboInsp.SelectedItem).ROIList.ToArray());
                //if (SelectedProject.SelectedRequest.SelectedInspection!=null) {
                //    __comboRois.SelectedItem = SelectedProject.SelectedRequest.SelectedInspection.SelectedROI;
                //}
                
            }
            
        }

        private void __comboRois_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                __proplist.ClearObjects();
                if (__comboRois.SelectedItem != null) {
                    __RoiProcList.Objects = ((ROI)__comboRois.SelectedItem).ProcessingFunctions.ToArray();

                }
                else {
                    __RoiProcList.ClearObjects();
                }
            } catch (Exception exp) {

                log.Error(exp);
                
            }
        }

        private void __RoiProcList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            ProcessingFunctionBase proc = __RoiProcList.SelectedObject as ProcessingFunctionBase;
            __proplist.ClearObjects();
            if (proc != null) {

                if (acceptType.Name == "ProcessingFunctionBase") {
                    ResultRef.ResultReferenceID = ((Request)__comboRequest.SelectedItem).Name + "." + ((Inspection)__comboInsp.SelectedItem).Name + "." + ((ROI)__comboRois.SelectedItem).Name + "." + ((ProcessingFunctionBase)__RoiProcList.SelectedObject).FunctionName;
                    ResultRef.ResultOutput = __RoiProcList.SelectedObject;
                    propertyGrid1.Refresh();
                } else {



                    PropertyInfo[] propinfs = proc.GetType().GetProperties();

                    foreach (PropertyInfo propinf in propinfs) {

                        Object[] customattrs = propinf.GetCustomAttributes(false);
                        foreach (Object item in customattrs) {
                            if (item is CategoryAttribute) {

                                if ((item as CategoryAttribute).Category == "Post-Processing"/* || (item as CategoryAttribute).Category == "Pre-Processing"*/) {

                                    String propTypeName = propinf.PropertyType.Name;

                                    if (propTypeName.Contains("object")) {
                                        //item.GetType().
                                    }

                                    if (propTypeName == acceptType.Name || acceptType.Name== "String") {
                                        __proplist.AddObject(propinf);
                                    } else if (propinf.PropertyType.Name == "Object" || propinf.PropertyType.Name == "object") {
                                        try {
                                            object obj = __RoiProcList.SelectedObject.GetType().GetProperty(propinf.Name).GetValue(__RoiProcList.SelectedObject, null);
                                            if (obj.GetType().Name == acceptType.Name) {
                                                __proplist.AddObject(propinf);
                                            }
                                        } catch (Exception exp) {


                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

        }

        private void __proplist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            try {

                
                if (ResultRef!=null) {
                    ResultRef.ResultReferenceID = ((Request)__comboRequest.SelectedItem).Name + "." + ((Inspection)__comboInsp.SelectedItem).Name + "." + ((ROI)__comboRois.SelectedItem).Name + "." + ((ProcessingFunctionBase)__RoiProcList.SelectedObject).FunctionName + "." + ((PropertyInfo)(__proplist.SelectedObject)).Name;
                    ResultRef.UpdateValue();
                    propertyGrid1.Refresh();
                }
                
            } catch (Exception exp) {

                log.Error(exp);
            }
        }


       


        private void button1_Click(object sender, EventArgs e) {
            if (ResultRef.IsValid) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }                       
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
         //   textBox2.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
           // textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e) {
            if (ResultRef!=null) {
                try {

                    if (acceptType.Name=="String") {
                        ResultRef.ResultReferenceID = "No value";                        
                    } else {
                        
                            Object newobj = Activator.CreateInstance(acceptType);
                            ResultRef.SelectedVisionProject = SelectedProject;
                            ResultRef.ResultReferenceID = newobj; 
                       
                    }

                    
                    propertyGrid1.SelectedObject = ResultRef;
                    ResultRef.UpdateValue();
                    propertyGrid1.Refresh();
                } catch (Exception exp) {

                    
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            ResultRef.ResultReferenceID = null;
            propertyGrid1.SelectedObject = ResultRef;
            ResultRef.UpdateValue();
            propertyGrid1.Refresh();
        }
    }
}
