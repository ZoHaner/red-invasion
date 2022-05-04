using System.Threading.Tasks;
using Code.Enemies;
using Code.Input;
using Code.Player;
using UnityEngine;

namespace Code.Services
{
    public class GameFactory : IGameFactory
    {
        private const string EnemyAddress = "Enemy";
        private const string PlayerAddress = "Player";
        private const string EnemiesPointsHolderAddress = "EnemiesPointsHolder";

        private readonly IAssetProvider _assetProvider;
        private readonly IUpdateProvider _updateProvider;
        private readonly IInputService _inputService;

        public GameFactory(IAssetProvider assetProvider, IUpdateProvider updateProvider, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _updateProvider = updateProvider;
            _inputService = inputService;
        }

        public async void SpawnPlayer()
        {
            var prefab = await _assetProvider.Load<GameObject>(PlayerAddress);
            var player = Object.Instantiate(prefab, Vector3.up, Quaternion.identity);
            
            var playerCamera = player.GetComponent<CameraRotationView>();
            playerCamera.Construct(_inputService);
            playerCamera.Initialize();
            
            var playerBody = player.GetComponent<BodyRotationView>();
            playerBody.Construct(_inputService);
            playerBody.Initialize();
            
            var playerMovement = player.GetComponent<PlayerMovementView>();
            playerMovement.Construct(_inputService);
            playerMovement.Initialize();
            
            RegisterUpdatableObject(playerCamera);
            RegisterUpdatableObject(playerBody);
            RegisterUpdatableObject(playerMovement);
        }
        
        public async void SpawnEnemies()
        {
            var spawnPoints = await GetSpawnPoints();
            var prefab = await _assetProvider.Load<GameObject>(EnemyAddress);

            foreach (var spawnPoint in spawnPoints)
            {
                SpawnEnemy(spawnPoint, prefab);
            }
        }

        private void SpawnEnemy(EnemySpawnParams enemyParams, GameObject prefab)
        {
            var enemy = Object.Instantiate(prefab, enemyParams.SpawnPosition, enemyParams.SpawnRotation);
            var enemyMovement = enemy.GetComponent<EnemyMovementView>();
            enemyMovement.Construct(enemyParams.LeftMoveBorder, enemyParams.RightMoveBorder, enemyParams.Speed);
            enemyMovement.Initialize();
            RegisterUpdatableObject(enemyMovement);
        }

        private async Task<EnemySpawnParams[]> GetSpawnPoints()
        {
            var holder = await _assetProvider.Load<EnemiesPointsHolder>(EnemiesPointsHolderAddress);
            return holder.SpawnPoints;
        }

        private void RegisterUpdatableObject(IUpdatable updatable) =>
            _updateProvider.Register(updatable);
    }
}