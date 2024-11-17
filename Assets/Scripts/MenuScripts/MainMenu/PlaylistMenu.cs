using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class PlaylistMenu : MonoBehaviour, IMenu
{
    [Header("Компоненты основного меню музыки")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _changeMusicFolderButton;
    [SerializeField] private TMP_InputField _folderInputField;
    [Header("Компоненты для списка треков")]
    [SerializeField] private MusicPanel _musicPanel;
    [SerializeField] private MusicFileManagerUI _MusicfileManagerUI;


    public event EventHandler<MainMenu.MenuSwitchEventArgs> ButtonClicked;
    private string _currentPlaylistPath;
    private readonly string InitialMusicFolderPath = (Application.dataPath + "/Music/");
    private bool IsMusicPathChanged;

    private void Start()
    {
        if (!Directory.Exists(InitialMusicFolderPath))
        {
            Directory.CreateDirectory(InitialMusicFolderPath);
        }

        if (GameManager.GetInstance().currentSave != null)
        {
            string musicPath = GameManager.GetInstance().musicPath;
            _folderInputField.text = musicPath;
            _currentPlaylistPath = musicPath;
            GameManager.GetInstance().SaveSetted += GameManager_SaveSetted;
        }

        UpdateMusicData(_folderInputField.text);

        _closeButton.onClick.AddListener(CloseButton_clicked);
        _changeMusicFolderButton.onClick.AddListener(ChangeFolder);
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        _folderInputField.text = GameManager.GetInstance().musicPath;
        if (!GameManager.GetInstance().currentSave._useBuiltInPlaylist)
        {
            UpdateMusicData(_folderInputField.text);
        }
    }

    private void CloseButton_clicked()
    {
        ButtonClicked?.Invoke(this, new MainMenu.MenuSwitchEventArgs() { MenuOff = "MusicMenu"});
    }

    public void ChangeFolder()
    {
        GameSave save = GameManager.GetInstance().currentSave;
        Debug.Log(Directory.Exists(_folderInputField.text));
        if (Directory.Exists(_folderInputField.text))
        {
            SaveManagerHandler.Save(save._saveName, _folderInputField.text, save._playerName, save._highScore, save._isNew, save._playerPassword, save._musicVolume, save._useBuiltInPlaylist);
            GameManager.GetInstance().SetSave(SaveManagerHandler.Load(save._saveName + ".json"));
            MusicManager.GetInstance().SetMusicPlaylistFromCurrentPath();
        }
        else
        {
            return;
        }

        UpdateMusicData(_folderInputField.text);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        GameManager.GetInstance().SaveSetted -= GameManager_SaveSetted;
    }

    #region Взял из FileManager
    #region Get folder content
    private void SetMusicPanels(string folderPath)
    {
        Debug.Log("File.Exists(folderPath) "+File.Exists(folderPath));
        //В этом foreach переменная file содержит весь путь, включая путь 
        if (Directory.Exists(folderPath))
            foreach (string file in Directory.GetFiles(folderPath))
            {
                Debug.Log("Получил расширение");
                switch (Path.GetExtension(file))
                {
                    case ".mp3":
                        InstantiateMusicPanel(file);

                        Debug.Log("Получил file " + file);
                        break;
                    default: break;
                }
            }
        else
            return;
    }
    #endregion

    public void UpdateMusicData(string folderPath)
    {
        foreach (Transform child in _MusicfileManagerUI.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        _currentPlaylistPath = folderPath;
        Debug.Log(_currentPlaylistPath);
        SetMusicPanels(folderPath);
    }

    /// <summary>
    /// Создаёт музыкальную панель и устанавливает значения ей
    /// </summary>
    /// <param name="filePath"></param>
    private void InstantiateMusicPanel(string filePath)
    {
        MusicPanel panel = Instantiate(_musicPanel, _MusicfileManagerUI.transform);
        panel.SetValues(filePath);
    }

    #endregion
}
