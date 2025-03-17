using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class ButtonSoundSwitcher : MonoBehaviour
{
    [SerializeField] private AssetReferenceAudioClip audioClip;
    private AudioSource _currentAudioSource;

    private void Awake()
    {
        _currentAudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioClip.LoadAssetAsync<AudioClip>().Completed += OnAudioClipLoaded; ;

    }

    private void OnAudioClipLoaded(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
            _currentAudioSource.clip = obj.Result;
        else
            Debug.LogError("Операция по получению рофлов не прошла.");
    }
}

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{

    public AssetReferenceAudioClip(string guid):base(guid)
    {
    }

}

