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
    [SerializeField] private SaveChooseUI _saveChooseUI;

    Dictionary<string, IMenu> menuNames;

    IMenu[] menus;
    
    private void Start()
    {
        menuNames = new Dictionary<string, IMenu> {
            { "MainMenu", _mainMenu },
            { "SettingsMenu", _settingsMenu },
            { "MusicMenu", _playlistMenu },
            { "LeaderBoardMenu", _leaderBoardMenu },
            { "LoreMenu", _loreMenu },
            { "SaveChooseUI",_saveChooseUI}
        };

        menus = GetComponentsInChildren<IMenu>(true);

        SubscribeToEvents(menus);

        if (GameManager.GetInstance().currentSave != null)
        {
            _saveMenu.ToggleVisible();
            _mainMenu.ToggleVisible();
        }
    }

    private void SubscribeToEvents(IMenu[] menus, bool subscribe = true)
    {
        if (subscribe)
        {
            foreach (IMenu menu in menus)
            {
                menu.ButtonClicked += SwitchMenu_ButtonClicked;
            }
        }
        else
        {
            foreach (IMenu menu in menus)
            {
                menu.ButtonClicked -= SwitchMenu_ButtonClicked;
            }
        }
    }

    private void SwitchMenu_ButtonClicked(object sender, MainMenu.MenuSwitchEventArgs e)
    {
        Debug.Log("off "+ menuNames.GetValueOrDefault(e.MenuOff)+ " on "+ menuNames.GetValueOrDefault(e.MenuOn));
        menuNames.GetValueOrDefault(e.MenuOff).ToggleVisible();
        menuNames.GetValueOrDefault(e.MenuOn).ToggleVisible();
    }

    private void OnDestroy()
    {
        SubscribeToEvents(menus, false);
    }

}