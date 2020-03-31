using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MyTools.Editor.Assets
{
    public class AssetInfo
    {
        public Object Obj { get; }
        public string Name { get; }
        public string Path { get; }
        public string Extension { get; }
        public System.Type Type { get; }

        public AssetInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new System.InvalidOperationException("Path is null or empty.");
            int slash = path.LastIndexOf('/');
            int dot = path.LastIndexOf('.');
            if (dot > slash)
            {
                //Debug.Log(string.Format("{0}\nslash = {1}; dot = {2}; Substring({3}, {4});", path, slash, dot, slash + 1, dot - (slash + 1)));
                Name = path.Substring(slash + 1, dot - (slash + 1));
                Extension = path.Substring(dot + 1);
            }
            else
            {
                Name = path.Substring(slash);
                Extension = "file";
            }
            Type = AssetDatabase.GetMainAssetTypeAtPath(path);
            Path = path;
            Obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        }
    }
}