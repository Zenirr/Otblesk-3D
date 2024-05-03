using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IObstacle 
{
    bool isRotatable { get; set; }
    void OnObstacleHit(GameObject gameObject);
    GameObject CurrentObstacleGameObject { get; set; }
    IObstacle ReturnAsObstacle();
}
