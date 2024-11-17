using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseMenu : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameSettingsMenu _optionsMenu;
    public event EventHandler ContinueButtonClicked;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        _optionsMenu.CloseButtonClicked += OptionsMenu_CloseButtonClicked;
    }

    private void OptionsMenu_CloseButtonClicked(object sender, EventArgs e)
    {
        _optionsMenu.ToggleVisible();
        ToggleVisible();
    }

    private void OnOptionsButtonClicked()
    {
        _optionsMenu.ToggleVisible();
        ToggleVisible();
    }

    private void OnExitButtonClicked()
    {
        Time.timeScale = 1f;
        GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GameIsOnMainMenu);
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
        _optionsButton.onClick.RemoveAllListeners();
        _optionsMenu.CloseButtonClicked -= OptionsMenu_CloseButtonClicked;
    }
}
