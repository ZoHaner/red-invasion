using UnityEngine;

namespace Code.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemiesPointsHolder", fileName = "EnemiesPointsHolder", order = 0)]
    public class EnemiesPointsHolder : ScriptableObject
    {
        private const string SpawnPointTag = "EnemySpawnPoint";
        
        [SerializeField] private Vector3[] _spawnPoints;
        
        public Vector3[] SpawnPoints => _spawnPoints;

        public void CollectSpawnPointPositionsOnScene()
        {
            var spawnObjects = GameObject.FindGameObjectsWithTag(SpawnPointTag);
            _spawnPoints = new Vector3[spawnObjects.Length];
            for (var i = 0; i < spawnObjects.Length; i++)
            {
                _spawnPoints[i] = spawnObjects[i].transform.position;
            }
        }
    }
}