using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseButton
{
    None = -1,
    Left = 0,
    Right = 1,
    Middle = 2,

};

public enum MouseEventType
{
    None = -1,
    Down = EventType.MouseDown,
    Up = EventType.MouseUp,
    Move = EventType.MouseMove,
    Drag = EventType.MouseDrag,
    Scroll = EventType.ScrollWheel,
    ContextClick = EventType.ContextClick,
}

public class MouseData
{
    public bool IsDown(MouseButton btn)
    {
        return (button == btn && type == MouseEventType.Down);
    }
    public bool IsDrag(MouseButton btn)
    {
        return (button == btn && type == MouseEventType.Drag);
    }
    public bool IsUp(MouseButton btn)
    {
        return (button == btn && type == MouseEventType.Up);
    }
    public bool IsScroll()
    {
        return (type == MouseEventType.Scroll);
    }
    public bool IsContextClick()
    {
        return (type == MouseEventType.ContextClick);
    }
    // コピー
    public MouseData Clone()
    {
        return (MouseData)MemberwiseClone();
    }

    /// ボタン
    public MouseButton button;
    /// イベントタイプ
    public MouseEventType type;
    /// 位置
    public Vector2 pos;
    /// 移動量
    public Vector2 delta;

    public float zoom = 1.0f;
    /// マウス判定
    public Rect rect
    {
        get
        {
            return new Rect(pos - new Vector2(5, 5) * zoom, new Vector2(10, 10) * zoom);
        }
    }
};
