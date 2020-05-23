using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class EndNode : BaseNode
{
    public ConnectionPoint endPoint;

    public override void DrawWindow()
    {
        GUILayout.Label("");
    }

    public override void DrawConnectionPoint()
    {
        endPoint.Draw();
    }

    public override void SetStyle()
    {
        Style.normal.background = EditorGUIUtility.Load("Textures/blueTex.png") as Texture2D;
        
        Style.normal.textColor = Color.white;
        Style.fontSize = 32;
        Style.alignment = TextAnchor.MiddleCenter;
    }

    public void SetData()
    {
        endPoint = ConnectionPoint.CreateConnectionPoint(this, ConnectionPointType.In, customGraph.OnClickInPoint);
    }
}
