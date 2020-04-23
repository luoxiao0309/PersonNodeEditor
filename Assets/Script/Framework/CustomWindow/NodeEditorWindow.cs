using NodeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeEditorWindow : EditorWindow
{
    public static NodeEditorWindow editorWindow;

    Rect sideWindowRect;
    Rect scrollViewRect;
    static Vector2 canvasSize = new Vector2(4000, 4000);
    public Vector2 scrollPos = new Vector2(canvasSize.x * 0.5f, canvasSize.y * 0.5f);

    [MenuItem("Window/Node Editor")]
    static void ShowEditor()
    {
#if UNITY_EDITOR
        if (editorWindow)
            editorWindow.Close();

        editorWindow = GetWindow<NodeEditorWindow>();
#endif
    }

    void OnGUI()
    {
        GUI.skin = GUIx.I.skin;
        CenterArea();
        LeftArea();
    }

    public void CenterArea()
    {
        Event e = Event.current;
        //remove mousewheel input
        if (e.type == EventType.ScrollWheel)
            e.Use();

        if (editorWindow)
            editorWindow.minSize = new Vector2(512, 512);
        else
            ShowEditor();

        
        sideWindowRect = new Rect(0, 0, 155, editorWindow.position.height);
        scrollViewRect = new Rect(sideWindowRect.width, 0, editorWindow.position.width - sideWindowRect.width, editorWindow.position.height);
        scrollPos = GUI.BeginScrollView(scrollViewRect, scrollPos, new Rect(0, 0, canvasSize.x, canvasSize.y), false, false);

        DrawBackGround();

        DrawNode();

        GUI.EndScrollView();
    }

    public void LeftArea()
    {
        GUILayout.BeginArea(sideWindowRect, GUI.skin.box);

        GUI.enabled = false;
        if (GUILayout.Button("Create"))
        {
        }

        GUI.enabled = true;
        if (GUILayout.Button("Load"))
        {
        }

        GUI.enabled = false;
        if (GUILayout.Button("Center View"))
        {

        }
        GUI.enabled = true;

        GUI.enabled = false;
        if (GUILayout.Button("Calc Order"))
        {

        }
        GUI.enabled = true;

        GUI.enabled = true;
        if (GUILayout.Button("Export to XML"))
        {
        }
        GUI.enabled = true;

        //GUI.enabled = Graph != null;
        if (GUILayout.Button("Import from XML"))
        {

        }
        GUILayout.EndArea();
    }

    public void DrawBackGround()
    {
        for (int i = 0; i < canvasSize.x / GUIx.I.background.fixedWidth; i++)
        {
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, 0, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 2, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 3, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 4, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 5, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 6, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 7, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
            //GUI.DrawTexture(new Rect(GUIx.I.background.fixedWidth * i, GUIx.I.background.fixedWidth * 8, GUIx.I.background.fixedWidth, GUIx.I.background.fixedHeight), GUIx.I.background.normal.background);
        }
    }

    void DrawNode()
    {
        BeginWindows();

        //if (_canvas != null)
        //{
        //    int nodeCount = _canvas.nodeList.Count;
        //    for (int i = 0; i < nodeCount; i++)
        //    {
        //        Node node = _canvas.nodeList[i];
        //        // will modify node.rect when the node move
        //        node.rect = GUILayout.Window(node.Id, node.rect, DrawNodeWindow, node.name);
        //        node.DrawKnob();
        //    }
        //}

        GUILayout.Window(1, new Rect(1100,1100,500,500), (id)=> {

        }, "sss");

        EndWindows();
    }
}
