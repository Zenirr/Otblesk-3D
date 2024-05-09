using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;

    [SerializeField] private MusicPanel _musicPanel;
    [SerializeField] private MusicFileManagerUI _MusicfileManagerUI;
    [SerializeField] private SaveChooseUI _SaveManagerUI;

    private string _currentPlayer;
    private string _currentPlaylistPath;
    private readonly string InitialMusicFolderPath = (Application.dataPath + "/Music/");
    private bool IsMusicPathChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (!Directory.Exists(InitialMusicFolderPath))
        {
            Directory.CreateDirectory(InitialMusicFolderPath);
        }
        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
    }

    private void GameManager_SaveSetted(object sender, EventArgs e)
    {
        _currentPlaylistPath = GameManager.Instance.musicPath;

        if (_currentPlaylistPath != InitialMusicFolderPath)
        {
            UpdateMusicData(_currentPlaylistPath);
        }
        else
        {
            UpdateMusicData(InitialMusicFolderPath);
        }
    }

    #region Get folder content
    private void GetMusicFilesFromFolder(string folderPath)
    {
        Debug.Log(folderPath);
        //В этом foreach переменная file содержит весь путь, включая путь 
        foreach (string file in Directory.GetFiles(folderPath))
        {
            switch (FilePathHandler.GetFileExtension(file))
            {
                case ".mp3":
                    InstantiateMusicPanel(file);
                    break;
                case ".ogg":
                    InstantiateMusicPanel(file);
                    break;
                case ".aiff":
                    InstantiateMusicPanel(file);
                    break;
                case ".wav":
                    InstantiateMusicPanel(file);
                    break;
                case ".mod":
                    InstantiateMusicPanel(file);
                    break;
                case ".it":
                    InstantiateMusicPanel(file);
                    break;
                case ".s3m":
                    InstantiateMusicPanel(file);
                    break;
                case ".xm":
                    InstantiateMusicPanel(file);
                    break;
                default: break;
            }
        }
    }
#endregion

    public void UpdateMusicData(string folderPath)
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        GetMusicFilesFromFolder(folderPath);
        _currentPlaylistPath = folderPath;
    }


    public void GetMusicFromFiles()
    {
        MusicPanel[] panels = _MusicfileManagerUI.GetComponentsInChildren<MusicPanel>(true);

        AudioClip[] clips = new AudioClip[panels.Length];

        Debug.Log(clips.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            StartCoroutine(GetAudioClip(panels[i].MusicPath, clips, i));
        }

        MusicManager.Instance.SetCustomPlaylist(clips);
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
            Debug.LogError("А у вас папки с путём "+folderPath+" не существует!");
        }
    }

    /// <summary>
    /// Функция которая возвращает интерфейс к прошлой папке, будет использоваться в версии с плейлистами
    /// </summary>
    public void BackButtonPressed()
    {
        if (_currentPlaylistPath != InitialMusicFolderPath)
        {
            Debug.Log("Current = " + _currentPlaylistPath + " initial = " + InitialMusicFolderPath);
            string nextFolderPath = Directory.GetParent(_currentPlaylistPath).ToString();
            UpdateMusicData(nextFolderPath);
            Debug.Log(nextFolderPath);
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

    public IEnumerator GetAudioClip(string file,MusicPanel panel)
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
            myClip.name = FilePathHandler.GetFileName(file);
            
            panel.SetAudioClip(myClip);
            MusicManager.Instance.SetCurrentAudio(myClip);
        }
    }

    public IEnumerator GetAudioClip(string file, AudioClip[] clip,int index)
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
            myClip.name = FilePathHandler.GetFileName(file);
            clip[index] = myClip;
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.SaveSetted -= GameManager_SaveSetted;
    }
}
