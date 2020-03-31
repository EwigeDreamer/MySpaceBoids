using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

namespace MyTools.Editor.Assets
{
    public class AllAssetManager
    {
        const string progressName = "1/{0}: Collecting all assets";

        public List<AssetInfo> assetList;
        public List<AssetInfo> brokenAssetList;
        public Dictionary<Object, AssetInfo> objDict;
        public Dictionary<string, AssetInfo> pathDict;

        public AllAssetManager()
        {
            var progressNameTmp = string.Format(progressName, DependencyResearcher.stepCount);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            EditorUtility.DisplayProgressBar(progressNameTmp, 
                "collecting asset pathes...", 0f);
            var pathes = AssetDatabase.GetAllAssetPaths();
            int count = pathes.Length;
            var assets = new List<AssetInfo>(count);
            var oDict = new Dictionary<Object, AssetInfo>(count);
            var pDict = new Dictionary<string, AssetInfo>(count);
            var brokenAssets = new List<AssetInfo>();
            AssetInfo info;
            try
            {
                for (int i = 0; i < count; ++i)
                {
                    var path = pathes[i];
                    if (path.Contains("Assets/") && !AssetDatabase.IsValidFolder(path))
                    {
                        EditorUtility.DisplayProgressBar(progressNameTmp,
                            string.Format("load {0}...", path), (float)i / count);
                        info = new AssetInfo(path);
                        if (info.Obj == null)
                        {
                            brokenAssets.Add(info);
                            continue;
                        }
                        oDict[info.Obj] = info;
                        pDict[info.Path] = info;
                        assets.Add(info);
                    }
                }
                EditorUtility.DisplayProgressBar(progressNameTmp,
                    "sorting...", 0f);
                assets = assets.OrderBy(o => o.Path).ToList();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            watch.Stop();
            float elapsedSec = (float)watch.Elapsed.TotalSeconds;
            Debug.LogError("Complete (" + elapsedSec + " seconds)");
            Debug.LogError(string.Format("Found and sorted {0} assets", assets.Count));
            assetList = assets;
            brokenAssetList = brokenAssets;
            objDict = oDict;
            pathDict = pDict;
        }
    }
}