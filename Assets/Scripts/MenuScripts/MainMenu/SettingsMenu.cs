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

    public event EventHandler CloseButtonClicked;
    public event EventHandler SaveDeleted;

    private void Start()
    {
        _toggleStandartPlaylist.isOn = GameManager.Instance.useBuiltInMusic;
        _closeButton?.onClick.AddListener(CloseButton_clicked);
        _musicSlider?.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _toggleStandartPlaylist?.onValueChanged.AddListener(OnStandartPLaylistToggle);
        _DeleteSave?.onClick.AddListener(OnSaveDeleteButtonPressed);
        _areYouSureMenu.SaveDeleted += AreYouSureMenu_SaveDeleted;
        

        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
    }

    private void AreYouSureMenu_SaveDeleted(object sender, EventArgs e)
    {
        _areYouSureMenu.ToggleVisible();
        SaveDeleted?.Invoke(this, new EventArgs());
    }

    private void OnSaveDeleteButtonPressed()
    {
        _areYouSureMenu.ToggleVisible();
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        float volume = GameManager.Instance.musicVolume;
        MusicManager.Instance.SetCurrentMusicVolume(volume);
        _musicSlider.value = volume;
    }

    private void OnStandartPLaylistToggle(bool newValue)
    {
        GameSave save = GameManager.Instance.currentSave;
        SaveManagerHandler.Save(save._saveName,save._musicPath,save._playerName,save._highScore,save._isNew,save._playerPassword,_musicSlider.value, _toggleStandartPlaylist.isOn);
        GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName+".json"));
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        MusicManager.Instance.SetCurrentMusicVolume(volume);
        
    }

    private void CloseButton_clicked()
    {
        GameSave save = GameManager.Instance.currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, save._playerPassword, _musicSlider.value, _toggleStandartPlaylist.isOn);
        GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName + ".json"));
        Debug.Log(_toggleStandartPlaylist.isOn);
        CloseButtonClicked?.Invoke(this, EventArgs.Empty);
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
        GameManager.Instance.SaveSetted -= GameManager_SaveSetted;
        _DeleteSave.onClick.RemoveAllListeners();
        _areYouSureMenu.SaveDeleted -= AreYouSureMenu_SaveDeleted;
    }

    private void OnEnable()
    {
        _musicSlider.value = GameManager.Instance.musicVolume;
    }
}
