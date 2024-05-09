using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public static MusicManager Instance;

    [SerializeField] private AudioClip[] _customPlaylist;
    [SerializeField] private AudioClip[] _buildInPlaylist;
    public MusicState _currentState { get; private set; }
    private AudioSource _audioSource;
    private bool _useStandartPlaylist;
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
        _audioSource = GetComponent<AudioSource>();
        _currentState = MusicState.MusicIsPaused;
        _startVolume = _audioSource.volume;
        GameManager.Instance.SaveSetted += GameManager_SaveSetted;
        SceneLoader.SceneChanged += SceneLoader_SceneChanged;
    }

    private void SceneLoader_SceneChanged(object sender, System.EventArgs e)
    {
    }

    private void GameManager_SaveSetted(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.useBuiltInMusic)
            SetCurrentAudio(_buildInPlaylist[Random.Range(0, _buildInPlaylist.Length - 1)]);
        else if (_customPlaylist.Length > 0)
            SetCurrentAudio(_customPlaylist[Random.Range(0, _customPlaylist.Length - 1)]);
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
    /// �������� ������� ���� �� ��������� ���������
    /// </summary>
    public void PlayPreviousTrack()
    {
        if (_customPlaylist is null)
            return;
        if (0 < _currentTrackIndex)
        {
            _currentTrackIndex--;
            AudioClip clip = _customPlaylist[_currentTrackIndex];
            SetCurrentAudio(_customPlaylist[_currentTrackIndex]);
            StartCoroutine(MusicPlayingTimer(clip.length));
        }
        else
        {
            _currentTrackIndex = _customPlaylist.Length - 1;
            AudioClip clip = _customPlaylist[_currentTrackIndex];
            SetCurrentAudio(_customPlaylist[_currentTrackIndex]);
            StartCoroutine(MusicPlayingTimer(clip.length));
        }
    }

    /// <summary>
    /// �������� ��������� ���� �� ���������
    /// </summary>
    public void PlayNextTrack()
    {
        if (_customPlaylist is null)
            return;
        if (_customPlaylist.Length > _currentTrackIndex + 1)
        {
            _currentTrackIndex++;
            AudioClip clip = _customPlaylist[_currentTrackIndex];
            SetCurrentAudio(clip);
            StartCoroutine(MusicPlayingTimer(clip.length));
        }
        else if (_customPlaylist.Length == _currentTrackIndex + 1)
        {
            _currentTrackIndex = 0;
            AudioClip clip = _customPlaylist[_currentTrackIndex];
            SetCurrentAudio(clip);
            StartCoroutine(MusicPlayingTimer(clip.length));
        }
    }

    /// <summary>
    /// ������������� ���������� ����� ���� � ������������ � ������� ���������� ������.
    /// </summary>
    /// <param name="clip">����� ����</param>
    public void SetCurrentAudio(AudioClip clip)
    {
        switch (_currentState)
        {
            case MusicState.MusicIsPlaying:
                _audioSource.clip = clip;
                _audioSource.Play();
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Switched!");
                break;
            case MusicState.MusicIsPaused:
                _audioSource.clip = clip;
                _audioSource.Play();
                _currentState = MusicState.MusicIsPlaying;
                Debug.Log("Music was Paused, but now is Playing!");
                break;
            default: break;
        }
    }

    public void SetCustomPlaylist(AudioClip[] clips)
    {
        _customPlaylist = null;
        _customPlaylist = clips;
        _currentTrackIndex = 0;
    }

    private IEnumerator MusicPlayingTimer(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        PauseContinueMusic();

        PlayNextTrack();
    }



}
