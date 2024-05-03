using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes{
        MainGame,
        ArcadeMachineRoom
    }

    public static event EventHandler SceneChanged;

    public static void Load(Scenes scene){
        SceneManager.LoadScene(scene.ToString());
        SceneChanged?.Invoke(null,EventArgs.Empty);
    }
}
