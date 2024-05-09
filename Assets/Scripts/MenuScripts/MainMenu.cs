using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//¬есь этот компонент нужен дл€ получени€ данных с главного меню о нажатии кнопок
public class MainMenu : MonoBehaviour,IMenu
{
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _playlistsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _changeSaveButton;
    [SerializeField] private Button _leaderBoardButton;

    public event EventHandler SettingsButtonClicked;
    public event EventHandler MusicButtonClicked;
    public event EventHandler ChangeSaveButtonClicked;
    public event EventHandler LeaderBoardButtonClicked;

    private void Start()
    {
        _playButton.onClick.AddListener(PlayButton_clicked);
        _settingsButton.onClick.AddListener(SettingsButton_clicked);
        _exitButton.onClick.AddListener(ExitButton_clicked);
        _playlistsButton.onClick.AddListener(MusicButton_clicked);
        _changeSaveButton.onClick.AddListener(ChangeSaveButton_Clicked);
        _leaderBoardButton.onClick.AddListener(OnLeaderBoardButtonClicked);
    }

    #region button actions
    private void OnLeaderBoardButtonClicked()
    {
        LeaderBoardButtonClicked.Invoke(this, EventArgs.Empty);
    }

    private void ChangeSaveButton_Clicked()
    {
        ChangeSaveButtonClicked?.Invoke(this,EventArgs.Empty);
    }

    private void PlayButton_clicked()
    {
        FileManager.Instance.GetMusicFromFiles();
        GameManager.Instance.SetCurrentGameState(GameManager.GameState.GamePlaying);
        SceneLoader.Load(SceneLoader.Scenes.MainGame);
    }

    private void SettingsButton_clicked()
    {
        SettingsButtonClicked?.Invoke(this,EventArgs.Empty);
    }

    private void MusicButton_clicked()
    {
        MusicButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    private void ExitButton_clicked()
    {
        Application.Quit();
    }
    #endregion

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf); 
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners(); 
        _playlistsButton.onClick.RemoveAllListeners();
        _changeSaveButton.onClick.RemoveAllListeners();
    }
}
