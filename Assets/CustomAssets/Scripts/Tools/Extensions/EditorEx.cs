#if UNITY_EDITOR
namespace MyTools.Extensions.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;
    using System.Linq;
    public static class EditorEx
    {
        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        public static object GetTargetObjectOfProperty(this SerializedProperty prop)
        {
            if (prop == null) return null;
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else obj = GetValue_Imp(obj, element);
            }
            return obj;
        }
        /// <summary>
        /// Center window on main editor window
        /// </summary>
        public static void CenterOnMainWin(this UnityEditor.EditorWindow aWin)
        {
            var main = GetEditorMainWindowPos();
            var pos = aWin.position;
            float w = (main.width - pos.width) * 0.5f;
            float h = (main.height - pos.height) * 0.5f;
            pos.x = main.x + w;
            pos.y = main.y + h;
            aWin.position = pos;
        }

        #region INTERNAL
        static object GetValue_Imp(object source, string name)
        {
            if (source == null) return null;
            var type = source.GetType();
            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null) return f.GetValue(source);
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null) return p.GetValue(source, null);
                type = type.BaseType;
            }
            return null;
        }
        static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
                if (!enm.MoveNext()) return null;
            return enm.Current;
        }
        #endregion

        #region idiotskie kostyli =\
        static System.Type[] GetAllDerivedTypes(this System.AppDomain aAppDomain, System.Type aType)
        {
            var result = new List<System.Type>();
            var assemblies = aAppDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(aType))
                        result.Add(type);
                }
            }
            return result.ToArray();
        }
        static Rect GetEditorMainWindowPos()
        {
            var containerWinType = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).Where(t => t.Name == "ContainerWindow").FirstOrDefault();
            if (containerWinType == null)
                throw new System.MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
            var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (showModeField == null || positionProperty == null)
                throw new System.MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
            var windows = Resources.FindObjectsOfTypeAll(containerWinType);
            foreach (var win in windows)
            {
                var showmode = (int)showModeField.GetValue(win);
                if (showmode == 4) // main window
                {
                    var pos = (Rect)positionProperty.GetValue(win, null);
                    return pos;
                }
            }
            throw new System.NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
        }
        #endregion

        public static void DrawFoldoutEditor(this Object obj, System.Action onChange, ref bool foldout)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, obj);
                if (foldout)
                {
                    var editor = Editor.CreateEditor(obj);
                    editor.OnInspectorGUI();
                    if (check.changed) onChange?.Invoke();
                }
            }
        }
    }
}
#endif