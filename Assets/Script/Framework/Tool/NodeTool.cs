using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeTool 
{
    public static void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);

        if (Handles.Button((start.center + end.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            Debug.LogWarning("删除连续");
        }
    }
}
