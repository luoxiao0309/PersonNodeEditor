using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventExtension
{
    public static bool IsMouseLeftClick(this Event e)
    {
        if (e.button == (int)MouseEnum.LeftClick)
        {
            return true;
        }
        return false;
    }

    public static bool IsMouseRightClick(this Event e)
    {
        if (e.button == (int)MouseEnum.RightClick)
        {
            return true;
        }
        return false;
    }

    public static bool IsMouseCenterClick(this Event e)
    {
        if (e.button == (int)MouseEnum.CenterClick)
        {
            return true;
        }
        return false;
    }

    public static bool IsMouseDownClick(this Event e)
    {
        if (e.type == EventType.MouseDown)
        {
            return true;
        }
        return false;
    }



    public static bool IsMouseUpClick(this Event e)
    {
        if (e.type == EventType.MouseUp)
        {
            return true;
        }
        return false;
    }

    public static bool IsMouseDragClick(this Event e)
    {
        if (e.type == EventType.MouseDrag)
        {
            return true;
        }
        return false;
    }
}
