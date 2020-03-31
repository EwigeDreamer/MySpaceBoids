using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;
using System.IO;
using System;

[InitializeOnLoad]
public static class AutoLoader
{
    const string tool = "Tools/AutoLoader/";
    const string enable = "Enable";
    const string disable = "Disable";
    const string prefsFileName = "autoloader_asset.prefs";

    static AutoLoader()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (!Enabled) return;
            if (state == PlayModeStateChange.ExitingEditMode) ExitFromEdit();
            if (state == PlayModeStateChange.EnteredPlayMode) EnterToPlay();
            if (state == PlayModeStateChange.EnteredEditMode) EnterToEdit();
        };
    }

    [MenuItem(tool + enable, false, 1)] static void EnableAutoload() => Enabled = true;
    [MenuItem(tool + enable, true)] static bool EnableAutoloadValid() => !Enabled;

    [MenuItem(tool + disable, false, 2)] static void DisableAutoload() => Enabled = false;
    [MenuItem(tool + disable, true)] static bool DisableAutoloadValid() => Enabled;

    static void ExitFromEdit()
    {
        if (!ESM.SaveCurrentModifiedScenesIfUserWantsTo())
        { CancelPlayMode = true; return; }
        SceneSetups = ESM.GetSceneManagerSetup();
        ESM.OpenScene(SceneUtility.GetScenePathByBuildIndex(0), OpenSceneMode.Single);
    }
    static void EnterToPlay()
    {
        if (!CancelPlayMode) return;
        CancelPlayMode = false;
        EditorApplication.isPlaying = false;
    }
    static void EnterToEdit()
    {
        ESM.RestoreSceneManagerSetup(SceneSetups);
        SceneSetups = null;
    }

    static bool Enabled
    {
        get => GetPrefs().enabled;
        set
        {
            var prefs = GetPrefs();
            prefs.enabled = value;
            SetPrefs(prefs);
        }
    }
    static bool CancelPlayMode
    {
        get => GetPrefs().cancel;
        set
        {
            var prefs = GetPrefs();
            prefs.cancel = value;
            SetPrefs(prefs);
        }
    }
    static SceneSetup[] SceneSetups
    {
        get => GetPrefs().setups;
        set
        {
            var prefs = GetPrefs();
            prefs.setups = value;
            SetPrefs(prefs);
        }
    }

    [System.Serializable]
    class AutoLoaderPrefs
    {
        public bool enabled = false;
        public bool cancel = false;
        public SceneSetup[] setups = null;
    }

    static AutoLoaderPrefs GetPrefs()
    {
        var path = GetPrefsFilePath();
        if (!File.Exists(path)) return new AutoLoaderPrefs();
        var prefs = JsonUtility.FromJson<AutoLoaderPrefs>(File.ReadAllText(path));
        if (prefs == null) return new AutoLoaderPrefs();
        return prefs;
    }
    static void SetPrefs(AutoLoaderPrefs prefs)
    {
        var path = GetPrefsFilePath();
        var json = JsonUtility.ToJson(prefs, true);
        File.WriteAllText(path, json);
    }

    static string GetProjectPath()
    {
        string path = Application.dataPath;
        return path.Substring(0, path.Length - "/Assets".Length);
    }

    static string GetPrefsFilePath() => Path.Combine(GetProjectPath(), prefsFileName);
}

