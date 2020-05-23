using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StartNode : BaseNode
{
    public ConnectionPoint startPoint;
    public override void DrawWindow()
    {
        //GUI.Box(WindowRect,"", Style);
        //GUI.Label(WindowRect, "Start", Style);
        //GUILayout.BeginArea(WindowRect);
        //GUI.BeginArea(WindowRect,Style);
        GUILayout.Label("Start");
        //GUI.EndArea();
    }

    public override void DrawConnectionPoint()
    {
        startPoint.Draw();
    }

    public override void SetStyle()
    {
        Style.normal.background = EditorGUIUtility.Load("Textures/redTex.png") as Texture2D;
        
        Style.normal.textColor = Color.white;
        Style.fontSize = 32;
        Style.alignment = TextAnchor.MiddleCenter;
    }

    public void SetData()
    {
        this.NodeType = NodeType.Window;
        startPoint = ConnectionPoint.CreateConnectionPoint(this, ConnectionPointType.Out, customGraph.OnClickOutPoint);
    }
}
