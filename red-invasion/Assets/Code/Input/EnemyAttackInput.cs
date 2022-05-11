using Code.Services;

namespace Code.Input
{
    public class EnemyAttackInput : IUpdatable, IAttackInput
    {
        private readonly float _shootingRate;
        private float _cooldown;
        private bool _canAttack;

        public EnemyAttackInput(float timeOffset, float shootingRate)
        {
            _cooldown = timeOffset;
            _shootingRate = shootingRate;
        }

        public void Tick(float deltaTime)
        {
            _cooldown += deltaTime;

            if (_cooldown >= _shootingRate)
            {
                _cooldown = 0f;
                _canAttack = true;
            }
            else
            {
                _canAttack = false;
            }
        }

        public bool IsAttackButtonPressed() => _canAttack;
    }
}