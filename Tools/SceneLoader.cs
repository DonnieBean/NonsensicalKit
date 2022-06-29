using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载管理类
/// </summary>
public class SceneLoader : NonsensicalMono
{
    [SerializeField] private string mainSceneName = "MainScene";

    private List<string> loadedScene = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        Subscribe((int)NonsensicalKit.Manager.NonsensicalManagerEnum.AllInitComplete, OnLoadComplete);

        Subscribe<string, bool>("loadScene", OnLoadScene);
        Subscribe("returnMainMenu", OnReturnMainMenu);
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
        SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Additive);
    }

    private void OnLoadScene(string sceneName, bool mainScene = true)
    {
        //Debug.Log(sceneName+":"+mainScene);
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
