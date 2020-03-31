using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using MyTools.Extensions.GameObjects;

namespace MyTools.Editor.Assets
{
    public class SceneObjectInfo
    {
        public SceneInfo Scene { get; }
        public string Name { get; }
        public string Path { get; }
        public string SiblingPath { get; }

        public SceneObjectInfo(GameObject obj, SceneInfo scene)
        {
            Name = obj.name;
            Scene = scene;
            Path = obj.GetPath();
            SiblingPath = obj.GetPathWithSiblingIndex();
        }
    }
}
