using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BiomController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Bioms tutorialBiom;
    [SerializeField] private List<Bioms> _bioms;
    [SerializeField] private Bioms _currentBiom;
    [SerializeField] private bool _isEndlessRegime;
    [SerializeField] private bool _isRandomGenerated;
    [SerializeField] private ScoreManager _scoreManager;

    [Header("References")]
    [SerializeField] private Teleporter _teleportFrom;
    [SerializeField] private EnvironmentRoadGenerator _environmentGenerator;

    public event EventHandler<EventArgs> BiomsChanged;


    private void Start()
    {
        _teleportFrom.Teleported += Teleport_Teleported;
        if (GameManager.Instance.isNew)
        {
            SetCurrentBiom(tutorialBiom);
        }
    }

    private void Teleport_Teleported(object sender, System.EventArgs e)
    {
        GameSave save = GameManager.Instance.currentSave;
        if (save._highScore < _scoreManager.currentScore)
        {
            SaveManagerHandler.Save(save._saveName, save._musicPath, save._playerName, _scoreManager.currentScore, false,save._playerPassword, save._musicVolume,save._useBuiltInPlaylist);
            GameManager.Instance.SetSave(SaveManagerHandler.Load(save._saveName+".json"));
        }

        if (_isEndlessRegime)
        {
            EndlessBiomsRegimeSetter();
        }
        else
        {
            IsntEndlessBiomsRegimeSetter();
        }
        BiomsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetCurrentBiom(Bioms biom)
    {
        _currentBiom = biom;
        _environmentGenerator.SetCurrentBiomGenerationParametrs(biom.straightChunk, biom.chunkPrefabs, biom.environmentPrefabs, biom.chunkMaxCount,biom.isRandomGenerated, biom.obstacles);
        if (biom.skyboxMaterial != null) 
        {
            RenderSettings.skybox = biom.skyboxMaterial; 
        }
        if (biom.lightOptions != null)
        {
            Lightmapping.lightingSettings = biom.lightOptions;
        }
        BiomsChanged?.Invoke(_currentBiom, EventArgs.Empty);
    }

    private void EndlessBiomsRegimeSetter()
    {
        Bioms newBiom = _bioms[Random.Range(0, _bioms.Count)];
        while (newBiom == _currentBiom)
        {
            newBiom = _bioms[Random.Range(0, _bioms.Count)];
        }
        _currentBiom = newBiom;
        _environmentGenerator.SetCurrentBiomGenerationParametrs(newBiom.straightChunk, newBiom.chunkPrefabs, newBiom.environmentPrefabs, newBiom.chunkMaxCount, newBiom.isRandomGenerated,newBiom.obstacles);
        if (newBiom.skyboxMaterial != null)
        {
            RenderSettings.skybox = newBiom.skyboxMaterial;
        }
        if (newBiom.lightOptions != null)
        {
            Lightmapping.lightingSettings = newBiom.lightOptions;
        }
        if (newBiom.lightmapData != null)
        {
            Lightmapping.lightingDataAsset = newBiom.lightmapData;
        }
        BiomsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void IsntEndlessBiomsRegimeSetter()
    {
        if (_bioms.Count <= 0)
        {
            //надо создать ивент который будет говорить gamemanager'у что игра выиграна
        }
        else
        {
            int newBiomId = Random.Range(0, _bioms.Count - 1);
            for (int i = newBiomId; i < _bioms.Count - 1; ++i)
            {
                if (i == _bioms.Count - 1)
                {
                    _bioms.RemoveAt(i);
                }
                else
                {
                    _bioms[i] = _bioms[i + 1];
                }
            }
            Bioms newBiom = _bioms[newBiomId];
            _currentBiom = newBiom;
        }
    }

}
