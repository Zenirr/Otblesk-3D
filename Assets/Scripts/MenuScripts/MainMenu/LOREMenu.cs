using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class LOREMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _closeButton;

    public event EventHandler CloseButtonClicked;

    private void Start()
    {
        _closeButton.onClick.AddListener(OnCloseClickButton);
    }

    private void OnCloseClickButton()
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
