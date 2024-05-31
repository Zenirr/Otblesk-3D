using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityBuff : MonoBehaviour, IObstacle
{
    [SerializeField] private float invincibilityDuration = 5f;
    public bool _isRotatable { get; set; }
    public void OnObstacleHit(GameObject gameObject)
    {
        Player player = gameObject.GetComponent<Player>();
        player.SetInvincible(invincibilityDuration);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Vehicle vehicle))
        {
            OnObstacleHit(vehicle.gameObject);
        }
    }
}
