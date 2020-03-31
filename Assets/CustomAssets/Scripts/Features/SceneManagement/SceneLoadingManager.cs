using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyTools.Singleton;
using MyTools.SceneManagement;
using System;
using MyTools.Extensions.Components;
using MyTools.Extensions.Coroutines;
using System.Collections.ObjectModel;
using MyTools.Helpers;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SceneLoadingManager
{
    public static ReadOnlyCollection<SceneKind> MainMenuScenes { get; } = new ReadOnlyCollection<SceneKind>(new[]
    {
        SceneKind.MainMenuWorld,
        SceneKind.MainMenuUI,
    });
    public static ReadOnlyCollection<SceneKind> GameScenes { get; } = new ReadOnlyCollection<SceneKind>(new[]
    {
        SceneKind.GameWorld,
        SceneKind.GameUI,
    });

    public static Coroutine LoadScenes(IList<SceneKind> kinds)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            var operations = new List<Coroutine>(kinds.Count);
            var sceneWrappers = new List<SceneWrapper>(kinds.Count);
            foreach (var kind in kinds)
            {
                var wrapper = SceneWrapper.GetWrapper(kind);
                operations.Add(wrapper.Load());
                sceneWrappers.Add(wrapper);
            }
            foreach (var operation in operations) yield return operation;
            sceneWrappers[0].Active();
            yield break;
        }
    }

    public static Coroutine UnloadScenes(IList<SceneKind> kinds)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            var operations = new List<Coroutine>(kinds.Count);
            foreach (var kind in kinds)
            {
                var wrapper = SceneWrapper.GetWrapper(kind);
                operations.Add(wrapper.Unload());
            }
            foreach (var operation in operations) yield return operation;
            yield break;
        }
    }

    public static Coroutine UnloadAll()
    {
        var loadedKinds = new List<SceneKind>();
        SceneWrapper.GetLoaded(loadedKinds);
        loadedKinds.Remove(SceneData.immortalSceneKind);
        return UnloadScenes(loadedKinds);
    }

    public static Coroutine FirstLoad()
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            yield return ShowWaitScreen(true);
            yield return UnloadAll();
            yield return LoadScenes(MainMenuScenes);
            yield return HideWaitScreen();
        }
    }

    public static Coroutine LoadMenu(Action onStartLoad = null, Action onCompleteLoad = null)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            yield return ShowWaitScreen();
            onStartLoad?.Invoke();
            yield return UnloadAll();
            yield return LoadScenes(MainMenuScenes);
            onCompleteLoad?.Invoke();
            yield return HideWaitScreen();
        }
    }
    public static Coroutine LoadGame(Action onStartLoad = null, Action onCompleteLoad = null)
    {
        return CorouWaiter.Start(GetRoutine());
        IEnumerator GetRoutine()
        {
            yield return ShowWaitScreen();
            onStartLoad?.Invoke();
            yield return UnloadAll();
            yield return LoadScenes(GameScenes);
            onCompleteLoad?.Invoke();
            yield return HideWaitScreen();
        }
    }

    private static Coroutine ShowWaitScreen(bool forced = false) => WaitScreen.I.Show(forced);
    private static Coroutine HideWaitScreen(bool forced = false) => WaitScreen.I.Hide(forced);
}
