using UnityEngine;

namespace Code.Input
{
    public class StandaloneInputService : IInputService
    {
        private const string HorizontalMoveAxisName = "Horizontal";
        private const string VerticalMoveAxisName = "Vertical";
        private const string HorizontalMouseAxisName = "Mouse X";
        private const string VerticalMouseAxisName = "Mouse Y";

        public Vector2 GetMoveAxis()
        {
            return new Vector2(
                UnityEngine.Input.GetAxis(HorizontalMoveAxisName),
                UnityEngine.Input.GetAxis(VerticalMoveAxisName)
            ).normalized;
        }

        public Vector2 GetLookAxis()
        {
            return new Vector2(
                UnityEngine.Input.GetAxis(HorizontalMouseAxisName),
                UnityEngine.Input.GetAxis(VerticalMouseAxisName)
            );
        }

        public bool IsAttackButtonPressed() => 
            UnityEngine.Input.GetMouseButtonDown(0);
    }
}