using UnityEngine;

namespace Code.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemiesPointsHolder", fileName = "EnemiesPointsHolder", order = 0)]
    public class EnemiesPointsHolder : ScriptableObject
    {
        [SerializeField] private EnemySpawnParams[] _spawnPoints;

        public EnemySpawnParams[] SpawnPoints => _spawnPoints;

        public void CollectSpawnPointPositionsOnScene()
        {
            var spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
            _spawnPoints = new EnemySpawnParams[spawnPoints.Length];

            for (var i = 0; i < spawnPoints.Length; i++)
            {
                var sp = spawnPoints[i];
                _spawnPoints[i] = new EnemySpawnParams(
                    sp.EnemyWalkingRange.LeftBorder,
                    sp.EnemyWalkingRange.RightBorder,
                    sp.transform.position,
                    sp.transform.rotation,
                    sp.EnemySpeed,
                    sp.ShootingRate);
            }
        }
    }
}