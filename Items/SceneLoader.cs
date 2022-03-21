using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NonsensicalMono
{

    List<string> loadedScene = new List<string>();
    protected override void Awake()
    {
        base.Awake();
        Subscribe((int)NonsensicalKit.Manager.NonsensicalManagerEnum.AllInitComplete, OnLoadComplete);

        Subscribe<string, bool>("loadScene", OnLoadScene);
        Subscribe("returnMainMenu", OnReturnMainMenu);
    }

    bool showEscapePanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showEscapePanel = true;
        }
    }
    private void OnGUI()
    {
        if (showEscapePanel)
        {
            GUIStyle guiStyle = GUIStyle.none;
            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            guiStyle.alignment = TextAnchor.MiddleCenter;

            int width = Screen.width;
            int height = Screen.height;
            GUI.Box(new Rect(width * 0.5f - 175, height * 0.5f - 112.5f, 350, 225), "");

            GUI.Label(new Rect(width * 0.5f - 50, height * 0.5f - 100, 100, 50), "是否退出程序", guiStyle);
            if (GUI.Button(new Rect(width * 0.5f - 130, height * 0.5f + 50, 60, 30), "确定"))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            if (GUI.Button(new Rect(width * 0.5f + 70, height * 0.5f + 50, 60, 30), "取消"))
            {
                showEscapePanel = false;
            }
        }
    }

    private void OnReturnMainMenu()
    {
        Publish("showMainMenu", true);
        foreach (var item in loadedScene)
        {
            SceneManager.UnloadSceneAsync(item);
        }
        loadedScene.Clear();
    }

    private void OnLoadComplete()
    {
        SceneManager.LoadSceneAsync("Main Scene", LoadSceneMode.Additive);
    }

    private void OnLoadScene(string sceneName, bool mainScene = true)
    {
        if (mainScene)
        {
            foreach (var item in loadedScene)
            {
                SceneManager.UnloadSceneAsync(item);
            }
            loadedScene.Clear();
        }
        loadedScene.Add(sceneName);
        var v = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (mainScene)
        {
            v.completed += (v) => { SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); };
        }
    }
}