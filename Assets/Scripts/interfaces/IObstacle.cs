using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IObstacle 
{
    bool _isRotatable { get; set; }
    void OnObstacleHit(GameObject gameObject);
}
