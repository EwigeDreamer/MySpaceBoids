using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using MyTools.Extensions.GameObjects;
using System.Linq;

namespace MyTools.Editor.Assets
{
    public static class DependencyResearcher
    {
        public const int stepCount = 2;

        [MenuItem("MyAssets/Tools/DependencyResearcher/Go")]
        static void Search()
        {
            var assetManager = new AllAssetManager();
            var dependManager = new AssetDependencyManager(assetManager);

            var exts = new Dictionary<string, int>(10);
            foreach (var info in assetManager.assetList)
            {
                var ext = info.Extension.ToLower();
                if (exts.TryGetValue(ext, out var count))
                    exts[ext] = count + 1;
                else
                    exts[ext] = 1;
            }
            var sortedExts = exts.OrderByDescending(a => a.Value);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Extensions:");
            foreach (var extPair in sortedExts)
            {
                sb.AppendLine(string.Format("{0}: {1}", extPair.Value, extPair.Key));
            }
            Debug.Log(sb.ToString());

            sb.Clear();
            sb.AppendLine("Broken assets:");
            foreach (var info in assetManager.brokenAssetList)
            {
                sb.AppendLine(info.Path);
            }
            Debug.LogWarning(sb.ToString());
        }

        [MenuItem("MyAssets/Tools/DependencyResearcher/No")]
        static void TestProgressBar()
        {
            var time = 5;
            var sec = 50;
            for (int i = 0; i < time * sec; ++i)
            {
                System.Threading.Thread.Sleep(20);
                EditorUtility.DisplayProgressBar(
                    "Unity.exe не отвечает...",
                    "закрытие через " + (time - i / sec).ToString() + " sec",
                    (float)i / (time * sec));
            }
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Хе-хе", " ШУТКА!", "Ах тыж сучий потрох..");
        }
    }
}
