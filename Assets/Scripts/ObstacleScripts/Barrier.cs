using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour, IObstacle
{
    //Барьер работает так - в триггер въезжает машина и с неё берётся скорость, если скорости не достаточно, то
    //машина не разобьётся при столкновении с коллайдерами.
    //Взятие скорости в триггере необходимо, ибо при столкновении с коллайдером скорость уже будет равна +- нулю
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
        if (_speedOnTriggerEnter > _speedToGameOver && GameManager.State != GameManager.GameState.GameOver )
            GameManager.Instance.SetCurrentGameState(GameManager.GameState.GameOver);
    }

    public void OnObstacleHit(GameObject gameObject)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody machineRigidbody) && !_isSpeedSetted)
        {
            _speedOnTriggerEnter = machineRigidbody.velocity.magnitude;
            Debug.Log("Машина вошла в триггер с скоростью " + _speedOnTriggerEnter);
            
            //почему-то при столкновении с внутренними коллайдерами барьера значение скорости сбрасывалось,
            //так что если скорость смертельная, то скорость устанавливается окончательно, что не даст сменить
            //заданную скорость, только при выходе из триггера оно будет сброшено
            if(_speedOnTriggerEnter > 3 )
            {
                _isSpeedSetted = true;
            }

            if (machineRigidbody.gameObject.TryGetComponent(out Player player) && player.isInvincible)
            {
                _speedOnTriggerEnter = 0;
                Debug.Log(machineRigidbody.velocity.magnitude + " - скорость больше одного");
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
