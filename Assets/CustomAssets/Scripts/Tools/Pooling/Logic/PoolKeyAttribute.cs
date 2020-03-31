using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
#endif

namespace MyTools.Pooling
{
    public interface IPoolKeys { string[] Keys { get; } }
    [System.Serializable]
    public class MyPoolKey
    {
# pragma warning disable 649
        [SerializeField] [PoolKey] string m_Key;
#pragma warning restore 649
        public string Key { get { return m_Key; } }
    }

    public class PoolKeyAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PoolKeyAttribute))]
    public class PoolKeyDrawer : PropertyDrawer
    {
        PoolKeyAttribute PkAttribute { get { return (PoolKeyAttribute)attribute; } }
        float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
        float LineSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }
        float LabelWidth { get { return EditorGUIUtility.labelWidth; } }

        List<IPoolKeys> m_IPoolKeysList = new List<IPoolKeys>();

        private class SerializedString
        {
            SerializedProperty serProp = null;
            public string String
            {
                get { return serProp != null ? serProp.stringValue : null; }
                set { if (serProp != null && serProp.stringValue != value) { serProp.stringValue = value; serProp.serializedObject.ApplyModifiedProperties(); } }
            }
            public SerializedString(SerializedProperty property) { serProp = property; }
        }

        public override void OnGUI(Rect rect, SerializedProperty serProp, GUIContent label)
        {
            var hasLabel = !string.IsNullOrWhiteSpace(label.text);
            if (serProp.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(EditorGUI.IndentedRect(rect), "Use PoolKey attribute with string");
                return;
            }
            if (m_IPoolKeysList.Count < 1)
                m_IPoolKeysList = Object.FindObjectsOfType<MonoBehaviour>().OfType<IPoolKeys>().ToList();

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect label1, field1;
            if (hasLabel)
                GetRectsPair(rect, indentLevel, out label1, out field1);
            else
            {
                label1 = rect;
                label1.width = 0f;
                field1 = rect;
            }

            //if (m_IPoolKeysList.Count < 1)
            //    EditorGUI.LabelField(EditorGUI.IndentedRect(rect), "There is no PoolKeys");
            SerializedString serString = new SerializedString(serProp);
            string curKey = serString.String;
            List<string> keyList = new List<string>();
            keyList.Add("[none]");
            foreach (var poolKeys in m_IPoolKeysList)
                keyList.AddRange(poolKeys.Keys);
            keyList = keyList.Distinct().Where(o => !string.IsNullOrEmpty(o)).ToList();
            int index = string.IsNullOrEmpty(curKey) ? 0 : -1;
            int count = keyList.Count;
            for (int i = 1; i < count; ++i)
                if (keyList[i] == curKey)
                {
                    index = i;
                    break;
                }
            if (index < 0)
            {
                keyList.Add(curKey);
                index = keyList.Count - 1;
            }
            if (hasLabel)
            {
                if (m_IPoolKeysList.Count > 0)
                    EditorGUI.LabelField(label1, label);
                else
                {
                    GUIStyle warningStyle = new GUIStyle(GUI.skin.label);
                    warningStyle.fontStyle = FontStyle.Bold;
                    warningStyle.normal.textColor = Color.red;
                    EditorGUI.LabelField(label1, "No pools in scene!", warningStyle);
                }
                int newIndex = EditorGUI.Popup(field1, index, keyList.ToArray());
                if (newIndex != index)
                {
                    if (newIndex == 0)
                        serString.String = string.Empty;
                    else
                        serString.String = keyList[newIndex];
                }
            }
            else
            {
                if (m_IPoolKeysList.Count > 0)
                {
                    int newIndex = EditorGUI.Popup(field1, index, keyList.ToArray());
                    if (newIndex != index)
                    {
                        if (newIndex == 0)
                            serString.String = string.Empty;
                        else
                            serString.String = keyList[newIndex];
                    }
                }
                else
                {
                    GUIStyle warningStyle = new GUIStyle(GUI.skin.label);
                    warningStyle.fontStyle = FontStyle.Bold;
                    warningStyle.normal.textColor = Color.red;
                    EditorGUI.LabelField(rect, "No pools in scene!", warningStyle);
                }
            }

            EditorGUI.indentLevel = indentLevel;
        }
        void GetRectsPair(Rect source, int indentLevel, out Rect label, out Rect field)
        {
            int indTmp = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel;
            label = EditorGUI.IndentedRect(source);
            label.xMax = source.xMin + LabelWidth;
            field = source;
            field.xMin += LabelWidth;
            EditorGUI.indentLevel = indTmp;
        }
    }
#endif
}
