using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    private AudioClip _currentAudioClip;
    private AudioClip _nextAudioClip;
    private float _currentTrackPlayTime;

    private void Start()
    {
        InputController.Instance.OnPlayNextTrackButtonPressed += InputController_OnPlayNextTrackButtonPressed;    
        InputController.Instance.OnPlayPreviousTrackButtonPressed += InputController_OnPlayPreviousTrackButtonPressed;
        InputController.Instance.OnPauseMusicButtonPressed += InputController_OnPauseMusicButtonPressed;
    }

    private void OnDestroy()
    {
        InputController.Instance.OnPlayNextTrackButtonPressed -= InputController_OnPlayNextTrackButtonPressed;
        InputController.Instance.OnPlayPreviousTrackButtonPressed -= InputController_OnPlayNextTrackButtonPressed;
        InputController.Instance.OnPauseMusicButtonPressed -= InputController_OnPlayNextTrackButtonPressed;
    }
    #region input controller events
    private void InputController_OnPlayNextTrackButtonPressed(object sender, System.EventArgs e)
    {
        MusicManager.GetInstance().PlayNextTrack();
    }

    private void InputController_OnPlayPreviousTrackButtonPressed(object sender, System.EventArgs e)
    {
        MusicManager.GetInstance().PlayPreviousTrack();
    }

    private void InputController_OnPauseMusicButtonPressed(object sender, System.EventArgs e)
    {
        MusicManager.GetInstance().PauseContinueMusic();
    }
    #endregion

}
