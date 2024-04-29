using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }

    public event EventHandler<EventArgs> OnCameraChangePositionInput;

    public event EventHandler<EventArgs> OnPauseButtonPressed;
    
    public event EventHandler<EventArgs> OnRestartButtonPressed;
    
    public event EventHandler<EventArgs> OnPlayNextTrackButtonPressed;
    public event EventHandler<EventArgs> OnPlayPreviousTrackButtonPressed;
    public event EventHandler<EventArgs> OnPauseMusicButtonPressed;


    public event EventHandler<InputMovementEventArgs> OnMovementInput;
    public class InputMovementEventArgs : EventArgs
    {
        public Vector2 inputValues;
    }

    StandartControls _inputActions;

    private void Awake()
    {
        Instance = this;
        _inputActions = new();

        #region Movement actions
        _inputActions.DriveControl.Movement.Enable();
        _inputActions.DriveControl.Movement.performed += Movement_performed;
        _inputActions.DriveControl.Movement.canceled += Movement_canceled;
        #endregion
        #region Restart action
        _inputActions.DriveControl.RestartDeleteLater.Enable();
        _inputActions.DriveControl.RestartDeleteLater.performed += RestartDeleteLater_performed;
        #endregion
        #region Change Camera
        _inputActions.DriveControl.ChangeCameraView.Enable();
        _inputActions.DriveControl.ChangeCameraView.started += ChangeCameraView_started;
        #endregion
        #region Pause action
        _inputActions.UIControl.PauseButton.Enable();
        _inputActions.UIControl.PauseButton.started += PauseButton_started;
        #endregion
        #region music
        _inputActions.MusicControl.Enable();
        _inputActions.MusicControl.PauseMusic.started += PauseMusic_started;
        _inputActions.MusicControl.PreviousMusicTrack.started += PreviousMusicTrack_started;
        _inputActions.MusicControl.NextMusicTrack.started += NextMusicTrack_started;
        #endregion
    }

    private void NextMusicTrack_started(InputAction.CallbackContext obj)
    {
        OnPlayNextTrackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void PreviousMusicTrack_started(InputAction.CallbackContext obj)
    {
        OnPlayPreviousTrackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void PauseMusic_started(InputAction.CallbackContext obj)
    {
        OnPauseMusicButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void PauseButton_started(InputAction.CallbackContext obj)
    {
        OnPauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void RestartDeleteLater_performed(InputAction.CallbackContext obj)
    {
        OnRestartButtonPressed?.Invoke(this,EventArgs.Empty);
    }

    private void ChangeCameraView_started(InputAction.CallbackContext obj)
    {
        OnCameraChangePositionInput?.Invoke(this,EventArgs.Empty);    
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        OnMovementInput?.Invoke(this, new InputMovementEventArgs { inputValues = Vector2.zero });
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        OnMovementInput?.Invoke(this, new InputMovementEventArgs { inputValues = _inputActions.DriveControl.Movement.ReadValue<Vector2>() });
    }
}
