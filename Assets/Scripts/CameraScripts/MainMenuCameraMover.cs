using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainMenuCameraMover : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _endFOV = 17f;
    [SerializeField] private float _endFocalLength=22;
    [SerializeField] private float _duration = 2f;
    

    private void Start()
    {
        transform.DOMove(_endPosition, _duration, false);
        m_Camera.DOFieldOfView(_endFOV, _duration);
        DOTween.To(x => m_Camera.focalLength = x, m_Camera.focalLength, _endFocalLength, _duration);
    }

}
