using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GamePaused,
        GamePlaying,
        GameOver,
        GameIsOnMainMenu,
        CutscenePlaying,
    }
    public event EventHandler<StateChangedEventArgs> OnGameStateChanged;
    public event EventHandler SaveSetted;
    public event EventHandler GameOver;

    public class StateChangedEventArgs : EventArgs
    {
        public GameState newState;
    }

    public string saveName { get; private set; }
    public string musicPath { get; private set; }
    public string playerName { get; private set; }
    public float highScore { get; private set; }
    public bool isNew { get; private set; }
    public float musicVolume { get; private set; }
    public string playerPassword { get; private set; }
    public bool useBuiltInMusic { get; private set; }
    public GameSave currentSave { get; private set; }

    public static GameManager Instance { get; private set; }
    public static GameState State { get; private set; }

    [SerializeField] private GameState StateForInspector;

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
        State = GameState.GameIsOnMainMenu;
    }

    public void SetCurrentGameState(GameState state)
    {
        switch (state)
        {
            case GameState.GamePaused:
                OnGameStateChanged?.Invoke(this,new StateChangedEventArgs{ newState = state });
                State = state;
                Time.timeScale = 0f;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.GamePlaying:
                OnGameStateChanged?.Invoke(this, new StateChangedEventArgs { newState = state });
                State = state;
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GameState.GameOver:
                OnGameStateChanged?.Invoke(this, new StateChangedEventArgs { newState = state });
                State = state;
                SaveManagerHandler.Save(saveName, musicPath, playerName, highScore, isNew, playerPassword, musicVolume, useBuiltInMusic);
                GameOver?.Invoke(this, EventArgs.Empty);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.CutscenePlaying:
                OnGameStateChanged?.Invoke(this, new StateChangedEventArgs { newState = state });
                State = state;
                break;
            case GameState.GameIsOnMainMenu:
                OnGameStateChanged?.Invoke(this, new StateChangedEventArgs { newState = state });
                State = state;
                Time.timeScale = 1f;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            default: break;
        }
        Debug.Log(State);
    }

    /// <summary>
    /// Устанавливает сохранение и все необходимые данные из него в текущий GameManager
    /// </summary>
    /// <param name="save"> Устанавливаемое сохранение</param>
    /// <param name="fireTheEvent">Активировать ли событие о установке сохранения? используется так как в глвном меню на этом завязано переключение между гланым меню и меню сохранения</param>
    public void SetSave(GameSave save)
    {
        saveName = save._saveName;
        musicPath = save._musicPath;
        playerName = save._playerName;
        highScore = save._highScore;
        isNew = save._isNew;
        musicVolume = save._musicVolume;
        playerPassword = save._playerPassword;
        useBuiltInMusic = save._useBuiltInPlaylist;
        MusicManager.Instance.SetCurrentMusicVolume(save._musicVolume);
        currentSave = save;

        SaveSetted?.Invoke(this, EventArgs.Empty);
    }
}
