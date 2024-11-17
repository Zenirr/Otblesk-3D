using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Toggle _toggleStandartPlaylist;
    [SerializeField] private Button _DeleteSave;
    [SerializeField] private AreYouSureMenu _areYouSureMenu;


    public event EventHandler<MainMenu.MenuSwitchEventArgs> ButtonClicked;

    private void Start()
    {
        _toggleStandartPlaylist.isOn = GameManager.GetInstance().useBuiltInMusic;
        _closeButton?.onClick.AddListener(CloseButton_clicked);
        _musicSlider?.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _toggleStandartPlaylist?.onValueChanged.AddListener(OnStandartPLaylistToggle);
        _DeleteSave?.onClick.AddListener(OnSaveDeleteButtonPressed);
        _areYouSureMenu.SaveDeleted += AreYouSureMenu_ButtonClicked;


        GameManager.GetInstance().SaveSetted += GameManager_SaveSetted;
    }


    private void AreYouSureMenu_ButtonClicked(object sender, EventArgs e)
    {
        ButtonClicked?.Invoke(this, new MainMenu.MenuSwitchEventArgs() { MenuOff = "SettingsMenu" });
    }

    private void OnSaveDeleteButtonPressed()
    {
        _areYouSureMenu.ToggleVisible();
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        float volume = GameManager.GetInstance().musicVolume;
        MusicManager.GetInstance().SetCurrentMusicVolume(volume);
        _musicSlider.value = volume;
    }

    private void OnStandartPLaylistToggle(bool newValue)
    {
        GameSave save = GameManager.GetInstance().currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, save._playerPassword, _musicSlider.value, _toggleStandartPlaylist.isOn);
        GameManager.GetInstance().SetSave(SaveManagerHandler.Load(save._saveName + ".json"));
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        MusicManager.GetInstance().SetCurrentMusicVolume(volume);
    }

    private void CloseButton_clicked()
    {
        GameSave save = GameManager.GetInstance().currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, save._playerPassword, _musicSlider.value, _toggleStandartPlaylist.isOn);
        GameManager.GetInstance().SetSave(SaveManagerHandler.Load(save._saveName + ".json"));
        Debug.Log(_toggleStandartPlaylist.isOn);
        ButtonClicked?.Invoke(this, new MainMenu.MenuSwitchEventArgs() { MenuOff = "SettingsMenu" });
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _musicSlider.onValueChanged.RemoveAllListeners();
        _toggleStandartPlaylist.onValueChanged.RemoveAllListeners();
        GameManager.GetInstance().SaveSetted -= GameManager_SaveSetted;
        _DeleteSave.onClick.RemoveAllListeners();
        _areYouSureMenu.SaveDeleted -= AreYouSureMenu_ButtonClicked;
    }

    private void OnEnable()
    {
        _musicSlider.value = GameManager.GetInstance().musicVolume;
    }
}
