namespace MyTools.Extensions.Rects
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using Random = UnityEngine.Random;
    public static class RectEx
    {
        public static void GetCustomLabelFieldPair(this Rect source, float labelWidth, out Rect label, out Rect field)
        {
            label = source;
            label.xMax -= source.width - labelWidth;
            field = source;
            field.xMin += labelWidth;
        }

        public static void GetRowsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2)
        {
            float l = (source.height - spacing) / 2f;
            float lsp = l + spacing;
            rect1 = source;
            rect1.yMax -= lsp;

            rect2 = source;
            rect2.yMin += lsp;
        }
        public static void GetRowsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3)
        {
            float l = (source.height - spacing * 2f) / 3f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            rect1 = source;
            rect1.yMax -= lsp2;

            rect2 = source;
            rect2.yMax -= lsp;
            rect2.yMin += lsp;

            rect3 = source;
            rect3.yMin += lsp2;
        }
        public static void GetRowsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4)
        {
            float l = (source.height - spacing * 3f) / 4f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            rect1 = source;
            rect1.yMax -= lsp3;

            rect2 = source;
            rect2.yMax -= lsp2;
            rect2.yMin += lsp;

            rect3 = source;
            rect3.yMax -= lsp;
            rect3.yMin += lsp2;

            rect4 = source;
            rect4.yMin += lsp3;
        }
        public static void GetRowsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4, out Rect rect5)
        {
            float l = (source.height - spacing * 4f) / 5f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            float lsp4 = lsp * 4f;
            rect1 = source;
            rect1.yMax -= lsp4;

            rect2 = source;
            rect2.yMax -= lsp3;
            rect2.yMin += lsp;

            rect3 = source;
            rect3.yMax -= lsp2;
            rect3.yMin += lsp2;

            rect4 = source;
            rect4.yMax -= lsp;
            rect4.yMin += lsp3;

            rect5 = source;
            rect5.yMin += lsp4;
        }
        public static void GetRowsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4, out Rect rect5, out Rect rect6)
        {
            float l = (source.height - spacing * 5f) / 6f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            float lsp4 = lsp * 4f;
            float lsp5 = lsp * 5f;
            rect1 = source;
            rect1.yMax -= lsp5;

            rect2 = source;
            rect2.yMax -= lsp4;
            rect2.yMin += lsp;

            rect3 = source;
            rect3.yMax -= lsp3;
            rect3.yMin += lsp2;

            rect4 = source;
            rect4.yMax -= lsp2;
            rect4.yMin += lsp3;

            rect5 = source;
            rect5.yMax -= lsp;
            rect5.yMin += lsp4;

            rect6 = source;
            rect6.yMin += lsp5;
        }
        public static void GetRowsNonAlloc<TColl>(this Rect source, TColl collection, int count, float spacing)
            where TColl : IList<Rect>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Clear();
            float l = (source.height - spacing * (count - 1)) / count;
            float lsp = l + spacing;
            Rect r;
            for (int i = 0; i < count; ++i)
            {
                r = source;
                r.yMax -= lsp * (count - 1 - i);
                r.yMin += lsp * i;
                collection.Add(r);
            }
        }
        public static void GetRowsNonAlloc<TColl>(this Rect source, TColl collection, float spacing)
            where TColl : IList<Rect>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            int count = collection.Count;
            float l = (source.height - spacing * (count - 1)) / count;
            float lsp = l + spacing;
            Rect r;
            for (int i = 0; i < count; ++i)
            {
                r = source;
                r.yMax -= lsp * (count - 1 - i);
                r.yMin += lsp * i;
                collection[i] = r;
            }
        }

        public static void GetColumnsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2)
        {
            float l = (source.width - spacing) / 2f;
            float lsp = l + spacing;
            rect1 = source;
            rect1.xMax -= lsp;

            rect2 = source;
            rect2.xMin += lsp;
        }
        public static void GetColumnsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3)
        {
            float l = (source.width - spacing * 2f) / 3f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            rect1 = source;
            rect1.xMax -= lsp2;

            rect2 = source;
            rect2.xMax -= lsp;
            rect2.xMin += lsp;

            rect3 = source;
            rect3.xMin += lsp2;
        }
        public static void GetColumnsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4)
        {
            float l = (source.width - spacing * 3f) / 4f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            rect1 = source;
            rect1.xMax -= lsp3;

            rect2 = source;
            rect2.xMax -= lsp2;
            rect2.xMin += lsp;

            rect3 = source;
            rect3.xMax -= lsp;
            rect3.xMin += lsp2;

            rect4 = source;
            rect4.xMin += lsp3;
        }
        public static void GetColumnsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4, out Rect rect5)
        {
            float l = (source.width - spacing * 4f) / 5f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            float lsp4 = lsp * 4f;
            rect1 = source;
            rect1.xMax -= lsp4;

            rect2 = source;
            rect2.xMax -= lsp3;
            rect2.xMin += lsp;

            rect3 = source;
            rect3.xMax -= lsp2;
            rect3.xMin += lsp2;

            rect4 = source;
            rect4.xMax -= lsp;
            rect4.xMin += lsp3;

            rect5 = source;
            rect5.xMin += lsp4;
        }
        public static void GetColumnsNonAlloc(this Rect source, float spacing, out Rect rect1, out Rect rect2, out Rect rect3, out Rect rect4, out Rect rect5, out Rect rect6)
        {
            float l = (source.width - spacing * 5f) / 6f;
            float lsp = l + spacing;
            float lsp2 = lsp * 2f;
            float lsp3 = lsp * 3f;
            float lsp4 = lsp * 4f;
            float lsp5 = lsp * 5f;
            rect1 = source;
            rect1.xMax -= lsp5;

            rect2 = source;
            rect2.xMax -= lsp4;
            rect2.xMin += lsp;

            rect3 = source;
            rect3.xMax -= lsp3;
            rect3.xMin += lsp2;

            rect4 = source;
            rect4.xMax -= lsp2;
            rect4.xMin += lsp3;

            rect5 = source;
            rect5.xMax -= lsp;
            rect5.xMin += lsp4;

            rect6 = source;
            rect6.xMin += lsp5;
        }
        public static void GetColumnsNonAlloc<TColl>(this Rect source, TColl collection, int count, float spacing)
            where TColl : IList<Rect>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Clear();
            float l = (source.width - spacing * (count - 1)) / count;
            float lsp = l + spacing;
            Rect r;
            for (int i = 0; i < count; ++i)
            {
                r = source;
                r.xMax -= lsp * (count - 1 - i);
                r.xMin += lsp * i;
                collection.Add(r);
            }
        }
        public static void GetColumnsNonAlloc<TColl>(this Rect source, TColl collection, float spacing)
            where TColl : IList<Rect>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            int count = collection.Count;
            float l = (source.width - spacing * (count - 1)) / count;
            float lsp = l + spacing;
            Rect r;
            for (int i = 0; i < count; ++i)
            {
                r = source;
                r.xMax -= lsp * (count - 1 - i);
                r.xMin += lsp * i;
                collection[i] = r;
            }
        }

        public static Vector2 RandomPoint(this Rect rect)
        {
            return new Vector2(
                Random.Range(rect.min.x, rect.max.x),
                Random.Range(rect.min.y, rect.max.y));
        }
    }
}
#if UNITY_EDITOR
namespace MyTools.Extensions.Rects.Editor
{
    using UnityEngine;
    using UnityEditor;
    public static class EditorRectEx
    {
        public static void GetIndentLabelFieldPair(this Rect source, int indentLevel, out Rect label, out Rect field)
        {
            var l = EditorGUIUtility.labelWidth;
            int indTmp = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel;
            label = EditorGUI.IndentedRect(source);
            label.xMax = source.xMin + l;
            field = source;
            field.xMin += l;
            EditorGUI.indentLevel = indTmp;
        }
    }
}
#endif
