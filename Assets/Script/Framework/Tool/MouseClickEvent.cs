using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标点击事件集合.
/// </summary>
public class MouseClickEvent
{
    private enum MouseEnum
    {
        /// <summary>
        /// 左键点击
        /// </summary>
        LeftClick = 0,
        /// <summary>
        /// 鼠标中键
        /// </summary>
        CenterClick = 2,
        /// <summary>
        /// 鼠标右键
        /// </summary>
        RightClick = 1
    }
    
    /// <summary>
    /// 鼠标中键点击
    /// </summary>
    /// <param name="e"></param>
    /// <param name="action"></param>
    public static void MouseCenterClick(Event e, Action<Event> action)
    {
        if (e.button == (int)MouseEnum.CenterClick)
        {
            action(e);
        }
    }

    /// <summary>
    /// 鼠标左键点击
    /// </summary>
    /// <param name="e"></param>
    /// <param name="action"></param>
    public static void MouseLeftClick(Event e, Action<Event> action)
    {
        if (e.button == (int)MouseEnum.LeftClick)
        {
            action(e);
        }
    }

    /// <summary>
    /// 鼠标右键点击
    /// </summary>
    /// <param name="e"></param>
    /// <param name="action"></param>
    public static void MouseRightClick(Event e, Action<Event> action)
    {
        if (e.button == (int)MouseEnum.RightClick)
        {
            action(e);
        }
    }

    /// <summary>
    /// 鼠标按下事件
    /// </summary>
    /// <param name="e"></param>
    /// <param name="action"></param>
    public static void MouseDownEvent(Event e,Action<Event> action)
    {
        if (e.type == EventType.MouseDown)
        {
            action(e);
        }
    } 

    /// <summary>
    /// 鼠标拖拽事件
    /// </summary>
    /// <param name="e"></param>
    /// <param name="action"></param>
    public static void MouseDragEvent(Event e, Action<Event> action)
    {
        if (e.type == EventType.MouseDrag)
        {
            action(e);
        }
    }
}
