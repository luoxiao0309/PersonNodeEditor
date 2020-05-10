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
        startPoint.Draw();
        GUI.Box(WindowRect,"", Style);
        GUI.Label(WindowRect, "Start", Style);
    }

    public override void SetStyle()
    {
        Style.normal.background = EditorGUIUtility.Load("Textures/redTex.png") as Texture2D;
        
        Style.normal.textColor = Color.white;
        Style.fontSize = 32;
        Style.alignment = TextAnchor.MiddleCenter;
    }

    public override void SetData()
    {
        startPoint = new ConnectionPoint(this, ConnectionPointType.Out, DialogNodeEditor.Instance.OnClickOutPoint);
    }
}
