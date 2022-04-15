using NonsensicalKit.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NonsensicalKit.Editor
{
    public class NonsensicalConfigurator : EditorWindow
    {

        [MenuItem("Tools/NonsensicalKit/配置管理")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(NonsensicalConfigurator));
        }

        private static class Data
        {
            public static bool jumpFirstOnPlay;

            //增加新全局宏时仅需修改下方四个数组


            public static bool[] useStates = new bool[3];


            public static readonly string[] Labels = new string[]
            {
                "使用HighlightingSystem组件",
                "使用FinalIK组件",
                "使用NewtonsoftJson组件",
            };

            public static readonly string[] PlayerPrefsStr = new string[]
            {
                "nk_nonsensicalConfigurator_useHigllightingSystem",
                "nk_nonsensicalConfigurator_useFinalIK",
                "nk_nonsensicalConfigurator_useNewtonsoftJson",
            };
            public static readonly string[] Lines = new string[]
            {
                "-define:USE_HIGHLIGHTINGSYSTEM,",
                "-define:USE_FINALIK,",
                "-define:USE_NEWTONSOFTJSON,",
            };
        }

        private void OnGUI()
        {
            Data.jumpFirstOnPlay = PlayerPrefs.GetInt("nk_nonsensicalConfigurator_jumpFirstOnPlay", 0) == 0 ? false : true;
            Data.jumpFirstOnPlay = EditorGUILayout.Toggle("运行时跳转至首个场景", Data.jumpFirstOnPlay);
            PlayerPrefs.SetInt("nk_nonsensicalConfigurator_jumpFirstOnPlay", Data.jumpFirstOnPlay ? 1 : 0);

            for (int i = 0; i < Data.Lines.Length; i++)
            {
                Data.useStates[i] = PlayerPrefs.GetInt(Data.PlayerPrefsStr[i], 0) == 0 ? false : true;
                Data.useStates[i] = EditorGUILayout.Toggle(Data.Labels[i], Data.useStates[i]);
                PlayerPrefs.SetInt(Data.PlayerPrefsStr[i], Data.useStates[i] ? 1 : 0);
            }
            if (GUILayout.Button("修改配置", GUILayout.Height(30f)))
            {

            }

            if (GUILayout.Button("修改csc.rsp文件", GUILayout.Height(30f)))
            {
                string cscFilePath = Path.Combine(Application.dataPath, "csc.rsp");
                if (File.Exists(cscFilePath) == false)
                {
                    FileHelper.Create(cscFilePath);
                }
                var s = File.ReadAllLines(cscFilePath);
                List<string> temp = new List<string>(s);
                if (temp.Count > 0)
                {
                    var last = temp[temp.Count - 1];
                    if (last[last.Length - 1] != ',')
                    {
                        temp[temp.Count - 1] += ',';
                    }
                }

                for (int i = 0; i < Data.Lines.Length; i++)
                {
                    if (Data.useStates[i] && temp.Contains(Data.Lines[i]) == false)
                    {
                        temp.Add(Data.Lines[i]);
                    }
                    else if (Data.useStates[i] == false && temp.Contains(Data.Lines[i]))
                    {
                        temp.Remove(Data.Lines[i]);
                    }
                }

                File.WriteAllLines(cscFilePath, temp.ToArray());
                Debug.Log("修改文件完成，建议重启项目");
            }
        }
    }
}
