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
    

    public BoxBaseNode() :base()
    {
        
    }

    public void InitData(GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint)
    {
        this.inPointStyle = inPointStyle;
        this.outPointStyle = outPointStyle;

        ConnectionPoint inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        ConnectionPoint outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        inPoints.Add(inPoint);
        outPoints.Add(outPoint);
    }

    public new void DrawWindow()
    {
        GUI.Box(WindowRect, "", nodeStyle);
        var rect2 = WindowRect;
        rect2.y += 10;
        rect2.x += 10;
        rect2.width -= 20;
        rect2.height -= 23;
        
        //DrawConnectPoint();
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
