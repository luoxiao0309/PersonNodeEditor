using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public GUIStyle nodeStyle { set; get; }
    
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
                Debug.LogWarning("选中......");
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

    public BaseNode()
    {

    }

    public virtual void DrawWindow()
    {
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

    /// <summary>
    /// Window窗口不响应MouseDown事件.
    /// </summary>
    /// <param name="e"></param>
    public void MouseEvent(Event e)
    {
        if (e.IsMouseDownClick())
        {
            if (e.IsMouseLeftClick())
            {
                if (WindowRect.Contains(e.mousePosition))
                {
                    Debug.LogWarning("可以拖拽事件");
                    isDragged = true;
                    //GUI.changed = true;
                    //style = selectedNodeStyle;
                }
                else
                {
                    //GUI.changed = true;
                    //style = defaultNodeStyle;
                }
            }
        }
        else if (e.IsMouseUpClick())
        {
            isDragged = false;
        }
        else if (e.IsMouseDragClick())
        {
            if (e.IsMouseLeftClick()&& isDragged)
            {
                Drag(e.delta);
                //位置变更,必须加.
                e.Use();
            }
        }
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
