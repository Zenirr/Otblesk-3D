using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private EnvironmentRoadGenerator _roadGenerator;
    public float currentScore { get; private set; }

    public void AddToCurrentScore(float score)
    {
        currentScore += score;
    }

    public void DropScore() 
    { 
        currentScore = 0;
    }
}
