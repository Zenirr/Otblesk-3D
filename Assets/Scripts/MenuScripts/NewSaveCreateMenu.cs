using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NewSaveCreateMenu : MonoBehaviour, IMenu
{
    [SerializeField] private TMP_InputField _inputField;
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
        _inputField.text = "";
        CancelButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void CreateSave()
    {
        string userName = _inputField.text.Trim();

        if (CheckNameForIssues(userName))
        {
            SaveManagerHandler.Save(SaveManagerHandler.STANDART_MUSIC_FOLDER_PATH, userName, 0f);
            _inputField.text = string.Empty;
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
            timeSinceStarted = Time.time - timeStartedLerping;
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
            if (FilePathHandler.GetFileExtension(file) == ".json")
            {
                isNameUnique = !(SaveManagerHandler.Load(file)._playerName == name.Trim());
            }
        }

        return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") && isNameUnique;
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
