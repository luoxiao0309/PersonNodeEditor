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
    StartNode,
    EndNode
}

/// <summary>
/// GUIStyle节点样式(无法序列化保存,只能后期赋值)
/// </summary>

[System.Serializable]
public abstract class BaseNode : ScriptableObject
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
    public GUIStyle Style = new GUIStyle();
    [NonSerialized]
    public GUIStyle nodeStyle;
    
    protected Action<ConnectionPoint> onRemoveConnectionPoint;
    
    public bool isDragged = false;

    public SerializedObject serializedObject;
    private List<string> excludes = new List<string>();

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
    
    public void InitData()
    {
        serializedObject = new SerializedObject(this);
        excludes.Add("m_Script");
        excludes.Add("id");
        excludes.Add("WindowRect");
        excludes.Add("windowTitle");
        excludes.Add("NodeType");
        excludes.Add("childNodes");
        excludes.Add("connections");
        excludes.Add("ParentNode");
        excludes.Add("HandleArea");
        excludes.Add("Resizable");
        excludes.Add("isDragged");

        SetStyle();
        SetData();
    }

    public virtual void SetStyle()
    {

    }

    public virtual void SetData()
    {

    }

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

        // Grab the latest data from the object
        //从对象抓取的最新数据
        serializedObject.Update();

        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        EditorGUIUtility.labelWidth = 84;

        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (excludes.Contains(iterator.name))
            {
                continue;
            }
            
            EditorGUILayout.PropertyField(iterator, true);
        }
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void DrawCurve()
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

    public bool ProcessEvents(Event e)
    {
        ProcessDefault(e);
        return false;
    }

    public virtual bool ProcessDefault(Event e)
    {
        //adds clickdrag
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (WindowRect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                    }
                }
                break;
            case EventType.MouseUp:
                isDragged = false;
                break;
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }
}
