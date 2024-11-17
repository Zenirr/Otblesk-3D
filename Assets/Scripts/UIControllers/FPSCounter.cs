using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class FPSCounter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _fpsVisual;
    private bool _isFPSCounterEnabled;
    StandartControls _controls;
    Coroutine currentCourutine;

    private void Start()
    {
        _controls = new StandartControls();
        _controls.FPSButton.Enable();
        _controls.FPSButton.FPS.performed += FPS_performed;
    }

    private void FPS_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isFPSCounterEnabled = !_isFPSCounterEnabled;
        _fpsVisual.gameObject.SetActive(_isFPSCounterEnabled);
        if (_isFPSCounterEnabled)
        {
            currentCourutine = StartCoroutine(FPSCounterCoroutine());
        }
    }
    
    private IEnumerator FPSCounterCoroutine()
    {
        _fpsVisual.text = "FPS" + Mathf.FloorToInt(1f / Time.unscaledDeltaTime).ToString();
        yield return new WaitForSecondsRealtime(1);
        currentCourutine = StartCoroutine(FPSCounterCoroutine());
    }

    private void OnDestroy()
    {
        _controls.FPSButton.Disable();
        _controls.FPSButton.FPS.performed -= FPS_performed;
    }
}
