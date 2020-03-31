using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools.Singleton;
using MyTools.Extensions.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using MyTools.Extensions.Reflection;
using System.Reflection;
#endif

namespace MyTools.Helpers
{
    public abstract class MonoValidate : ImprovedBehaviour
    {
        protected virtual void OnValidate() { }
        protected bool ValidateFind<T>(ref T field, bool includeInactive = false) where T : UnityEngine.Object
        { if (field == null) field = gameObject.scene.FindObjectOfType<T>(includeInactive); return field != null; }
        protected bool ValidateGetComponent<T>(ref T field, bool forced = false) where T : class
        { if (field == null || forced) field = GetComponent<T>(); return field != null; }
        protected bool ValidateGetComponentInParent<T>(ref T field, bool forced = false) where T : class
        { if (field == null || forced) field = GetComponentInParent<T>(); return field != null; }
        protected bool ValidateGetComponentInChildren<T>(ref T field, bool forced = false, bool includeInactive = false) where T : class
        { if (field == null || forced) field = GetComponentInChildren<T>(includeInactive); return field != null; }

        protected bool ValidateGetComponents<T>(ref T[] collection) where T : class
        { collection = GetComponents<T>(); return collection.Length > 0; }
        protected bool ValidateGetComponents<T>(ref List<T> collection) where T : class
        { if (collection == null) collection = new List<T>(); GetComponents(collection); return collection.Count > 0; }
        protected bool ValidateGetComponentsInParent<T>(ref T[] collection, bool includeInactive = false) where T : class
        { collection = GetComponentsInParent<T>(includeInactive); return collection.Length > 0; }
        protected bool ValidateGetComponentsInParent<T>(ref List<T> collection, bool includeInactive = false) where T : class
        { if (collection == null) collection = new List<T>(); GetComponentsInParent(includeInactive, collection); return collection.Count > 0; }
        protected bool ValidateGetComponentsInChildren<T>(ref T[] collection, bool includeInactive = false) where T : class
        { collection = GetComponentsInChildren<T>(includeInactive); return collection.Length > 0; }
        protected bool ValidateGetComponentsInChildren<T>(ref List<T> collection, bool includeInactive = false) where T : class
        { if (collection == null) collection = new List<T>(); GetComponentsInChildren(includeInactive, collection); return collection.Count > 0; }
    }

#if UNITY_EDITOR
    namespace Inspector
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(MonoValidate), true)]
        public class MonoValidateEditor : Editor
        {
            const string buttonLabel = "Invoke OnValidate";
            const string undoName = "invoke_onvalidate";
            const string methodName = "OnValidate";
            public override void OnInspectorGUI()
            {
                var rect = EditorGUILayout.GetControlRect(false);
                if (GUI.Button(rect, buttonLabel))
                {
                    Undo.RecordObjects(targets, undoName);
                    foreach (var tgt in targets) tgt.InvokePrivateMethod(methodName);
                }
                DrawDefaultInspector();
            }
        }
    }
#endif
}