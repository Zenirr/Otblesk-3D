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
        if (AudioClip == null)
        {
            StartCoroutine(GetAudioClip(MusicPath));
            MusicManager.Instance.SetCurrentAudio(MusicPath);
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
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            myClip.LoadAudioData();
            myClip.name = Path.GetFileName(file);

            SetAudioClip(myClip);
            MusicManager.Instance.SetCurrentAudio(myClip);
        }
    }
}
