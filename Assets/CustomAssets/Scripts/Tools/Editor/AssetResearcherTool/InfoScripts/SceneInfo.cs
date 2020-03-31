using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MyTools.Editor.Assets
{
    public class SceneInfo
    {
        public AssetInfo Asset { get; }
        public string Name => Asset.Name;
        public string Path => Asset.Path;
        Scene m_Scene;
        bool m_IsLoaded = false;

        private SceneInfo() { }
        public SceneInfo(AssetInfo asset) : this()
        {
            Asset = asset;
        }

        public Scene LoadScene()
        {
            if (m_IsLoaded) EditorSceneManager.CloseScene(m_Scene, true);
            m_Scene = EditorSceneManager.OpenScene(Path, OpenSceneMode.Additive);
            m_IsLoaded = true;
            return m_Scene;
        }

        public void RemoveScene()
        {
            if (!m_IsLoaded) return;
            EditorSceneManager.CloseScene(m_Scene, true);
            m_IsLoaded = false;
        }
    }
}
