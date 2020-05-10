using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowNodeStyle
{
    public SerializedObject serializedObject;

    public static SerializedObject GetEditor(BaseNode target)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        return serializedObject;
    }

    //public override void OnInspectorGUI()
    //{
    //    // Grab the latest data from the object
    //    //从对象抓取的最新数据
    //    serializedObject.Update();

    //    SerializedProperty iterator = serializedObject.GetIterator();
    //    Debug.LogWarning("输出数量:" + serializedObject.targetObjects.Length);
    //    bool enterChildren = true;
    //    EditorGUIUtility.labelWidth = 84;
    //    while (iterator.NextVisible(enterChildren))
    //    {
    //        enterChildren = false;
    //        EditorGUILayout.PropertyField(iterator, true);
    //    }
    //    serializedObject.ApplyModifiedProperties();
    //}


}
