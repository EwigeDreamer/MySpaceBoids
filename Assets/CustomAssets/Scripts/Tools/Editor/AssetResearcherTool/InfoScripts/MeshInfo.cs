using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using MyTools.Extensions.GameObjects;

namespace MyTools.Editor.Assets
{
    public class MeshInfo
    {
        public AssetInfo Asset { get; }
        public Object Mesh { get; }
        public string Name => Mesh.name;
        public string Path { get; }

        public MeshInfo(AssetInfo asset, Object mesh, string path)
        {
            Asset = asset;
            Mesh = mesh;
            Path = path;
        }
    }
}