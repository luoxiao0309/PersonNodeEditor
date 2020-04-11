using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorHelper
{
    /// <summary>
    /// 在窗口中绘制子Windows窗口.
    /// </summary>
    /// <param name="editorWindow"></param>
    /// <param name="action"></param>
    public static void DrawChildWindow(this EditorWindow editorWindow, System.Action action)
    {
        editorWindow.BeginWindows();
        action();
        editorWindow.EndWindows();
    }

    public static void AddMenuItem(this GenericMenu genericMenu, MouseMenuItem menuItem)
    {
        if (menuItem.userData != null)
        {
            genericMenu.AddItem(menuItem.content, menuItem.on, () => {
                menuItem.func2(menuItem.userData);
            });
        }
        else
        {
            genericMenu.AddItem(menuItem.content, menuItem.on, () => {
                menuItem.func1();
            });
        }
    }
}
