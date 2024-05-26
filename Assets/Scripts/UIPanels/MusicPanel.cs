using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MusicPanel : MonoBehaviour, IPanel
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public string MusicName { get; private set; }
    public string MusicPath { get; private set; }
    public AudioClip AudioClip { get; private set; }

    private void Start()
    {
        _button.onClick.AddListener(OnButtonPressed);
        StartCoroutine(GetAudioClip(MusicPath));
    }

    public void OnButtonPressed()
    {
        if (AudioClip != null)
        {
            MusicManager.Instance.SetCurrentAudio(AudioClip);
        }
        else
        {
            MusicManager.Instance.SetCurrentAudio(MusicPath);
        }
    }

    public void SetAudioClip(AudioClip clip)
    {
        AudioClip = clip;
    }

    public void SetValues(string filePath)
    {
        MusicName = Path.GetFileName(filePath);
        MusicPath = filePath;
        _textMeshPro.text = MusicName;
    }

    private IEnumerator GetAudioClip(string file)
    {
        MusicManager.Instance._audioDecoder.Import(file);
        while (!MusicManager.Instance._audioDecoder.isInitialized && !MusicManager.Instance._audioDecoder.isError)
        {
            yield return null;
        }
        if (MusicManager.Instance._audioDecoder.isError)
        {
            Debug.LogError(MusicManager.Instance._audioDecoder.error);
        }
        AudioClip = MusicManager.Instance._audioDecoder.audioClip;
        yield break;
    }
}
