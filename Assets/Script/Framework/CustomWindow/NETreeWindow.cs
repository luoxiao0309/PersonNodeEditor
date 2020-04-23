using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class NETreeWindow : EditorWindow
{

    private static float titleHeight = 20;
    private static float leftAreaWidth = 200;
    private static float rightAreaWidth = 200;
    public Rect scrollViewRect = new Rect(0, 0, 10000, 10000);
    public Vector2 scrollPos;
    //public NECanvas canvas { get { return m_cCanvas; } }
    //private NECanvas m_cCanvas;

    private int m_nTreeComposeIndex = 0;

    private List<Type> m_lstNodeType;
    private List<Type> m_lstNodeDataType;
    private GUIStyle m_cToolBarBtnStyle;
    private GUIStyle m_cToolBarPopupStyle;

    //private NENode m_cRoot;

    [MenuItem("Tools/NETreeWindow")]
    static public void OpenWindow()
    {
        var window = EditorWindow.GetWindow<NETreeWindow>();
        window.titleContent = new GUIContent("NETreeWindow");
        var position = window.position;
        position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;

        window.Show();
        window.FocusCanvasCenterPosition();
    }

    void OnEnable()
    {
        m_cToolBarBtnStyle = null;
        m_cToolBarPopupStyle = null;
        scrollPos = new Vector2(scrollViewRect.width / 2f, scrollViewRect.height / 2f);
    }
        

    public void FocusCanvasCenterPosition()
    {
        //if (m_cCanvas != null)
        //{
        //    float canvasWidth = position.width - leftAreaWidth - rightAreaWidth;
        //    float canvasHeight = position.height - titleHeight;
        //    Vector2 firstScrollPos = new Vector2((m_cCanvas.scrollViewRect.width - canvasWidth) / 2, (m_cCanvas.scrollViewRect.height - canvasHeight) / 2);
        //    m_cCanvas.scrollPos = firstScrollPos;
        //}
    }
        
    void OnDisable()
    {
        //m_cCanvas.Dispose();
        //m_cCanvas = null;
        m_cToolBarBtnStyle = null;
        m_cToolBarPopupStyle = null;
    }

    private int toolBarIndex = 0;
    private Vector3 leftScrollPos;
    private Vector3 rightScrollPos;
    void OnGUI()
    {
        if (m_cToolBarBtnStyle == null)
        {
            m_cToolBarBtnStyle = new GUIStyle((GUIStyle)"toolbarbutton");
        }

        if (m_cToolBarPopupStyle == null)
        {
            m_cToolBarPopupStyle = new GUIStyle((GUIStyle)"ToolbarPopup");
        }

        float centerAreaWidth = position.width - leftAreaWidth - rightAreaWidth;
        if (centerAreaWidth < 0) centerAreaWidth = 0;

        //画布整体描述区域
        Rect leftArea = new Rect(0, titleHeight, leftAreaWidth, position.height - titleHeight);
        GUILayout.BeginArea(leftArea);
        GUILayout.Label("总描述", m_cToolBarBtnStyle, GUILayout.Width(leftArea.width));
        leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        //画布区域
        Rect centerArea = new Rect(leftArea.width, titleHeight, centerAreaWidth, position.height - titleHeight);
        GUILayout.BeginArea(centerArea);
        Draw(centerArea);
        GUILayout.EndArea();

        //单个节点描述区域
        Rect rightArea = new Rect(leftArea.width + centerAreaWidth, titleHeight, rightAreaWidth, position.height - titleHeight);
        GUILayout.BeginArea(rightArea);
        GUILayout.Label("节点描述", m_cToolBarBtnStyle, GUILayout.Width(rightArea.width));
        rightScrollPos = GUILayout.BeginScrollView(rightScrollPos, false, true);
        float oldLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = oldLabelWidth;
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        //标题区域
        Rect titleRect = new Rect(0, 0, position.width, titleHeight);
        m_cToolBarBtnStyle.fixedHeight = titleRect.height;
        m_cToolBarPopupStyle.fixedHeight = titleRect.height;
        Debug.LogWarning("titleRect:" + titleRect);
        //水平区域
        GUILayout.BeginArea(titleRect);
        //GUILayout.Label("", tt,GUILayout.Width(50),GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        //GUILayout.Label("丹洞肃松枢", m_cToolBarBtnStyle, GUILayout.Width(10));
        //int oldTreeComposeIndex = m_nTreeComposeIndex;

        GUILayout.Label("44333", m_cToolBarBtnStyle);
        if (GUILayout.Button("创建", m_cToolBarBtnStyle, GUILayout.Width(50))) { }
        if (GUILayout.Button("加载", m_cToolBarBtnStyle, GUILayout.Width(50))) { }
        if (GUILayout.Button("保存", m_cToolBarBtnStyle, GUILayout.Width(50))) { }
        //GUILayout.Label("", m_cToolBarBtnStyle, GUILayout.Width(10));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        if (GUI.changed)
            Repaint();
    }

    public void Draw(Rect position)
    {
        Rect rect = new Rect(0, 0, position.width, position.height);
        scrollPos = GUI.BeginScrollView(rect, scrollPos, scrollViewRect, true, true);

        if (Event.current.IsMouseRightClick())
        {
            Debug.LogWarning("位置:" + Event.current.mousePosition);
        }
        BeginWindows();
        var c = GetNodeColor();
        GUI.color = c;
        var rect1 = new Rect(5000, 5000, 200, 200);
        rect1 = GUILayout.Window(GetHashCode(), rect1, (id)=> {
            var c1 = GetNodeColor();
            c1.a *= 0.5f;
            GUI.color = c1;
            GUILayout.TextField("ssd");
            ShowWindowMenu(rect1);

            GUI.Button(new Rect(100,50,30,30), ">", EditorStyles.miniButtonRight);
            GUI.Button(new Rect(70, 50, 30, 30), ">", EditorStyles.miniButtonLeft);

            GUI.DragWindow();
        }, "sdd", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        EndWindows();
        GUI.EndScrollView();
    }

    protected Color GetNodeColor()
    {
        return Color.yellow;
    }

    void ShowWindowMenu(Rect WindowRect)
    {
        GUIStyle style = GUI.skin.GetStyle("PaneOptions");
        Rect rect = new Rect(WindowRect.width - 20, 5, 20, 20);

        if (GUI.Button(rect, "", style))
        {
            Debug.LogWarning("ddds");
        }
    }
}
