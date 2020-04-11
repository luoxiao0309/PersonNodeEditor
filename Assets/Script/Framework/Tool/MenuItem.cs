using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMenuItem
{
    public GUIContent content;
    public bool on;
    public Action func1;
    public Action<object> func2;
    public object userData;

    public MouseMenuItem(GUIContent content, bool on, Action func1)
    {
        this.content = content;
        this.on = on;
        this.func1 = func1;
    }

    public MouseMenuItem(GUIContent content, bool on, Action<object> func2)
    {
        this.content = content;
        this.on = on;
        this.func2 = func2;
    }

    public static MouseMenuItem CreateMenuItem(GUIContent content, bool on,Action func1)
    {
        return new MouseMenuItem(content,on,func1);
    }

    public static MouseMenuItem CreateMenuItem(GUIContent content, bool on, Action<object> func2)
    {
        return new MouseMenuItem(content, on, func2);
    }
}
