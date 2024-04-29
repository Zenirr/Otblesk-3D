using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SaveMenu : MonoBehaviour,IMenu
{
    [SerializeField] private NewSaveCreateMenu _newSaveMenu;
    [SerializeField] private SaveChooseUI _saveChooseMenu;

    private void Start()
    {
        _newSaveMenu.NewSaveCreated += NewSaveMenu_NewSaveCreated;
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
