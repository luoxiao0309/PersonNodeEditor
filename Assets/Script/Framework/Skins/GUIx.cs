using UnityEngine;

namespace NodeSystem
{
    public class GUIx : ScriptableObject
    {
        //public static string skinPath = "Assets/Resources/";
        public GUISkin skin;

        static GUIx backingField_I;
        public static GUIx I
        {
            get
            {
                if (backingField_I)
                    return backingField_I;
                else
                {
                    return Resources.Load<GUIx>("GUI/GUIx");
                }
            }
            set { backingField_I = value; }
        }

        void Awake()
        {
            //skin = Resources.Load<GUISkin>("GUISkin");
        }

        public Color nodeColor;
        public Color nodeSelectedColor;
        public Color nodeHighlightColor;

        //styles example
        //use: GUIx.I.toggle
        public GUIStyle toggleStyle { get { return skin.GetStyle("Toggle"); } }
        public GUIStyle nodeStyle { get { return skin.GetStyle("Node"); } }
        public GUIStyle socketStyle { get { return skin.GetStyle("Socket"); } }
        public GUIStyle connectionStyle { get { return skin.GetStyle("Connection"); } }
        public GUIStyle window { get { return skin.GetStyle("Window"); } }
        public GUIStyle background { get { return skin.GetStyle("Background"); } }


        //simplyfy new GUIContent("");
        //use: GUIx.empty
        public static GUIContent empty = new GUIContent("");


        //calculate GUIRect size
        static public Vector2 GUIContentSize(GUIStyle guiStyle, GUIContent content)
        {
            return guiStyle.CalcSize(content);
        }        
    }
}
