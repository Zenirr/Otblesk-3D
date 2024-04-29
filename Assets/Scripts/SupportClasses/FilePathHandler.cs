using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FilePathHandler
{
    /// <summary>
    /// �������� ���� � ����� � ��� ����������� � ���������� ��� ����� �����
    /// </summary>
    /// <param name="filePath"> ���� � ����� � \ � �������� �����������</param>
    /// <returns>string ���������� ��� �����</returns>
    public static string GetFileName(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "���� � ����� �� ����� ���� null ��� ������.");
        }

        // ����������� Path.GetFileName ��� ���������� ����� ����� �� ������� ����
        return Path.GetFileName(filePath);
    }

    /// <summary>
    /// �������� ������ �������� ����� � ���������� ��� ����������
    /// </summary>
    /// <param name="file">������ �������� �����</param>
    /// <returns>string ���������� ���������� ����� ����� � ������</returns>
    public static string GetFileExtension(string file)
    {
        if (string.IsNullOrEmpty(file))
        {
            throw new ArgumentNullException(nameof(file), "���� � ����� �� ����� ���� null ��� ������.");
        }
        return file[file.LastIndexOf('.')..];
    }
}
