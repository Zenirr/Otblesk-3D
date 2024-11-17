using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _invincibleBuffActive;
    public Vehicle _vehicle { get; private set; }
    public bool isInvincible { get; private set; }
    private bool _isLosed = true;
    private Coroutine _currentCoroutine;

    private void LateUpdate()
    {
        if (transform.position.y < -5 && GameManager.State != GameManager.GameState.GameOver && _isLosed)
        {
            Debug.Log("Lose = " +_isLosed);
            _isLosed = false;
            GameManager.GetInstance().SetCurrentGameState(GameManager.GameState.GameOver);
        }
    }
    public void SetInvincible(float InvincibleTime)
    {
        isInvincible = true;
        if (_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(InvincibleCoroutine(InvincibleTime));
        else
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(InvincibleCoroutine(InvincibleTime));
        }
        _invincibleBuffActive.gameObject.SetActive(true);
    }

    private IEnumerator InvincibleCoroutine(float durationOfBuff)
    {
        Debug.Log("Начало баффа с временем " + durationOfBuff);
        yield return new WaitForSeconds(durationOfBuff);
        isInvincible = false;
        _invincibleBuffActive.gameObject.SetActive(false);
    }
}
