using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageGenerator : MonoBehaviour
{
    [SerializeField] GameObject _garbagePrefab;

    [SerializeField] int _garbageCountMax, _garbageCountMin;
    [SerializeField] float _garbageSpawntime; float _timerSpawn;

    [SerializeField] Transform _edgeLeft, _edgeRight;

    Queue<Transform> _garbagePool = new Queue<Transform>();
    int _garbageActiveCount = 0;

    Transform _tempGO;
    private void Awake()
    {
        //Garbage pool fill
        for (int i = 0; i < _garbageCountMax+1; i++)
        {
            _tempGO = Instantiate(_garbagePrefab, transform.position, Quaternion.identity).transform;
            _tempGO.gameObject.SetActive(false);

            _garbagePool.Enqueue(_tempGO.transform);
        }
    }
    private void Start()
    {
        Garbage.OnCollected += DecreaseGarbageCounter;

        for (int i = 0; i < _garbageCountMin; i++)
        {
            SpawnGarbageFromPool(GetRandomXInRoom());
        }

    }

    private void Update()
    {
        _timerSpawn += Time.deltaTime;

        if (_garbageActiveCount < _garbageCountMax)
        {

            if (_timerSpawn >= _garbageSpawntime)
            {
                SpawnGarbageFromPool(GetRandomXInRoom());

                _timerSpawn = 0;
            }
        }

    }

    public void SpawnGarbageFromPool(float X)
    {
        _tempGO = _garbagePool.Dequeue();

        _tempGO.position = new Vector3(X,0f,0f);
        _tempGO.gameObject.SetActive(true);

        _garbageActiveCount++;

        _garbagePool.Enqueue(_tempGO);
    }

    void DecreaseGarbageCounter(Garbage _garbage)
    {
        _garbageActiveCount--;
    }

    float GetRandomXInRoom()
    {
        float x = Random.Range(_edgeLeft.position.x, _edgeRight.position.x);
        return x;
    }

    private void OnDestroy()
    {
        Garbage.OnCollected -= DecreaseGarbageCounter;
    }
}
