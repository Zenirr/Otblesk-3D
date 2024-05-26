using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour,IMenu
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _musicSlider;

    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        _closeButton?.onClick.AddListener(CloseButton_clicked);
        _musicSlider?.onValueChanged.AddListener(OnMusicSliderValueChanged);

        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        float volume = GameManager.Instance.musicVolume;
        MusicManager.Instance.SetCurrentMusicVolume(volume);
        _musicSlider.value = volume;
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        MusicManager.Instance.SetCurrentMusicVolume(volume);
    }

    private void CloseButton_clicked()
    {
        GameSave save = GameManager.Instance.currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, save._playerPassword, _musicSlider.value);
        GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName + ".json"));

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
        GameManager.Instance.SaveSetted -= GameManager_SaveSetted;
    }

    private void OnEnable()
    {
        _musicSlider.value = GameManager.Instance.musicVolume;
    }
}
