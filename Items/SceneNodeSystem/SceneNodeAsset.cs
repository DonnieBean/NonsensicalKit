using NonsensicalKit.Manager;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneNodes", menuName = "ScriptableObjects/SceneNodeConfigData")]
public class SceneNodeAsset : NonsensicalConfigDataBase
{
    public List<SceneNodeData> SceneNodes;
}