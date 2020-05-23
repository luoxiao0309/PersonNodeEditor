using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType { In, Out }

/// <summary>
/// 连接点.
/// </summary>
[Serializable]
public class ConnectionPoint:ScriptableObject
{
    public Rect pointRect
    {
        set
        {
            rect = value;
        }
        get
        {
            return rect;
        }
    }
    private Rect rect;
    public Vector2 offset;
    public ConnectionPointType type;
    [SerializeField]
    public BaseNode node;
    public GUIStyle style;
    
    /// <summary>
    /// 连接点点击事件.
    /// </summary>
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public static ConnectionPoint CreateConnectionPoint(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> onClickConnectionPoint)
    {
        ConnectionPoint connPoint = ScriptableObject.CreateInstance<ConnectionPoint>();
        connPoint.InitData(node, type, onClickConnectionPoint);
        connPoint.name = node.GetType().ToString() + "_" + type.ToString();
        return connPoint;
    }

    public ConnectionPoint(BaseNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> onClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        rect = new Rect(0, 0, 10f, 20f);
        this.OnClickConnectionPoint = onClickConnectionPoint;
    }

    public void InitData(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> onClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        rect = new Rect(0, 0, 20f, 20f);
        this.OnClickConnectionPoint = onClickConnectionPoint;
    }

    public void Draw()
    {
        rect.y = node.WindowRect.y + (node.WindowRect.height * 0.5f) - rect.height * 0.5f;
        switch (type)
        {
            case ConnectionPointType.In:
                if (style == null)
                {
                    style = EditorStyles.miniButtonLeft;
                }
                rect.x = node.WindowRect.x - rect.width;
                break;
            case ConnectionPointType.Out:
                if (style == null)
                {
                    style = EditorStyles.miniButtonRight;
                }
                rect.x = node.WindowRect.x + node.WindowRect.width;
                break;
        }

        //GUI.Button(new Rect(200, 50, 30, 30), ">", EditorStyles.miniButtonLeft);
        if (GUI.Button(rect, ">", EditorStyles.miniButtonRight))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }

    public void Draw(Rect customRect)
    {
        rect.x = customRect.x;
        rect.y = customRect.y;
        rect.height = 20;
        rect.width = 20;
        
        //GUI.Button(new Rect(200, 50, 30, 30), ">", EditorStyles.miniButtonLeft);
        if (GUI.Button(rect, ">", EditorStyles.miniButtonRight))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }

    public void SetRect(Rect customRect)
    {
        rect = customRect;
    }

    public void DrawLayout(float height=20)
    {
        if (GUILayout.Button( ">", EditorStyles.miniButtonRight,GUILayout.Height(height)))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
