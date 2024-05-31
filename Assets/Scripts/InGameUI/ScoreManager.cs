using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreVisual;
    public float _currentScore { get; private set; }
    private float _timeCounter;
    private void Update()
    {
        if (_timeCounter < 10)
        {
            _timeCounter+=Time.deltaTime;
            return;
        }
        _currentScore -= Mathf.CeilToInt(_timeCounter)*10;
        _timeCounter = 0;
        UpdateScoreVisual();
    }

    private void UpdateScoreVisual()
    {
        _scoreVisual.text = _currentScore.ToString();
    }

    public void AddToCurrentScore(float score)
    {
        _currentScore += score;
        UpdateScoreVisual();
    }

    public void DropScore() 
    { 
        _currentScore = 0;
    }
}
