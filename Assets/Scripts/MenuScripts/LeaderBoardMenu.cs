using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardMenu : MonoBehaviour, IMenu
{
    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
