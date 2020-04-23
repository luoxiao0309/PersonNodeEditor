using UNEB;
using UnityEngine;

public class CurveNode : Node
{
    private AnimationCurve _curve = new AnimationCurve();
    private readonly Rect kCurveRange = new Rect(-1, -1, 2, 2);

    private const float kBodyHeight = 100f;

    public override void Init()
    {
        bodyRect.height += kBodyHeight;
        bodyRect.width = 150f;
    }

    public override void OnBodyGUI()
    {
       
    }
}