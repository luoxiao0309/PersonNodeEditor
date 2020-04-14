using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxNode : DrawNode
{
    public override void DrawCurve(BaseNode b)
    {
        
    }

    public override void DrawWindow(BaseNode b)
    {
        GUI.Box(b.WindowRect, "", b.nodeStyle);
        var rect2 = b.WindowRect;
        rect2.y += 10;
        rect2.x += 10;
        rect2.width -= 20;
        rect2.height -= 23;
        //EditorGUI.TextField(rect2, "");
    }
}
