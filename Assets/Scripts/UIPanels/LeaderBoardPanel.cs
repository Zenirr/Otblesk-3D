using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardPanel : MonoBehaviour, IPanel
{
    [SerializeField] private TextMeshProUGUI _tMPName;
    [SerializeField] private TextMeshProUGUI _tMPScore;
    [SerializeField] private TextMeshProUGUI _tMPPlace;

    //������� �� ����� ���� 1, � ����� ���� 0
    public void SetValues(GameSave save,int place)
    {
        _tMPName.text = "�����: "+save._playerName;
        _tMPScore.text = "����: "+save._highScore.ToString();
        _tMPPlace.text = place.ToString();
    }
}
