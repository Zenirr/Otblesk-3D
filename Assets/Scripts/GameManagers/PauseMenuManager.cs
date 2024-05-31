using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

    [SerializeField] private GamePauseMenu _pauseMenu;
    [SerializeField] private GameOverMenu _gameOverMenu;

    private void Start()
    {
        InputController.Instance.OnPauseButtonPressed += InputController_OnPauseButtonPressed;
        GameManager.Instance.GameOver += GameManager_GameOver;
        _pauseMenu.ContinueButtonClicked += PauseMenu_ContinueButtonClicked;
    }

    private void PauseMenu_ContinueButtonClicked(object sender, System.EventArgs e)
    {
        Continue();
    }

    private void GameManager_GameOver(object sender, System.EventArgs e)
    {
        _gameOverMenu.ToggleVisible();
        InputController.Instance.OnPauseButtonPressed -= InputController_OnPauseButtonPressed;
    }

    private void InputController_OnPauseButtonPressed(object sender, System.EventArgs e)
    {
        //���� ����� ��� ���������� ��� ���� ����� ������������ ��� �������� � �������� �� ���� ����� ����� ESC
        if (GameManager.State == GameManager.GameState.GamePlaying)
        {
            GameManager.Instance.SetCurrentGameState(GameManager.GameState.GamePaused);
            _pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            Continue();
        }
    }

    public void Continue()
    {
        _pauseMenu.gameObject.SetActive(false);
        GameManager.Instance.SetCurrentGameState(GameManager.GameState.GamePlaying);
    }

    private void OnDestroy()
    {
        InputController.Instance.OnPauseButtonPressed -= InputController_OnPauseButtonPressed;
        GameManager.Instance.GameOver -= GameManager_GameOver;
        _pauseMenu.ContinueButtonClicked -= PauseMenu_ContinueButtonClicked;
    }
}
