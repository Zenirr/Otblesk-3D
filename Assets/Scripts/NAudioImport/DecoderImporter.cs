using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public abstract class DecoderImporter : AudioImporter
{
    public override void Abort()
    {
        if (abort)
        {
            return;
        }
        if (import == null || !import.IsAlive)
        {
            return;
        }
        abort = true;
        if (!isInitialized)
        {
            Destroy(audioClip);
        }
        object @lock = _lock;
        lock (@lock)
        {
            executionQueue.Clear();
        }
        waitForMainThread.Set();
        import.Join();
    }

    protected override void Import()
    {
        bufferSize = 262144;
        buffer = new float[bufferSize];
        isDone = false;
        isInitialized = false;
        abort = false;
        index = 0;
        progress = 0f;
        waitForMainThread = new AutoResetEvent(false);
        import = new Thread(new ThreadStart(DoImport));
        import.Start();
    }

    private void DoImport()
    {
        Initialize();
        if (isError)
        {
            return;
        }
        info = GetInfo();
        Dispatch(new Action(CreateClip));
        Decode();
        Cleanup();
        progress = 1f;
        isDone = true;
    }

    private void Decode()
    {
        while (index < info.lengthSamples)
        {
            int samples = GetSamples(buffer, 0, bufferSize);
            if (samples == 0 || abort)
            {
                break;
            }
            if (index + bufferSize >= info.lengthSamples)
            {
                Array.Resize<float>(ref buffer, info.lengthSamples - index);
            }
            Dispatch(new Action(SetData));
            index += samples;
            progress = (float)index / (float)info.lengthSamples;
        }
    }

    private void CreateClip()
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.uri.LocalPath);
        audioClip = AudioClip.Create(fileNameWithoutExtension, info.lengthSamples / info.channels, info.channels, info.sampleRate, false);
        waitForMainThread.Set();
    }

    private void SetData()
    {
        if (audioClip == null)
        {
            Abort();
            return;
        }
        audioClip.SetData(buffer, index / info.channels);
        if (!isInitialized)
        {
            isInitialized = true;
            OnLoaded();
        }
        waitForMainThread.Set();
    }

    protected void OnError(string error)
    {
        this.error = error;
        isError = true;
        progress = 1f;
    }
    private void Dispatch(Action action)
    {
        object @lock = _lock;
        lock (@lock)
        {
            executionQueue.Enqueue(action);
        }
        waitForMainThread.WaitOne();
    }

    private void Update()
    {
        object @lock = _lock;
        lock (@lock)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue()();
            }
        }
    }

    protected abstract void Initialize();
    protected abstract void Cleanup();
    protected abstract int GetSamples(float[] buffer, int offset, int count);
    protected abstract DecoderImporter.AudioInfo GetInfo();

    private DecoderImporter.AudioInfo info;

    private int bufferSize;

    private float[] buffer;

    private AutoResetEvent waitForMainThread;

    private Thread import;

    private int index;

    private bool abort;

    private Queue<Action> executionQueue = new Queue<Action>();
    private object _lock = new object();
    protected class AudioInfo
    {
        public int lengthSamples { get; private set; }
        public int sampleRate { get; private set; }
        public int channels { get; private set; }
        public AudioInfo(int lengthSamples, int sampleRate, int channels)
        {
            this.lengthSamples = lengthSamples;
            this.sampleRate = sampleRate;
            this.channels = channels;
        }
    }
}
