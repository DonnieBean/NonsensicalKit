using NonsensicalKit.Manager;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneNodes", menuName = "ScriptableObjects/SceneNodeConfigData")]
public class SceneNodeAsset : NonsensicalConfigDataBase
{
    public List<SceneNodeData> SceneNodes;

    public override void CopyForm<T>(T from)
    {
        SceneNodeAsset fromData = from as SceneNodeAsset;
        if (fromData != null)
        {
            SceneNodes = fromData.SceneNodes;
        }
    }
}