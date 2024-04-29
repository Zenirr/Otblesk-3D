using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private AudioClip[] _customPlaylist;

    public MusicState _currentState { get; private set; }
    private AudioSource _audioSource;
    private Playlist _standartPlaylist;
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
    
    /// <summary>
    /// Включает прошлый трек из заданного плейлиста
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
    /// Включает следующий трек из плейлиста
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
    /// Устанавливают переданный аудио файл в соответствии с текущим состоянием музыки.
    /// </summary>
    /// <param name="clip">Аудио файл</param>
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
