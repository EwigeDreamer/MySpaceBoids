using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

namespace MyTools.Editor.Assets
{
    public class AssetDependencyManager
    {
        const string progressName = "2/{0}: Finding all dependencies";

        public Dictionary<AssetInfo, List<AssetInfo>> recurDependencyList;
        public Dictionary<AssetInfo, List<AssetInfo>> noRecurDependencyList;

        private AssetDependencyManager() { }
        public AssetDependencyManager(AllAssetManager assetManager) : this()
        {
            var progressNameTmp = string.Format(progressName, DependencyResearcher.stepCount);
            var assets = assetManager.assetList;
            int count = assets.Count;
            var recDepDict = new Dictionary<AssetInfo, List<AssetInfo>>();
            var noRecDepDict = new Dictionary<AssetInfo, List<AssetInfo>>();
            try
            {
                for (int i = 0; i < count; ++i)
                {
                    var asset = assets[i];

                    EditorUtility.DisplayProgressBar(progressNameTmp,
                        string.Format("check {0}...", asset.Path), (float)i / count);

                    var recDeps = new List<AssetInfo>();
                    var noRecDeps = new List<AssetInfo>();

                    //Debug.Log(string.Format("<color=#ff0000ff>{0}</color>", asset.Path));

                    var recDepPaths = AssetDatabase.GetDependencies(asset.Path, true);
                    foreach (var path in recDepPaths)
                        if (assetManager.pathDict.TryGetValue(path, out var info))
                            recDeps.Add(info);
                    var noRecDepPaths = AssetDatabase.GetDependencies(asset.Path, false);
                    foreach (var path in noRecDepPaths)
                        if (assetManager.pathDict.TryGetValue(path, out var info))
                            noRecDeps.Add(info);

                    recDepDict[asset] = recDeps;
                    noRecDepDict[asset] = noRecDeps;
                }
                recurDependencyList = recDepDict;
                noRecurDependencyList = noRecDepDict;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}