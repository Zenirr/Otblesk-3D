using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour, IObstacle
{
    public GameObject CurrentObstacleGameObject { get => this.gameObject; set => CurrentObstacleGameObject = this.gameObject; }
    public bool isRotatable{get;set;}

    public void OnObstacleHit(GameObject gameObject)
    {
    }

    public IObstacle ReturnAsObstacle()
    {
        return this;
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
