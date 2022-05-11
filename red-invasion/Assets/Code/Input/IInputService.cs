using UnityEngine;

namespace Code.Input
{
    public interface IInputService : IAttackInput
    {
        Vector2 GetMoveAxis();
        Vector2 GetLookAxis();
        bool GetJump();
    }

    public interface IAttackInput
    {
        bool IsAttackButtonPressed();
    }
}