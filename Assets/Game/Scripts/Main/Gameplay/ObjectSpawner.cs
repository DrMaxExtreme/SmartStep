using UnityEngine;

namespace SmartStep.Gameplay
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Настройки спавна объектов")]
        [SerializeField] private int _initialSpawnCount = 5;
        [SerializeField] private int _maxSpawnCount = 7;
        [SerializeField] private int _movesToIncreaseSpawn = 2;
        [SerializeField] private float _spawnIncreaseMultiplier = 2f;

        private int _currentSpawnCount;
        private int _moveCounter;

        public int CurrentSpawnCount => _currentSpawnCount;

        public void Init()
        {
            _currentSpawnCount = _initialSpawnCount;
            _moveCounter = 0;
        }

        public void OnMoveCompleted()
        {
            _moveCounter++;

            if (_moveCounter >= _movesToIncreaseSpawn)
            {
                _moveCounter = 0;
                //_currentSpawnCount = Mathf.Min(_maxSpawnCount, Mathf.RoundToInt(_currentSpawnCount * _spawnIncreaseMultiplier));
            }
        }
    }
}
