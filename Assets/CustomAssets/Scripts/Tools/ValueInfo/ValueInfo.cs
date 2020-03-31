using System;
using UnityEngine;

namespace MyTools.ValueInfo
{
    [Serializable]
    public struct IntInfo
    {
        public int min;
        public int max;
        public int value;

        public IntInfo(int value)
        {
            if (value != 0)
            {
                bool positive = value > 0;
                min = positive ? 0 : value;
                max = positive ? value : 0;
            }
            else
            {
                min = 0;
                max = 1;
            }
            this.value = value;
        }

        public int Min { get => min; set => SetMin(value); }
        public int Max { get => max; set => SetMax(value); }
        public int Value { get => value; set => SetValue(value); }

        public float ValueToMaxRatio => max == 0 ? 0f : ((float)value) / max;
        public float Normalize => (float)value / (max - min);

        public bool IsMax => value == max;
        public bool IsMin => value == min;
        public bool IsZero => value == 0;
        public void ToMin() => value = min;
        public void ToMax() => value = max;
        public void ToZero() => value = min = 0;

        public void SetMin(int min)
        {
            if (min > max) min = max;
            this.min = min;
            if (value < this.min) value = this.min;
        }
        public void SetMax(int max)
        {
            if (max < min) max = min;
            this.max = max;
            if (value > this.max) value = this.max;
        }
        public void SetValue(int value)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            this.value = value;
        }

        public static implicit operator int(IntInfo info) => info.value;
        public static IntInfo operator +(IntInfo a, int b) { a.Value += b; return a; }
        public static IntInfo operator +(int a, IntInfo b) { b.Value += a; return b; }
        public static IntInfo operator -(IntInfo a, int b) { a.Value -= b; return a; }
        public static int operator -(int a, IntInfo b) { return a - b.Value; }
    }


    [Serializable]
    public struct FloatInfo
    {
        public float min;
        public float max;
        public float value;

        public FloatInfo(float value)
        {
            bool positive = value > 0;
            min = positive ? 0 : value;
            max = positive ? value : 0;
            this.value = value;
        }

        public float Min { get => min; set => SetMin(value); }
        public float Max { get => max; set => SetMax(value); }
        public float Value { get => value; set => SetValue(value); }

        public float ValueToMaxRatio => max.IsVerySmall() ? 0f : value / max;
        public float Normalize => value / (max - min);

        public bool IsMax => (value - max).IsVerySmall();
        public bool IsMin => (value - min).IsVerySmall();
        public bool IsZero => value.IsVerySmall();
        public void ToMin() => value = min;
        public void ToMax() => value = max;
        public void ToZero() => value = min = 0f;

        public void SetMin(float min)
        {
            if (min > max) min = max;
            this.min = min;
            if (value < this.min) value = this.min;
        }
        public void SetMax(float max)
        {
            if (max < min) max = min;
            this.max = max;
            if (value > this.max) value = this.max;
        }
        public void SetValue(float value)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            this.value = value;
        }

        public static implicit operator float(FloatInfo info) => info.value;
        public static FloatInfo operator +(FloatInfo a, float b) { a.Value += b; return a; }
        public static FloatInfo operator +(float a, FloatInfo b) { b.Value += a; return b; }
        public static FloatInfo operator -(FloatInfo a, float b) { a.Value -= b; return a; }
        public static float operator -(float a, FloatInfo b) { return a - b.Value; }
    }

    public static class ExtensionMethods
    {
        public static bool IsVerySmall(this float value) => value < 1e-5f && value > -1e-5f;
    }

#if UNITY_EDITOR
    namespace Editor
    {
        using UnityEditor;
        using MyTools.Extensions.Rects;
        using MyTools.Extensions.Editor;

        [CustomPropertyDrawer(typeof(IntInfo))]
        public class IntInfoDrawer : PropertyDrawer
        {
            float LineHeight => EditorGUIUtility.singleLineHeight;
            float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
            float LabelWidth => EditorGUIUtility.labelWidth;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                int lines = 2;
                return (LineHeight * lines) + (LineSpacing * (lines - 1));
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.GetCustomLabelFieldPair(LabelWidth, out var l, out var f);
                GUI.Box(f, "");
                l.GetRowsNonAlloc(LineSpacing, out var l1, out var l2);
                EditorGUI.LabelField(l1, label);
                f.GetRowsNonAlloc(LineSpacing, out var f1, out var f2);
                f1.GetColumnsNonAlloc(LineSpacing, out var f11, out var f12, out var f13);
                var minProp = property.FindPropertyRelative("min");
                var maxProp = property.FindPropertyRelative("max");
                var valueProp = property.FindPropertyRelative("value");

                float labWidthTmp = EditorGUIUtility.labelWidth;

                EditorGUI.ProgressBar(f2, valueProp.intValue / (float)(maxProp.intValue - minProp.intValue), label.text);

                EditorGUIUtility.labelWidth = 23f;
                EditorGUI.PropertyField(f11, minProp);
                EditorGUIUtility.labelWidth = 37f;
                //EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(f12, valueProp);
                //EditorGUI.EndDisabledGroup();
                EditorGUIUtility.labelWidth = 28f;
                EditorGUI.PropertyField(f13, maxProp);

                property.serializedObject.ApplyModifiedProperties();

                EditorGUIUtility.labelWidth = labWidthTmp;

                EditorGUI.indentLevel = indentLevel;
            }
        }

        [CustomPropertyDrawer(typeof(FloatInfo))]
        public class FloatInfoDrawer : PropertyDrawer
        {
            float LineHeight => EditorGUIUtility.singleLineHeight;
            float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
            float LabelWidth => EditorGUIUtility.labelWidth;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                int lines = 2;
                return (LineHeight * lines) + (LineSpacing * (lines - 1));
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.GetCustomLabelFieldPair(LabelWidth, out var l, out var f);
                GUI.Box(f, "");
                l.GetRowsNonAlloc(LineSpacing, out var l1, out var l2);
                EditorGUI.LabelField(l1, label);
                f.GetRowsNonAlloc(LineSpacing, out var f1, out var f2);
                f1.GetColumnsNonAlloc(LineSpacing, out var f11, out var f12, out var f13);
                var minProp = property.FindPropertyRelative("min");
                var maxProp = property.FindPropertyRelative("max");
                var valueProp = property.FindPropertyRelative("value");

                float labWidthTmp = EditorGUIUtility.labelWidth;

                EditorGUI.ProgressBar(f2, valueProp.floatValue / (maxProp.floatValue - minProp.floatValue), label.text);

                EditorGUIUtility.labelWidth = 23f;
                EditorGUI.PropertyField(f11, minProp);
                EditorGUIUtility.labelWidth = 37f;
                //EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(f12, valueProp);
                //EditorGUI.EndDisabledGroup();
                EditorGUIUtility.labelWidth = 28f;
                EditorGUI.PropertyField(f13, maxProp);

                property.serializedObject.ApplyModifiedProperties();

                EditorGUIUtility.labelWidth = labWidthTmp;

                EditorGUI.indentLevel = indentLevel;
            }
        }
    }
#endif
}
