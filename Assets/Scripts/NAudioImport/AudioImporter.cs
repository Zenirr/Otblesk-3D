using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioImporter : MonoBehaviour
{
    public event Action<AudioClip> Loaded;
    public Uri uri { get; private set; }
    public virtual AudioClip audioClip { get; protected set; }
    public virtual float progress { get; protected set; }
    public virtual bool isDone { get; protected set; }
    public virtual bool isInitialized { get; protected set; }
    public virtual bool isError { get; protected set; }
    public virtual string error { get; protected set; }
    public void Import(string uri)
    {
        Abort();
        this.uri = new Uri(uri);
        this.isError = false;
        this.error = string.Empty;
        Import();
    }

    public abstract void Abort();
    protected abstract void Import();
    protected void OnLoaded()
    {
        if(Loaded != null)
        {
            Loaded(audioClip);
        }
    }
}
