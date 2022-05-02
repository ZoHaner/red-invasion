using UnityEngine;

namespace Code.Input
{
    public interface IInputService
    {
        Vector2 GetMoveAxis();
        Vector2 GetLookAxis();
        bool IsAttackButtonPressed();
    }
}