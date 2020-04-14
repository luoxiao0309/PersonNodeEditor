using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class CustomGraph : ScriptableObject
{
    [SerializeField]
    public List<BaseNode> windows = new List<BaseNode>();
    //[SerializeField]
    private int idCount = 1;
    /// <summary>
    /// 显示看的，作为调试使用.
    /// </summary>
    [SerializeField]
    public int IdCount = 1;
    
    public BaseNode AddWindowNode(DrawNode type, float width, float height, string title, Vector3 pos, NodeType nodeType=NodeType.Window)
    {
        BaseNode baseNode = new BaseNode();
        baseNode.drawNode = type;
        baseNode.WindowRect.width = width;
        baseNode.WindowRect.height = height;
        baseNode.windowTitle = title;
        baseNode.WindowRect.x = pos.x;
        baseNode.WindowRect.y = pos.y;
        baseNode.id = idCount;
        baseNode.NodeType = nodeType;
        //baseNode.InitInOut();
        windows.Add(baseNode);
        IdCount = idCount;
        idCount++;
        return baseNode;
    }

    public BaseNode AddBoxNode(DrawNode type, float width, float height, string title, Vector3 pos,
                                   GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, NodeType nodeType = NodeType.Box)
    {
        BoxBaseNode baseNode = new BoxBaseNode(inPointStyle, outPointStyle,OnClickInPoint,OnClickOutPoint);
        baseNode.drawNode = type;
        baseNode.WindowRect.width = width;
        baseNode.WindowRect.height = height;
        baseNode.windowTitle = title;
        baseNode.WindowRect.x = pos.x;
        baseNode.WindowRect.y = pos.y;
        baseNode.id = idCount;
        baseNode.NodeType = nodeType;
        windows.Add(baseNode);
        IdCount = idCount;
        idCount++;
        baseNode.inPointStyle = inPointStyle;
        baseNode.outPointStyle = outPointStyle;
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
