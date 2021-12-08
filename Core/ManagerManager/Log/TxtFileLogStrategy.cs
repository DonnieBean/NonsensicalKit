﻿using NonsensicalKit.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 项目内路径的txt文件输出策略
    /// </summary>
    public class TxtFileLogStrategy:LogStrategy
    {
        string fullLogFilePath;
        string path; 
        string name;
        FileStream fs;
        StreamWriter sw;
        public void Init()
        {
            if (AppConfigManager.Instance.TryGetConfig<NonsensicalManagerConfigData>(out var v))
            {
                fullLogFilePath = Path.Combine(Application.dataPath , v.LogFilePath,$"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            else
            {
                fullLogFilePath = Path.Combine(Application.dataPath, "NonsensicalLog", $"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            //Debug.Log("日志文件路径："+fullLogFilePath);
            path = Path.GetDirectoryName(fullLogFilePath);
            name = Path.GetFileName(fullLogFilePath);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                fs = new FileStream(fullLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Write);
                sw = new StreamWriter(fs, Encoding.UTF8);
            }
            catch (Exception)
            {
               Debug.Log("流创建失败");
            }
        }

        public void Log(LogContext info)
        {
            try
            {
                sw.Write(info.message);
                sw.Flush();
            }
            catch (Exception)
            {
                Manager.LogManager.Instance.Log("文件写入错误");
            }
        }

        public void Recycle()
        {
            sw?.Close();
            fs?.Close();
        }
    }
}
