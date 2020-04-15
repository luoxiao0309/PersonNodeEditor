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
    /// 选中窗口
    /// </summary>
    private BaseNode selectedNode;
    private List<int> listDeleteNodes = new List<int>();
    /// <summary>
    /// 画线
    /// </summary>
    public bool makeTransitionMode = false;
    /// <summary>
    /// 鼠标按下初始位置.
    /// </summary>
    public Vector2 initMousePos = Vector2.zero;
    /// <summary>
    /// 重设大小Icon
    /// </summary>
    public GUIContent _resizeIcon;
    /// <summary>
    /// 输入点样式
    /// </summary>
    private GUIStyle inPointStyle;
    /// <summary>
    /// 输出点样式.
    /// </summary>
    private GUIStyle outPointStyle;
    /// <summary>
    /// 节点样式
    /// </summary>
    private GUIStyle nodeStyle;
    /// <summary>
    /// 选中节点样式
    /// </summary>
    private GUIStyle selectedNodeStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;
    #endregion

    [MenuItem("自定义/节点")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    private void OnEnable()
    {
        //EditorUtility.l
        Texture2D resizeHandle = EditorGUIUtility.Load("ResizeHandle.png") as Texture2D;
        //Texture2D resizeHandle = AssetDatabase.LoadAssetAtPath("Assets/Node/Textures/PNG/ResizeHandle.png", typeof(Texture2D)) as Texture2D;
        _resizeIcon = new GUIContent(resizeHandle);
        //if (graphNode == null)
        //{
        //    graphNode = CreateInstance<GraphNode>();
        //    graphNode.windowRect = new Rect(10, position.height * .7f, 200, 100);
        //    graphNode.windowTitle = "Graph";
        //}
        //customGraph = Resources.Load("CustomGraph") as CustomGraph;
        //style = settings.skin.GetStyle("window");
        //activeStyle = settings.activeSkin.GetStyle("window");

        //inPointStyle = new GUIStyle();
        //inPointStyle.normal.background = EditorGUIUtility.Load("joinL.png") as Texture2D;
        //inPointStyle.active.background = EditorGUIUtility.Load("joinL.png") as Texture2D;
        //inPointStyle.border = new RectOffset(4, 4, 12, 12);

        //StyleHelper.inStyle = inPointStyle;

        //outPointStyle = new GUIStyle();
        //outPointStyle.normal.background = EditorGUIUtility.Load("joinR.png") as Texture2D;
        //outPointStyle.active.background = EditorGUIUtility.Load("joinR.png") as Texture2D;
        //outPointStyle.border = new RectOffset(4, 4, 12, 12);

        #region 节点样式
        //nodeStyle = new GUIStyle();
        //nodeStyle.normal.background = EditorGUIUtility.Load(
        //    "builtin skins/darkskin/images/node1.png") as Texture2D;
        //nodeStyle.border = new RectOffset(12, 12, 12, 12);

        nodeStyle = new GUIStyle();
        //nodeStyle.normal.background = EditorGUIUtility.Load("boxGround.png") as Texture2D;
        nodeStyle.normal.background = Resources.Load("boxGround") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load(
            "builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load(
            "builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load(
            "builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load(
            "builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load(
            "builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        #endregion
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
        Event e = Event.current;
        mousePosition = e.mousePosition;

        DrawBackground();
        DrawToolbar();

        EditorGUILayout.LabelField("", GUILayout.Width(100));
        EditorGUILayout.LabelField("Assign Graph", GUILayout.Width(100));
        customGraph = EditorGUILayout.ObjectField(customGraph, typeof(CustomGraph), false, GUILayout.Width(200)) as CustomGraph;
        
        if (customGraph==null)
        {
            return;
        }
        
        //不能使用LeftClick来判断,有问题.
        if (e.IsMouseDownClick())
        {
            makeTransitionMode = false;
            ClickedOnWindow(e);
        }

        #region Window 窗口连线.
        if (selectedNode != null && makeTransitionMode)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
            DrawNodeCurve(selectedNode.WindowRect, mouseRect, Color.blue);
        }
        #endregion

        //绘画窗口(绘制Windows窗口会卡 GUI.changed, 所以你只能不断刷.)
        this.DrawChildWindow(()=> {
            for (int i = 0; i < customGraph.windows.Count; i++)
            {
                BaseNode b = customGraph.windows[i];
                if (b.NodeType==NodeType.Window)
                {
                    b.WindowRect = GUI.Window(i, b.WindowRect,
                           DrawNodeWindow, b.windowTitle + ": " + b.id);
                }
                else
                {
                    BoxBaseNode boxBaseNode = new BoxBaseNode(inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint);
                    boxBaseNode = b as BoxBaseNode;

                    boxBaseNode.nodeStyle = nodeStyle;
                    boxBaseNode.DrawWindow();
                }
            }
        });

        //绘制连线
        for (int i = 0; i < customGraph.windows.Count; i++)
        {
            BaseNode b = customGraph.windows[i];

            //绘制box矩形连线.
            if (b.connections.Count>0)
            {
                //foreach 一直刷会有问题.
                for (int m = 0; m < b.connections.Count; m++)
                {
                    var item = b.connections[m];
                    item.Draw();
                }
            }
            else
            {
                //绘制窗口连线
                if (b.childNodes.Count > 0)
                {
                    foreach (var nodeId in b.childNodes)
                    {
                        var item = customGraph.GetBaseNodeById(nodeId);
                        DrawNodeCurve(b.WindowRect, item.WindowRect, Color.blue);
                    }
                }
            }
        }
        
        DrawConnectionLine(e);

        //在画线时,不响应其他的事件
        if (makeTransitionMode == false)
        {
            UserInput(e);
        }

        //反复刷新.
        Repaint();
    }

    //绘画窗口函数
    void DrawNodeWindow(int id)
    {
        var window = customGraph.windows[id];
        window.DrawWindow();
        
        #region 重设大小
        ResizeWindow(id, window);
        #endregion

        //设置改窗口可以拖动
        GUI.DragWindow();
    }

    /// <summary>
    /// 重置窗口大小.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="window"></param>
    void ResizeWindow(int id, BaseNode window)
    {
        //if (GUIUtility.hotControl == 0) { window.Resizable = false; }
        float _cornerX = window.WindowRect.width;
        float _cornerY = window.WindowRect.height;

        GUILayout.BeginArea(new Rect(1, _cornerY - 16, _cornerX - 3, 14));
        GUILayout.BeginHorizontal(EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();

        window.HandleArea = GUILayoutUtility.GetRect(_resizeIcon, GUIStyle.none);
        GUI.DrawTexture(new Rect(window.HandleArea.xMin + 3, window.HandleArea.yMin - 3, 20, 20), _resizeIcon.image);

        if (!window.Resizable && ((Event.current.type == EventType.MouseDown) || (Event.current.type == EventType.MouseDrag)))
        {
            if (window.HandleArea.Contains(Event.current.mousePosition, true))
            {
                window.Resizable = true;
                //"FocusType.Passive" 此控件无法接收键盘焦点。
                //GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
            }
        }

        EditorGUIUtility.AddCursorRect(window.HandleArea, MouseCursor.ResizeUpLeft);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        if (window.Resizable && (Event.current.type == EventType.MouseDrag))
        {
            ResizeNode(id, Event.current.delta.x, Event.current.delta.y);
            Repaint();
            //当你已经使用一个事件时调用这个方法。事件的类型将设置为 EventType.Used，使其他GUI元素忽略它。
            Event.current.Use();
        }

        MouseClickEvent.MouseUpEvent(Event.current, (e1) => {
            window.Resizable = false;
        });
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            if (e.IsMouseDownClick())
            {
                selectedInPoint = null;
            }
            else
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    4
                );
            }
        }
        if (selectedOutPoint != null && selectedInPoint == null)
        {
            if (e.IsMouseDownClick())
            {
                selectedOutPoint = null;
            }
            else
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    4
                );
            }
        }
    }

    void DrawNodeCurve(Rect start, Rect end, Color color)
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

    /// <summary>
    /// 用户输入事件
    /// </summary>
    /// <param name="e"></param>
    void UserInput(Event e)
    {
        //鼠标右键事件
        MouseClickEvent.MouseRightClick(e, (e1) =>
        {
            if (!ClickedOnWindow(e1))
            {
                AddNewNode(e1);
            }
            else
            {
                ModifyNode(e1);
            }
        });

        //拖拽全部节点(整个背景窗口).
        MouseClickEvent.MouseCenterClick(e, (e1) =>
        {
            MouseClickEvent.MouseDownEvent(e1, (e2) =>
            {
                initMousePos = e.mousePosition;
            });

            MouseClickEvent.MouseDragEvent(e1, (e2) =>
            {
                Vector2 deltaMousePos = e2.mousePosition - initMousePos;
                for (int i = 0; i < customGraph.windows.Count; i++)
                {
                    BaseNode b = customGraph.windows[i];
                    b.WindowRect.x += deltaMousePos.x;
                    b.WindowRect.y += deltaMousePos.y;
                }

                initMousePos = e.mousePosition;
                Repaint();
            });
        });

        ProcessNodeEvents(e);
    }

    /// <summary>
    /// 节点的鼠标输入事件
    /// </summary>
    /// <param name="e"></param>
    private void ProcessNodeEvents(Event e)
    {
        for (int i = 0; i < customGraph.windows.Count; i++)
        {
            var window = customGraph.windows[i];
            window.MouseEvent(e);
        }
    }

    void ResizeNode(int id, float deltaX, float deltaY)
    {
        var windows = customGraph.windows;

        float targetWidth = windows[id].WindowRect.width;
        float targetHeight = windows[id].WindowRect.height;

        if ((windows[id].WindowRect.width + deltaX) > 50)
            targetWidth = windows[id].WindowRect.width + deltaX;

        if ((windows[id].WindowRect.height + deltaY) > 50)
            targetHeight = windows[id].WindowRect.height + deltaY;

        windows[id].WindowRect = new Rect(windows[id].WindowRect.position.x, windows[id].WindowRect.position.y,
                                          targetWidth, targetHeight);
    }

    /// <summary>
    /// 判断是否点击在窗口上
    /// </summary>
    /// <param name="e"></param>
    public bool ClickedOnWindow(Event e)
    {
        bool clickedOnwindow = false;
        selectedNode = null;

        for (int i = 0; i < customGraph.windows.Count; i++)
        {
            if (customGraph.windows[i].WindowRect.Contains(e.mousePosition))
            {
                clickedOnwindow = true;
                selectedNode = customGraph.windows[i];
                break;
            }
        }

        return clickedOnwindow;
    }

    public void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        
        menu.AddItem(new GUIContent("Add WindowNode"), false, () =>
        {
            var menuNode= ScriptableObject.CreateInstance<MenuNode>();
            BaseNode baseNode = customGraph.AddWindowNode(menuNode, 200, 100, "WindowMenu", new Vector3(e.mousePosition.x, e.mousePosition.y));
        });
        menu.AddItem(new GUIContent("Add BoxNode"), false, () =>
        {
            var boxNode = ScriptableObject.CreateInstance<BoxNode>();
            BaseNode baseNode = customGraph.AddBoxNode(boxNode, 200, 100, "WindowMenu", new Vector3(e.mousePosition.x, e.mousePosition.y), inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, NodeType.Box);
            baseNode.nodeStyle = nodeStyle;
        });
        menu.ShowAsContext();
        //e.Use();
    }

    void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("delete All Node"), false, ()=> {
            listDeleteNodes.Clear();
            SearchChilds(selectedNode);

            if (selectedNode.ParentNode != 0)
            {
                var Node = customGraph.GetBaseNodeById(selectedNode.ParentNode);
                Node.childNodes.Remove(selectedNode.id);
            }

            customGraph.ClearWindows(listDeleteNodes);

            selectedNode = null;
        });
        menu.AddItem(new GUIContent("Add Window"), false, () => {
            BaseNode baseNode = customGraph.AddWindowNode(null, 200, 100, "TestNode", new Vector3(e.mousePosition.x, e.mousePosition.y));
            selectedNode.childNodes.Add(baseNode.id);
            baseNode.ParentNode = selectedNode.id;
        });
        
        menu.AddItem(new GUIContent("画线"), false, () => {
            makeTransitionMode = true;
        });
        menu.ShowAsContext();
        //e.Use();
    }

    /// <summary>
    /// 遍历子节点
    /// </summary>
    /// <param name="node"></param>
    public void SearchChilds(BaseNode node)
    {
        listDeleteNodes.Add(node.id);
        if (node.childNodes.Count>0)
        {
            listDeleteNodes.AddRange(node.childNodes);
            foreach (var item in node.childNodes)
            {
                var childNode = customGraph.GetBaseNodeById(item);
                SearchChilds(childNode);
            }
        }
    }

    /// <summary>
    /// 绘制背景(放在第一行, 否则遮罩)
    /// </summary>
    public void DrawBackground()
    {
        //if (skin == null)
        //{
        //    //此函数将在Assets/Editor Default Resources/+path中查找资源。如果没有，它将按名称尝试内置编辑器资源。
        //    skin = EditorGUIUtility.Load("skin.guiskin") as GUISkin;
        //}
            
        //GUI.skin = skin;

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

    /// <summary>
    /// 绘制工具栏
    /// </summary>
    protected void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.FlexibleSpace();
        //EditorGUILayout.LabelField("Position:" + offset.ToString(), EditorStyles.label);
        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
        {
            //OpenSaveDialog();
        }
        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
        {
            //OpenLoadDialog();
        }
        if (GUILayout.Button("Reset", EditorStyles.toolbarButton))
        {
            //BackToCenter();
        }
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
        {
            //Clear();
        }
        GUILayout.EndHorizontal();
    }

    #region 连接点、连接线
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;
        Debug.LogWarning("OnClickInPoint");

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
        Event.current.Use();
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        Debug.LogWarning("OnClickOutPoint");
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
        Event.current.Use();
    }

    private void CreateConnection()
    {
        Connection connection = new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);
        selectedInPoint.node.AddConnection(connection);
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connection.inPoint.node.RemoveConnection(connection);
        Event.current.Use();
    }
    #endregion
}
