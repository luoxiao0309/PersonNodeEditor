﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType { In, Out }

/// <summary>
/// 连接点.
/// </summary>
[Serializable]
public class ConnectionPoint
{
    public Rect rect;
    public Vector2 offset;
    public ConnectionPointType type;
    [SerializeField]
    public BaseNode node;
    public GUIStyle style;

    /// <summary>
    /// 连接点点击事件.
    /// </summary>
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint()
    {

    }

    public ConnectionPoint(BaseNode node, ConnectionPointType type, GUIStyle style)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public ConnectionPoint(BaseNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> onClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        rect = new Rect(0, 0, 10f, 20f);
        this.OnClickConnectionPoint = onClickConnectionPoint;
    }

    public ConnectionPoint(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> onClickConnectionPoint)
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
        
        if (GUI.Button(rect, ">", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
