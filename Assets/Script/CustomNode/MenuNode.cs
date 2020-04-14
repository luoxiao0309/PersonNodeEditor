using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuNode : DrawNode
{
    private GameObject controlledObject;
    public Texture2D winIcon;
    public Texture2D curveJoinR;
    public Texture2D curveJoinL;

    public override void DrawCurve(BaseNode b)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Window的回调函数,GUI绘制视图没效果,GUILayout绘制视图有效果.
    /// </summary>
    /// <param name="b"></param>
    public override void DrawWindow(BaseNode b)
    {
        //Debug.LogWarning("MenuNodeMenuNodeMenuNodeMenuNodeMenuNode");
        winIcon = EditorGUIUtility.Load("markPanel.png") as Texture2D;
        curveJoinR = EditorGUIUtility.Load("joinR.png") as Texture2D;
        curveJoinL = EditorGUIUtility.Load("joinL.png") as Texture2D;

        if (controlledObject == null)
            if (GUILayout.Button("Create GameObject"))
                controlledObject = new GameObject();

        controlledObject = (GameObject)EditorGUILayout.ObjectField(controlledObject, typeof(GameObject), true);

        //GUI.Box(b.WindowRect, "sddd");
        //var rect2 = b.WindowRect;
        //rect2.y += 10;
        //rect2.x += 10;
        //rect2.width -= 20;
        //rect2.height -= 23;
        //EditorGUI.TextField(rect2, "");
        //GUILayout.Box("sddda");
        //b.DrawConnectPoint();
    }
}
