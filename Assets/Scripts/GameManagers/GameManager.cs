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

    public event EventHandler SaveSetted;

    public string saveName { get; private set; }
    public string musicPath { get; private set; }
    public string playerName { get; private set; }
    public float highScore { get; private set; }

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

    public void SetCurrentGameState(GameState state)
    {
        switch (state)
        {
            case GameState.GamePaused:
                State = state;
                break;
            case GameState.GamePlaying:
                State = state;
                break;
            case GameState.GameOver:
                State = state;
                SaveManagerHandler.Save(saveName, musicPath, playerName, highScore);
                break;
            case GameState.CutscenePlaying:
                State = state;
                break;
            case GameState.GameIsOnMainMenu:
                State = state;
                break;
            default: break;
        }
        StateForInspector = State;
    }

    public void SetSave(GameSave save)
    {
        saveName = save._saveName;
        musicPath = save._musicPath;
        playerName = save._playerName;
        highScore = save._highScore;
        SaveSetted?.Invoke(this, EventArgs.Empty);
    }
}
