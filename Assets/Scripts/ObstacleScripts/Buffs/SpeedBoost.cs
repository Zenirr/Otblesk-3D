using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpeedBoost : MonoBehaviour, IObstacle
{
    public GameObject CurrentObstacleGameObject { get => this.gameObject; set => CurrentObstacleGameObject = this.gameObject; }
    public bool _isRotatable { get; set; }
    [SerializeField] private float speedBoost = 100;
    public void OnObstacleHit(GameObject gameObject)
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.AddForce(speedBoost * (body.transform.localRotation * Vector3.forward), ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle vehicle))
        {
            OnObstacleHit(vehicle.gameObject);
        }
    }
}
