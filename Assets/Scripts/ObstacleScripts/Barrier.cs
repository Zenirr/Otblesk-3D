using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour, IObstacle
{
    public void OnObstacleHit()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Vehicle>(out Vehicle PlayerVehicle))
        {
            GameManager.Instance.SetCurrentGameState(GameManager.GameState.GameOver);
        }
    }
}
