using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SaveMenu : MonoBehaviour,IMenu
{
    [SerializeField] private NewSaveCreateMenu _newSaveMenu;
    [field: SerializeField] public SaveChooseUI _saveChooseMenu { get; private set; }

    private void Start()
    {
        _newSaveMenu.NewSaveCreated += NewSaveMenu_NewSaveCreated;
        _newSaveMenu.CancelButtonClicked += NewSaveMenu_CancelButtonClicked;
    }

    private void NewSaveMenu_CancelButtonClicked(object sender, System.EventArgs e)
    {
        _newSaveMenu.ToggleVisible();
        _saveChooseMenu.ToggleVisible();
    }

    private void NewSaveMenu_NewSaveCreated(object sender, System.EventArgs e)
    {
        _newSaveMenu.ToggleVisible();
        _saveChooseMenu.ToggleVisible();
        _saveChooseMenu.UpdateData();
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        _newSaveMenu.NewSaveCreated -= NewSaveMenu_NewSaveCreated;
    }
}
