using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] int _maxEnemies;
    [SerializeField] float _spawningInterval;
    int _enemiesSpawned = 0;
    float _currentSpawningCooldown = 0;
    Transform _spawningPoint;

    private void Start()
    {
        _spawningPoint = transform.GetChild(0);
    }

    private void Update()
    {
        _currentSpawningCooldown -= Time.deltaTime;
        if(_currentSpawningCooldown <= 0 && _enemiesSpawned < _maxEnemies)
        {
            _currentSpawningCooldown = _spawningInterval;
            Instantiate(_enemyPrefab, _spawningPoint.position, _spawningPoint.rotation);
            _enemiesSpawned++;
        }
    }
}
