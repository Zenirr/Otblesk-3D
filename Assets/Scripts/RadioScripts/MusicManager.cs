using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Этот класс отвечает за содержание музыки и её проигрывание, является singleton'ом и задаётся на начальном экране 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public enum MusicState
    {
        MusicIsPlaying,
        MusicIsFading,
        MusicIsPaused,
        MusicIsStarts
    }

    public static MusicManager Instance;

    [SerializeField] private string[] _customPlaylistPaths;
    [SerializeField] private string[] _customPlaylistAudioclips;
    [SerializeField] private AudioClip[] _buildInPlaylist;

    public MusicState _currentState { get; private set; }
    private AudioSource _audioSource;
    private float _startVolume;
    private int _currentTrackIndex;
    private Coroutine CurrentMusicTimeCourutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _currentState = MusicState.MusicIsPaused;
        _startVolume = _audioSource.volume;
        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
    }


    private void GameManager_SaveSetted(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.useBuiltInMusic)
            SetCurrentAudio(_buildInPlaylist[Random.Range(0, _buildInPlaylist.Length - 1)]);
        else if (_customPlaylistPaths.Length > 0)
            SetCurrentAudio(_customPlaylistPaths[Random.Range(0, _customPlaylistPaths.Length - 1)]);
    }

    #region pause and continue Methods

    public void PauseContinueMusic()
    {
        if (_currentState == MusicState.MusicIsStarts || _currentState == MusicState.MusicIsPlaying || _currentState == MusicState.MusicIsFading)
        {
            PauseMusic();
        }
        else if (_currentState == MusicState.MusicIsPaused)
        {
            ContinueMusic();
        }
    }

    private void PauseMusic()
    {
        _currentState = MusicState.MusicIsPaused;
        _audioSource.Pause();
    }

    private void ContinueMusic()
    {
        _currentState = MusicState.MusicIsPlaying;
        _audioSource.Play();
        CurrentMusicTimeCourutine = StartCoroutine(MusicPlayingTimer(_audioSource.clip.length - _audioSource.time));
    }
    #endregion

    public void SetCurrentAudioVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    /// <summary>
    /// Включает прошлый трек из заданного плейлиста
    /// </summary>
    public void PlayPreviousTrack()
    {
        if (_customPlaylistPaths is null)
            return;
        if (0 < _currentTrackIndex)
        {
            _currentTrackIndex--;
            SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
        }
        else
        {
            _currentTrackIndex = _customPlaylistPaths.Length - 1;
            SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
        }
    }

    /// <summary>
    /// Включает следующий трек из плейлиста
    /// </summary>
    public void PlayNextTrack()
    {
        if (_customPlaylistPaths is null)
            return;
        if (_customPlaylistPaths.Length > _currentTrackIndex + 1)
        {
            _currentTrackIndex++;
            SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
        }
        else if (_customPlaylistPaths.Length == _currentTrackIndex + 1)
        {
            _currentTrackIndex = 0;
            SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
        }
    }

    /// <summary>
    /// Устанавливают аудио файл находящийс по переданному пути в соответствии с текущим состоянием музыки.
    /// </summary>
    /// <param name="clipPath">Путь до аудио файла</param>
    public void SetCurrentAudio(string clipPath)
    {
        switch (_currentState)
        {
            case MusicState.MusicIsPlaying:
                StartCoroutine(GetAndSetAudioClipToAudioSource(clipPath));
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Switched to " +clipPath);
                break;
            case MusicState.MusicIsPaused:
                StartCoroutine(GetAndSetAudioClipToAudioSource(clipPath));
                
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Paused, but now is Playing!");
                break;
            default: break;
        }
    }

    /// <summary>
    /// Устанавливают аудио файл находящийс по переданному пути в соответствии с текущим состоянием музыки.
    /// </summary>
    /// <param name="clip">Путь до аудио файла</param>
    public void SetCurrentAudio(AudioClip clip)
    {
        switch (_currentState)
        {
            case MusicState.MusicIsPlaying:
                PauseContinueMusic();
                _audioSource.clip = clip;
                _audioSource.Play();
                StartCoroutine(MusicPlayingTimer(clip.length));
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Switched!");
                break;
            case MusicState.MusicIsPaused:
                _audioSource.clip = clip;
                _audioSource.Play();
                StartCoroutine(MusicPlayingTimer(clip.length));
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Paused, but now is Playing!");
                break;
            default: break;
        }
    }

    public void SetCustomPlaylist(List<string> clipPaths)
    {
        _customPlaylistPaths = null;
        _customPlaylistPaths = clipPaths.ToArray();
        _currentTrackIndex = 0;
    }

    private IEnumerator MusicPlayingTimer(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        PauseContinueMusic();

        PlayNextTrack();
    }

    /// <summary>
    /// Получает путь к аудиофайлу и запускает его в текущем audioSource
    /// </summary>
    /// <param name="filepath"> Путь к аудио файлу</param>
    /// <returns></returns>
    public IEnumerator GetAndSetAudioClipToAudioSource(string filepath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            myClip.LoadAudioData();
            myClip.name = Path.GetFileName(filepath);
            _audioSource.clip = myClip;
            StartCoroutine(MusicPlayingTimer(myClip.length));
            _audioSource.Play();
        }
    }

}
