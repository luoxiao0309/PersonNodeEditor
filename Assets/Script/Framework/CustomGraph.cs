using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class CustomGraph : ScriptableObject
{
    [SerializeField]
    public List<BaseNode> windows = new List<BaseNode>();

    [SerializeField]
    private int idCount = 1;
    /// <summary>
    /// 显示看的，作为调试使用.
    /// </summary>
    [SerializeField]
    public int IdCount = 1;

    public BaseNode AddCustomwNode<T>(float width, float height, string title, Vector3 pos, NodeType nodeType) where T:BaseNode
    {
        T baseNode = ScriptableObject.CreateInstance<T>();
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
        return baseNode;
    }

    public BaseNode AddWindowNode(float width, float height, string title, Vector3 pos, NodeType nodeType=NodeType.Window)
    {
        WindowNode baseNode = ScriptableObject.CreateInstance<WindowNode>();
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
        return baseNode;
    }

    public BaseNode AddMenuWindowNode(float width, float height, string title, Vector3 pos, NodeType nodeType = NodeType.Window)
    {
        BaseNode baseNode = ScriptableObject.CreateInstance<MenuNode>();
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

    public BaseNode AddInputNode(float width, float height, string title, Vector3 pos, NodeType nodeType = NodeType.InputNode)
    {
        InputNode baseNode = ScriptableObject.CreateInstance<InputNode>();
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
        return baseNode;
    }

    public BaseNode AddCalcNodeNode(float width, float height, string title, Vector3 pos, NodeType nodeType = NodeType.CalcNode)
    {
        CalcNode baseNode = ScriptableObject.CreateInstance<CalcNode>();
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
        return baseNode;
    }

    public BaseNode AddBoxNode(float width, float height, string title, Vector3 pos,
                                   GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, NodeType nodeType = NodeType.Box)
    {
        BoxNode baseNode = ScriptableObject.CreateInstance<BoxNode>();
        baseNode.InitData(inPointStyle, outPointStyle,OnClickInPoint,OnClickOutPoint);
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
