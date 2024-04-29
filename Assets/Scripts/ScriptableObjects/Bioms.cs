using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName ="NewBiom",menuName ="ScriptableObjects/NewBiom")]
public class Bioms : ScriptableObject
{
    public string biomName;

    [Header("Environment Options")]
    public Chunk[] chunkPrefabs;
    public Chunk[] straightChunk;
    public ChunkEnvironment[] environmentPrefabs;
    

    [Header("Visual Options")]
    public Transform sunPosition;
    
}
