using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentRoadGenerator : MonoBehaviour
{
    [Header("��������� ���������")]
    [SerializeField] private Chunk[] _chunks;
    [SerializeField] private Chunk[] _straightChunks;
    [SerializeField] private ChunkEnvironment[] _environment;
    [SerializeField] private float _newChunkGenerateOffset; //���������� �� ������ �� ������� ����� ���������� ����� �����
    [SerializeField] private int _chunksCountGenerationLimit; //������� ������ ������ ������ ����� ��������� ����������� ���������� ������ ������ ���������� �����
    [SerializeField] private float _startLineLength; //����� ��������� ����� �� ������ ������, ������� ������� ���0�� 100 - 150

    [Header("������� �� �����")]
    [SerializeField] private Chunk _lastCreatedChunk;//��������� ��������� ����, � ����� ������ - �������
    [SerializeField] private Vehicle _car;
    [SerializeField] private Teleporter _teleporter; // �������� ������� ����� ��������� � ����� ������
    [SerializeField] private BiomController _biomeController;
    [SerializeField] private Chunk _zeroPointChunk; // ���� ������� ����� � ������� ����� � �������� ������ ������� ��� ���� ������

    public event EventHandler<Chunk> ChunkCreated;

    private float _currentChunksLength;
    private int _chunksCreated; //������� �� ������� ���� ���������� ������
    private Queue<Chunk> _chunksQueue;

    private void OnDestroy()
    {
        InputController.Instance.OnRestartButtonPressed -= InputController_OnRestartButtonPressed;
    }

    private void Start()
    {
        _chunksQueue = new Queue<Chunk>();
        _biomeController.BiomsChanged += BiomeController_BiomsChanged;
        InputController.Instance.OnRestartButtonPressed += InputController_OnRestartButtonPressed;
    }

    private void InputController_OnRestartButtonPressed(object sender, EventArgs e)
    {
        BackOnTheRoad();
    }

    private void BiomeController_BiomsChanged(object sender, EventArgs e)
    {
        ClearSceneFromChunks();
    }

    private void Update()
    {
        if (IsVehicleAtTheBorder())
        {
            RoadGenerate();
        }
    }

    private void RoadGenerate()
    {
        if (_chunksCreated < _chunksCountGenerationLimit)
        {
            if (_currentChunksLength < _startLineLength)
            {
                ChunkSpawn(_straightChunks);
            }
            else
            {
                ChunkSpawn(_chunks, _chunksQueue.Count - 1);
            }
        }
        else
        {
            TeleportSpawn();
        }
    }

    /// <summary>
    /// ������ � ��������� ��������� ����, ��������� �� ����������� �������, � ������� ������������. 
    /// ����� ������� ��� � ������� ��� ��������� ������.
    /// </summary>
    /// <param name="chunks">������ ������ ��� ������ ���������� �����</param>
    private void ChunkSpawn(Chunk[] chunks)
    {
        Chunk chunk = Instantiate(chunks[Random.Range(0, chunks.Length)]);
        chunk.gameObject.SetActive(true);
        chunk.transform.position = _lastCreatedChunk._end.transform.position + (chunk.transform.position - chunk._start.transform.position);
        chunk.SetEnvironmentObjects(_environment);
        _lastCreatedChunk = chunk;
        _chunksQueue.Enqueue(chunk);
        _chunksCreated++;
        ChunkCreated?.Invoke(this,chunk);
        _currentChunksLength += _lastCreatedChunk.GetLength();
    }

    /// <summary>
    /// ������ � ��������� ���� ������� �������� ��������. 
    /// ����� ������� ��� � ������� ��� ��������� ������.
    /// </summary>
    /// <param name="chunks">������ ������ ��� ������ ���������� �����</param>
    /// <param name="index">������ �����, ��� ���������� ��������� ���� ������ ������������ ������</param>
    private void ChunkSpawn(Chunk[] chunks,int index)
    {
        Chunk chunk = Instantiate(chunks[index]);
        chunk.gameObject.SetActive(true);
        chunk.transform.position = _lastCreatedChunk._end.transform.position + (chunk.transform.position - chunk._start.transform.position);
        chunk.SetEnvironmentObjects(_environment);
        _lastCreatedChunk = chunk;
        _chunksQueue.Enqueue(chunk);
        _chunksCreated++;
        ChunkCreated?.Invoke(this, chunk);
        _currentChunksLength += _lastCreatedChunk.GetLength();
    }

    /// <summary>
    /// ���������� ��� ����� �� ����� � ������� ������ ������, ����� ����� ������� ���� - ��������� ���������
    /// </summary>
    private void ClearSceneFromChunks()
    {
        foreach (var chunk in _chunksQueue)
        {
            Destroy(chunk.gameObject);
        }
        _chunksQueue.Clear();
        _lastCreatedChunk = _zeroPointChunk;
        _currentChunksLength = 0;
    }

    private void TeleportSpawn()
    {
        _teleporter.gameObject.SetActive(true);
        _teleporter.gameObject.transform.position = _lastCreatedChunk._end.transform.position - Vector3.forward;
    }

    private bool IsVehicleAtTheBorder()
    {
        return _lastCreatedChunk.transform.position.z < _car.transform.position.z + _newChunkGenerateOffset;
    }

    public void SetCurrentChunkListAndEnvironmentList(Chunk[] straightChunk, Chunk[] chunks, ChunkEnvironment[] environments,int maxChunksCount)
    {
        _straightChunks = straightChunk;
        _chunks = chunks;
        _environment = environments;
        _chunksCountGenerationLimit = maxChunksCount;
        _chunksCreated = 0;
    }

    private void BackOnTheRoad()
    {
        Chunk currentChunk = (Chunk)_chunksQueue.First();
        _car.transform.SetPositionAndRotation(currentChunk._start.transform.position + Vector3.forward + Vector3.up, Quaternion.Euler(0, 0, 0));
        _car.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
