using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AmazingNodeEditor
{
    public static class GridDrawer 
    {
        public static void DrawGrid(float gridSpacing, float gridOpacity, Rect rect, ref Vector2 offset, ref Vector2 drag)
        {
            int widthDivs = Mathf.CeilToInt(rect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(rect.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(EditorConfig.gridColor.r, EditorConfig.gridColor.g, EditorConfig.gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            var newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; ++i)
            {
                var beg = new Vector3(gridSpacing * i, -gridSpacing, 0f) + newOffset;
                var end = new Vector3(gridSpacing * i, rect.height, 0f) + newOffset;
                Handles.DrawLine(beg, end);
            }

            for (int j = 0; j < heightDivs; ++j)
            {
                var beg = new Vector3(-gridSpacing, gridSpacing * j, 0f) + newOffset;
                var end = new Vector3(rect.width, gridSpacing * j, 0f) + newOffset;
                Handles.DrawLine(beg, end);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}
