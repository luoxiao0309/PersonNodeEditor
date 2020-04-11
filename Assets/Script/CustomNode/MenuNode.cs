using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuNode : DrawNode
{
    public Texture2D winIcon;
    public Texture2D curveJoinR;
    public Texture2D curveJoinL;

    public override void DrawCurve(BaseNode b)
    {
        throw new System.NotImplementedException();
    }

    public override void DrawWindow(BaseNode b)
    {
        Debug.LogWarning("MenuNodeMenuNodeMenuNodeMenuNodeMenuNode");
        winIcon = EditorGUIUtility.Load("markPanel.png") as Texture2D;
        curveJoinR = EditorGUIUtility.Load("joinR.png") as Texture2D;
        curveJoinL = EditorGUIUtility.Load("joinL.png") as Texture2D;
    }
}
