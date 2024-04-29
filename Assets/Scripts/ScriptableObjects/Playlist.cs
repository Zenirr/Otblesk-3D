using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlaylist", menuName = "ScriptableObjects/NewPlaylist")]
public class Playlist : ScriptableObject
{
    [SerializeField] private AudioClip[] _music;
    public AudioClip[] Music => _music;
    
    public void SetCurrentMusic(AudioClip[] music)
    {
        _music = music;
    }
}