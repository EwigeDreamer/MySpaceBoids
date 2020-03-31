using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

namespace DependencyResearcher
{
    public class MyDependencyResearcher
    {
        const string separator = "================================";
        const string subSeparator = "--------------------------------";
        const string notUsedMsg = "Not used anywhere";
        const string noDependMsg = "Has no dependencies";
        const string assetLabel = "Asset";
        const string inUseAssetLabel = "UseInAsset";
        const string inUseGOLabel = "UseInGameObject";

        //[MenuItem("MyAssets/MyDependencyResearcher")]
        private static void SaveTxt(/*string[] strs, string name = "Text"*/)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //if (string.IsNullOrEmpty(name))
            //    return;
            //string[] allAssetPathes = AssetDatabase.GetAllAssetPaths();

            AssetInfoManager manager = new AssetInfoManager();

            Dictionary<string, int> exts = new Dictionary<string, int>();
            int maxLen = 0;
            foreach (var asset in manager.assetList)
            {
                if (exts.ContainsKey(asset.extension.ToLower()))
                    ++exts[asset.extension.ToLower()];
                else
                {
                    exts.Add(asset.extension.ToLower(), 1);
                    if (asset.extension.Length > maxLen)
                        maxLen = asset.extension.Length;
                }
            }
            exts = exts.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            string projectPath = AssetInfoManager.GetGlobalProjectPath();
            using (var sv = new StreamWriter(string.Format("{0}/{1}.txt", projectPath, "AssetDependencies")))
            {

                sv.WriteLine(string.Format("{0} assets:", manager.assetList.Count));
                foreach (var ext in exts)
                {
                    string dots = "";
                    int space = maxLen + 3 - ext.Key.Length;
                    for (int i = 0; i < space; ++i)
                        dots += ".";
                    sv.WriteLine(string.Format("{0}{1}{2}", ext.Key, dots, ext.Value));
                }
                sv.WriteLine(separator);

                foreach (var asset in manager.assetList)
                {
                    if (asset.extension.ToLower() != "fbx")
                        continue;
                    sv.WriteLine(string.Format("<{0}>:=<{1}><{2}><{3}>", assetLabel, asset.path, asset.extension, asset.type.Name));

                    if (asset.fbxInfs.Length < 1)
                    {
                        sv.WriteLine("Has no meshes");
                        sv.WriteLine(separator);
                        continue;
                    }
                    for (int i = 0; i < asset.fbxInfs.Length; ++i)
                    {
                        sv.WriteLine(string.Format("<{0} {1}>:=<{2}><{3}>", "Mesh", i, asset.fbxInfs[i].mesh.name, asset.fbxInfs[i].path));
                        if (asset.fbxInfs[i].goInfo.Length < 1 && asset.fbxInfs[i].asInfo.Length < 1)
                            sv.WriteLine("\t" + notUsedMsg);
                        for (int j = 0; j < asset.fbxInfs[i].goInfo.Length; ++j)
                        {
                            sv.WriteLine(string.Format("\t<{0} {1}>:=<{2}><{3}>", inUseGOLabel, j, asset.fbxInfs[i].goInfo[j].sceneInfo.path, asset.fbxInfs[i].goInfo[j].gObjPath));
                        }
                        for (int j = 0; j < asset.fbxInfs[i].asInfo.Length; ++j)
                        {
                            sv.WriteLine(string.Format("\t<{0} {1}>:=<{2}>", inUseAssetLabel, j, asset.fbxInfs[i].asInfo[j].asset.path));
                            for (int k = 0; k < asset.fbxInfs[i].asInfo[j].pathes.Count; ++k)
                                sv.WriteLine(string.Format("\t\t<{0} {1}>:=<{2}>", "MeshFilter|SkinnedMesh", k, asset.fbxInfs[i].asInfo[j].pathes[k]));
                        }
                    }



                    //if (asset.inUseAssets.Length < 1 && asset.inUseGameObjects.Length < 1)
                    //    sv.WriteLine(notUsedMsg);
                    //else
                    //{
                    //    for (int i = 0; i < asset.inUseAssets.Length; ++i)
                    //        sv.WriteLine(string.Format("<{0} {1}>:=<{2}>", inUseAssetLabel, i, asset.inUseAssets[i].path));
                    //    for (int i = 0; i < asset.inUseGameObjects.Length; ++i)
                    //        sv.WriteLine(string.Format("<{0} {1}>:=<{2}><{3}>", inUseGOLabel, i, asset.inUseGameObjects[i].sceneInfo.path, asset.inUseGameObjects[i].gObjPath));
                    //}
                    sv.WriteLine(separator);
                }


            }

            using (var sv = new StreamWriter(string.Format("{0}/{1}.txt", projectPath, "ModelDoubles")))
            {
                int assCount = 0;
                int grpCount = 0;
                foreach (var item1 in manager.FbxNamingDoublesGroupInfs)
                    foreach (var item2 in item1)
                    {
                        ++grpCount;
                        foreach (var item3 in item2.FbxNamingDoubles)
                            ++assCount;
                    }
                sv.WriteLine(string.Format("{0} files, {1} doubles groups:", assCount, grpCount));
                if (grpCount > 0)
                {
                    sv.WriteLine("GroupName...GroupCount");
                    maxLen = 0;
                    foreach (var doublesGroups in manager.FbxNamingDoublesGroupInfs)
                        foreach (var doubles in doublesGroups)
                        {
                            if (maxLen < doubles.FbxNamingDoubles[0].name.Length)
                                maxLen = doubles.FbxNamingDoubles[0].name.Length;
                        }
                    foreach (var doublesGroups in manager.FbxNamingDoublesGroupInfs)
                        foreach (var doubles in doublesGroups)
                        {
                            string dots = "";
                            int space = maxLen + 3 - doubles.FbxNamingDoubles[0].name.Length;
                            for (int i = 0; i < space; ++i)
                                dots += ".";
                            sv.WriteLine(string.Format("{0}.{1}{2}{3}:", doubles.FbxNamingDoubles[0].name, doubles.FbxNamingDoubles[0].extension, dots, doubles.FbxNamingDoubles.Count));
                            for (int i = 0; i < doubles.FbxNamingDoubles.Count; ++i)
                            {
                                int meshCount = 0;
                                int asLinksCount = 0;
                                int asgoLinksCount = 0;
                                int goLinksCount = 0;
                                foreach (var mesh in doubles.FbxNamingDoubles[i].fbxInfs)
                                {
                                    ++meshCount;
                                    asLinksCount += mesh.asInfo.Length;
                                    goLinksCount += mesh.goInfo.Length;
                                    foreach (var asInf in mesh.asInfo)
                                        asgoLinksCount += asInf.pathes.Count;
                                }
                                sv.WriteLine(string.Format("   fbx {0}: {1} meshes, {2}:{3} asLinks, {4} goLinks", i, meshCount, asLinksCount, asgoLinksCount, goLinksCount));
                            }
                        }
                    sv.WriteLine(separator);

                    foreach (var doublesGroups in manager.FbxNamingDoublesGroupInfs)
                        foreach (var doubles in doublesGroups)
                        {
                            foreach (var asset in doubles.FbxNamingDoubles)
                            {
                                sv.WriteLine(string.Format("<{0}>:=<{1}><{2}><{3}>", assetLabel, asset.path, asset.extension, asset.type.Name));

                                if (asset.fbxInfs.Length < 1)
                                {
                                    sv.WriteLine("Has no meshes");
                                    sv.WriteLine(subSeparator);
                                    continue;
                                }
                                for (int i = 0; i < asset.fbxInfs.Length; ++i)
                                {
                                    sv.WriteLine(string.Format("<{0} {1}>:=<{2}><{3}>", "Mesh", i, asset.fbxInfs[i].mesh.name, asset.fbxInfs[i].path));
                                    if (asset.fbxInfs[i].goInfo.Length < 1 && asset.fbxInfs[i].asInfo.Length < 1)
                                        sv.WriteLine("   " + notUsedMsg);
                                    for (int j = 0; j < asset.fbxInfs[i].goInfo.Length; ++j)
                                    {
                                        sv.WriteLine(string.Format("   <{0} {1}>:=<{2}><{3}>", inUseGOLabel, j, asset.fbxInfs[i].goInfo[j].sceneInfo.path, asset.fbxInfs[i].goInfo[j].gObjPath));
                                    }
                                    for (int j = 0; j < asset.fbxInfs[i].asInfo.Length; ++j)
                                    {
                                        sv.WriteLine(string.Format("   <{0} {1}>:=<{2}>", inUseAssetLabel, j, asset.fbxInfs[i].asInfo[j].asset.path));
                                        for (int k = 0; k < asset.fbxInfs[i].asInfo[j].pathes.Count; ++k)
                                            sv.WriteLine(string.Format("      <{0} {1}>:=<{2}>", asset.fbxInfs[i].asInfo[j].pathes[k][0], k, asset.fbxInfs[i].asInfo[j].pathes[k][1]));
                                    }
                                }
                                if (asset != doubles.FbxNamingDoubles[doubles.FbxNamingDoubles.Count - 1])
                                    sv.WriteLine(subSeparator);
                            }
                            sv.WriteLine(separator);
                        }
                }
            }
            using (var sv = new StreamWriter(string.Format("{0}/{1}.txt", projectPath, "FbxNoReferences")))
            {
                sv.WriteLine(separator);
                foreach (var scene in manager.sceneList)
                {
                    bool have = false;
                    foreach (var obj in scene.gObjInfs)
                        if (obj.missingModels.Length > 0)
                        {
                            have = true;
                            break;
                        }
                    if (!have)
                        continue;

                    sv.WriteLine(string.Format("{0}: {1} objects", scene.name, scene.path));
                    foreach (var obj in scene.gObjInfs)
                        foreach (var miss in obj.missingModels)
                            sv.WriteLine(string.Format("{0}", miss));
                    sv.WriteLine(separator);
                }
            }

                using (var sv = new StreamWriter(string.Format("{0}/{1}.txt", projectPath, "ObjectDependencies")))
            {

                sv.WriteLine(string.Format("{0} scenes:", manager.sceneList.Count));
                foreach (var scene in manager.sceneList)
                {
                    sv.WriteLine(string.Format("{0}: {1} objects", scene.name, scene.gObjInfs.Length));
                }
                sv.WriteLine(separator);

                foreach (var scene in manager.sceneList)
                {
                    foreach (var obj in scene.gObjInfs)
                    {
                        sv.WriteLine(string.Format("{0}/{1}", scene.name, obj.gObjPath));
                        foreach (var ass in obj.dependencyAssets)
                        {
                            sv.WriteLine(string.Format("\t{0}", ass.path));
                        }
                    }
                    sv.WriteLine(separator);
                }
            }

            watch.Stop();
            float elapsedSec = (float)watch.Elapsed.TotalSeconds;
            Debug.LogError("Complete (" + elapsedSec + " seconds)");
        }

        public static string ToStringNullSafe(object value)
        {
            return (value ?? string.Empty).ToString();
        }
    }


    public class AssetInfoManager
    {
        public List<AssetInfo> assetList;
        public List<SceneInfo> sceneList;
        private List<List<AssetInfo>> FbxNamingDoublesGroups;
        public List<List<FbxNamingDoublesGroupInfo>> FbxNamingDoublesGroupInfs;

        public Dictionary<string, List<AssetInfo>> assetDictionary = new Dictionary<string, List<AssetInfo>>(25);

        public AssetInfoManager()
        {
            AssetDatabase.SaveAssets();
            Init();
        }

        private void Init()
        {
            MyGC.ResetGCLog();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            assetList = GetAllAssets();
            assetList = InitAssets(assetList);
            MyGC.CheckGC(true);

            watch.Stop();
            float elapsedSec = (float)watch.Elapsed.TotalSeconds;
            Debug.LogError("Assets complete (" + elapsedSec + " seconds)");
            watch = System.Diagnostics.Stopwatch.StartNew();

            sceneList = InitScenes(assetList);
            assetList = CalculateAssetObjectDependencies(assetList, sceneList);
            FbxNamingDoublesGroups = FindFbxNamingDoubles(assetList, out FbxNamingDoublesGroupInfs);
            MyGC.CheckGC(true);

            watch.Stop();
            elapsedSec = (float)watch.Elapsed.TotalSeconds;
            Debug.LogError("Objects complete (" + elapsedSec + " seconds)");

            MyGC.LogGC();
        }

        private List<AssetInfo> GetAllAssets()
        {
            List<AssetInfo> assets = new List<AssetInfo>(1000);

            string[] allAssetPathes = AssetDatabase.GetAllAssetPaths();
            foreach (var path in allAssetPathes)
            {
                if (path.Contains("Assets/") && !AssetDatabase.IsValidFolder(path))
                    assets.Add(new AssetInfo(path));
                MyGC.CheckGC();
            }
            assets = assets.OrderBy(o => o.path).ToList();
            return assets;
        }
        private List<AssetInfo> InitAssets(List<AssetInfo> assets)
        {
            foreach (var asset in assets)
            {
                asset.FirstInitDependencies();
                MyGC.CheckGC();
            }
            var assArr = assets.ToArray();
            foreach (var asset in assets)
            {
                asset.SecondInitDependencies(assArr);
                MyGC.CheckGC();
            }
            return assets;
        }
        private List<AssetInfo> InitAssetInfoFields(List<AssetInfo> assets)
        {
            foreach (var asset in assets)
            {
                int count;

                count = asset.dependencyRecurPathes.Length;
                asset.dependencyRecurAssets = new AssetInfo[count];
                for (int i = 0; i < count; ++i)
                {
                    foreach (var checkAsset in assets)
                        if (checkAsset.path == asset.dependencyRecurPathes[i])
                        {
                            asset.dependencyRecurAssets[i] = checkAsset;
                        }
                }
            }

            return assets;
        }
        const string sceneExt = "unity";
        private List<SceneInfo> InitScenes(List<AssetInfo> assets)
        {
            //int count = SceneManager.sceneCountInBuildSettings;
            List<SceneInfo> sceneInfoList = new List<SceneInfo>(10);
            //for (int i = 0; i < count; ++i)
            //{
            //    sceneInfoList.Add(new SceneInfo(i, assets));
            //}

            Scene curScene = EditorSceneManager.GetActiveScene();
            string curScenePath = curScene.path;
            EditorSceneManager.CloseScene(curScene, true);


            foreach (var asset in assets)
            {
                if (asset.extension.ToLower() == sceneExt.ToLower()
                    //*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*ВЫПИЛИТЬ!!!!!*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*
                    //&& asset.path == "Assets/ImportLeftAssets/RMC/Scene/DemoScene.unity"
                    //&& asset.path == "Assets/MINECRAFT/YULIYA/minecraft_scene 2"
                    )
                {
                    //Debug.LogError("SCEEEEEENEEEEEEEE!!!!!!!!!!!!!!");
                    sceneInfoList.Add(new SceneInfo(asset, assets));
                    MyGC.CheckGC(true);
                }
            }
            
            EditorSceneManager.OpenScene(curScenePath, OpenSceneMode.Single);


            return sceneInfoList;
        }

        private List<AssetInfo> CalculateAssetObjectDependencies(List<AssetInfo> assets, List<SceneInfo> scenes)
        {
            //List<GameObjectInfo> inUseGOTmp = new List<GameObjectInfo>(100);
            List<AssetInfo> models = new List<AssetInfo>();
            List<AssetInfo> prefabs = new List<AssetInfo>();
            foreach (var asset in assets)
                switch (asset.extension.ToLower())
                {
                    case "fbx":
                        models.Add(asset);
                        break;
                    case "prefab":
                        prefabs.Add(asset);
                        break;
                }
            List<GameObjectInfo> objs = new List<GameObjectInfo>();
            List<AssetInfo.MeshInfo.AssetModelUsageInfo> assts = new List<AssetInfo.MeshInfo.AssetModelUsageInfo>();
            foreach (var model in models)
                foreach (var fbxInf in model.fbxInfs)
                {
                    objs.Clear();
                    assts.Clear();
                    foreach (var scene in scenes)
                        foreach (var objInf in scene.gObjInfs)
                        {
                            foreach (var mesh in objInf.meshes)
                                if (mesh == fbxInf.mesh)
                                    objs.Add(objInf);
                            foreach (var skinMesh in objInf.skinnedMeshes)
                                if (skinMesh == fbxInf.mesh && !objs.Contains(objInf))
                                    objs.Add(objInf);
                        }
                    foreach (var prefab in prefabs)
                        foreach (var modInf in prefab.fbxInfs)
                            if (modInf.mesh == fbxInf.mesh)
                            {
                                bool contains = false;
                                foreach (var inf in assts)
                                    if (inf.asset == prefab)
                                    {
                                        contains = true;
                                        inf.pathes.Add(new string[] { modInf.type.ToString(), modInf.path });
                                        break;
                                    }
                                if (!contains)
                                {
                                    assts.Add(new AssetInfo.MeshInfo.AssetModelUsageInfo() { asset = prefab, pathes = new List<string[]>() { new string[] { modInf.type.ToString(),  modInf.path } } });
                                }
                            }
                    fbxInf.goInfo = objs.ToArray();
                    fbxInf.asInfo = assts.ToArray();
                }

            //inUseGOTmp.Clear();
            //foreach (var scene in scenes)
            //    foreach (var objInf in scene.gObjInfs)
            //    {
            //        foreach (var depAss in objInf.dependencyAssets)
            //            if (depAss == asset)
            //            {
            //                inUseGOTmp.Add(objInf);
            //                MyGC.CheckGC();
            //                break;
            //            }
            //    }
            //asset.inUseGameObjects = inUseGOTmp.ToArray();

            return assets;
        }

        private List<List<AssetInfo>> FindFbxNamingDoubles(List<AssetInfo> assets, out List<List<FbxNamingDoublesGroupInfo>> groupInfs)
        {
            List<AssetInfo> models = new List<AssetInfo>();
            foreach (var asset in assets)
                switch (asset.extension.ToLower())
                {
                    case "fbx":
                        models.Add(asset);
                        break;
                }

            List<List<AssetInfo>> doublesList = new List<List<AssetInfo>>();

            foreach (var model1 in models)
            {
                bool passed = false;
                foreach (var dbs in doublesList)
                {
                    foreach (var mod in dbs)
                        if (mod == model1)
                        {
                            passed = true;
                            break;
                        }
                    if (passed)
                        break;
                }
                if (passed)
                    continue;
                List<AssetInfo> doubles = new List<AssetInfo>() { model1 };
                foreach (var model2 in models)
                {
                    if (model2 != model1 && model2.name == model1.name)
                        doubles.Add(model2);
                }
                //if (doubles.Count > 1)
                    doublesList.Add(doubles);
            }


            doublesList = doublesList.OrderBy(a => a.Count).ToList();
            List<List<FbxNamingDoublesGroupInfo>> groups = new List<List<FbxNamingDoublesGroupInfo>>();
            int count = 0;
            foreach (var doubles in doublesList)
            {
                if (doubles.Count != count)
                {
                    count = doubles.Count;
                    groups.Add(new List<FbxNamingDoublesGroupInfo>(){ new FbxNamingDoublesGroupInfo() { FbxNamingDoubles = doubles } });
                }
                else
                    groups[groups.Count - 1].Add(new FbxNamingDoublesGroupInfo() { FbxNamingDoubles = doubles });
            }
            for (int i = 0; i < groups.Count; ++i)
                groups[i] = groups[i].OrderBy(a => a.InUseCount).ToList();
            doublesList.Clear();
            foreach (var group in groups)
                foreach (var dbs in group)
                    doublesList.Add(dbs.FbxNamingDoubles);
            groupInfs = groups;
            return doublesList;
        }

        public class FbxNamingDoublesGroupInfo
        {
            public List<AssetInfo> FbxNamingDoubles = new List<AssetInfo>();
            public int InUseCount
            {
                get
                {
                    int count = 0;
                    foreach (var model in FbxNamingDoubles)
                        foreach (var mesh in model.fbxInfs)
                            count += mesh.asInfo.Length + mesh.goInfo.Length;
                    return count;
                }
            }
        }

        public class AssetInfo
        {
            public Object obj = null;

            public class MeshInfo
            {
                public enum Type { MeshFilter, SkinnedMesh}
                public Type type = Type.MeshFilter;
                public Object mesh = null;
                public AssetModelUsageInfo[] asInfo = new AssetModelUsageInfo[0];
                public GameObjectInfo[] goInfo = new GameObjectInfo[0];
                public string path = "";

                public class AssetModelUsageInfo
                {
                    public AssetInfo asset = null;
                    public List<string[]> pathes = new List<string[]>();
                }
            }
            public MeshInfo[] fbxInfs = new MeshInfo[0];
            //public string guid = "";

            public string path = "";
            public string name = "";
            public string extension = "";
            public string[] dependencyRecurPathes = new string[0];
            public string[] dependencyNoRecurPathes = new string[0];
            public string[] inUsePathes = new string[0];

            public AssetInfo[] dependencyRecurAssets = new AssetInfo[0];
            public AssetInfo[] dependencyNoRecurAssets = new AssetInfo[0];
            public AssetInfo[] inUseAssets = new AssetInfo[0];

            public GameObjectInfo[] inUseGameObjects = new GameObjectInfo[0];

            public System.Type type = null;

            public AssetInfo(string newPath)
            {
                path = newPath;
                if (string.IsNullOrEmpty(path))
                    return;
                obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                //guid = AssetDatabase.AssetPathToGUID(path);
                int slash = path.LastIndexOf('/');
                int dot = path.LastIndexOf('.');
                if (dot > -1)
                {
                    name = path.Substring(slash + 1, dot - (slash + 1));
                    extension = path.Substring(dot + 1);
                }
                else
                {
                    name = path.Substring(slash);
                    extension = "file";
                }
                type = AssetDatabase.GetMainAssetTypeAtPath(path);
            }
            private bool firstInit = false;
            public void FirstInitDependencies()
            {
                if (firstInit) return;

                if ((extension.ToLower() == "fbx" || extension.ToLower() == "prefab") && obj is GameObject)
                {
                    GameObject goT = obj as GameObject;
                    GameObject goTmp = Object.Instantiate(goT);
                    goTmp.name = goT.name;
                    var filters = goTmp.GetComponentsInChildren<MeshFilter>();
                    var skins = goTmp.GetComponentsInChildren<SkinnedMeshRenderer>();
                    List<MeshInfo> meshes = new List<MeshInfo>();
                    foreach (var fltr in filters)
                        if (fltr.sharedMesh != null)
                            meshes.Add(new MeshInfo() { mesh = fltr.sharedMesh, path = GameObjectInfo.GetGameObjectPath(fltr.transform), type = MeshInfo.Type.MeshFilter });
                    foreach (var skin in skins)
                        if (skin.sharedMesh != null)
                            meshes.Add(new MeshInfo() { mesh = skin.sharedMesh, path = GameObjectInfo.GetGameObjectPath(skin.transform), type = MeshInfo.Type.SkinnedMesh });

                    fbxInfs = meshes.ToArray();
                    Object.DestroyImmediate(goTmp);
                }



                List<string> depsRecList = new List<string>(AssetDatabase.GetDependencies(path, false));
                List<string> depsNoRecList = new List<string>(AssetDatabase.GetDependencies(path, true));
                depsRecList.Remove(path);
                depsRecList.Sort();
                depsNoRecList.Sort();
                dependencyRecurPathes = depsRecList.ToArray();
                dependencyNoRecurPathes = depsNoRecList.ToArray();
                firstInit = true;
            }
            private bool secondInit = false;
            public void SecondInitDependencies(AssetInfo[] assets)
            {
                if (secondInit) return;
                List<string> inUseList = new List<string>(100);
                foreach (var checkedAsset in assets)
                    foreach (var depRecur in checkedAsset.dependencyRecurPathes)
                        if (string.Equals(depRecur, path))
                            inUseList.Add(checkedAsset.path);
                inUsePathes = inUseList.ToArray();

                int count;

                count = dependencyRecurPathes.Length;
                dependencyRecurAssets = new AssetInfo[count];
                for (int i = 0; i < count; ++i)
                    foreach (var asset in assets)
                        if (asset.path == dependencyRecurPathes[i])
                            dependencyRecurAssets[i] = asset;

                count = dependencyNoRecurPathes.Length;
                dependencyNoRecurAssets = new AssetInfo[count];
                for (int i = 0; i < count; ++i)
                    foreach (var asset in assets)
                        if (asset.path == dependencyNoRecurPathes[i])
                            dependencyNoRecurAssets[i] = asset;

                count = inUsePathes.Length;
                inUseAssets = new AssetInfo[count];
                for (int i = 0; i < count; ++i)
                    foreach (var asset in assets)
                        if (asset.path == inUsePathes[i])
                            inUseAssets[i] = asset;

                secondInit = true;
            }
        }

        public class SceneInfo
        {
            public AssetInfo assetInfo;
            public string name { get { return assetInfo.name; } }
            public string path { get { return assetInfo.path; } }
            public GameObjectInfo[] gObjInfs;
            public SceneInfo(AssetInfo sceneAssetInfo, List<AssetInfo> assets)
            {
                assetInfo = sceneAssetInfo;
                Scene scene = EditorSceneManager.OpenScene(assetInfo.path, OpenSceneMode.Additive);
                var objs = scene.GetRootGameObjects();
                List<Transform> trs = new List<Transform>(1000);
                foreach (var obj in objs)
                {
                    trs.AddRange(obj.GetComponentsInChildren<Transform>());
                    MyGC.CheckGC();
                    //break;
                }
                List<GameObjectInfo> gObjInfList = new List<GameObjectInfo>(1000);

                foreach (var tr in trs)
                {
                    gObjInfList.Add(new GameObjectInfo(tr, this/*, assets*/));
                    MyGC.CheckGC();
                }

                //только так, иначе выдаст неверные пути!!!
                foreach (var goInfo in gObjInfList)
                {
                    goInfo.Init(assets);
                    MyGC.CheckGC();
                }

                gObjInfs = gObjInfList.ToArray();
                EditorSceneManager.CloseScene(scene, true);
            }



        }

        public class GameObjectInfo
        {
            public SceneInfo sceneInfo = null;
            public string gObjPath = "";
            public AssetInfo[] dependencyAssets = new AssetInfo[0];
            //public int siblingIndex = -1;
            public string[] missingModels;

            public Object[] meshes = new Object[0];
            public Object[] skinnedMeshes = new Object[0];

            private Transform trTmp;

            public GameObjectInfo(Transform tr, SceneInfo sInfo)
            {
                if (tr == null || sInfo == null)
                    return;
                trTmp = tr;
                //siblingIndex = tr.GetSiblingIndex();
                sceneInfo = sInfo;
                gObjPath = GetGameObjectPath(tr);
            }

            public void Init(List<AssetInfo> assets)
            {

                if (trTmp == null)
                    return;

                MakeAlone(trTmp);

                List<string> missingMeshesList = new List<string>();
                MeshFilter[] mshFltrs = trTmp.GetComponents<MeshFilter>();
                List<Object> mshs = new List<Object>();
                List<Object> skndMshs = new List<Object>();
                //meshes = new Object[count];
                foreach (var filt in mshFltrs)
                    if (filt.sharedMesh != null)
                        mshs.Add(filt.sharedMesh);
                    else
                        missingMeshesList.Add(string.Format("<{0}>:<{1}>", gObjPath, filt.GetType().Name));
                SkinnedMeshRenderer[] sknMsh = trTmp.GetComponents<SkinnedMeshRenderer>();
                foreach (var skin in sknMsh)
                    if (skin.sharedMesh != null)
                        skndMshs.Add(skin.sharedMesh);
                    else
                        missingMeshesList.Add(string.Format("<{0}>:<{1}>", gObjPath, skin.GetType().Name));

                meshes = mshs.ToArray();
                skinnedMeshes = skndMshs.ToArray();
                missingModels = missingMeshesList.ToArray();
                //string[] meshPathes = new string[count];
                //for (int i = 0; i < count; ++i)
                //{
                //    meshPathes[i] = meshes[i] != null ? AssetDatabase.GetAssetPath(meshes[i]) : "";
                //}

                //Object[] depObjs = EditorUtility.CollectDependencies(new Object[] { trTmp.gameObject });

                List<AssetInfo> depAssetsList = new List<AssetInfo>(100);

                //важная вещь
                //AssetInfo assTmp;
                //foreach (var depObj in depObjs)
                //{
                //    if (depObj != null && AssetDatabase.Contains(depObj) && ContainsInAssets(depObj, assets, out assTmp) && !depAssetsList.Contains(assTmp))
                //        depAssetsList.Add(assTmp);
                //    MyGC.CheckGC();
                //}

                //foreach (var asset in assets)
                //    foreach (var path in meshPathes)
                //        if (path == asset.path && !depAssetsList.Contains(asset))
                //            depAssetsList.Add(asset);



                dependencyAssets = depAssetsList.ToArray();

            }

            private bool ContainsInAssets(Object obj, List<AssetInfo> assets, out AssetInfo asset)
            {
                asset = null;
                if (obj == null || assets == null || assets.Count == 0)
                    return false;
                foreach (var fasset in assets)
                    if (obj == fasset.obj)
                    {
                        asset = fasset;
                        return true;
                    }
                return false;
            }
            private bool ContainsInAssets(string path, List<AssetInfo> assets)
            {
                if (string.IsNullOrEmpty(path) || assets == null || assets.Count == 0)
                    return false;
                foreach (var asset in assets)
                    if (path == asset.path)
                        return true;
                return false;
            }

            private void MakeAlone(Transform tr)
            {
                for (int i = tr.childCount - 1; i > -1; --i)
                    tr.GetChild(i).parent = null;
                tr.parent = null;
            }

            public static string GetGameObjectPath(Transform tr)
            {
                string path = "";
                path = GetTransformName(tr);
                while (tr.parent != null)
                {
                    tr = tr.parent;
                    path = string.Format("{0}/{1}", GetTransformName(tr), path);
                }
                return path;
            }
            public static string GetTransformName(Transform tr)
            {
                string trName = tr.name;
                if (tr.parent != null)
                {
                    if (tr.parent.childCount > 1)
                    {
                        foreach (Transform ch in tr.parent)
                            if (ch != tr && ch.name == tr.name)
                            {
                                trName = string.Format("[{0}]{1}", tr.GetSiblingIndex(), trName);
                                break;
                            }
                    }
                }
                return trName;
            }
        }


        public static string GetGlobalProjectPath()
        {
            string assetsPath = Application.dataPath;
            return assetsPath.Substring(0, assetsPath.LastIndexOf("/Assets"));
        }
    }

    public static class MyGC
    {
        static int gcCounter = 0;
        static int gcCount = 500;
        static List<string> gcLog = new List<string>(100);
        static System.DateTime startTime;
        public static void CheckGC(bool forced = false)
        {
            if (forced)
            {
                gcCounter = 0;
                GcCollectWithLog();
                return;
            }
            ++gcCounter;
            if (gcCounter > gcCount)
            {
                gcCounter = 0;
                GcCollectWithLog();
            }
        }
        private static void GcCollectWithLog()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long preBytes = System.GC.GetTotalMemory(false);
            System.GC.Collect();
            long postBytes = System.GC.GetTotalMemory(true);
            watch.Stop();
            long collectBytes = preBytes - postBytes;
            float elapsedSec = (float)watch.Elapsed.TotalSeconds;
            float seconds = (float)System.DateTime.Now.Subtract(startTime).TotalSeconds;
            gcLog.Add(string.Format("{0}: collected {1} b in {2} s", seconds, collectBytes, elapsedSec));
        }
        public static void ResetGCLog()
        {
            gcLog.Clear();
            startTime = System.DateTime.Now;
            gcLog.Add("[" + startTime.ToString() + "] Cleared");
        }
        public static void LogGC()
        {
            string projectPath = GetGlobalProjectPath();
            using (var sv = new StreamWriter(string.Format("{0}/{1}.txt", projectPath, "GCLog")))
            {
                foreach (var str in gcLog)
                    sv.WriteLine(str);
            }
        }
        public static string GetGlobalProjectPath()
        {
            string assetsPath = Application.dataPath;
            return assetsPath.Substring(0, assetsPath.LastIndexOf("/Assets"));
        }
    }



}