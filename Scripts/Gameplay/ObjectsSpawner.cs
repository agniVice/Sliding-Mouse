using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public static ObjectsSpawner Instance;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject[] _particlePrefabs;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private Sprite[] _sprites;

    private float _timeToSpawn;
    private float _currentTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (_currentTime <= 0)
        {
            SpawnRandomObject();
            SetRandomTime();
        }
        else
        {
            _currentTime -= Time.fixedDeltaTime;
        }
    }
    public void SpawnRandomObject()
    {
        var type = GetRandomType();

        var obj = Instantiate(_prefab, GetRandomPos(), Quaternion.identity);
        obj.GetComponent<Object>().Initialize(type, GetRandomSpeed(), _sprites[(int)type], _particlePrefabs[(int)type]);
    }
    private void SetRandomTime()
    {
        if (PlayerScore.Instance.Score > 50)
            _timeToSpawn = Random.Range(0.3f, 0.5f);
        if (PlayerScore.Instance.Score > 30)
            _timeToSpawn = Random.Range(0.5f, 0.7f);
        if (PlayerScore.Instance.Score > 20)
            _timeToSpawn = Random.Range(1f, 1.2f);
        if (PlayerScore.Instance.Score > 10)
            _timeToSpawn = Random.Range(1.2f, 1.5f);
        if (PlayerScore.Instance.Score < 5)
            _timeToSpawn = Random.Range(1.5f, 2f);

        _currentTime = _timeToSpawn;
    }
    private float GetRandomSpeed()
    {
        if (PlayerScore.Instance.Score > 50)
            return Random.Range(24f, 30f);
        if (PlayerScore.Instance.Score > 30)
            return Random.Range(18f, 21f);
        if (PlayerScore.Instance.Score > 20)
            return Random.Range(15f, 18f);
        if (PlayerScore.Instance.Score > 10)
            return Random.Range(12f, 15f);
        if (PlayerScore.Instance.Score < 5)
            return Random.Range(10f, 12f);

        return Random.Range(10, 12f);
    }
    private Vector2 GetRandomPos()
    {
        return _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
    }
    private ObjectType GetRandomType()
    {
        return (ObjectType)Random.Range(0, 2);
    }
}
