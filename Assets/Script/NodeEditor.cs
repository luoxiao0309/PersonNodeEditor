using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NodeEditor : EditorWindow
{
    #region 属性列表
    /// <summary>
    /// 鼠标位置
    /// </summary>
    Vector3 mousePosition;
    CustomGraph customGraph;
    /// <summary>
    /// 是否点击在窗口上
    /// </summary>
    private bool clickedOnwindow = false;
    /// <summary>
    /// 选中窗口
    /// </summary>
    private BaseNode selectedNode;
    /// <summary>
    /// 
    /// </summary>
    private GUISkin skin;
    private List<int> listDeleteNodes = new List<int>();
    #endregion

    [MenuItem("自定义/节点")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    private void OnEnable()
    {
        //if (graphNode == null)
        //{
        //    graphNode = CreateInstance<GraphNode>();
        //    graphNode.windowRect = new Rect(10, position.height * .7f, 200, 100);
        //    graphNode.windowTitle = "Graph";
        //}
        //customGraph = Resources.Load("CustomGraph") as CustomGraph;
        //style = settings.skin.GetStyle("window");
        //activeStyle = settings.activeSkin.GetStyle("window");
    }

    //private void Update()
    //{
    //    if (listDeleteNodes.Count>0)
    //    {
    //        customGraph.ClearWindows(listDeleteNodes);
    //        listDeleteNodes.Clear();
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (customGraph!=null)
    //    {
    //        EditorUtility.SetDirty(customGraph);
    //    }
    //}
    
    /// <summary>
    /// 添加子窗口只能在OnGUI下绘制
    /// </summary>
    void OnGUI()
    {
        DrawBackground();

        EditorGUILayout.LabelField("", GUILayout.Width(100));
        EditorGUILayout.LabelField("Assign Graph", GUILayout.Width(100));
        customGraph = EditorGUILayout.ObjectField(customGraph, typeof(CustomGraph), false, GUILayout.Width(200)) as CustomGraph;

        if (customGraph==null)
        {
            return;
        }
        
        //绘画窗口
        this.DrawChildWindow(()=> {
            for (int i = 0; i < customGraph.windows.Count; i++)
            {
                BaseNode b = customGraph.windows[i];

                b.windowRect = GUI.Window(b.id, b.windowRect,
                           DrawNodeWindow, b.windowTitle+": "+ b.id);
            }
        });
        
        //绘制连线
        for (int i = 0; i < customGraph.windows.Count; i++)
        {
            BaseNode b = customGraph.windows[i];
            
            if (b.childNodes.Count>0)
            {
                foreach (var nodeId in b.childNodes)
                {
                    var item = customGraph.GetBaseNodeById(nodeId);
                    DrawNodeCurve(b.windowRect, item.windowRect, Color.blue);
                }
            }
        }

        Event e = Event.current;
        mousePosition = e.mousePosition;

        UserInput(e);
    }

    //绘画窗口函数
    void DrawNodeWindow(int id)
    {
        //创建一个GUI Button
        if (GUILayout.Button("Link"))
        {
            Debug.Log("Clikc Link Button: "+id);
        }
        //设置改窗口可以拖动
        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);
    }

    /// <summary>
    /// 用户输入事件
    /// </summary>
    /// <param name="e"></param>
    void UserInput(Event e)
    {
        MouseClickEvent.MouseLeftClick(e,()=>{
            SelectWindow(e);
        });

        MouseClickEvent.MouseRightClick(e,()=> {
            SelectWindow(e);

            if (!clickedOnwindow)
            {
                AddNewNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        });
    }

    /// <summary>
    /// 选中窗口.
    /// </summary>
    /// <param name="e"></param>
    public void SelectWindow(Event e)
    {
        clickedOnwindow = false;
        selectedNode = null;

        for (int i = 0; i < customGraph.windows.Count; i++)
        {
            if (customGraph.windows[i].windowRect.Contains(e.mousePosition))
            {
                clickedOnwindow = true;
                selectedNode = customGraph.windows[i];
                break;
            }
        }
    }

    public void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Add State"), false, () =>
        {
            Debug.LogWarning("Add State");
            BaseNode baseNode = customGraph.AddNodeOnGraph(null, 150, 100, "TestNode", new Vector3(e.mousePosition.x, e.mousePosition.y));

        });

        //menu.AddMenuItem(MouseMenuItem.CreateMenuItem(new GUIContent("Add State"), false, () =>
        //{
        //    Debug.LogWarning("Add State");
        //    Success = true;
        //    BaseNode baseNode = customGraph.AddNodeOnGraph(null, 150, 100, "TestNode", new Vector3(e.mousePosition.x, e.mousePosition.y));
        //}));

        menu.AddItem(new GUIContent("Add Comment"), false, null);
        menu.ShowAsContext();
        //e.Use();
    }

    void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("delete All Node"), false, ()=> {
            listDeleteNodes.Clear();
            listDeleteNodes.AddRange(selectedNode.childNodes);
            listDeleteNodes.Add(selectedNode.id);

            if (selectedNode.ParentNode != 0)
            {
                var Node = customGraph.GetBaseNodeById(selectedNode.ParentNode);
                Node.childNodes.Remove(selectedNode.id);
            }

            customGraph.ClearWindows(listDeleteNodes);

            selectedNode = null;
        });
        menu.AddItem(new GUIContent("Add State"), false, () => {
            BaseNode baseNode = customGraph.AddNodeOnGraph(null, 150, 100, "TestNode", new Vector3(e.mousePosition.x, e.mousePosition.y));
            selectedNode.childNodes.Add(baseNode.id);
            baseNode.ParentNode = selectedNode.id;
        });
        menu.ShowAsContext();
        //e.Use();
    }

    /// <summary>
    /// 绘制背景(放在第一行, 否则遮罩)
    /// </summary>
    public void DrawBackground()
    {
        if (skin == null)
        {
            //此函数将在Assets/Editor Default Resources/+path中查找资源。如果没有，它将按名称尝试内置编辑器资源。
            skin = EditorGUIUtility.Load("skin.guiskin") as GUISkin;
        }
            
        GUI.skin = skin;

        float w = position.width;
        float h = position.width;

        Texture2D bg = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        bg.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f));
        bg.Apply();
        GUI.DrawTexture(new Rect(0, 0, w, h), bg);

        Handles.BeginGUI();
        Handles.color = new Color(0.7f, 0.7f, 0.7f, 0.1f);
        for (int i = 0; i * 60 <= w; i++) Handles.DrawLine(new Vector3(60 * i, 0), new Vector3(60 * i, h));
        for (int i = 0; i * 60 <= h; i++) Handles.DrawLine(new Vector3(0, 60 * i), new Vector3(w, 60 * i));
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        for (int i = 0; i * 20 <= w; i++) if (i % 3 != 0) Handles.DrawLine(new Vector3(20 * i, 0), new Vector3(20 * i, h));
        for (int i = 0; i * 20 <= h; i++) if (i % 3 != 0) Handles.DrawLine(new Vector3(0, 20 * i), new Vector3(w, 20 * i));
        Handles.EndGUI();
    }
}
