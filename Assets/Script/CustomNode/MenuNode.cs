using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class MenuNode : BaseNode
{
    private GameObject controlledObject;
    [SerializeField]
    public Texture2D winIcon;
    [SerializeField]
    public Texture2D curveJoinR;
    [SerializeField]
    public Texture2D curveJoinL;
    
    public override void DrawWindow()
    {
        //this.name = "MenuNode";
        ////Debug.LogWarning("MenuNodeMenuNodeMenuNodeMenuNodeMenuNode");
        //winIcon = EditorGUIUtility.Load("markPanel.png") as Texture2D;
        //curveJoinR = EditorGUIUtility.Load("joinR.png") as Texture2D;
        //curveJoinL = EditorGUIUtility.Load("joinL.png") as Texture2D;

        //if (controlledObject == null)
        //    if (GUILayout.Button("Create GameObject"))
        //        controlledObject = new GameObject();

        //controlledObject = (GameObject)EditorGUILayout.ObjectField(controlledObject, typeof(GameObject), true);
        base.DrawWindow();
    }
}
