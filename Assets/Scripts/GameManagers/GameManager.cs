using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
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

    private static GameManager Instance;
    public static GameState State { get; private set; }

    [SerializeField] private GameState StateForInspector;


    

    public static GameManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GameManager();
            
        }
        return Instance;
    }
    public void SetCurrentGameState(GameState state)
    {
        switch (state)
        {
            case GameState.GamePaused:
                OnGameStateChanged?.Invoke(this, new StateChangedEventArgs { newState = state });
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
    /// ������������� ���������� � ��� ����������� ������ �� ���� � ������� GameManager
    /// </summary>
    /// <param name="save"> ��������������� ����������</param>
    /// <param name="fireTheEvent">������������ �� ������� � ��������� ����������? ������������ ��� ��� � ������ ���� �� ���� �������� ������������ ����� ������ ���� � ���� ����������</param>
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
        MusicManager.GetInstance().SetCurrentMusicVolume(save._musicVolume);
        currentSave = save;

        SaveSetted?.Invoke(this, EventArgs.Empty);
    }
}
