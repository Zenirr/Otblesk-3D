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
    }

    public void OnButtonPressed()
    {
        MusicManager.GetInstance().SetCurrentAudio(MusicPath);
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
        Debug.Log(file);
        MusicManager.GetInstance()._audioDecoder.Import(file);
        while (!MusicManager.GetInstance()._audioDecoder.isInitialized && !MusicManager.GetInstance()._audioDecoder.isError)
        {
            yield return null;
        }
        if (MusicManager.GetInstance()._audioDecoder.isError)
        {
            Debug.LogError(MusicManager.GetInstance()._audioDecoder.error);
        }
        AudioClip = MusicManager.GetInstance()._audioDecoder.audioClip;
        yield break;
    }
}
