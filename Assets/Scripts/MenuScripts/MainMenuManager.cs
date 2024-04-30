using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private SettingsMenu _settingsMenu;
    [SerializeField] private PlaylistMenu _musicMenu;
    [SerializeField] private SaveMenu _saveMenu;
    [SerializeField] private LeaderBoardMenu _leaderBoardMenu;

    
    private void Start()
    {
        _mainMenu.MusicButtonClicked += MainMenu_MusicButtonClicked;
        _mainMenu.SettingsButtonClicked += MainMenu_SettingsButtonClicked;
        _mainMenu.ChangeSaveButtonClicked += MainMenu_ChangeSaveButtonClicked;
        _mainMenu.LeaderBoardButtonClicked += MainMenu_LeaderBoardButtonClicked;

        _settingsMenu.CloseButtonClicked += MainMenu_SettingsButtonClicked;
        _musicMenu.CloseButtonClicked += MainMenu_MusicButtonClicked;
        _leaderBoardMenu.CancelButtonClicked += MainMenu_LeaderBoardButtonClicked;
        _saveMenu._saveChooseMenu.SaveChoosed += _saveChooseMenu_SaveChoosed;
    }




    #region click events
    private void MainMenu_LeaderBoardButtonClicked(object sender, EventArgs e)
    {
        _leaderBoardMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }

    private void _saveChooseMenu_SaveChoosed(object sender, EventArgs e)
    {
        _saveMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }

    private void MainMenu_ChangeSaveButtonClicked(object sender, System.EventArgs e)
    {
        _saveMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }

    private void MainMenu_SettingsButtonClicked(object sender, System.EventArgs e)
    {
        _settingsMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }

    private void MainMenu_MusicButtonClicked(object sender, System.EventArgs e)
    {
        _musicMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }
    #endregion

    private void OnDestroy()
    {
        _mainMenu.MusicButtonClicked -= MainMenu_MusicButtonClicked;
        _mainMenu.SettingsButtonClicked -= MainMenu_SettingsButtonClicked;
        _settingsMenu.CloseButtonClicked -= MainMenu_SettingsButtonClicked;
        _musicMenu.CloseButtonClicked -= MainMenu_MusicButtonClicked;
        _mainMenu.ChangeSaveButtonClicked -= MainMenu_ChangeSaveButtonClicked;
        _leaderBoardMenu.CancelButtonClicked -= MainMenu_LeaderBoardButtonClicked;

    }
}
