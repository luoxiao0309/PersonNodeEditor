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
        endPoint.Draw();
        GUI.Box(WindowRect,"", Style);
        GUI.Label(WindowRect, "END", Style);
    }

    public override void SetStyle()
    {
        Style.normal.background = EditorGUIUtility.Load("Textures/blueTex.png") as Texture2D;
        
        Style.normal.textColor = Color.white;
        Style.fontSize = 32;
        Style.alignment = TextAnchor.MiddleCenter;
    }

    public override void SetData()
    {
        endPoint = new ConnectionPoint(this, ConnectionPointType.In, DialogNodeEditor.Instance.OnClickInPoint);
    }
}
