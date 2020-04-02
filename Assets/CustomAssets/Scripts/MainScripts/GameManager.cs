using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public static class GameManager
{
    public static Coroutine StartGame()
    {
        return CorouWaiter.Start(Routine());
        IEnumerator Routine()
        {
            yield return SceneLoadingManager.LoadGame(onCompleteLoad: () => InitGame());
        }
        void InitGame()
        {
            GameView.I.Init();
            GameView.I.Player.OnAllPlanetsCaptured += () => PopupManager.OpenPopup<EndLevelPopup>().SetWindow(true);
            PauseManager.PauseEnabled = true;
        }
    }

    public static Coroutine StopGame()
    {
        return CorouWaiter.Start(Routine());
        IEnumerator Routine()
        {
            PauseManager.PauseEnabled = false;
            yield return SceneLoadingManager.LoadMenu();
        }
    }
}
