
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UNEB
{
    /// <summary>
    /// The visual representation of a logic unit such as an object or function.
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        public static readonly Vector2 kDefaultSize = new Vector2(140f, 110f);

        /// <summary>
        /// The space reserved between knobs.
        /// </summary>
        public const float kKnobOffset = 4f;

        /// <summary>
        /// The space reserved for the header (title) of the node.
        /// </summary>
        public const float kHeaderHeight = 15f;

        /// <summary>
        /// The max label width for a field in the body.
        /// </summary>
        public const float kBodyLabelWidth = 100f;

        /// <summary>
        /// The rect of the node in canvas space.
        /// </summary>
        [HideInInspector]
        public Rect bodyRect;

        /// <summary>
        /// How much additional offset to apply when resizing.
        /// </summary>
        public const float resizePaddingX = 20f;
        
        /// <summary>
        /// Use this for initialization.
        /// </summary>
        public virtual void Init()
        {
            bodyRect.size = kDefaultSize;
        }
        
        /// <summary>
        /// Render the title/header of the node. By default, renders on top of the node.
        /// </summary>
        public virtual void OnNodeHeaderGUI()
        {
            // Draw header
            GUILayout.Box(name, HeaderStyle);
        }

        /// <summary>
        /// Draws the body of the node. By default, after the connections.
        /// </summary>
        public virtual void OnBodyGUI() { }

        // Handles the coloring and layout of the body.
        // This is for convenience so the user does not need to worry about this boiler plate code.
        protected virtual void onBodyGuiInternal()
        {
            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = kBodyLabelWidth;

            // Cache the old label style.
            // Do this first before changing the EditorStyles.label style.
            // So the original values are kept.
            var oldLabelStyle = UnityLabelStyle;
            
            EditorGUILayout.BeginVertical();

            GUILayout.Space(kKnobOffset);
            OnBodyGUI();

            // Revert back to old label style.
            EditorStyles.label.normal = oldLabelStyle.normal;
            EditorStyles.label.active = oldLabelStyle.active;
            EditorStyles.label.focused = oldLabelStyle.focused;

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUILayout.EndVertical();
        }
        
        /// <summary>
        /// Get the Y value of the top header.
        /// </summary>
        public float HeaderTop
        {
            get { return bodyRect.yMin + kHeaderHeight; }
        }
        
        #region Styles and Contents

        private static GUIStyle _unityLabelStyle;

        /// <summary>
        /// Caches the default EditorStyle.
        /// There is a strange bug with it being overriden when opening an Animation window.
        /// </summary>
        public static GUIStyle UnityLabelStyle
        {
            get
            {
                if (_unityLabelStyle == null)
                {
                    _unityLabelStyle = new GUIStyle(EditorStyles.label);
                }

                return _unityLabelStyle;
            }
        }
        

        private static GUIStyle _headerStyle;
        public GUIStyle HeaderStyle
        {
            get
            {
                if (_headerStyle == null)
                {

                    _headerStyle = new GUIStyle();

                    _headerStyle.stretchWidth = true;
                    _headerStyle.alignment = TextAnchor.MiddleLeft;
                    _headerStyle.padding.left = 5;
                    _headerStyle.normal.textColor = Color.white * 0.9f;
                    _headerStyle.fixedHeight = kHeaderHeight;
                }

                return _headerStyle;
            }
        }

        #endregion
    }
}