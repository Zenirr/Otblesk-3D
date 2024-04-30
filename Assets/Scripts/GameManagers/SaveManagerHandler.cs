using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Playables;
using UnityEngine;

public static class SaveManagerHandler
{
    public static readonly string STANDART_MUSIC_FOLDER_PATH = Application.dataPath + "/Music/";
    public static readonly string SAVE_FOLDER = Application.persistentDataPath+ "/Saves/";
    public static readonly string SAVE_NAME = "NewSave";

    /// <summary>
    /// ������������ ��� �������� ������ ����������
    /// </summary>
    /// <param name="musicPath">������ ���� � ������ ��������� �������������</param>
    /// <param name="playerName">������ ����� ������������</param>
    /// <param name="highScore">������ ���� ������</param>
    public static void Save(string musicPath , string playerName, float highScore,float musicVolume = 0.3f)
    {
        int saveCount = 1;

        while (File.Exists(SAVE_FOLDER+SAVE_NAME+saveCount+".json"))
        {
            saveCount++;

        }

        GameSave gameSave = new GameSave()
        {
            _saveName = SAVE_NAME+saveCount,
            _musicPath = musicPath,
            _playerName = playerName,
            _highScore = highScore,
            _isNew = true,
            _musicVolume = musicVolume
        };
        
        string resultPath = SAVE_FOLDER + SAVE_NAME + saveCount + ".json";
        Debug.Log(resultPath);

        string jsonString = JsonUtility.ToJson(gameSave);
        
        File.WriteAllText(resultPath, jsonString);
    }

    /// <summary>
    /// ������������� ��� ���������� ��� ������������� ����������
    /// </summary>
    /// <param name="saveName">�������� ���������� ��� ����������</param>
    /// <param name="musicPath">������ ���� � ������ ��������� �������������</param>
    /// <param name="playerName">������ ����� ������������</param>
    /// <param name="highScore">������ ���� ������</param>
    public static void Save(string saveName, string musicPath, string playerName, float highScore,bool isNew,float musicVolume = 0.3f)
    {
        GameSave gameSave = new GameSave()
        {
            _saveName = saveName,
            _musicPath = musicPath,
            _playerName = playerName,
            _highScore = highScore,
            _isNew = isNew,
            _musicVolume = musicVolume
        };

        string resultPath = SAVE_FOLDER + saveName + ".json";

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
            File.Create(resultPath);
        }

        string jsonString = JsonUtility.ToJson(gameSave);
        File.WriteAllText(resultPath, jsonString);
    }

    /// <summary>
    /// ������� � ����� ���������� ���� � ��������� ������� ����������,
    /// </summary>
    /// <param name="saveName">�������� ����� ���������� � ����������� .json</param>
    /// <returns></returns>
    public static GameSave Load(string saveName)
    {
        string jsonString = File.ReadAllText(SAVE_FOLDER+saveName);
        return JsonUtility.FromJson<GameSave>(jsonString);
    }
}

public class GameSave
{
    public string _saveName;
    public string _musicPath;
    public string _playerName;
    public float _highScore;
    public bool _isNew;
    public float _musicVolume;
}
