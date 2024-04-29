using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _closeButton;

    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseButton_clicked);
    }

    private void CloseButton_clicked()
    {
        CloseButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
    }
}
