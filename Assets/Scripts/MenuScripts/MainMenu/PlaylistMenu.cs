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

    public event EventHandler CloseButtonClicked;
    private string _currentPlaylistPath;
    private readonly string InitialMusicFolderPath = (Application.dataPath + "/Music/");
    private bool IsMusicPathChanged;

    private void Start()
    {
        if (!Directory.Exists(InitialMusicFolderPath))
        {
            Directory.CreateDirectory(InitialMusicFolderPath);
        }

        if (GameManager.Instance.currentSave != null)
        {
            string musicPath = GameManager.Instance.musicPath;
            _folderInputField.text = musicPath;
            _currentPlaylistPath = musicPath; 
            GameManager.Instance.SaveSetted += GameManager_SaveSetted;
        }

        UpdateMusicData(_folderInputField.text);

        _closeButton.onClick.AddListener(CloseButton_clicked);
        _changeMusicFolderButton.onClick.AddListener(ChangeFolder);
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        _folderInputField.text = GameManager.Instance.musicPath;
        if (!GameManager.Instance.currentSave._useBuiltInPlaylist)
        {
            UpdateMusicData(_folderInputField.text);
        }
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

        UpdateMusicData(_folderInputField.text);
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

    #region Взял из FileManager
    #region Get folder content
    private void SetMusicPanels(string folderPath)
    {
        Debug.Log(folderPath);
        //В этом foreach переменная file содержит весь путь, включая путь 
        foreach (string file in Directory.GetFiles(folderPath))
        {
            switch (Path.GetExtension(file))
            {
                case ".mp3":
                    InstantiateMusicPanel(file);
                    break;
                default: break;
            }
        }
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


    public void ChangeMusicFolder(string folderPath)
    {
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(InitialMusicFolderPath);
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogError("А у вас папки с путём " + folderPath + " не существует!");
        }
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

    public IEnumerator GetAudioClip(string file, MusicPanel panel)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            myClip.LoadAudioData();
            myClip.name = Path.GetFileName(file);

            panel.SetAudioClip(myClip);
            MusicManager.Instance.SetCurrentAudio(myClip);
        }
    }

    public IEnumerator GetAudioClip(string file, AudioClip[] clip, int index)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            myClip.LoadAudioData();
            myClip.name = Path.GetFileName(file);
            clip[index] = myClip;
        }
    }
    #endregion
}
