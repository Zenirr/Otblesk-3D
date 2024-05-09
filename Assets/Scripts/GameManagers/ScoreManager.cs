using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float currentScore { get; private set; }

    private void Update()
    {
        currentScore -= Time.deltaTime;
    }

    public void AddToCurrentScore(float score)
    {
        currentScore += score;
    }

    public void DropScore() 
    { 
        currentScore = 0;
    }
}
