using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class SaveChooseUI : MonoBehaviour,IMenu
{
    [SerializeField] private NewSaveCreateMenu _newSaveCreateMenu;
    [SerializeField] private SavePanel _savePanel;
    [SerializeField] private Button _createSaveButton;
    [SerializeField] private GameObject _savePanelsContentHolder;
    [SerializeField] private Button _exitButton;

    public event EventHandler SaveChoosed;
    private void Start()
    {
        if (!Directory.Exists(SaveManagerHandler.SAVE_FOLDER))
        {
            Directory.CreateDirectory(SaveManagerHandler.SAVE_FOLDER);
        }
        UpdateData();
        _createSaveButton.onClick.AddListener(SaveButtonOnClick);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void SaveButtonOnClick()
    {
        _newSaveCreateMenu.ToggleVisible();
        ToggleVisible();
    }

    public void UpdateData()
    {
        foreach (Transform child in _savePanelsContentHolder.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        GetSaveFilesFromFolder();
    }

    private void GetSaveFilesFromFolder()
    {//файл уже содержит весь путь к папке сохранени€, так что в дальнейшем от ненужной части
     //избавл€емс€ и оставл€ем только название файла которое включает его расширение
        foreach (string file in Directory.GetFiles(SaveManagerHandler.SAVE_FOLDER))
        {
            if (FilePathHandler.GetFileExtension(file) == ".json")
            {
                SavePanel savePanel = Instantiate(_savePanel, _savePanelsContentHolder.transform);
                savePanel.SetValues(SaveManagerHandler.Load(FilePathHandler.GetFileName(file)));
                //прот€гиваем костыль по отключению дальше
                savePanel.SaveChoosed += SavePanel_SaveChoosed;
            }
        }
    }

    private void SavePanel_SaveChoosed(object sender, System.EventArgs e)
    {
        SaveChoosed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _createSaveButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
        foreach (SavePanel sp in _savePanelsContentHolder.transform.GetComponentsInChildren<SavePanel>())
        {
            sp.SaveChoosed -= SavePanel_SaveChoosed;
        }
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
