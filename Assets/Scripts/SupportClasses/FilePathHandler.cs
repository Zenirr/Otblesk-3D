using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FilePathHandler
{
    /// <summary>
    /// Получает путь к файлу с его расширением и возвращает имя этого файла
    /// </summary>
    /// <param name="filePath"> Путь к файлу с \ в качестве разделителя</param>
    /// <returns>string содержащий имя файла</returns>
    public static string GetFileName(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "Путь к файлу не может быть null или пустым.");
        }

        // Используйте Path.GetFileName для извлечения имени файла из полного пути
        return Path.GetFileName(filePath);
    }

    /// <summary>
    /// Получает полное название файла и возвращает его расширение
    /// </summary>
    /// <param name="file">Полное название файла</param>
    /// <returns>string содержащий расширение этого файла с точкой</returns>
    public static string GetFileExtension(string file)
    {
        if (string.IsNullOrEmpty(file))
        {
            throw new ArgumentNullException(nameof(file), "Путь к файлу не может быть null или пустым.");
        }
        return file[file.LastIndexOf('.')..];
    }
}
