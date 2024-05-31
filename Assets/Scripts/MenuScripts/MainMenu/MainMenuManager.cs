using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private SettingsMenu _settingsMenu;
    [SerializeField] private PlaylistMenu _playlistMenu;
    [SerializeField] private SaveMenu _saveMenu;
    [SerializeField] private LeaderBoardMenu _leaderBoardMenu;
    [SerializeField] private LOREMenu _loreMenu;
    private void Start()
    {
        _mainMenu.MusicButtonClicked += MainMenu_MusicButtonClicked;
        _mainMenu.SettingsButtonClicked += MainMenu_SettingsButtonClicked;
        _mainMenu.ChangeSaveButtonClicked += MainMenu_ChangeSaveButtonClicked;
        _mainMenu.LeaderBoardButtonClicked += MainMenu_LeaderBoardButtonClicked;
        _mainMenu.PlayButtonClicked += MainMenu_PlayButtonClicked;
        _mainMenu.LoreButtonClicked += MainMenu_LoreButtonClicked;
        _settingsMenu.CloseButtonClicked += MainMenu_SettingsButtonClicked;
        _settingsMenu.SaveDeleted += SettingsMenu_SaveDeleted;
        _playlistMenu.CloseButtonClicked += MainMenu_MusicButtonClicked;
        _leaderBoardMenu.CancelButtonClicked += MainMenu_LeaderBoardButtonClicked;
        _saveMenu._saveChooseMenu.SaveChoosed += SaveChooseMenu_SaveChoosed;
        _loreMenu.CloseButtonClicked += LoreMenu_CloseButtonClicked;


        if (GameManager.Instance.currentSave != null)
        {
            _saveMenu.ToggleVisible();
            _mainMenu.ToggleVisible();
        }
    }


    private void MainMenu_PlayButtonClicked(object sender, EventArgs e)
    {
        MusicManager.Instance.SetMusicPlaylistFromCurrentPath();
    }

    private void SettingsMenu_SaveDeleted(object sender, EventArgs e)
    {
        _saveMenu.ToggleVisible();
        _settingsMenu.ToggleVisible();
    }

    #region click events
    private void MainMenu_LoreButtonClicked(object sender, EventArgs e)
    {
        _loreMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }
    private void MainMenu_LeaderBoardButtonClicked(object sender, EventArgs e)
    {
        _leaderBoardMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }

    private void LoreMenu_CloseButtonClicked(object sender, EventArgs e)
    {

        _mainMenu.ToggleVisible();
        _loreMenu.ToggleVisible();
    }

    private void SaveChooseMenu_SaveChoosed(object sender, EventArgs e)
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
        _playlistMenu.ToggleVisible();
        _mainMenu.ToggleVisible();
    }
    #endregion

    private void OnDestroy()
    {
        _mainMenu.MusicButtonClicked -= MainMenu_MusicButtonClicked;
        _mainMenu.SettingsButtonClicked -= MainMenu_SettingsButtonClicked;
        _settingsMenu.CloseButtonClicked -= MainMenu_SettingsButtonClicked;
        _playlistMenu.CloseButtonClicked -= MainMenu_MusicButtonClicked;
        _mainMenu.ChangeSaveButtonClicked -= MainMenu_ChangeSaveButtonClicked;
        _leaderBoardMenu.CancelButtonClicked -= MainMenu_LeaderBoardButtonClicked;
        _saveMenu._saveChooseMenu.SaveChoosed -= SaveChooseMenu_SaveChoosed;
        _loreMenu.CloseButtonClicked -= LoreMenu_CloseButtonClicked;
    }
}
