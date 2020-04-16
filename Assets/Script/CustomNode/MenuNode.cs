using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuNode : BaseNode
{
    private GameObject controlledObject;
    public Texture2D winIcon;
    public Texture2D curveJoinR;
    public Texture2D curveJoinL;
    
    public override void DrawWindow()
    {
        ////Debug.LogWarning("MenuNodeMenuNodeMenuNodeMenuNodeMenuNode");
        //winIcon = EditorGUIUtility.Load("markPanel.png") as Texture2D;
        //curveJoinR = EditorGUIUtility.Load("joinR.png") as Texture2D;
        //curveJoinL = EditorGUIUtility.Load("joinL.png") as Texture2D;

        //if (controlledObject == null)
        //    if (GUILayout.Button("Create GameObject"))
        //        controlledObject = new GameObject();

        //controlledObject = (GameObject)EditorGUILayout.ObjectField(controlledObject, typeof(GameObject), true);

        GUI.Button(new Rect(0,32,20,20),"ss");
    }
}
