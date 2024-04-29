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

    private void Start()
    {
        _mainMenu.MusicButtonClicked += MainMenu_MusicButtonClicked;
        _mainMenu.SettingsButtonClicked += MainMenu_SettingsButtonClicked;
        _mainMenu.ChangeSaveButtonClicked += MainMenu_ChangeSaveButtonClicked;

        _settingsMenu.CloseButtonClicked += MainMenu_SettingsButtonClicked;
        _musicMenu.CloseButtonClicked += MainMenu_MusicButtonClicked;
        GameManager.Instance.SaveSetted += GameManager_SaveSetted;

        if (!string.IsNullOrEmpty(GameManager.Instance.saveName))
        {
            GameManager_SaveSetted(this,EventArgs.Empty);
        }
    }

    #region click events
    private void GameManager_SaveSetted(object sender, System.EventArgs e)
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

    }
}
