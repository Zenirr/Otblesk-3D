using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour, IObstacle
{
    [SerializeField] private float _speedToGameOver = 3f;
    [SerializeField] private BarrierColliderObject[] colliderObjects;
    public GameObject CurrentObstacleGameObject { get => this.gameObject; set => CurrentObstacleGameObject = this.gameObject; }
    public bool _isRotatable { get; set; }
    float _speedOnTriggerEnter = 0;
    bool _isSpeedSetted = false;
    private void Start()
    {
        foreach (BarrierColliderObject collider in colliderObjects)
        {
            collider.ColliderTouched += Collider_ColliderTouched;
        }
    }

    private void Collider_ColliderTouched(object sender, System.EventArgs e)
    {
        if (_speedOnTriggerEnter > _speedToGameOver && GameManager.State != GameManager.GameState.GameOver)
            GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GameOver);
    }

    public void OnObstacleHit(GameObject gameObject)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody machineRigidbody) && !_isSpeedSetted)
        {
            _speedOnTriggerEnter = machineRigidbody.linearVelocity.magnitude;
            if (_speedOnTriggerEnter > 3)
            {
                _isSpeedSetted = true;
            }

            if (machineRigidbody.gameObject.TryGetComponent(out Player player) && player.isInvincible)
            {
                _speedOnTriggerEnter = 0;
                Debug.Log(machineRigidbody.linearVelocity.magnitude + " - �������� ������ ������");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _isSpeedSetted = false;
    }

    private void OnDestroy()
    {
        foreach (BarrierColliderObject collider in colliderObjects)
        {
            collider.ColliderTouched -= Collider_ColliderTouched;
        }
    }
}
