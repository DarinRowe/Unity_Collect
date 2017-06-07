// **************************************************************************
// Title:SettingManager 设置数据保存管理（通过一个<string, string>的字典来保存数据，数据保存于文件中）
// UpDate: 16-9-13
// Copyright (c) RAYCOZ 
// **************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace RCGameFrame
{
    public static class SettingManager
    {
        public static string DevicePersistentPath = null;                                            // Device persistent path      设备路径
        static string SettingPath = "Settings";                                                      // Setting files path          设置文件的路径
        private static Dictionary<string, string> dicSettings = new Dictionary<string, string>();    // Save Setting  Dictionary    保存设置数据的字典

        #region Setting Load & Save 设置的加载和保存
        // 加载配置
        public static void Load()
        {
            #if UNITY_EDITOR
                DevicePersistentPath = Application.dataPath + "/../PersistentPath";
            #elif UNITY_STANDALONE_WIN
			    DevicePersistentPath = Application.dataPath + "/PersistentPath";
            #elif UNITY_STANDALONE_OSX
			    DevicePersistentPath = Application.dataPath + "/PersistentPath";
            #else
			    DevicePersistentPath = Application.persistentDataPath;
            #endif
            string settingPath = string.Format("{0}/{1}/{2}",
            DevicePersistentPath, SettingPath, "RCSetting.txt");
            if (!File.Exists(settingPath))
                Debugger.LogWarning("Can't find RCSetting.txt");
                return;
            // 解析配置
            // 配置格式：key=value
            string[] lines = File.ReadAllLines(settingPath);
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];
                string[] kv = line.Split('=');
                QLog.Assert(kv.Length == 2, "Error Setting Format : " + line);
                string k = kv[0].Trim();
                string v = kv[1].Trim();
                dicSettings[k] = v;
            }
        }

        // 保存配置
        public static void Save()
        {
            //保存文件地址            
            string settingPath = string.Format("{0}/{1}/{2}",
            DevicePersistentPath, SettingPath, "RCSetting.txt");
            //获取字典名
            string settingDir = Path.GetDirectoryName(settingPath);
            //如果文件中不存在这个字典，则创建新的字典
            if (!Directory.Exists(settingDir))
                Directory.CreateDirectory(settingDir);
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, string> kv in dicSettings)
                lines.Add(string.Format("{0}={1}", kv.Key, kv.Value));
            File.WriteAllLines(settingPath, lines.ToArray());
        }
        #endregion

        #region Get 获取值
        public static int GetInt(string key, int defaultVal = 0)
        {
            string val = null;
            if (dicSettings.TryGetValue(key, out val))
                return System.Convert.ToInt32(val);
            dicSettings[key] = defaultVal.ToString();
            return defaultVal;
        }

        public static float GetFloat(string key, float defaultVal = 0f)
        {
            string val = null;
            if (dicSettings.TryGetValue(key, out val))
                return System.Convert.ToSingle(val);
            dicSettings[key] = defaultVal.ToString();
            return defaultVal;
        }

        public static bool GetBool(string key, bool defaultVal = false)
        {
            string val = null;
            if (dicSettings.TryGetValue(key, out val))
                return System.Convert.ToBoolean(val);
            dicSettings[key] = defaultVal.ToString();
            return defaultVal;
        }

        public static string GetString(string key, string defaultVal = "")
        {
            string val = null;
            if (dicSettings.TryGetValue(key, out val))
                return val.ToString();
            dicSettings[key] = defaultVal.ToString();
            return defaultVal;
        }
        #endregion

        #region Set 添加值

        public static void SetInt(string key, int val)
        {
            dicSettings[key] = val.ToString();
        }

        public static void SetFloat(string key, float val)
        {
            dicSettings[key] = val.ToString();
        }

        public static void SetBool(string key, bool val)
        {
            dicSettings[key] = val.ToString();
        }

        public static void SetString(string key, string val)
        {
            dicSettings[key] = val;
        }
        #endregion
    }
}

