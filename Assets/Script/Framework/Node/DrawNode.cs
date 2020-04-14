using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DrawNode : ScriptableObject
{
    public abstract void DrawWindow(BaseNode b);
    public abstract void DrawCurve(BaseNode b);
}
