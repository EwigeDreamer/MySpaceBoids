using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyTools.Extensions.Rects;
using System.Threading;
using System.IO;
using MyTools.Extensions.Editor;

namespace PNGMergerEditor
{
    public class PNGMerger : EditorWindow
    {
        GUIStyle m_LabelStyle = null;
        GUIStyle m_ResolutionStyle = null;

        float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
        float LineSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }
        float LabelWidth { get { return EditorGUIUtility.labelWidth; } }

        [MenuItem("MyAssets/Tools/PNG Merger")]
        static void CreateWindow()
        {
            PNGMerger window = (PNGMerger)GetWindow(typeof(PNGMerger), true, "PNG Merger");
            window.Init();
            window.Show();
        }

        const float m_Width = 280;
        const float m_Height = 130;

        void Init()
        {
            Object obj = this;
            maxSize = new Vector2(m_Width, m_Height);
            //minSize = new Vector2(m_Width, m_Height);
            //float halfWidth = m_Width / 2f;
            //float halfHeight = m_Height / 2f;
            //float halfScrWidth = Screen.currentResolution.width / 2f;
            //float halfScrHeight = Screen.currentResolution.height / 2f;
            //position = new Rect()
            //{
            //    xMin = halfScrWidth - halfWidth,
            //    xMax = halfScrWidth + halfWidth,
            //    yMin = halfScrHeight - halfHeight,
            //    yMax = halfScrHeight + halfHeight
            //};
            this.CenterOnMainWin();
        }

        const float m_MapFieldSize = 64f;
        Texture2D m_TexR;
        Texture2D m_TexG;
        Texture2D m_TexB;
        Texture2D m_TexA;

        void OnGUI()
        {
            var labStyle = m_LabelStyle;
            if (labStyle == null)
            {
                labStyle = new GUIStyle(GUI.skin.label);
                labStyle.fontStyle = FontStyle.Bold;
                labStyle.alignment = TextAnchor.MiddleCenter;
                m_LabelStyle = labStyle;
            }
            var resStyle = m_ResolutionStyle;
            if (resStyle == null)
            {
                resStyle = new GUIStyle(GUI.skin.label);
                resStyle.fontSize = 8;
                resStyle.alignment = TextAnchor.MiddleCenter;
                m_ResolutionStyle = resStyle;
            }
            var spacing = LineSpacing;
            var width = m_MapFieldSize * 4f + spacing * 3f;

            var labelRect = EditorGUILayout.GetControlRect(false);
            var labelRectCenter = labelRect.center;
            labelRect.xMin = labelRectCenter.x - width / 2f;
            labelRect.xMax = labelRectCenter.x + width / 2f;
            labelRect.GetColumnsNonAlloc(spacing, out var lab1, out var lab2, out var lab3, out var lab4);
            labStyle.normal.textColor = Color.red;
            EditorGUI.LabelField(lab1, "R", labStyle);
            labStyle.normal.textColor = Color.green;
            EditorGUI.LabelField(lab2, "G", labStyle);
            labStyle.normal.textColor = Color.blue;
            EditorGUI.LabelField(lab3, "B", labStyle);
            labStyle.normal.textColor = Color.gray;
            EditorGUI.LabelField(lab4, "A", labStyle);

            var texRect = EditorGUILayout.GetControlRect(false, m_MapFieldSize);
            var texRectCenter = texRect.center;
            texRect.xMin = texRectCenter.x - width / 2f;
            texRect.xMax = texRectCenter.x + width / 2f;
            texRect.GetColumnsNonAlloc(spacing, out var col1, out var col2, out var col3, out var col4);
            m_TexR = (Texture2D)EditorGUI.ObjectField(col1, m_TexR, typeof(Texture2D), false);
            m_TexG = (Texture2D)EditorGUI.ObjectField(col2, m_TexG, typeof(Texture2D), false);
            m_TexB = (Texture2D)EditorGUI.ObjectField(col3, m_TexB, typeof(Texture2D), false);
            m_TexA = (Texture2D)EditorGUI.ObjectField(col4, m_TexA, typeof(Texture2D), false);

            var resRect = EditorGUILayout.GetControlRect(false);
            var resRectCenter = resRect.center;
            resRect.xMin = resRectCenter.x - width / 2f;
            resRect.xMax = resRectCenter.x + width / 2f;
            resRect.GetColumnsNonAlloc(spacing, out var res1, out var res2, out var res3, out var res4);
            EditorGUI.LabelField(res1, TexRes(m_TexR), resStyle);
            EditorGUI.LabelField(res2, TexRes(m_TexG), resStyle);
            EditorGUI.LabelField(res3, TexRes(m_TexB), resStyle);
            EditorGUI.LabelField(res4, TexRes(m_TexA), resStyle);

            var mergeButtonRect = EditorGUILayout.GetControlRect(false);
            if (GUI.Button(mergeButtonRect, "Merge!"))
            {
                var path = EditorUtility.SaveFilePanel("Save merge result as PNG", Application.dataPath, "merged.png", "png");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
                {
                    var tex = CreateTex(m_TexR, m_TexG, m_TexB, m_TexA);
                    var data = tex.EncodeToPNG();
                    if (data != null) File.WriteAllBytes(path, data);
                    DestroyImmediate(tex);
                    Thread.Sleep(500);
                    EditorUtility.RevealInFinder(path);
                }
                else
                    Debug.LogWarning("Texture not saved: path is empty.");
            }
        }

        string TexRes(Texture2D tex)
        {
            if (tex == null) return "None";
            return tex.width.ToString() + "x" + tex.height.ToString();
        }

        Texture2D CreateTex(Texture2D r, Texture2D g, Texture2D b, Texture2D a)
        {
            int w = 1;
            if (r != null && r.width > w) w = r.width;
            if (g != null && g.width > w) w = g.width;
            if (b != null && b.width > w) w = b.width;
            if (a != null && a.width > w) w = a.width;
            int h = 1;
            if (r != null && r.height > h) h = r.height;
            if (g != null && g.height > h) h = g.height;
            if (b != null && b.height > h) h = b.height;
            if (a != null && a.height > h) h = a.height;

            var tex = new Texture2D(w, h);

            Color[] bgPxls = new Color[w * h];
            var pCount = bgPxls.Length;
            for (int i = 0; i < pCount; ++i) bgPxls[i] = new Color(0f, 0f, 0f, 0f);
            tex.SetPixels(bgPxls);
            bgPxls = null;
            System.GC.Collect();

            if (r != null)
            {
                var part = tex.GetPixels(0, 0, r.width, r.height);
                var pxls = r.GetPixels();
                if (part.Length != pxls.Length) throw new System.Exception("WATAFAK");
                int count = part.Length;
                for (int i = 0; i < count; ++i)
                    part[i].r = (pxls[i].r + pxls[i].g + pxls[i].b) / 3f;
                tex.SetPixels(0, 0, r.width, r.height, part);
                part = null;
                pxls = null;
                System.GC.Collect();
            }
            if (g != null)
            {
                var part = tex.GetPixels(0, 0, g.width, g.height);
                var pxls = g.GetPixels();
                if (part.Length != pxls.Length) throw new System.Exception("WATAFAK");
                int count = part.Length;
                for (int i = 0; i < count; ++i)
                    part[i].g = (pxls[i].r + pxls[i].g + pxls[i].b) / 3f;
                tex.SetPixels(0, 0, g.width, g.height, part);
                part = null;
                pxls = null;
                System.GC.Collect();
            }
            if (b != null)
            {
                var part = tex.GetPixels(0, 0, b.width, b.height);
                var pxls = b.GetPixels();
                if (part.Length != pxls.Length) throw new System.Exception("WATAFAK");
                int count = part.Length;
                for (int i = 0; i < count; ++i)
                    part[i].b = (pxls[i].r + pxls[i].g + pxls[i].b) / 3f;
                tex.SetPixels(0, 0, b.width, b.height, part);
                part = null;
                pxls = null;
                System.GC.Collect();
            }
            if (a != null)
            {
                var part = tex.GetPixels(0, 0, a.width, a.height);
                var pxls = a.GetPixels();
                if (part.Length != pxls.Length) throw new System.Exception("WATAFAK");
                int count = part.Length;
                for (int i = 0; i < count; ++i)
                    part[i].a = (pxls[i].r + pxls[i].g + pxls[i].b) / 3f;
                tex.SetPixels(0, 0, a.width, a.height, part);
                part = null;
                pxls = null;
                System.GC.Collect();
            }

            return tex;
        }
    }
}