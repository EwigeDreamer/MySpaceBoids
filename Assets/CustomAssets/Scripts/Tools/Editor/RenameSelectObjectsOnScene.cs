/*--------------------------------------------------
 * Переименовывание выделенных объектов по шаблону.
 * 
 * Способ применения:
 * - выделить группу объектов на сцене;
 * - нажать Ctrl+Shift+R;
 * - в появившемся окне ввести шаблон
 *     ('{0}' - означает интекс объекта в группе);
 * - нажать кнопку 'Rename';
--------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using MyTools.Extensions.Editor;

namespace RenamerEditor
{
    public class RenameSelectObjectsOnScene : EditorWindow
    {
        [MenuItem("MyAssets/Tools/Object renamer %#r")]
        static void CreateWindow()
        {
            RenameSelectObjectsOnScene window = (RenameSelectObjectsOnScene)GetWindow(typeof(RenameSelectObjectsOnScene), true, "Rename objects");
            window.Init();
            window.Show();
        }

        string m_Sample = "Object ({0})";
        
        const float m_Width = 250f;
        const float m_Height = 50f;

        void Init()
        {
            Object obj = this;
            maxSize = new Vector2(m_Width, m_Height);
            minSize = new Vector2(m_Width, m_Height);
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

        void OnGUI()
        {
            float widthTmp = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60f;
            m_Sample = EditorGUILayout.TextField(new GUIContent("Template", "{0} - is an index of a group of selected objects."), m_Sample);
            EditorGUIUtility.labelWidth = widthTmp;

            if (GUI.Button(EditorGUILayout.GetControlRect(), "Rename!"))
                Rename();
        }

        void Rename()
        {
            Object[] objs = Selection.objects;
            //List<Transform> trs = new List<Transform>(objs.Length);
            List<Transform> nullParentTrs = new List<Transform>();
            Dictionary<Transform, List<Transform>> childrenGroups = new Dictionary<Transform, List<Transform>>();
            foreach (var obj in objs)
                if (obj is GameObject)
                {
                    Transform tr = ((GameObject)obj).transform;
                    if (tr.parent == null)
                        nullParentTrs.Add(tr);
                    else
                    {
                        if (childrenGroups.ContainsKey(tr.parent))
                            childrenGroups[tr.parent].Add(tr);
                        else
                            childrenGroups.Add(tr.parent, new List<Transform>() { tr });
                    }
                }
            nullParentTrs = nullParentTrs.OrderBy(a => a.GetSiblingIndex()).ToList();
            List<List<Transform>> trsList = new List<List<Transform>>();
            foreach (var pair in childrenGroups)
                trsList.Add(pair.Value.OrderBy(a => a.GetSiblingIndex()).ToList());

            if (m_Sample.Contains("{0}"))
                for (int i = 0; i < nullParentTrs.Count; ++i)
                    nullParentTrs[i].name = string.Format(m_Sample, i);
            else
                for (int i = 0; i < nullParentTrs.Count; ++i)
                    nullParentTrs[i].name = m_Sample;

            foreach (var trList in trsList)
                if (m_Sample.Contains("{0}"))
                    for (int i = 0; i < trList.Count; ++i)
                        trList[i].name = string.Format(m_Sample, i);
                else
                    for (int i = 0; i < trList.Count; ++i)
                        trList[i].name = m_Sample;
        }
    }
}