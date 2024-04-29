using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;

    private void Start()
    {
        InputController.Instance.OnPauseButtonPressed += InputController_OnPauseButtonPressed;
    }

    private void InputController_OnPauseButtonPressed(object sender, System.EventArgs e)
    {
        if (GameManager.State == GameManager.GameState.GamePlaying)
        {
            GameManager.Instance.SetCurrentGameState(GameManager.GameState.GamePaused);
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
        }
        else
        {
            Continue();
        }
    }

    public void Continue()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameManager.Instance.SetCurrentGameState(GameManager.GameState.GamePlaying);
    }

    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Load(SceneLoader.Scenes.MainGame);
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.SetCurrentGameState(GameManager.GameState.GameIsOnMainMenu);
        SceneLoader.Load(SceneLoader.Scenes.MainMenu);
    }

    private void OnDestroy()
    {
        InputController.Instance.OnPauseButtonPressed -= InputController_OnPauseButtonPressed;
    }
}
