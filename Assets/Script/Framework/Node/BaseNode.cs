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
    public string WindowTitle;
    /// <summary>
    /// 重设大小Icon
    /// </summary>
    public GUIContent _resizeIcon;
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
    public CustomGraph customGraph;

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

        Texture2D resizeHandle = EditorGUIUtility.Load("ResizeHandle.png") as Texture2D;
        //Texture2D resizeHandle = AssetDatabase.LoadAssetAtPath("Assets/Node/Textures/PNG/ResizeHandle.png", typeof(Texture2D)) as Texture2D;
        _resizeIcon = new GUIContent(resizeHandle);

        SetStyle();
    }

    public virtual void SetStyle()
    {

    }

    public void DrawWindowNode(int i)
    {
        this.WindowRect = GUI.Window(i, this.WindowRect, DrawNodeWindow, this.WindowTitle);
        //this.WindowRect = GUILayout.Window(i, this.WindowRect, DrawNodeWindow, this.windowTitle, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        this.DrawConnectionPoint();
    }

    public virtual void DrawConnectionPoint()
    {

    }

    public virtual void DrawWindow()
    {
        //this.name = this.GetType().ToString();
        //Color temp = GUI.backgroundColor;
        //GUI.backgroundColor = Color.red;

        //if (GUI.Button(new Rect(WindowRect.width - 18, -1, 18, 18), "X"))
        //{
        //    Debug.LogWarning("关闭...");
        //}
        //GUI.backgroundColor = temp;

        //// Grab the latest data from the object
        ////从对象抓取的最新数据
        //serializedObject.Update();

        //SerializedProperty iterator = serializedObject.GetIterator();
        //bool enterChildren = true;
        //EditorGUIUtility.labelWidth = 84;

        //while (iterator.NextVisible(enterChildren))
        //{
        //    enterChildren = false;
        //    if (excludes.Contains(iterator.name))
        //    {
        //        continue;
        //    }
            
        //    EditorGUILayout.PropertyField(iterator, true);
        //}
        //serializedObject.ApplyModifiedProperties();
    }

    //绘画窗口函数
    void DrawNodeWindow(int id)
    {
        // 标题栏
        const int titleHeight = 18;
        Rect title = WindowRect;
        title.height = titleHeight;
        title.position = new Vector2(-1, 0);
        GUIStyle style = new GUIStyle("dropDownButton");
        style.fontSize = (int)((titleHeight - 3));
        style.fixedHeight = titleHeight; // バー高さ
        var temp = GUI.backgroundColor;
        if (active)
        {
            GUI.color = new Color(0.6f, 0.6f, 1.0f, 1f);
        }
        else
        {
            GUI.color = new Color(0.4f, 0.6f, 1.0f, 1.0f);
        }
        GUI.Label(title, WindowTitle, style);
        GUI.color = temp;

        DrawWindow();

        #region 重设大小
        //ResizeWindow(id, this);
        #endregion

        //设置改窗口可以拖动
        GUI.DragWindow();
    }

    #region 重置大小
    /// <summary>
    /// 重置窗口大小.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="window"></param>
    void ResizeWindow(int id, BaseNode window)
    {
        //if (GUIUtility.hotControl == 0) { window.Resizable = false; }
        float _cornerX = window.WindowRect.width;
        float _cornerY = window.WindowRect.height;

        GUILayout.BeginArea(new Rect(1, _cornerY - 16, _cornerX - 3, 14));
        GUILayout.BeginHorizontal(EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();

        window.HandleArea = GUILayoutUtility.GetRect(_resizeIcon, GUIStyle.none);
        GUI.DrawTexture(new Rect(window.HandleArea.xMin + 3, window.HandleArea.yMin - 3, 20, 20), _resizeIcon.image);

        if (!window.Resizable && ((Event.current.type == EventType.MouseDown) || (Event.current.type == EventType.MouseDrag)))
        {
            if (window.HandleArea.Contains(Event.current.mousePosition, true))
            {
                window.Resizable = true;
                //"FocusType.Passive" 此控件无法接收键盘焦点。
                //GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
            }
        }

        EditorGUIUtility.AddCursorRect(window.HandleArea, MouseCursor.ResizeUpLeft);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        if (window.Resizable && (Event.current.type == EventType.MouseDrag))
        {
            ResizeNode(id, Event.current.delta.x, Event.current.delta.y);
            //Repaint();
            //当你已经使用一个事件时调用这个方法。事件的类型将设置为 EventType.Used，使其他GUI元素忽略它。
            Event.current.Use();
        }

        MouseClickEvent.MouseUpEvent(Event.current, (e1) =>
        {
            window.Resizable = false;
        });
    }
    
    public void ResizeNode(int id, float deltaX, float deltaY)
    {
        float targetWidth = this.WindowRect.width;
        float targetHeight = this.WindowRect.height;

        if ((this.WindowRect.width + deltaX) > 50)
            targetWidth = this.WindowRect.width + deltaX;

        if ((this.WindowRect.height + deltaY) > 50)
            targetHeight = this.WindowRect.height + deltaY;

        this.WindowRect = new Rect(this.WindowRect.position.x, this.WindowRect.position.y,targetWidth, targetHeight);
    }
    #endregion

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
        if (connections.Count == 0)
        {
            connections.Add(connection);
        }
        else
        {
            foreach (var item in connections)
            {
                if ((item.inPoint.Equals(connection.inPoint) == false) || (item.outPoint.Equals(connection.outPoint) == false))
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
