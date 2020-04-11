using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class CustomGraph : ScriptableObject
{
    [SerializeField]
    public List<BaseNode> windows = new List<BaseNode>();
    [SerializeField]
    public int idCount
    {
        get
        {
            return windows.Count+1;
        }
    }

    public BaseNode AddNodeOnGraph(DrawNode type, float width, float height, string title, Vector3 pos)
    {
        BaseNode baseNode = new BaseNode();
        baseNode.drawNode = type;
        baseNode.windowRect.width = width;
        baseNode.windowRect.height = height;
        baseNode.windowTitle = title;
        baseNode.windowRect.x = pos.x;
        baseNode.windowRect.y = pos.y;
        windows.Add(baseNode);
        baseNode.id = idCount;
        return baseNode;
    }

    public BaseNode GetBaseNodeById(int nodeId)
    {
        BaseNode baseNode = windows.Find((node)=> { return (node.id == nodeId); });
        if (baseNode==null)
        {
            Debug.LogWarning("当前空节点为: "+nodeId);
        }
        return baseNode;
    }

    /// <summary>
    /// 清除窗口
    /// </summary>
    /// <param name="Nodes"></param>
    public void ClearWindows(List<int> Nodes)
    {
        List<BaseNode> list = new List<BaseNode>();
        foreach (var item in Nodes)
        {
            var node = GetBaseNodeById(item);
            list.Add(node);
        }

        foreach (var item in list)
        {
            windows.Remove(item);
        }
    }
}
