using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu]
[Serializable]
public class CustomGraph : ScriptableObject
{
    [SerializeField]
    public List<BaseNode> windows = new List<BaseNode>();
    public List<Connection> connections = new List<Connection>();

    public ConnectionPoint selectedInPoint;
    public ConnectionPoint selectedOutPoint;

    private int idCount = 1;
    /// <summary>
    /// 显示看的，作为调试使用.
    /// </summary>
    [SerializeField]
    public int IdCount = 1;
    [SerializeField]
    private string savePath = "";
    
    public void SetSavePath(string Path)
    {
        savePath = Path;
    }

    public string GetSavePath()
    {
        return savePath;
    }

    public T AddCustomwNode<T>(float width, float height, string title, Vector3 pos, NodeType nodeType) where T:BaseNode
    {
        T baseNode = AddCustomwNode<T>(width,height,title,pos);
        baseNode.NodeType = nodeType;

        if (string.IsNullOrEmpty(savePath) == false)
        {
            AssetDatabase.AddObjectToAsset(baseNode, this);
            //存在问题(罗霄)
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        return baseNode;
    }

    public T AddCustomwNode<T>(float width, float height, string title, Vector3 pos) where T : BaseNode
    {
        T baseNode = ScriptableObject.CreateInstance<T>();
        baseNode.customGraph = this;
        baseNode.name = baseNode.GetType().Name;
        baseNode.WindowRect.width = width;
        baseNode.WindowRect.height = height;
        baseNode.WindowTitle = title;
        baseNode.WindowRect.x = pos.x;
        baseNode.WindowRect.y = pos.y;
        baseNode.id = idCount;
        baseNode.InitData();
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
        baseNode.WindowTitle = title;
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
        baseNode.WindowTitle = title;
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
        baseNode.WindowTitle = title;
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
        baseNode.WindowTitle = title;
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
        baseNode.WindowTitle = title;
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

    #region 管理连接
    
    #endregion


    #region 连接点、连接线
    public void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;
        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }

        //使用当前事件,阻止其他事件响应.
        Event.current.Use();
    }

    /// <summary>
    /// 输出连接点
    /// </summary>
    /// <param name="outPoint"></param>
    public void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }
        Event.current.Use();
    }

    private void CreateConnection()
    {
        AddConnection(Connection.CreateConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        RemoveConnection(connection);
        Event.current.Use();
    }

    public void AddConnection(Connection conn)
    {
        connections.Add(conn);
    }

    public void RemoveConnection(Connection conn)
    {
        connections.Remove(conn);
    }

    public void DrawConnections()
    {
        if (connections.Count > 0)
        {
            for (int m = 0; m < connections.Count; m++)
            {
                var item = connections[m];
                item.Draw();
            }
        }
    }
    #endregion

}
