using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DialogNode : BaseNode
{
    public ConnectionPoint inPoint; //not really used, for editor structure
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();
    public List<string> Triggers = new List<string>();
    
    public string text;
    public AudioClip clip;
    
    //private float button_height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 4f;
    private bool isAddClicked = false;
    private bool isRemoveClicked = false;

    public override void DrawWindow()
    {
        WindowRect.height = 0;
        GUILayout.BeginVertical();

        WindowTitle = EditorGUILayout.TextField("Title", WindowTitle);
        EditorGUILayout.PrefixLabel("Text");
        text = EditorGUILayout.TextArea(text, GUILayout.Height(EditorGUIUtility.singleLineHeight * 5));
        clip = (AudioClip)EditorGUILayout.ObjectField("Audio", clip, typeof(AudioClip), false, GUILayout.MinWidth(200));

        GUILayout.BeginHorizontal();
        isRemoveClicked = GUILayout.Button("-");
        isAddClicked = GUILayout.Button("+");
        if (isAddClicked)
        {
            outPoints.Add(ConnectionPoint.CreateConnectionPoint(this, ConnectionPointType.Out, customGraph.OnClickOutPoint));
            Triggers.Add("");
        }
        else if (isRemoveClicked && outPoints.Count > 1)
        {
            outPoints.RemoveAt(outPoints.Count - 1);
            Triggers.RemoveAt(Triggers.Count - 1);
        }
        GUILayout.EndHorizontal();

        WindowRect.height = 200;

        for (int i = 0; i < Triggers.Count; i++)
        {
            Triggers[i] = EditorGUILayout.TextField("Option " + i, Triggers[i], GUILayout.Height(20));
            if (Event.current.type == EventType.Repaint)
            {
                //outPoints[i].pointRect = GUILayoutUtility.GetLastRect();
                var rect = GUILayoutUtility.GetLastRect();
                rect.x = WindowRect.x + WindowRect.width;
                rect.y = WindowRect.y + rect.y;
                rect.width = 20;
                rect.height = 20;
                outPoints[i].SetRect(rect);
            }
            WindowRect.height += 20;
        }
        GUILayout.EndVertical();
    }

    public override void SetStyle()
    {
        Style.normal.background = EditorGUIUtility.Load("Textures/redTex.png") as Texture2D;

        Style.normal.textColor = Color.white;
        Style.fontSize = 32;
        Style.alignment = TextAnchor.MiddleCenter;
    }

    public override void DrawConnectionPoint()
    {
        inPoint.Draw();
        foreach (var item in outPoints)
        {
            item.Draw(item.pointRect);
        }
    }

    public void SetData()
    {
        inPoint = ConnectionPoint.CreateConnectionPoint(this, ConnectionPointType.In, customGraph.OnClickInPoint);
        outPoints.Add(ConnectionPoint.CreateConnectionPoint(this, ConnectionPointType.Out, customGraph.OnClickOutPoint));
        Triggers.Add("default");
    }
}
