using NonsensicalKit.Manager;
using System.Collections.Generic;

public class SceneNodeManager : NonsensicalManagerBase<SceneNodeManager>
{
    public SceneNode crtSceneNode { get; private set; }

    private List<SceneNode> sceneNodes;
    private Dictionary<string, SceneNode> dic;

    protected override void InitStart()
    {
        InitComplete();
    }

    protected override void LateInitStart()
    {
        if (AppConfigManager.Instance.TryGetConfig<SceneNodeAsset>(out var v))  
        {
            BuildSceneNodes(v.SceneNodes);
            BuildDic();
        }
        else
        {
            LogManager.Instance.LogError("未获取到场景节点数据");
        }
        LateInitComplete();
    }

    public void SwitchNode(string nodeName)
    {
        if (dic.ContainsKey(nodeName))
        {
            crtSceneNode = dic[nodeName];

            Publish<SceneNode>((uint) SceneNodeEnum.SwitchNode, crtSceneNode);
        }
        else
        {
            LogManager.Instance.LogWarning("错误的节点名称");
        }
    }

    private void BuildSceneNodes(List<SceneNodeData> datas)
    {
        sceneNodes = new List<SceneNode>();
        Queue<SceneNode> sns = new Queue<SceneNode>();
        Queue<SceneNodeData> snds = new Queue<SceneNodeData>();

        foreach (var item in datas)
        {
            var v = new SceneNode(item.NodeName, null, new SceneNode[item.ChildNode.Length]);
            sceneNodes.Add(v); 
            sns.Enqueue(v);
            snds.Enqueue(item);
        }

        while (sns.Count>0)
        {
            SceneNode crtSN = sns.Dequeue();
            SceneNodeData crtSND = snds.Dequeue();

            for (int i = 0; i < crtSND.ChildNode.Length; i++)
            {
                int l = crtSND.ChildNode[i].ChildNode.Length;
                var v = new SceneNode(crtSND.ChildNode[i].NodeName, crtSN, new SceneNode[l]);
                crtSN.ChildNode[i] = v;
                if (l>0)
                {
                    sns.Enqueue(v);
                    snds.Enqueue(crtSND.ChildNode[i]);
                }
            }
        }
    }

    private void BuildDic()
    {
        dic = new Dictionary<string, SceneNode>();

        Queue<SceneNode> nodes = new Queue<SceneNode>();
        foreach (var item in sceneNodes)
        {
            nodes.Enqueue(item);
        }

        while (nodes.Count > 0)
        {
            SceneNode crtSN = nodes.Dequeue();
            if (dic.ContainsKey(crtSN.NodeName))
            {
                LogManager.Instance.LogWarning("节点名称重复");
            }
            else
            {
                dic.Add(crtSN.NodeName, crtSN);
            }
            foreach (var item in crtSN.ChildNode)
            {
                nodes.Enqueue(item);
            }
        }
    }

    public bool CheckState(string nodeName)
    {
        if (dic.ContainsKey(nodeName))
        {
            SceneNode crt = dic[nodeName];

            while (true)
            {
                if (crt==crtSceneNode)
                {
                    return true;
                }
                else if (crt.ParentNode != null)
                {
                    crt = crt.ParentNode;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }
}

public class SceneNode
{
    public string NodeName;
    public SceneNode ParentNode;
    public SceneNode[] ChildNode;

    public SceneNode(string nodeName, SceneNode parentNode, SceneNode[] childNode)
    {
        NodeName = nodeName;
        ParentNode = parentNode;
        ChildNode = childNode;
    }
}

[System.Serializable]
public class SceneNodeData
{
    public string NodeName;
    public SceneNodeData[] ChildNode;
}
