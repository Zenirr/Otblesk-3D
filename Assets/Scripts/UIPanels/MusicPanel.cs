using System.Collections;
using System.Collections.Generic;
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
        if (AudioClip != null)
        {
            MusicManager.Instance.SetCurrentAudio(AudioClip);
        }
        else
        {
            StartCoroutine(FileManager.Instance.GetAudioClip(MusicPath,this));
            MusicManager.Instance.SetCurrentAudio(AudioClip);
        }
    }

    public AudioClip GetAudioClip()
    {
        if(AudioClip == null)
        {
            StartCoroutine(FileManager.Instance.GetAudioClip(MusicPath, this));
            return AudioClip;
        }
        else 
        {
            return AudioClip;
        }
    } 

    public void SetAudioClip(AudioClip clip)
    {
        AudioClip = clip;
    }

    public void SetValues(string filePath)
    {
        MusicName = FilePathHandler.GetFileName(filePath);
        MusicPath = filePath;
        _textMeshPro.text = MusicName;
    }

}
