using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    public event EventHandler ContinueButtonClicked;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnExitButtonClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.SetCurrentGameState(GameManager.GameState.GameIsOnMainMenu);
        SceneLoader.Load(SceneLoader.Scenes.ArcadeMachineRoom);
    }

    private void OnContinueButtonClicked()
    {
        ContinueButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _continueButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}
