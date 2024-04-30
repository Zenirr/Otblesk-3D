using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [field: SerializeField] public GameObject _start { get; private set; }
    [field: SerializeField] public GameObject _end { get; private set; }
    [field: SerializeField] public int _linesCount { get; private set; }
    [field: SerializeField] public Transform[] _environmentObjectsPlacement { get; private set; }
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
                Transform environmentObjectPlacementTransform = _environmentObjectsPlacement[i].transform;
                
                Vector3 position = environmentObjectPlacementTransform.position;
                ChunkEnvironment currentEnvironmentObject;
                if (chunkEnvironment.isRotatable)
                {
                    currentEnvironmentObject = Instantiate(chunkEnvironment, position, rotation);
                }
                else
                {
                    currentEnvironmentObject = Instantiate(chunkEnvironment, position, environmentObjectPlacementTransform.rotation);
                }
                _environmentObjects.Add(currentEnvironmentObject);
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
            _environmentObjects.Clear();
        }
    }

    public float GetLength()
    {
        return _end.transform.position.z + _start.transform.position.z;
    }
}
