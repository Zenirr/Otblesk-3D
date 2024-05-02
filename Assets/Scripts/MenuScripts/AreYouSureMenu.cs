using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreYouSureMenu : MonoBehaviour, IMenu
{
    [SerializeField] private TMP_InputField _passwordTMP;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _diclineButton;

    public event EventHandler SaveDeleted;

    private void Start()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        _diclineButton.onClick.AddListener(OnDiclineButtonClicked);
    }

    private void OnDiclineButtonClicked()
    {
        _passwordTMP.text = string.Empty;
        ToggleVisible();
    }

    private void OnConfirmButtonClicked()
    {
        string password = _passwordTMP.text.Trim();
        GameSave save = GameManager.Instance.currentSave;
        if (CheckPassword(password) && save._playerPassword == password)
        {
            SaveManagerHandler.DeleteCurrentSave(save._saveName + ".json");
            SaveDeleted.Invoke(this,EventArgs.Empty);
        }
        _passwordTMP.text = string.Empty;
    }

    private bool CheckPassword(string password)
    {
        return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"^[a-zA-Z_Р-пр-џЈИ]\w*$");
    }

    private void OnDestroy()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
