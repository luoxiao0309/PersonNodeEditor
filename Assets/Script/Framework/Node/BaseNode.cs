using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseNode
{
    public int id;
    public DrawNode drawNode;
    public Rect windowRect;
    public string windowTitle;

    [SerializeField]
    public List<int> childNodes = new List<int>();
    //父节点.
    public int ParentNode = 0;

    public void DrawWindow()
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this);
        }
    }

    public void DrawCurve()
    {
        if (drawNode != null)
        {
            drawNode.DrawCurve(this);
        }
    }
}
