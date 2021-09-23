using System;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    [CreateAssetMenu(fileName = "NonsensicalConfigData", menuName = "ScriptableObjects/NonsensicalConfigData")]
    public class NonsensicalConfigDataTemplate : NonsensicalConfigDataBase
    {
        public string ServiceUri = "https://localhost:5001/";
        public string AssetBundlesPath = "AssetBundle";
    }

    public class NonsensicalConfigDataBase : ScriptableObject
    {
        public string ConfigID = "ID" + Guid.NewGuid().ToString().Substring(0, 4);
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/AAAAA",order =1)]
    public class Item : ScriptableObject
    {
        public string ServiceUri = "https://localhost:5001/";
        public Sprite sp;
        public string AssetBundlesPath = "AssetBundle";
        [TextArea]
        public string info = "AssetBundle";
        public bool e ;
    }
}

