using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveChooseUI : MonoBehaviour,IMenu
{
    [SerializeField] private NewSaveCreateMenu _newSaveCreateMenu;
    [SerializeField] private SavePanel _savePanel;
    [SerializeField] private Button _createSaveButton;
    [SerializeField] private GameObject _savePanelsContentHolder;

    private void Start()
    {
        if (!Directory.Exists(SaveManagerHandler.SAVE_FOLDER))
        {
            Directory.CreateDirectory(SaveManagerHandler.SAVE_FOLDER);
        }
        UpdateData();
        _createSaveButton.onClick.AddListener(SaveButtonOnClick);
        
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
    {
        foreach (string file in Directory.GetFiles(SaveManagerHandler.SAVE_FOLDER))
        {
            if (FilePathHandler.GetFileExtension(file) == ".json")
            {
                SavePanel savePanel = Instantiate(_savePanel, _savePanelsContentHolder.transform);
                savePanel.SetValues(SaveManagerHandler.Load(file));
            }
        }
    }

    private void OnDestroy()
    {
        _createSaveButton.onClick.RemoveAllListeners();
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
