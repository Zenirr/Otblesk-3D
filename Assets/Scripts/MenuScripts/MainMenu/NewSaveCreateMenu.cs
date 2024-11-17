using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NewSaveCreateMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _loginInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private CanvasGroup _wrongNameAlert;

    public event EventHandler NewSaveCreated;
    public event EventHandler CancelButtonClicked;

    private void Start()
    {
        _createButton.onClick.AddListener(CreateSave);
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnCancelButtonClicked()
    {
        _loginInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
        _wrongNameAlert.gameObject.SetActive(false);
        CancelButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void CreateSave()
    {
        string username = _loginInputField.text.Trim();
        string password = _passwordInputField.text.Trim();

        if (CheckNameForIssues(username) && CheckPasswordForIssues(password))
        {
            SaveManagerHandler.Save(SaveManagerHandler.STANDART_MUSIC_FOLDER_PATH, username, 0f, password);
            _loginInputField.text = string.Empty;
            _passwordInputField.text = string.Empty;
            NewSaveCreated?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            StartCoroutine(VisabilityChanger(_wrongNameAlert, 1f, 0f));
        }
    }

    private IEnumerator VisabilityChanger(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            percentageComplete /= lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }

    }
    /// <summary>
    /// Возвращает true если с именем всё в порядке, и оно ещё не записано в других сохранениях
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool CheckNameForIssues(string name)
    {
        bool isNameUnique = true;
        foreach (string file in Directory.GetFiles(SaveManagerHandler.SAVE_FOLDER))
        {
            if (Path.GetExtension(file) == ".json")
            {
                isNameUnique = !(SaveManagerHandler.Load(Path.GetFileName(file))._playerName == name.Trim());
                if (!isNameUnique)
                    break;

            }
        }

        return !string.IsNullOrEmpty(name) && (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") || Regex.IsMatch(name, @"^[А-Яа-яЁё_]\w*$")) && isNameUnique && name.Length < 40;
    }
    /// <summary>
    /// Возвращает true если с именем всё в порядке, и оно ещё не записано в других сохранениях
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private bool CheckPasswordForIssues(string password)
    {
        return !string.IsNullOrEmpty(password) && (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") || Regex.IsMatch(name, @"^[А-Яа-яЁё_]\w*$")) && password.Length < 40;
    }

    private void OnDestroy()
    {
        _createButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
