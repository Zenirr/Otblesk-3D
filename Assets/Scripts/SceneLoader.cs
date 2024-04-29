using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes{
        MainGame,
        MainMenu
    }

    public static void Load(Scenes scene){
        SceneManager.LoadScene(scene.ToString());
    }
}
