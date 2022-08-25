using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _maxSpawnedPlatforms;
    [SerializeField] private GameObject _lastPlatform;
    [SerializeField] private Vector3 _lastScale;

    public static SpawnManager SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.SharedInstance.IsGameStarted) return;

        if (ObjectPool.SharedInstance.PlatformObjectPool.CountActive < _maxSpawnedPlatforms)
        {
            ObjectPool.SharedInstance.PlatformObjectPool.Get(out var newPlatform);
            var scale = newPlatform.GetComponent<PlatformController>().GetPlatformScale();
            var spawnPosition = _lastPlatform == null 
                ?  Vector3.zero 
                : new Vector3(_lastPlatform.transform.position.x, _lastPlatform.transform.position.y);
            var platformShift = _lastPlatform == null ? 0 : GetRandomSpawnDistance();
            spawnPosition.x += _lastScale.x / 2.0f + scale.x / 2.0f + platformShift;
            newPlatform.transform.position = spawnPosition;
            _lastPlatform = newPlatform;
            _lastScale = scale;
        }
    }

    private float GetRandomSpawnDistance()
    {
        var maxDistance = GameManager.SharedInstance.BridgeMaxLength;
        var minDistance = GameManager.SharedInstance.MinDistanceBehindPlatforms;
        var randomDistance = Random.Range(minDistance, maxDistance);

        return randomDistance;
    }
}
