using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Chunk : MonoBehaviour
{
    [field: SerializeField] public GameObject _start { get; private set; }
    [field: SerializeField] public GameObject _end { get; private set; }
    [field: SerializeField] public int _linesCount { get; private set; }
    [field: SerializeField] public Transform[] _environmentObjectsPlacement { get; private set; }
    [field: SerializeField] public Transform[] _obstaclePlacement { get; private set; }
    [field: SerializeField] public List<GameObject> _obstacle { get; private set; }
    [field: SerializeField] public List<ChunkEnvironment> _environmentObjects { get; private set; }
    [field: SerializeField] public float _score { get; private set; }

    public void SetEnvironmentObjects(ChunkEnvironment[] objects)
    {
        if (_environmentObjectsPlacement.Length > 0)
        {
            _environmentObjects = new List<ChunkEnvironment>();
            for (int i = 0; i < _environmentObjectsPlacement.Length; i++)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 378), 0));
                ChunkEnvironment chunkEnvironment = objects[Random.Range(0, objects.Length)];
                Transform transform = _environmentObjectsPlacement[i].transform;
                Vector3 position = transform.position;
                ChunkEnvironment currentEnvironmentObject;
                if (chunkEnvironment._isRotatable)
                {
                    currentEnvironmentObject = Instantiate(chunkEnvironment, position, rotation);
                }
                else
                {
                    currentEnvironmentObject = Instantiate(chunkEnvironment, position, transform.rotation);
                }
                _environmentObjects.Add(currentEnvironmentObject);
            }
        }
    }

    public void SetObstacles(GameObject[] obstacles)
    {
        if (_obstaclePlacement.Length > 0)
        {
            _obstacle = new List<GameObject>();
            for (int i = 0; i < _obstaclePlacement.Length; i++)
            {
                if (Random.value > 0.5)
                {
                    GameObject chunkObject = obstacles[Random.Range(0, obstacles.Length)];
                    Transform transform = _obstaclePlacement[i].transform;
                    Vector3 position = transform.position;
                    GameObject currentObject = Instantiate(chunkObject, position, transform.rotation);
                    _obstacle.Add(currentObject);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (_environmentObjectsPlacement.Length > 0 && _environmentObjects.Count > 0)
        {
            foreach (ChunkEnvironment chunkEnvironment in _environmentObjects)
            {
                // проверка нужна чтобы редактор не выдавал ошибку при выключени сцены, ибо код пытается удалить обёект который уже удалил юнити
                if (chunkEnvironment != null)
                    Destroy(chunkEnvironment.gameObject);
            }
            foreach (GameObject thisObject in _obstacle)
            {
                if (thisObject != null)
                    Destroy(thisObject);

            }
            _environmentObjects.Clear();
            _obstacle.Clear();
        }
    }

    public float GetLength()
    {
        return _end.transform.position.z + _start.transform.position.z;
    }
}
