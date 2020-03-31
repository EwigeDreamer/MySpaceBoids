using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
using MyTools.Extensions.Rects.Editor;
#endif

namespace MyTools.SceneManagement
{
    public class SceneNameAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneNameAttribute))]
    public class SceneNameDrawer : PropertyDrawer
    {
        //TODO сделать аттрибут, который закеширует имя сцены

        //SceneIndexAttribute SiAttribute { get { return (SceneIndexAttribute)attribute; } }
        //float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
        //float LineSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }
        //float LabelWidth { get { return EditorGUIUtility.labelWidth; } }

        //GUIStyle m_RichStyle = new GUIStyle(GUI.skin.label) { richText = true };

        //private class SerializedInt
        //{
        //    SerializedProperty serProp = null;
        //    public int Int
        //    {
        //        get { return serProp != null ? serProp.intValue : 0; }
        //        set { if (serProp != null && serProp.intValue != value) { serProp.intValue = value; serProp.serializedObject.ApplyModifiedProperties(); } }
        //    }
        //    public SerializedInt(SerializedProperty property) { serProp = property; }
        //}

        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    //if (property.propertyType == SerializedPropertyType.String)
        //    //    return LineHeight * 2f + LineSpacing;
        //    return LineHeight;
        //}

        //public override void OnGUI(Rect rect, SerializedProperty serProp, GUIContent label)
        //{
        //    if (serProp.propertyType != SerializedPropertyType.Integer)
        //    {
        //        EditorGUI.LabelField(EditorGUI.IndentedRect(rect), "Use ResourcesPath attribute with string");
        //        return;
        //    }


        //    int indentLevel = EditorGUI.indentLevel;
        //    EditorGUI.indentLevel = 0;

        //    SerializedInt indexProp = new SerializedInt(serProp);

        //    int sceneCount = SceneManager.sceneCountInBuildSettings;
        //    List<string> sceneNameList = new List<string>(sceneCount);
        //    for (int i = 0; i < sceneCount; ++i)
        //        sceneNameList.Add(string.Format("[{0}] {1}", i, NameFromIndex(i)));



        //    bool warning = false;
        //    if (indexProp.Int < 0 || indexProp.Int > sceneCount - 1)
        //    {
        //        indexProp.Int = sceneCount;
        //        sceneNameList.Add("Invalid index");
        //        warning = true;
        //    }
            
        //    string labelStr = warning ? string.Format("<color=red><b>{0}</b></color>", label.text) : label.text;
        //    rect.GetIndentLabelFieldPair(indentLevel, out var popupLabel, out var popupField);
        //    EditorGUI.LabelField(popupLabel, labelStr, m_RichStyle);
        //    indexProp.Int = EditorGUI.Popup(popupField, indexProp.Int, sceneNameList.ToArray());

        //    EditorGUI.indentLevel = indentLevel;
        //}

        //private static string NameFromIndex(int BuildIndex)
        //{
        //    string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        //    int slash = path.LastIndexOf('/');
        //    string name = path.Substring(slash + 1);
        //    int dot = name.LastIndexOf('.');
        //    return name.Substring(0, dot);
        //}
    }
#endif
}
