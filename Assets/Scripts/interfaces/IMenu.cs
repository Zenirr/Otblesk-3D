using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MainMenu;

public interface IMenu 
{
    void ToggleVisible();
    public event EventHandler<MainMenu.MenuSwitchEventArgs> ButtonClicked;
}
