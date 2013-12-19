using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace VisionModule {

    //public class ProcessingFunctionAvaible : Attribute {

    //    Type InspectionType{get;set;}

    //    public ProcessingFunctionAvaible(Type InspType) {
    //        InspectionType = InspType;
    //    }
    //}

    public class ProcessingFunctionAttribute : Attribute {
        public String FunctionType { get; set; }
        

        [Browsable(false), XmlAttribute]
        public String FunctionGroup { get; set; }
        public ProcessingFunctionAttribute(String FuncType,String FuncGroup) {
            FunctionType = FuncType;
            FunctionGroup = FuncGroup;
            
        }
    }
}
