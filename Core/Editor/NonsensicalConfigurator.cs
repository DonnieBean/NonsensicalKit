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

        [MenuItem("Tools/NonsensicalKit/Test")]
        private static void Test()
        {
            Debug.Log(EditorHelper.CheckScriptingDefine("Test"));
            EditorHelper.SetScriptingDefine("Test");
            Debug.Log(EditorHelper.CheckScriptingDefine("Test"));
            Debug.Log("测试");
        }
        [MenuItem("Tools/NonsensicalKit/配置管理")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(NonsensicalConfigurator));
        }

        private static class Data
        {
            public static bool jumpFirstOnPlay;

            //增加新全局宏时仅需修改下方三个数组


            public static bool[] useStates = new bool[2];


            public static readonly string[] Labels = new string[]
            {
                "使用HighlightingSystem组件",
                "使用NewtonsoftJson组件",
            };

            public static readonly string[] Lines = new string[]
            {
                "USE_HIGHLIGHTINGSYSTEM",
                "USE_NEWTONSOFTJSON",
            };
        }
        private void OnGUI()
        {
            Data.jumpFirstOnPlay = PlayerPrefs.GetInt("nk_nonsensicalConfigurator_jumpFirstOnPlay", 0) == 0 ? false : true;
            Data.jumpFirstOnPlay = EditorGUILayout.Toggle("运行时跳转至首个场景", Data.jumpFirstOnPlay);
            PlayerPrefs.SetInt("nk_nonsensicalConfigurator_jumpFirstOnPlay", Data.jumpFirstOnPlay ? 1 : 0);

            bool flag = false;
            for (int i = 0; i < Data.Lines.Length; i++)
            {
                Data.useStates[i] = EditorHelper.CheckScriptingDefine(Data.Lines[i]);
                bool temp = Data.useStates[i];
                Data.useStates[i] = EditorGUILayout.Toggle(Data.Labels[i], Data.useStates[i]);

                if (temp != Data.useStates[i])
                {
                    flag = true;
                }
            }
            if (flag)
            {
                for (int i = 0; i < Data.Lines.Length; i++)
                {
                    if (Data.useStates[i])
                    {
                        EditorHelper.SetScriptingDefine(Data.Lines[i]);
                    }
                    else
                    {
                        EditorHelper.RemoveScriptingDefine(Data.Lines[i]);
                    }
                }
            }
        }
    }
}