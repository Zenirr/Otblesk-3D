using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio.Wave;


public class NAudioDecoder : DecoderImporter
{
    protected override void Initialize()
    {
        try
        {
            if (!uri.IsFile)
            {
                throw new FormatException("NAudioImporter does not support URLs");
            }
            reader = new Mp3FileReader(uri.LocalPath);
            sampleProvider = reader.ToSampleProvider();
        }
        catch (Exception ex)
        {
            OnError(ex.Message);
        }
    }
    protected override void Cleanup()
    {
        if (reader != null)
        {
            reader.Dispose();
        }
        reader = null;
        sampleProvider = null;
    }

    protected override AudioInfo GetInfo()
    {
        WaveFormat waveFormat = reader.WaveFormat;
        return new AudioInfo((int)reader.Length / (waveFormat.BitsPerSample / 8), waveFormat.SampleRate, waveFormat.Channels);
    }

    protected override int GetSamples(float[] buffer, int offset, int count)
    {
        return  sampleProvider.Read(buffer, offset, count);
    }

    private Mp3FileReader reader;

    private ISampleProvider sampleProvider;
}
