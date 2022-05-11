namespace Code.Enemies
{
    public class EnemySpawner
    {
        private readonly EnemyFactory _enemyFactory;

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
            }
        }

        private void OnEnemyHit(EnemyMovementView enemyMovementView)
        {
            enemyMovementView.Hitted -= OnEnemyHit;
            _enemyFactory.ReleaseEnemy(enemyMovementView);
        }
    }
}