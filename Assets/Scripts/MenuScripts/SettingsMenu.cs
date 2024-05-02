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
    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseButton_clicked);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _toggleStandartPlaylist.onValueChanged.AddListener(OnStandartPLaylistToggle);
        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        float volume = GameManager.Instance.musicVolume;
        MusicManager.Instance.SetCurrentAudioVolume(volume);
        _musicSlider.value = volume;
    }

    private void OnStandartPLaylistToggle(bool newValue)
    {
        GameSave save = GameManager.Instance.currentSave;
        SaveManagerHandler.Save(save._saveName,save._musicPath,save._playerName,save._highScore,save._isNew,save._musicVolume,newValue);
        GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName+".json"));
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        MusicManager.Instance.SetCurrentAudioVolume(volume);
    }

    private void CloseButton_clicked()
    {
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
    }

    private void OnEnable()
    {
        _musicSlider.value = GameManager.Instance.musicVolume;
    }

    private void OnDisable()
    {
        GameSave save = GameManager.Instance.currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, _musicSlider.value);
        GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName+".json"));
    }
}
