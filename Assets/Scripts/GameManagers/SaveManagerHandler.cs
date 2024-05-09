using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManagerHandler
{
    public static readonly string STANDART_MUSIC_FOLDER_PATH = Application.dataPath + "/Music/";
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    public static readonly string SAVE_NAME = "NewSave";

    /// <summary>
    /// Используется для создания нового сохранения
    /// </summary>
    /// <param name="musicPath">Строка пути к музыки выбранной пользователем</param>
    /// <param name="playerName">Строка имени пользователя</param>
    /// <param name="highScore">Лучший счёт игрока</param>
    public static void Save(string musicPath, string playerName, float highScore, string playerPassword, float musicVolume = 0.1f, bool useBuiltInPlaylist = false)
    {
        int saveCount = 1;

        while (File.Exists(SAVE_FOLDER + SAVE_NAME + saveCount + ".json"))
        {
            saveCount++;

        }

        GameSave gameSave = new GameSave()
        {
            _saveName = SAVE_NAME + saveCount,
            _musicPath = musicPath,
            _playerName = playerName,
            _highScore = highScore,
            _isNew = true,
            _musicVolume = musicVolume,
            _useBuiltInPlaylist = useBuiltInPlaylist,
            _playerPassword = playerPassword
        };

        string resultPath = SAVE_FOLDER + SAVE_NAME + saveCount + ".json";
        Debug.Log(resultPath);

        string jsonString = JsonUtility.ToJson(gameSave);

        File.WriteAllText(resultPath, jsonString);
    }

    /// <summary>
    /// Использоуется для перезаписи уже существующего сохранения
    /// </summary>
    /// <param name="saveName">Название сохранения без расширения</param>
    /// <param name="musicPath">Строка пути к музыки выбранной пользователем</param>
    /// <param name="playerName">Строка имени пользователя</param>
    /// <param name="highScore">Лучший счёт игрока</param>
    public static void Save(string saveName, string musicPath, string playerName, float highScore, bool isNew, string playerPassword, float musicVolume = 0.1f, bool useBuiltInPlaylist = false)
    {
        GameSave gameSave = new GameSave()
        {
            _saveName = saveName,
            _musicPath = musicPath,
            _playerName = playerName,
            _highScore = highScore,
            _isNew = isNew,
            _musicVolume = musicVolume,
            _useBuiltInPlaylist = useBuiltInPlaylist,
            _playerPassword = playerPassword

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
    /// Находит в папке сохранения файл с названием нужного сохранения, расширение .json обязательно дописывать!
    /// </summary>
    /// <param name="saveName">Название файла сохранения с расширением .json</param>
    /// <returns></returns>
    public static GameSave Load(string saveName)
    {
        string jsonString = File.ReadAllText(SAVE_FOLDER + saveName);
        return JsonUtility.FromJson<GameSave>(jsonString);
    }



    public static void DeleteCurrentSave(string saveName)
    {
        try
        {
            File.Delete(SAVE_FOLDER + saveName);
        }
        catch 
        {
            return;
        }
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
    public bool _useBuiltInPlaylist;
    public string _playerPassword;
}
