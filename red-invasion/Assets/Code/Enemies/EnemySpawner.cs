using System.Collections.Generic;

namespace Code.Enemies
{
    public class EnemySpawner
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly HashSet<EnemyMovementView> _activeEnemies = new HashSet<EnemyMovementView>();

        public EnemySpawner(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        public void SpawnEnemiesAtSpawnPoints()
        {
            var enemies = _enemyFactory.SpawnEnemiesAtSpawnPoints();
            foreach (var enemy in enemies)
            {
                enemy.Hitted += OnEnemyHit;
                _activeEnemies.Add(enemy);
            }
        }

        public void ReleaseActiveEnemies()
        {
            foreach (var enemy in _activeEnemies)
            {
                enemy.Hitted -= OnEnemyHit;
                _enemyFactory.ReleaseEnemy(enemy);
            }

            _activeEnemies.Clear();
        }

        private void OnEnemyHit(EnemyMovementView enemyMovementView)
        {
            enemyMovementView.Hitted -= OnEnemyHit;
            _activeEnemies.Remove(enemyMovementView);
            _enemyFactory.ReleaseEnemy(enemyMovementView);
        }
    }
}