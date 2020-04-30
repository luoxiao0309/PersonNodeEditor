using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AmazingNodeEditor
{
    public static class EditorConfig
    {
        public const string defaultSkinPath = "builtin skins/darkskin/images/node1.png";
        public const string defaultActiveSkinPath = "builtin skins/darkskin/images/node1 on.png";
        public const string defaultInPointSkinPath = "builtin skins/darkskin/images/btn left.png";
        public const string defaultInPointActiveSkinPath = "builtin skins/darkskin/images/btn left on.png";
        public const string defaultOutPointSkinPath = "builtin skins/darkskin/images/btn right.png";
        public const string defaultOutPointActiveSkinPath = "builtin skins/darkskin/images/btn right on.png";
        public const int defaultNodeSize = 12;
        public const int defaultPointSize = 4;

        public const float smallGridSpacing = 20f;
        public const float largeGridSpacing = 100f;
        public const float smallGridOpacity = 0.2f;
        public const float largeGridOpacity = 0.4f;

        public static Color gridColor;

        //public static NodeStyleInfo CreateDefaultNodeStyle()
        //{
        //    var defaultNodeStyle = new NodeStyleInfo();

        //    defaultNodeStyle.defaultNodeStyle = new GUIStyle();
        //    defaultNodeStyle.defaultNodeStyle.normal.background = EditorGUIUtility.Load(defaultSkinPath) as Texture2D;
        //    defaultNodeStyle.defaultNodeStyle.border = new RectOffset(defaultNodeSize, defaultNodeSize, defaultNodeSize, defaultNodeSize);

        //    defaultNodeStyle.selectedNodeStyle = new GUIStyle();
        //    defaultNodeStyle.selectedNodeStyle.normal.background = EditorGUIUtility.Load(defaultSkinPath) as Texture2D;
        //    defaultNodeStyle.selectedNodeStyle.border = new RectOffset(defaultNodeSize, defaultNodeSize, defaultNodeSize, defaultNodeSize);

        //    defaultNodeStyle.inPointStyle = new GUIStyle();
        //    defaultNodeStyle.inPointStyle.normal.background = EditorGUIUtility.Load(defaultInPointSkinPath) as Texture2D;
        //    defaultNodeStyle.inPointStyle.active.background = EditorGUIUtility.Load(defaultInPointActiveSkinPath) as Texture2D;
        //    defaultNodeStyle.inPointStyle.border = new RectOffset(defaultPointSize, defaultPointSize, defaultNodeSize, defaultNodeSize);

        //    defaultNodeStyle.outPointStyle = new GUIStyle();
        //    defaultNodeStyle.outPointStyle.normal.background = EditorGUIUtility.Load(defaultOutPointSkinPath) as Texture2D;
        //    defaultNodeStyle.outPointStyle.active.background = EditorGUIUtility.Load(defaultOutPointActiveSkinPath) as Texture2D;
        //    defaultNodeStyle.outPointStyle.border = new RectOffset(defaultPointSize, defaultPointSize, defaultNodeSize, defaultNodeSize);

        //    return defaultNodeStyle;
        //}
    }
}
