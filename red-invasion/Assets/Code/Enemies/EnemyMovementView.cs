using UnityEngine;

namespace Code.Enemies
{
    public class EnemyMovementView : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float _speed = 12f;

        private Vector3 _leftBorder;
        private Vector3 _rightBorder;

        private EnemyMovementController _enemyMovementController;

        public void Construct(Vector3 leftMoveBorder, Vector3 rightMoveBorder, float speed)
        {
            _leftBorder = leftMoveBorder;
            _rightBorder = rightMoveBorder;
            _speed = speed;
        }

        public void Initialize()
        {
            _enemyMovementController = new EnemyMovementController();
            SubscribeOnEvents();
        }

        private void OnDestroy() =>
            UnsubscribeFromEvents();

        public void Tick(float deltaTime)
        {
            _enemyMovementController.CalculateNextPosition(_leftBorder, _rightBorder, transform.position, _speed, deltaTime);
        }

        private void ApplyMovement(Vector3 position) =>
            transform.position = position;

        private void SubscribeOnEvents() =>
            _enemyMovementController.Moved += ApplyMovement;

        private void UnsubscribeFromEvents() =>
            _enemyMovementController.Moved -= ApplyMovement;
    }
}