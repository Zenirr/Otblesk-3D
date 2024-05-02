using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlaylistMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _changeMusicFolderButton;
    [SerializeField] private TMP_InputField _folderInputField;

    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        if (GameManager.Instance.currentSave != null)
        {
            _folderInputField.text = GameManager.Instance.musicPath;
        }
        else
        {
            GameManager.Instance.SaveSetted += GameManager_SaveSetted;
        }
        _closeButton.onClick.AddListener(CloseButton_clicked);
        _changeMusicFolderButton.onClick.AddListener(ChangeFolder);
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        _folderInputField.text = GameManager.Instance.musicPath;
    }

    private void CloseButton_clicked()
    {
        CloseButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeFolder()
    {
        GameSave save = GameManager.Instance.currentSave;

        if (Directory.Exists(_folderInputField.text))
        {
            SaveManagerHandler.Save(save._saveName, _folderInputField.text, save._playerName, save._highScore, save._isNew, save._playerPassword, save._musicVolume, save._useBuiltInPlaylist);
        }
        else
        {
            return;
        }

        FileManager.Instance.UpdateMusicData(_folderInputField.text);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        GameManager.Instance.SaveSetted -= GameManager_SaveSetted;
    }
}
