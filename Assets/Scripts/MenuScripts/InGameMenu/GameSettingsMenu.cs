using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _musicSlider;

    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        _closeButton?.onClick.AddListener(CloseButton_clicked);
        _musicSlider?.onValueChanged.AddListener(OnMusicSliderValueChanged);

        GameManager.GetInstance().SaveSetted += GameManager_SaveSetted;
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        float volume = GameManager.GetInstance().musicVolume;
        MusicManager.GetInstance().SetCurrentMusicVolume(volume);
        _musicSlider.value = volume;
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        MusicManager.GetInstance().SetCurrentMusicVolume(volume);
    }

    private void CloseButton_clicked()
    {
        GameSave save = GameManager.GetInstance().currentSave;
        SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, save._highScore, save._isNew, save._playerPassword, _musicSlider.value);
        GameManager.GetInstance().SetSave(SaveManagerHandler.Load(save._saveName + ".json"));

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
        GameManager.GetInstance().SaveSetted -= GameManager_SaveSetted;
    }

    private void OnEnable()
    {
        _musicSlider.value = GameManager.GetInstance().musicVolume;
    }
}
