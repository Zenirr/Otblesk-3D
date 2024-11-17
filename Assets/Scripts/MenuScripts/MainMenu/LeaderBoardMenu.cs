using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _cancelButton;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _content;
    [SerializeField] private LeaderBoardPanel _panel;

    public event EventHandler<MainMenu.MenuSwitchEventArgs> ButtonClicked;

    private void Start()
    {
        _cancelButton.onClick.AddListener(OnCancelButtonCliked);
        UpdateData();
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void UpdateData()
    {
        foreach (Transform child in _content.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        GetSaveFilesFromFolder();
    }

    private void GetSaveFilesFromFolder()
    {//file уже содержит весь путь к папке сохранения, так что в дальнейшем от ненужной части
     //избавляемся и оставляем только название файла которое включает его расширение
        List<GameSave> list = new List<GameSave>();

        foreach (string file in Directory.GetFiles(SaveManagerHandler.SAVE_FOLDER))
        {
            if (Path.GetExtension(file) == ".json")
            {
                
                list.Add(SaveManagerHandler.Load(Path.GetFileName(file)));
                
            }
        }
        
        list.Sort(delegate (GameSave save1, GameSave save2) { return save2._highScore.CompareTo(save1._highScore); });
        
        int i = 0;
        foreach (GameSave save in list)
        {
            i++;
            LeaderBoardPanel panel = Instantiate(_panel, _content.transform);
            panel.SetValues(save,i);
        }
    }

    private void OnCancelButtonCliked()
    {
        ButtonClicked?.Invoke(this, new MainMenu.MenuSwitchEventArgs() {MenuOff= "LeaderBoardMenu" });
    }

    private void OnDestroy()
    {
        _cancelButton.onClick.RemoveAllListeners();
    }
}
