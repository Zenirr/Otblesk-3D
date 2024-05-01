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
    [Header("Параметры генерации")]
    [SerializeField] private Chunk[] _chunks;
    [SerializeField] private Chunk[] _straightChunks;
    [SerializeField] private ChunkEnvironment[] _environment;
    [SerializeField] private float _newChunkGenerateOffset; //Расстояние от машины на котором будут спавниться новые чанки
    [SerializeField] private int _chunksCountGenerationLimit; //Сколько чанков должно пройти чтобы появилась вероятность заспавнить портал вместо следующего чанка
    [SerializeField] private float _startLineLength; //Длина стартовой линии из прямых чвнков, советую ставить где0то 100 - 150

    [Header("Объекты на сцене")]
    [SerializeField] private Chunk _lastCreatedChunk;//последний созданный чанк, в самом начале - нулевой
    [SerializeField] private Vehicle _car;
    [SerializeField] private Teleporter _teleporter; // Телепорт который будет спавнится в конце дороги
    [SerializeField] private BiomController _biomeController;
    [SerializeField] private Chunk _zeroPointChunk; // Чанк который стоит в нулевой точке и является точкой отсчёта для всех других

    public event EventHandler<Chunk> ChunkCreated;

    private float _currentChunksLength;
    private int _chunksCreated; //сколько за текущий биом заспавнено чанков
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
    /// Создаёт и размещает случайный чанк, выбранный из переданного массива, в игровом пространстве. 
    /// Также заносит его в очередь уже созданных чанков.
    /// </summary>
    /// <param name="chunks">Массив чанков для выбора случайного чанка</param>
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
    /// Создаёт и размещает чанк который определён индексом. 
    /// Также заносит его в очередь уже созданных чанков.
    /// </summary>
    /// <param name="chunks">Массив чанков для выбора случайного чанка</param>
    /// <param name="index">Индекс чанка, для реализации параметра есть список существующих чанков</param>
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
    /// Уничтожает все чанки со сцены и очищает список чанков, также задаёт нулевой чанк - последним созданным
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
