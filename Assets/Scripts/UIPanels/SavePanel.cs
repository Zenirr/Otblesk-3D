using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour, IPanel
{
    [SerializeField] private Button _chooseButton;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _highScore;
    private GameSave _gameSave;

    public event EventHandler SaveChoosed;

    private void Start()
    {
        _chooseButton.onClick.AddListener(ChooseButtonPressed);
    }

    public void SetValues(GameSave save) 
    {
        _gameSave = save;
        _highScore.text = "Рекорд: " + save._highScore.ToString();
        _userName.text = save._playerName;
    }

    private void ChooseButtonPressed() 
    {
        GameManager.Instance.SetSave(_gameSave);
        //вот эта строка кода является костылём который протянется аж до менеджера главного меню
        SaveChoosed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _chooseButton.onClick.RemoveAllListeners();
    }
}
