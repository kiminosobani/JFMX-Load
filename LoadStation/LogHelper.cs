using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace LoadStation
{
    class LogHelper
    {
        public const int LogLevel_Debug = 1;
        public const int LogLevel_Info =  2;
        public const int LogLevel_Warn =  3;
        public const int LogLevel_Error = 4;
        public const int LogLevel_Fatal = 5;
          /// <summary>
         /// 输出日志到Log4Net
         /// </summary>
         /// <param name="t"></param>
         /// <param name="ex"></param>
         #region static void WriteLog(Type t, Exception ex)
 
         public static void WriteLog(Type t, Exception ex)
         {
             log4net.ILog log = log4net.LogManager.GetLogger(t);
             log.Error("Error", ex);
         }
 
         #endregion
         /// <summary>
         /// 输出日志到Log4Net
         /// </summary>
         /// <param name="t"></param>
         /// <param name="msg"></param>
         #region static void WriteLog(Type t, int Level,string msg)

         public static void WriteLog(Type t, int Level,string msg)
         {
             log4net.ILog log = log4net.LogManager.GetLogger(t);
             switch (Level)
             {
                 case 1: log.Debug(msg); break;
                 case 2: log.Info(msg); break;
                 case 3: log.Warn(msg); break;
                 case 4: log.Error(msg); break;
                 case 5: log.Fatal(msg); break;
             }
            
         }
 
         #endregion
 
    }
}
