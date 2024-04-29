using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BiomController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private List<Bioms> _bioms;
    [SerializeField] private Bioms _currentBiom;
    [SerializeField] private bool _isEndlessRegime;
    [Header("References")]
    [SerializeField] private Teleporter _teleportFrom;
    [SerializeField] private EnvironmentRoadGenerator _environmentGenerator;

    public event EventHandler<EventArgs> BiomsChanged;


    private void Start()
    {
        _teleportFrom.Teleported += Teleport_Teleported;
    }

    private void Teleport_Teleported(object sender, System.EventArgs e)
    {
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

    private void EndlessBiomsRegimeSetter()
    {
        Bioms newBiom = _bioms[Random.Range(0, _bioms.Count)];
        while (newBiom == _currentBiom) 
        {
            newBiom = _bioms[Random.Range(0, _bioms.Count)];
        }
        _currentBiom = newBiom;
        BiomsChanged?.Invoke(this,EventArgs.Empty);
        _environmentGenerator.SetCurrentChunkListAndEnvironmentList(newBiom.straightChunk,newBiom.chunkPrefabs, newBiom.environmentPrefabs);
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
