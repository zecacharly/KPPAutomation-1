using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using log4net.Layout;
//using log4net.Repository.Hierarchy;
//using log4net;
//using log4net.Appender;

namespace KPPAutomationCore.Debug {




    //public static class KPPLoggerManager {
    //    private static PatternLayout _layout = new PatternLayout();
    //    private const string LOG_PATTERN = "%d [%t] %-5p %m%n";

    //    public static string DefaultPattern {
    //        get { return LOG_PATTERN; }
    //    }

    //    static KPPLoggerManager() {
    //        _layout.ConversionPattern = DefaultPattern;
    //        _layout.ActivateOptions();

    //        Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
    //        hierarchy.Configured = true;
    //    }

    //    private static PatternLayout DefaultLayout {
    //        get { return _layout; }
    //    }

    //    public static ILog GetNamedLogger(string name) {


    //        return LogManager.GetLogger(name);
    //    }

    //    private class KPPRollingFileAppender : RollingFileAppender {

    //        protected override void Append(log4net.Core.LoggingEvent loggingEvent) {

    //            base.Append(loggingEvent);
    //        }
    //    }

    //    public static ILog AddNamedLogger(string name) {
    //        Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
    //        Logger newLogger = hierarchy.GetLogger(name) as Logger;
            
    //        PatternLayout patternLayout = new PatternLayout();
    //        patternLayout.ConversionPattern = LOG_PATTERN;
    //        patternLayout.ActivateOptions();

    //        KPPRollingFileAppender roller = new KPPRollingFileAppender();
    //        roller.Layout = patternLayout;
    //        roller.AppendToFile = true;
    //        roller.RollingStyle = RollingFileAppender.RollingMode.Size;
    //        roller.MaxSizeRollBackups = 4;
    //        roller.MaximumFileSize = "100KB";
    //        roller.StaticLogFileName = true;
    //        roller.File = name + ".log";
    //        roller.ActivateOptions();
            
    //        newLogger.AddAppender(roller);

    //        return LogManager.GetLogger(name);
    //    }
    //}

}
