using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CalcNode : BaseNode
{
    private BaseNode input1;
    private Rect input1Rect;

    private BaseNode input2;
    private Rect input2Rect;

    private CalculationType calculationType;

    public enum CalculationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public override void DrawWindow()
    {
        Event e = Event.current;

        calculationType = (CalculationType)EditorGUILayout.EnumPopup("Calculation Type", calculationType);

        string input1Title = "None";

        if (input1)
        {
            //input1Title = input1.getResult();
            input1Title = "dss";
        }

        GUILayout.Label("Input 1: " + input1Title);

        if (e.type == EventType.Repaint)
        {
            input1Rect = GUILayoutUtility.GetLastRect();
        }
        
        string input2Title = "None";

        if (input2)
        {
            input2Title = "dddd";
        }

        GUILayout.Label("Input 2: " + input2Title);

        if (e.type == EventType.Repaint)
        {
            input2Rect = GUILayoutUtility.GetLastRect();
        }
    }

    public void SetInput(BaseNode input, Vector2 clickPos)
    {
        clickPos.x -= WindowRect.x;
        clickPos.y -= WindowRect.y;

        if (input1Rect.Contains(clickPos))
        {
            input1 = input;

        }
        else if (input2Rect.Contains(clickPos))
        {
            input2 = input;
        }
    }

    public override void DrawCurve()
    {
        if (input1)
        {
            Rect rect = WindowRect;
            rect.x += input1Rect.x;
            rect.y += input1Rect.y + input2Rect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeTool.DrawNodeCurve(input1.WindowRect, rect, Color.blue);
        }

        if (input2)
        {
            Rect rect = WindowRect;
            rect.x += input2Rect.x;
            rect.y += input2Rect.y + input2Rect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeTool.DrawNodeCurve(input2.WindowRect, rect, Color.blue);
        }
    }
}
