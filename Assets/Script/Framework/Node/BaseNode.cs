using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public enum NodeType
{
    Window,
    InputNode,
    CalcNode,
    Box,
}

/// <summary>
/// GUIStyle节点样式(无法序列化保存,只能后期赋值)
/// </summary>

[System.Serializable]
[CreateAssetMenu]
public abstract class BaseNode:ScriptableObject
{
    #region 属性
    public int id;
    [SerializeField]
    public Rect WindowRect;
    public string windowTitle;
    /// <summary>
    /// 节点类型
    /// </summary>
    [SerializeField]
    public NodeType NodeType = NodeType.Window;
    [SerializeField]
    public List<int> childNodes = new List<int>();
    [SerializeField]
    public List<Connection> connections = new List<Connection>();
    //父节点.
    public int ParentNode = 0;
    /// <summary>
    /// 重设大小.
    /// </summary>
    [SerializeField]
    public Rect HandleArea;
    public bool Resizable = false;

    [NonSerialized]
    public GUIStyle nodeStyle;
    
    protected Action<ConnectionPoint> onRemoveConnectionPoint;
    
    public bool isDragged = false;
    
    /// <summary>
    /// 是否激活.
    /// </summary>
    private bool active = false;
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            if (value)
            {
                GUI.BringWindowToFront(1);
            }
            else
            {
                //GUI.BringWindowToBack(handle);
            }
            active = value;
        }
    }
    #endregion
    
    public virtual void DrawWindow()
    {
        this.name = this.GetType().ToString();
        Color temp = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;

        if (GUI.Button(new Rect(WindowRect.width - 18, -1, 18, 18), "X"))
        {
            Debug.LogWarning("关闭...");
        }
        GUI.backgroundColor = temp;
    }

    public virtual void DrawCurve()
    {

    }
    

    public void WindowMenu(Event e,Action<Event> SelectEvent, Action<Event> UnSelect)
    {

    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove Node"), false, null);
        genericMenu.ShowAsContext();
    }

    public void Drag(Vector2 delta)
    {
        WindowRect.position += delta;
    }

    public void AddConnection(Connection connection)
    {
        if (connections.Count==0)
        {
            connections.Add(connection);
        }
        else
        {
            foreach (var item in connections)
            {
                if ((item.inPoint.Equals(connection.inPoint)==false)||(item.outPoint.Equals(connection.outPoint)==false))
                {
                    connections.Add(connection);
                }
            }
        }
    }

    public void RemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }
}
