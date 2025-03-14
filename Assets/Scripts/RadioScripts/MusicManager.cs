using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ����� �������� �� ���������� ������ � � ������������, �������� singleton'�� � ������� �� ��������� ������ 
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

    private static MusicManager Instance;

    [SerializeField] private string[] _customPlaylistPaths;
    [SerializeField] private AudioClip[] _buildInPlaylist;
    
    // ENCAPSULATION
    public MusicState _currentState { get; private set; }
    public NAudioDecoder _audioDecoder { get; private set; }
    
    private AudioSource _audioSource;
    public string _currentPlaylistPath;
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

    public static MusicManager GetInstance()
    {
        return Instance;
    }

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            _audioDecoder = GetComponent<NAudioDecoder>();
            _currentState = MusicState.MusicIsPaused;
            GameManager.GetInstance().SaveSetted += GameManager_SaveSetted;
        }
    }

    public void SetMusicPlaylistFromCurrentPath()
    {
        if (GameManager.GetInstance().useBuiltInMusic)
        {
            _currentPlaylistPath = string.Empty;
            _customPlaylistPaths = new string[0];
            PlayNextTrack();
        }
        else
        {
            _currentPlaylistPath = string.Empty;
            _customPlaylistPaths = new string[0];

            _currentPlaylistPath = GameManager.GetInstance().musicPath;
            List<string> clips = GetMusicFilesPaths(_currentPlaylistPath);
            SetCustomPlaylist(clips);
            PlayNextTrack();
        }
    }

    private List<string> GetMusicFilesPaths(string folderPath)
    {
        List<string> musicPaths = new List<string>();
        foreach (string file in Directory.GetFiles(folderPath))
        {
            switch (Path.GetExtension(file))
            {
                case ".mp3":
                    musicPaths.Add(file);
                    break;
                default: break;
            }
        }
        return musicPaths;
    }

    private void GameManager_SaveSetted(object sender, System.EventArgs e)
    {
        SetCurrentMusicVolume(GameManager.GetInstance().musicVolume);
        if (!(GameManager.State == GameManager.GameState.GamePlaying || GameManager.State == GameManager.GameState.GamePaused || GameManager.State == GameManager.GameState.GameOver))
        {
            _customPlaylistPaths = null;
            _currentPlaylistPath = GameManager.GetInstance().currentSave._musicPath;
            _currentTrackIndex = 0;
            if (GameManager.GetInstance().useBuiltInMusic)
                SetCurrentAudio(_buildInPlaylist[Random.Range(0, _buildInPlaylist.Length - 1)]);
            else
                SetMusicPlaylistFromCurrentPath();
        }
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
        StopAndStartTimeCoroutineCoroutine(_audioSource.clip.length);
        _currentState = MusicState.MusicIsPaused;
        _audioSource.Pause();
    }

    private void ContinueMusic()
    {
        _currentState = MusicState.MusicIsPlaying;
        _audioSource.Play();
        StopAndStartTimeCoroutineCoroutine(_audioSource.clip.length - _audioSource.time);
    }
    #endregion

    public void SetCurrentMusicVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    /// <summary>
    /// �������� ������� ���� �� ��������� ���������
    /// </summary>
    public void PlayPreviousTrack()
    {
        if (GameManager.GetInstance().useBuiltInMusic)
        {
            if (0 < _currentTrackIndex)
            {
                _currentTrackIndex--;
                SetCurrentAudio(_buildInPlaylist[_currentTrackIndex]);
            }
            else
            {
                _currentTrackIndex = _buildInPlaylist.Length - 1;
                SetCurrentAudio(_buildInPlaylist[_currentTrackIndex]);
            }
        }
        else
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
    }

    /// <summary>
    /// �������� ��������� ���� �� ���������
    /// </summary>
    public void PlayNextTrack()
    {
        if (GameManager.GetInstance().useBuiltInMusic)
        {
            if (_buildInPlaylist.Length > _currentTrackIndex + 1)
            {
                _currentTrackIndex++;
                SetCurrentAudio(_buildInPlaylist[_currentTrackIndex]);
            }
            else if (_buildInPlaylist.Length == _currentTrackIndex + 1)
            {
                _currentTrackIndex = 0;
                SetCurrentAudio(_buildInPlaylist[_currentTrackIndex]);
            }
        }
        else
        {
            if (_customPlaylistPaths is null)
                return;
            if (_customPlaylistPaths.Length > _currentTrackIndex + 1)
            {
                _currentTrackIndex++;
                Debug.Log(_customPlaylistPaths[_currentTrackIndex]);
                SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
            }
            else if (_customPlaylistPaths.Length == _currentTrackIndex + 1)
            {
                _currentTrackIndex = 0;
                Debug.Log(_customPlaylistPaths[_currentTrackIndex]);
                SetCurrentAudio(_customPlaylistPaths[_currentTrackIndex]);
            }
        }
    }

    /// <summary>
    /// ������������� ����� ���� ���������� �� ����������� ���� � ������������ � ������� ���������� ������.
    /// </summary>
    /// <param name="clipPath">���� �� ����� �����</param>
    public void SetCurrentAudio(string clipPath)
    {
        switch (_currentState)
        {
            case MusicState.MusicIsPlaying:
                GetAndSetAudioClipToAudioSource(clipPath);
                _currentState = MusicState.MusicIsPlaying;

                Debug.Log("Music was Switched to " + clipPath);
                break;
            case MusicState.MusicIsPaused:
                GetAndSetAudioClipToAudioSource(clipPath);

                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was paused, but now Switched to " + clipPath);
                break;
            default: break;
        }
    }

    /// <summary>
    /// ������������� ����� ���� ���������� �� ����������� ���� � ������������ � ������� ���������� ������.
    /// </summary>
    /// <param name="clip">���� �� ����� �����</param>
    public void SetCurrentAudio(AudioClip clip)
    {
        switch (_currentState)
        {
            case MusicState.MusicIsPlaying:
                PauseContinueMusic();
                _audioSource.clip = clip;
                _audioSource.Play();
                StopAndStartTimeCoroutineCoroutine(clip.length);
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Switched!");
                break;
            case MusicState.MusicIsPaused:
                _audioSource.clip = clip;
                _audioSource.Play();
                StopAndStartTimeCoroutineCoroutine(clip.length);
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Paused, but now is Playing!");
                break;
            default: break;
        }
    }

    private void SetCustomPlaylist(List<string> clipPaths)
    {
        _customPlaylistPaths = null;
        _customPlaylistPaths = clipPaths.ToArray();
        _currentTrackIndex = 0;
    }

    private IEnumerator MusicPlayingTimer(float audioClipLength)
    {
        Debug.Log(audioClipLength);

        yield return new WaitForSeconds(audioClipLength);

        PlayNextTrack();
    }

    private void StopAndStartTimeCoroutineCoroutine(float musicTime)
    {
        if (CurrentMusicTimeCourutine != null)
            StopCoroutine(CurrentMusicTimeCourutine);

        CurrentMusicTimeCourutine = StartCoroutine(MusicPlayingTimer(musicTime));
    }

    /// <summary>
    /// �������� ���� � ���������� � ��������� ��� � ������� audioSource
    /// </summary>
    /// <param name="filepath"> ���� � ����� �����</param>
    /// <returns></returns>
    public async void GetAndSetAudioClipToAudioSource(string filepath)
    {
        _audioDecoder.Import(filepath);
        while (!_audioDecoder.isInitialized && !_audioDecoder.isError)
        {
            await Task.Yield();
        }
        if (_audioDecoder.isError)
        {
            Debug.LogError(_audioDecoder.error);
        }
        _audioSource.clip = _audioDecoder.audioClip;
        _audioSource.Play();

        StopAndStartTimeCoroutineCoroutine(_audioDecoder.audioClip.length);
    }

}
