using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UNEB;

[CustomEditor(typeof(NodeGraph))]
public class NodeGraphInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NodeGraph myTarget = (NodeGraph)target;
        if (GUILayout.Button("Show Graph"))
        {
            ShowGraph(myTarget);
        }
    }

    public void OnShowGraphClicked()
    {

    }

    public void ShowGraph(NodeGraph sender)
    {
        NodeEditor nodeEditor = (NodeEditor)EditorWindow.GetWindow(typeof(NodeEditor));
        nodeEditor.Show();
    }
}
