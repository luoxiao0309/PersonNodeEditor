using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseNode))]
public class NodeStyle : Editor
{
    void OnEnable()
    {
       
    }

    public override void OnInspectorGUI()
    {
        // Grab the latest data from the object
        //从对象抓取的最新数据
        serializedObject.Update();

        SerializedProperty iterator = serializedObject.GetIterator();
        Debug.LogWarning("输出数量:"+ serializedObject.targetObjects.Length);
        bool enterChildren = true;
        EditorGUIUtility.labelWidth = 84;
        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;
            EditorGUILayout.PropertyField(iterator, true);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
