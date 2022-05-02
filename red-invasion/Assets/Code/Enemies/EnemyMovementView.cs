using UnityEngine;

namespace Code.Enemies
{
    public class EnemyMovementView : MonoBehaviour
    {
        [SerializeField] private float _speed = 12f;
        
        [SerializeField][HideInInspector] private WalkingRange _walkingRange;

        private EnemyMovementController _enemyMovementController;

        public void Initialize()
        {
            _enemyMovementController = new EnemyMovementController();
            SubscribeOnEvents();
        }

        private void OnDestroy() => 
            UnsubscribeFromEvents();


        public void Tick(float deltaTime)
        {
            _enemyMovementController.CalculateNextPosition(_walkingRange, transform.position, _speed, deltaTime);
        }

        private void ApplyMovement(Vector3 position) => 
            transform.position = position;

        private void SubscribeOnEvents() => 
            _enemyMovementController.Moved += ApplyMovement;

        private void UnsubscribeFromEvents() => 
            _enemyMovementController.Moved -= ApplyMovement;
    }
}