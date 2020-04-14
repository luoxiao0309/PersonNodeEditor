using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Box节点.
/// </summary>
[Serializable]
public class BoxBaseNode : BaseNode
{
    [SerializeField]
    public List<ConnectionPoint> inPoints = new List<ConnectionPoint>();
    public GUIStyle inPointStyle;

    [SerializeField]
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();
    public GUIStyle outPointStyle;
    

    public BoxBaseNode(GUIStyle inPointStyle, GUIStyle outPointStyle,Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) :base()
    {
        this.inPointStyle = inPointStyle;
        this.outPointStyle = outPointStyle;
        
        ConnectionPoint inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        ConnectionPoint outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        inPoints.Add(inPoint);
        outPoints.Add(outPoint);
    }

    /// <summary>
    /// 拖拽.
    /// </summary>
    /// <param name="delta"></param>
    public new void Drag(Vector2 delta)
    {
        WindowRect.position += delta;
    }

    public new void DrawWindow()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }

        DrawConnectPoint();
    }

    /// <summary>
    /// 绘制连接点.
    /// </summary>
    public void DrawConnectPoint()
    {
        for (int i = 0; i < inPoints.Count; i++)
        {
            inPoints[i].Draw();
        }
        for (int i = 0; i < outPoints.Count; i++)
        {
            outPoints[i].Draw();
        }
    }
}
