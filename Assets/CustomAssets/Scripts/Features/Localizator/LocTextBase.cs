namespace MyTools.Localizator.Text
{
    using UnityEngine;
    using MyTools.Helpers;

    public abstract class LocTextBase : MonoValidate
    {
#pragma warning disable 649
        [SerializeField] [HideInInspector] string key;
#pragma warning restore 649

        private void OnEnable()
        {
            Localizator.OnLangChanged -= UpdateLoc;
            Localizator.OnLangChanged += UpdateLoc;
            UpdateLoc();
            Enable();
        }
        private void OnDisable()
        {
            Localizator.OnLangChanged -= UpdateLoc;
            Disable();
        }

        void UpdateLoc() { OnUpdateLoc(Localizator.GetLocalizedValue(this.key)); }

        protected virtual void Enable() { }
        protected virtual void Disable() { }
        protected abstract void OnUpdateLoc(string str);
    }

#if UNITY_EDITOR
    namespace Editor
    {
        using UnityEditor;
        [CustomEditor(typeof(LocTextBase), true)]

        public class LocTextBaseEditor : Editor
        {

            const string label = "Translations:";
            bool showFoldOut = false;
            SerializedProperty keySerProp;

            private void OnEnable()
            {
                keySerProp = this.serializedObject.FindProperty("key");
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                EditorGUILayout.Space();
                DrawKey();
                DrawTranslations();
            }

            void DrawKey()
            {
                EditorGUILayout.PropertyField(keySerProp);
                serializedObject.ApplyModifiedProperties();
            }

            void DrawTranslations()
            {
                showFoldOut = EditorGUILayout.Foldout(showFoldOut, label);
                if (showFoldOut)
                {
                    //TODO доделать локализатор
                }
            }
        }
    }
#endif
}
