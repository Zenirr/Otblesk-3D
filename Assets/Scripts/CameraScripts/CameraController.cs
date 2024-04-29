using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    private enum FollowType
    {
        Transposer,
        ThirdPersonFollow
    }

    //ставится на virtualCamera, позиции это оффсет от главного объекта слежки
    [Header("Camera settings")]
    [SerializeField] Vector3[] _cameraPositions;
    
    private int _currentCameraIndex = 1;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTransposer _virtualCameraTransposer;
    private Cinemachine3rdPersonFollow _virtualCamera3rdPersonFollow;
    private FollowType _currentFollowType;

    

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _virtualCameraTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _currentFollowType = FollowType.Transposer;

        if(_virtualCameraTransposer == null)
        {
            _virtualCamera3rdPersonFollow = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            _currentFollowType = FollowType.ThirdPersonFollow;
        }

        InputController.Instance.OnCameraChangePositionInput += InputController_OnCameraChangePositionInput;
    }

    private void OnDestroy()
    {
        InputController.Instance.OnCameraChangePositionInput -= InputController_OnCameraChangePositionInput;
    }

    private void InputController_OnCameraChangePositionInput(object sender, System.EventArgs e)
    {
        switch (_currentFollowType)
        {
            case FollowType.Transposer: ChangeCameraPositionForTransposer(); break;
            case FollowType.ThirdPersonFollow: ChangeCameraPositionFor3rdPersonFollow(); break;
            default: break;
        }
        
    }

    private void ChangeCameraPositionForTransposer()
    {
        if (_cameraPositions.Length > 0)
        {
            if (_currentCameraIndex == _cameraPositions.Length - 1)
            {
                _currentCameraIndex = 0;
                _virtualCameraTransposer.m_FollowOffset = _cameraPositions[_currentCameraIndex];
                return;
            }
            _currentCameraIndex++;
            _virtualCameraTransposer.m_FollowOffset = _cameraPositions[_currentCameraIndex];
        }
    }

    private void ChangeCameraPositionFor3rdPersonFollow()
    {
        if (_cameraPositions.Length > 0)
        {
            if (_currentCameraIndex == _cameraPositions.Length - 1)
            {
                _currentCameraIndex = 0;
                _virtualCamera3rdPersonFollow.ShoulderOffset = _cameraPositions[_currentCameraIndex];
                return;
            }
            _currentCameraIndex++;
            _virtualCamera3rdPersonFollow.ShoulderOffset = _cameraPositions[_currentCameraIndex];
        }
    }
}
