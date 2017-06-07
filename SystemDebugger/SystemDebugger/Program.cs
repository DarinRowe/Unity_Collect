using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RCGameFrame
{
    public class Debugger 
    {

        // Debugger默认为关闭，调试模式时候开启
        public static bool DebuggerEnable = false;
        //Log
        //重载
        public static void Log(object message)
        {
            Log(message, null);
        }
        public static void Log(object message, UnityEngine.Object context)
        {
            if (DebuggerEnable)
            {
                Debug.Log(message, context);
            }
        }
        //LogError
        public static void LogError(object message)
        {
            LogError(message, null);
        }
        public static void LogError(object message, UnityEngine.Object context)
        {
            if (DebuggerEnable)
            {
                Debug.LogError(message, context);
            }
        }
        //LogWarning
        public static void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        public static void LogWarning(object message, UnityEngine.Object context)
        {

            if (DebuggerEnable)
            {
                Debug.LogWarning(message, context);
            }
        }
    }
}
