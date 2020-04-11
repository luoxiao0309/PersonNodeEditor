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
    public int ParentNode = 0;
}
