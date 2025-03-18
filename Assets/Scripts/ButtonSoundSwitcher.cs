using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class ButtonSoundSwitcher : MonoBehaviour
{
    [SerializeField] private string key;
    private AudioSource _currentAudioSource;

    private void Awake()
    {
        _currentAudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Addressables.LoadAssetAsync<AudioClip>(key).Completed += OnAudioClipLoaded; ;

    }

    private void OnAudioClipLoaded(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
            _currentAudioSource.clip = obj.Result;
        else
            Debug.LogError("Операция по получению рофлов не прошла.");
    }
}


