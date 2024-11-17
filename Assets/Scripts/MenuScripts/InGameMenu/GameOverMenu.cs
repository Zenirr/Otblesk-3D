using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnExitButtonClicked()
    {
        GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GameIsOnMainMenu);
        SceneLoader.Load(SceneLoader.Scenes.ArcadeMachineRoom);
    }

    private void OnRestartButtonClicked()
    {
        GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GamePlaying);
        SceneLoader.Load(SceneLoader.Scenes.MainGame);
        Debug.Log(Time.timeScale);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
