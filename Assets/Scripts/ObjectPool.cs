using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pooledObjects;
    
    public static ObjectPool SharedInstance;
    public ObjectPool<GameObject> PlatformObjectPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        SetBulletPool();
    }

    private void SetBulletPool()
    {
        PlatformObjectPool = new ObjectPool<GameObject>(createFunc: CreateRandomGroundPrefab,
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 10, collectionCheck: false,
            maxSize: 15);
    }

    private GameObject CreateRandomGroundPrefab()
    {
        if (PlatformObjectPool.CountAll < _pooledObjects.Count)
        {
            return Instantiate(_pooledObjects[PlatformObjectPool.CountAll]);
        }
        var index = Random.Range(0, _pooledObjects.Count);
        var objectToSpawn = _pooledObjects[index];
        
        return Instantiate(objectToSpawn);
    }
}
