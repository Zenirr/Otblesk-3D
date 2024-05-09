using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardPanel : MonoBehaviour, IPanel
{
    [SerializeField] private TextMeshProUGUI _tMPName;
    [SerializeField] private TextMeshProUGUI _tMPScore;
    [SerializeField] private TextMeshProUGUI _tMPPlace;

    //страшно не когда теб€ 1, а когда теб€ 0
    public void SetValues(GameSave save,int place)
    {
        _tMPName.text = "»грок: "+save._playerName;
        _tMPScore.text = "—чЄт: "+save._highScore.ToString();
        _tMPPlace.text = place.ToString();
    }
}
