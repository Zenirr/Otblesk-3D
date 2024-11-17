using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class LOREMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _closeButton;

    public event EventHandler<MainMenu.MenuSwitchEventArgs> ButtonClicked;

    private void Start()
    {
        _closeButton.onClick.AddListener(OnCloseClickButton);
    }

    private void OnCloseClickButton()
    {
        ButtonClicked?.Invoke(this, new MainMenu.MenuSwitchEventArgs() { MenuOff = "LoreMenu" });

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
