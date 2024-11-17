using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//¬есь этот компонент нужен дл€ получени€ данных с главного меню о нажатии кнопок
public class MainMenu : MonoBehaviour, IMenu
{

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _playlistsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _changeSaveButton;
    [SerializeField] private Button _leaderBoardButton;
    [SerializeField] private Button _loreButton;
    public class MenuSwitchEventArgs : EventArgs
    {
        public string MenuOff = "MainMenu";
        public string MenuOn = "MainMenu";
    }

    public event EventHandler<MenuSwitchEventArgs> ButtonClicked;


    private static MainMenu Instance;


    public static MainMenu GetInstance()
    {
        return Instance;
    }

    private void InstantiateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        _playButton.onClick.AddListener(PlayButton_clicked);
        _settingsButton.onClick.AddListener(SettingsButton_clicked);
        _exitButton.onClick.AddListener(ExitButton_clicked);
        _playlistsButton.onClick.AddListener(MusicButton_clicked);
        _changeSaveButton.onClick.AddListener(ChangeSaveButton_Clicked);
        _leaderBoardButton.onClick.AddListener(OnLeaderBoardButtonClicked);
        _loreButton.onClick.AddListener(OnLoreBoardButtonClicked);
        InstantiateSingleton();
    }

    #region button actions
    private void OnLoreBoardButtonClicked()
    {
        ButtonClicked.Invoke(this, new MenuSwitchEventArgs() { MenuOn = "LoreMenu", MenuOff = "MainMenu" });
    }

    private void OnLeaderBoardButtonClicked()
    {
        ButtonClicked.Invoke(this, new MenuSwitchEventArgs() { MenuOn = "LeaderBoardMenu", MenuOff = "MainMenu" });
    }

    private void ChangeSaveButton_Clicked()
    {
        ButtonClicked.Invoke(this, new MenuSwitchEventArgs() { MenuOn = "SaveChooseUI", MenuOff = "MainMenu" });
    }

    private void PlayButton_clicked()
    {
        MusicManager.GetInstance().SetMusicPlaylistFromCurrentPath();
        GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GamePlaying);
        SceneLoader.Load(SceneLoader.Scenes.MainGame);

    }

    private void SettingsButton_clicked()
    {
        ButtonClicked.Invoke(this, new MenuSwitchEventArgs() { MenuOn = "SettingsMenu", MenuOff = "MainMenu" });
    }

    private void MusicButton_clicked()
    {
        ButtonClicked.Invoke(this, new MenuSwitchEventArgs() { MenuOn = "MusicMenu", MenuOff = "MainMenu" });
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
