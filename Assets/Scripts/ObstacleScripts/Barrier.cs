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
        if (collision.gameObject.TryGetComponent(out Vehicle PlayerVehicle))
        {
            if (PlayerVehicle.TryGetComponent(out Rigidbody machineRigidbody) && machineRigidbody.velocity.magnitude > 1)
            {
                GameManager.Instance.SetCurrentGameState(GameManager.GameState.GameOver);
            }
        }
    }
}
