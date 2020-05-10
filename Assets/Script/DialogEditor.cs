using AmazingNodeEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


public class DialogNodeEditor : EditorWindow
{
    #region 属性列表
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
    public bool dragAccess = false;
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

    private List<Connection> connections = new List<Connection>();

    static DialogNodeEditor editorWindow;
    
    private MouseData mouse = new MouseData();
    public MouseData mouseData { get { return mouse; } }

    protected Vector2 offset;
    protected Vector2 drag;

    [MenuItem("自定义/对话框")]
    static void ShowEditor()
    {
        editorWindow = EditorWindow.GetWindow<DialogNodeEditor>();
    }

    public static DialogNodeEditor Instance
    {
        get
        {
            if (editorWindow==null)
            {
                editorWindow = EditorWindow.GetWindow<DialogNodeEditor>();
            }
            return editorWindow;
        }
    }

    private void OnEnable()
    {
        //EditorUtility.l
        Texture2D resizeHandle = EditorGUIUtility.Load("ResizeHandle.png") as Texture2D;
        //Texture2D resizeHandle = AssetDatabase.LoadAssetAtPath("Assets/Node/Textures/PNG/ResizeHandle.png", typeof(Texture2D)) as Texture2D;
        _resizeIcon = new GUIContent(resizeHandle);
        

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
    
    /// <summary>
    /// 添加子窗口只能在OnGUI下绘制
    /// </summary>
    void OnGUI()
    {
        Event e = Event.current;

        GridDrawer.DrawGrid(20, 0.2f, position, ref offset, ref drag);
        GridDrawer.DrawGrid(100, 0.4f, position, ref offset, ref drag);

        //DrawBackground();

        DrawToolbar();
        //DrawGroup();
        
        //绘制选择区域
        DrawSelectArea();

        if (customGraph == null)
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
            Repaint();
        }
        #endregion

        //绘画窗口(绘制Windows窗口会卡 GUI.changed, 所以你只能不断刷.)
        this.DrawChildWindow(() =>
        {
            for (int i = 0; i < customGraph.windows.Count; i++)
            {
                BaseNode b = customGraph.windows[i];

                if (b.NodeType == NodeType.StartNode)
                {
                    StartNode boxBaseNode = b as StartNode;
                    boxBaseNode.InitData();
                    boxBaseNode.DrawWindow();
                }
                else if (b.NodeType == NodeType.EndNode)
                {
                    EndNode boxBaseNode = b as EndNode;
                    boxBaseNode.InitData();
                    boxBaseNode.DrawWindow();
                }
            }
        });

        ProcessNodes(e);
        if (connections.Count>0)
        {
            for (int m = 0; m < connections.Count; m++)
            {
                var item = connections[m];
                item.Draw();
            }
        }

        DrawConnectionLine(e);

        //在画线时,不响应其他的事件
        if (makeTransitionMode == false)
        {
            UserInput(e);
        }

        Repaint();
    }

    private void ProcessNodes(Event e)
    {
        if (customGraph.windows != null)
        {
            for (int i = customGraph.windows.Count - 1; i >= 0; i--)
            {
                bool guiChanged = customGraph.windows[i].ProcessEvents(e);

                //if (guiChanged) {
                //    GUI.changed = true;
                //}
            }
        }
    }



    //绘画窗口函数
    void DrawNodeWindow(int id)
    {
        var window = customGraph.windows[id];

        // 标题栏
        const int titleHeight = 18;
        Rect title = window.WindowRect;
        title.height = titleHeight;
        title.position = new Vector2(-1, 0);
        GUIStyle style = new GUIStyle("dropDownButton");
        style.fontSize = (int)((titleHeight - 3));
        style.fixedHeight = titleHeight; // バー高さ
        var temp = GUI.backgroundColor;
        GUI.color = new Color(0.4f, 0.6f, 1.0f, 1.0f);
        GUI.Label(title, window.windowTitle, style);
        GUI.color = temp;

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
            //Repaint();
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
                //Repaint();
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
                //Repaint();
            }
        }
    }

    void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

        if (DistancePointLine(Event.current.mousePosition, startTan, endTan) < 10)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.blue, null, 4);
        }
        else
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.yellow, null, 4);
        }
        
        if (Handles.Button((start.center + end.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            Debug.LogWarning("删除连续");
        }

        //Repaint();
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

        ////拖拽全部节点(整个背景窗口).
        //MouseClickEvent.MouseCenterClick(e, (e1) =>
        //{
        //    MouseClickEvent.MouseDownEvent(e1, (e2) =>
        //    {
        //        initMousePos = e.mousePosition;
        //    });

        //    MouseClickEvent.MouseDragEvent(e1, (e2) =>
        //    {
        //        Vector2 deltaMousePos = e2.mousePosition - initMousePos;
        //        for (int i = 0; i < customGraph.windows.Count; i++)
        //        {
        //            BaseNode b = customGraph.windows[i];
        //            b.WindowRect.x += deltaMousePos.x;
        //            b.WindowRect.y += deltaMousePos.y;
        //        }

        //        initMousePos = e.mousePosition;
        //    });
        //});

        if (mouseData.IsDown(MouseButton.Middle))
        {
            dragAccess = true;
        }
        if (mouseData.IsUp(MouseButton.Middle))
        {
            dragAccess = false;
        }

        drag = Vector2.zero;
        if (mouseData.IsDrag(MouseButton.Middle))
        {
            drag = e.delta;

            if (customGraph.windows != null)
            {
                for (int i = 0; i < customGraph.windows.Count; ++i)
                {
                    customGraph.windows[i].Drag(e.delta);
                }
            }
        }

        #region 拖拽单个节点
        //if (mouseData.IsDown(MouseButton.Left))
        //{
        //    if (selectedNode)
        //        initMousePos = e.mousePosition;
        //}

        //if (Event.current.type == EventType.MouseDrag)
        //{
        //    if (Event.current.IsMouseLeftClick())
        //    {
        //        if (selectedNode)
        //        {
        //            Vector2 deltaMousePos = e.mousePosition - initMousePos;
        //            selectedNode.Drag(deltaMousePos);
        //            //持续Repaint下,是不行的.
        //            //selectedNode.Drag(e.delta);
        //            initMousePos = e.mousePosition;
        //        }

        //        Event.current.Use();
        //    }
        //}
        #endregion
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
            var window = customGraph.windows[i];
            if (window.WindowRect.Contains(e.mousePosition))
            {
                clickedOnwindow = true;
                window.Active = true;
                selectedNode = window;
            }
            else
            {
                window.Active = false;
            }
        }
        return clickedOnwindow;
    }

    public void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        
        menu.AddItem(new GUIContent("Add STARTNode"), false, () =>
        {
            customGraph.AddCustomwNode<StartNode>(200, 100, "STARTNode", new Vector3(e.mousePosition.x, e.mousePosition.y),NodeType.StartNode);
        });
        menu.AddItem(new GUIContent("Add ENDNode"), false, () =>
        {
            customGraph.AddCustomwNode<EndNode>(200, 100, "ENDNode", new Vector3(e.mousePosition.x, e.mousePosition.y), NodeType.EndNode);
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

            //删除节点.
            //var assetPath = path + selNode.id + ".asset";
            //Debug.Log("删除标记:" + assetPath);
            //AssetDatabase.DeleteAsset(assetPath);
            //AssetDatabase.SaveAssets();

            selectedNode = null;
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

    [OnOpenAsset]
    private static bool OnOpenNodeGraph(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        var graph = obj as CustomGraph;
        if (graph != null)
        {
            var window = GetWindow<DialogNodeEditor>();
            window.customGraph = graph;
            window.minSize = new Vector2(500, 350);
            window.titleContent = new GUIContent("Node Editor");
            return true;
        }

        return false;
    }

    #region 工具栏
    /// <summary>
    /// 绘制工具栏
    /// </summary>
    protected void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.FlexibleSpace();
        //EditorGUILayout.LabelField("Position:" + offset.ToString(), EditorStyles.label);
        if (GUILayout.Button("File", EditorStyles.toolbarDropDown))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("测试1"), false, () => {
                
            });
            menu.AddItem(new GUIContent("测试2"), false, () => {
                
            });
            
            menu.ShowAsContext();
        }

        if (GUILayout.Button("创建", EditorStyles.toolbarButton))
        {
            customGraph = ScriptableObject.CreateInstance<CustomGraph>();
        }

        //无法序列化连接，因为它没有无参数的构造函数。
        if (GUILayout.Button("Save XML", EditorStyles.toolbarButton))
        {
            var savePath = EditorUtility.SaveFilePanel(
                "保存剧情数据",
                "Assets",
                "WordEditorData",
                "xml"
            );

            FileInfo fileInfo = new FileInfo(savePath);
            if (fileInfo.Directory.Exists == false)
            {
                fileInfo.Directory.Create();
            }
            if (fileInfo.Exists == false)
            {
                File.Create(savePath).Dispose();
            }

            if (customGraph != null)
            {
                XMLSaver.Serialize(customGraph, savePath);
            }
        }

        if (GUILayout.Button("Load XML", EditorStyles.toolbarButton))
        {

        }

        if (GUILayout.Button("Save Objects", EditorStyles.toolbarButton))
        {
            var savePath = EditorUtility.SaveFilePanel(
                "保存剧情数据",
                "Assets",
                "WordEditorData",
                "asset"
            );
            savePath = savePath.Substring(Application.dataPath.Length - 6);
            AssetDatabase.CreateAsset(customGraph, savePath);
            for (int nodeCnt = 0; nodeCnt < customGraph.windows.Count; nodeCnt++)
            {
                // Add every node and every of it's inputs/outputs into the file. 
                // Results in a big mess but there's no other way
                BaseNode node = customGraph.windows[nodeCnt];
                AssetDatabase.AddObjectToAsset(node, customGraph);
                //for (int inCnt = 0; inCnt < node.inputs.Count; inCnt++)
                //    AssetDatabase.AddObjectToAsset(node.inputs[inCnt], node);
                //for (int outCnt = 0; outCnt < node.outputs.Count; outCnt++)
                //    AssetDatabase.AddObjectToAsset(node.outputs[outCnt], node);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("Load Objects", EditorStyles.toolbarButton))
        {
            var path = EditorUtility.OpenFilePanelWithFilters(
                "保存剧情数据",
                "Assets",
                new string[] { "剧情数据", "asset" }
            );
            path = path.Substring(Application.dataPath.Length - 6);

            customGraph = AssetDatabase.LoadAssetAtPath<CustomGraph>(path);
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

    protected void DrawMenuBar()
    {
        float menuBarHeight = 20f;
        Rect menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            
        }

        GUILayout.Space(5);

        if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
        {
            
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    #endregion

    #region 连接点、连接线
    public void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;
        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }

        //使用当前事件,阻止其他事件响应.
        Event.current.Use();
    }
    /// <summary>
    /// 输出连接点
    /// </summary>
    /// <param name="outPoint"></param>
    public void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }
        Event.current.Use();
    }

    private void CreateConnection()
    {
        //Connection connection = new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);
        //selectedInPoint.node.AddConnection(connection);
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
        Event.current.Use();
    }
    #endregion

    #region 鼠标触碰到线
    /// <summary>
    /// 鼠标点到连线的距离
    /// </summary>
    /// <param name="point"></param>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <returns></returns>
    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }

    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 rhs = point - lineStart;
        Vector3 vector2 = lineEnd - lineStart;
        float magnitude = vector2.magnitude;
        Vector3 lhs = vector2;
        if (magnitude > 1E-06f)
        {
            lhs = (Vector3)(lhs / magnitude);
        }
        float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineStart + ((Vector3)(lhs * num2)));
    }
    #endregion

    #region 绘制组
    [NonSerialized]
    private GUIStyle backgroundStyle;
    [NonSerialized]
    private GUIStyle altBackgroundStyle;
    [NonSerialized]
    private GUIStyle opBackgroundStyle;
    [NonSerialized]
    private GUIStyle headerTitleStyle;
    [NonSerialized]
    private GUIStyle headerTitleEditStyle;
    private bool edit=false;
    private Color color = new Color(0, 0, 1, 1);

    /// <summary>
    /// Draws the NodeGroup
    /// </summary>
    public void DrawGroup()
    {
        if (backgroundStyle == null)
            GenerateStyles();
        
        Rect handleRect = new Rect(400,126,400,15);
        GUI.Box(handleRect, GUIContent.none, opBackgroundStyle);

        // Body
        //groupBodyRect:(x:400.80, y:126.40, width:400.00, height:400.00)
        Rect groupBodyRect = new Rect(400,126,400,400);
        GUI.Box(groupBodyRect, GUIContent.none, backgroundStyle);
        
        // Header
        //groupHeaderRect:(x:400.80, y:96.40, width:400.00, height:30.00)
        Rect groupHeaderRect = new Rect(400,96,400,30);
        GUILayout.BeginArea(groupHeaderRect, true ? GUIStyle.none : altBackgroundStyle);
        GUILayout.BeginHorizontal();

        GUILayout.Space(8);
        string title1 = "";
        // Header Title
        if (edit)
            title1 = GUILayout.TextField(title1, headerTitleEditStyle, GUILayout.MinWidth(40));
        else
            GUILayout.Label(title1, headerTitleStyle, GUILayout.MinWidth(40));

        GUILayout.Space(10);
        GUILayout.FlexibleSpace();

        if (edit)
        {
            GUILayout.Space(10);
            color = UnityEditor.EditorGUILayout.ColorField(color);
        }

        // Edit Button
        if (GUILayout.Button("E", new GUILayoutOption[] { GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false) }))
            edit = !edit;

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    /// <summary>
    /// Generates all the styles for this node group based of the color
    /// </summary>
    private void GenerateStyles()
    {
        // Transparent background
        Texture2D background = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.4f));
        // lighter, less transparent background
        Texture2D altBackground = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.6f));
        // nearly opaque background
        Texture2D opBackground = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.9f));

        backgroundStyle = new GUIStyle();
        backgroundStyle.normal.background = background;
        backgroundStyle.padding = new RectOffset(10, 10, 5, 5);

        altBackgroundStyle = new GUIStyle();
        altBackgroundStyle.normal.background = altBackground;
        altBackgroundStyle.padding = new RectOffset(10, 10, 5, 5);

        opBackgroundStyle = new GUIStyle();
        opBackgroundStyle.normal.background = opBackground;
        opBackgroundStyle.padding = new RectOffset(10, 10, 5, 5);

        //			dragHandleStyle = new GUIStyle ();
        //			dragHandleStyle.normal.background = background;
        //			//dragHandleStyle.hover.background = altBackground;
        //			dragHandleStyle.padding = new RectOffset (10, 10, 5, 5);

        headerTitleStyle = new GUIStyle();
        headerTitleStyle.fontSize = 20;
        headerTitleStyle.normal.textColor = Color.white;

        headerTitleEditStyle = new GUIStyle(headerTitleStyle);
        headerTitleEditStyle.normal.background = background;
        headerTitleEditStyle.focused.background = background;
        headerTitleEditStyle.focused.textColor = Color.white;
    }
    #endregion

    #region 选择区域
    Rect m_SelectedArea;
    public List<BaseNode> nodeList
    {
        get
        {
            return customGraph.windows;
        }
    }
    public List<BaseNode> selectedNodeList = new List<BaseNode>();

    /// <summary>
    /// 范围选择
    /// </summary>
    void SelectArea()
    {
        // 节点单体选择
        bool isOverrapped = false;
        if (mouseData.IsDown(MouseButton.Left))
        {
            foreach (var node in this.nodeList)
            {
                if (node.WindowRect.Overlaps(mouseData.rect))
                {
                    Debug.LogWarning("区域重叠");
                    node.Active = true;
                    isOverrapped = true;

                    if (selectedNodeList.Count == 0)
                    {
                        if (selectedNode != null)
                        {
                            if (selectedNode != node)
                            {
                                selectedNode.Active = false;
                            }
                        }
                    }
                    selectedNode = node;
                    // 选择变焦中心的节点
                    //gridContorol.GridZoomCenterPoint = mouseData.pos;
                }
                else
                {
                    if (selectedNodeList.Count == 0)
                    {
                        node.Active = false;
                    }
                }
            }

        }

        //------------------------------
        // 多节点拖尾
        //------------------------------
        if (mouseData.IsDrag(MouseButton.Left))
        {
            //var find = selectedNodeList.Find(n => n.handle == selectedNode.handle);
            //if (find == null || selectedNode == null)
            //{
            //    ClearSelectedNodes();
            //}
            foreach (var node in selectedNodeList)
            {
                isOverrapped = true;
                if (selectedNode != node)
                {
                    node.WindowRect.position += mouseData.delta;
                }
            }
        }
        if (mouseData.IsUp(MouseButton.Left) /*|| mouseData.IsDrag(MouseButton.Left)*/)
        {
            // 被选择的人们
            bool bOtherNodeSelect = true;
            foreach (var node in selectedNodeList)
            {
                if (node.WindowRect.Overlaps(mouseData.rect))
                {
                    bOtherNodeSelect = false;
                }
            }
            if (bOtherNodeSelect)
            {
                isOverrapped = false;
                ClearSelectedNodes();
            }
        }

        if (isOverrapped)
        {
            CleareSelectArea();
            return;
        }

        // 连接中的边缘删除
        //if (mouseData.IsDown(MouseButton.Left))
        //{
        //    if (ActiveEdge != null)
        //    {
        //        ActiveEdge.Remove();
        //        edgeList.Remove(ActiveEdge);
        //        ActiveEdge = null;
        //    }
        //}

        // 范围选择
        if (mouseData.IsDown(MouseButton.Left))
        {
            m_SelectedArea.position = mouseData.pos;
        }
        else if (mouseData.IsDrag(MouseButton.Left))
        {
            m_SelectedArea.width = mouseData.pos.x - m_SelectedArea.position.x;
            m_SelectedArea.height = mouseData.pos.y - m_SelectedArea.position.y;

            //Repaint();
        }
        else if (mouseData.IsUp(MouseButton.Left))
        {
            GUIHelper.RectAdjust(ref m_SelectedArea);

            // 激活选择范围内的节点
            if ((m_SelectedArea.xMax - m_SelectedArea.xMin) != 0 && (m_SelectedArea.yMax - m_SelectedArea.yMin) != 0)
            {
                foreach (var node in this.nodeList)
                {
                    if (node.WindowRect.Overlaps(m_SelectedArea))
                    {
                        node.Active = true;
                        selectedNodeList.Add(node);
                    }
                    else
                    {
                        node.Active = false;
                    }
                }
            }

            CleareSelectArea();
        }
        else
        {
            CleareSelectArea();
        }

        // 因为节点拖拽中偶尔会从现点伸长…
        if (m_SelectedArea.position.x == 0 && m_SelectedArea.position.y == 0)
        {
            CleareSelectArea();
        }
    }

    //范围选择到此为止
    void CleareSelectArea()
    {
        m_SelectedArea.width = 0;
        m_SelectedArea.height = 0;
        m_SelectedArea.x = 0;
        m_SelectedArea.y = 0;
    }

    void ClearSelectedNodes()
    {
        foreach (var node in selectedNodeList)
        {
            node.Active = false;
        }
        selectedNodeList.Clear();
    }

    /// <summary>
    /// 绘制选择区域.
    /// </summary>
    public void DrawSelectArea()
    {
        //假如是Drag事件, 这停止在这.
        if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint)
        {
            ClearMouse();
            UpdateMouse();

            SelectArea();
        }

        GUIHelper.Fill(m_SelectedArea, new Color(0.5f, 0.5f, 0.5f, 0.5f));
        GUIHelper.DrawRect(m_SelectedArea, Color.white, 2);
    }
    #endregion 

    #region 鼠标数据更新
    /// <summary>
    /// 鼠标数据更新
    /// </summary>
    void UpdateMouse()
    {
        mouse.pos = Event.current.mousePosition;
        mouse.delta = Event.current.delta;

        if (Event.current.type == EventType.MouseDown ||
            Event.current.type == EventType.MouseUp ||
            Event.current.type == EventType.MouseDrag ||
            Event.current.type == EventType.ScrollWheel ||
            Event.current.type == EventType.ContextClick)
        {
            mouse.button = (MouseButton)Event.current.button;
            mouse.type = (MouseEventType)Event.current.type;

            //prevMouse = mouse.Clone();
        }
        if (Event.current.type == EventType.MouseMove)
        {
            mouse.type = (MouseEventType)Event.current.type;
            //prevMouse = mouse.Clone(); ;
        }

        // 在拖拽过程中，如果光标在窗口外的话，请将事件返回。
        if (Event.current.type == EventType.Ignore)
        {
            //mouse = prevMouse.Clone();
            mouse.type = MouseEventType.Up;
        }
    }

    /// <summary>
    /// 清除鼠标信息
    /// </summary>
    void ClearMouse()
    {
        mouse.delta = new Vector2(0, 0);
        mouse.button = MouseButton.None;
        mouse.type = MouseEventType.None;
    }
    #endregion
}
