using UnityEngine;

namespace Code.Helpers
{
    public static class MathHelpers
    {
        public static Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 lineDirection = lineEnd - lineStart;
            float lineLength = lineDirection.magnitude;
            lineDirection.Normalize();
            float projectLength = Mathf.Clamp(Vector3.Dot(point - lineStart, lineDirection), 0f, lineLength);
            return lineStart + lineDirection * projectLength;
        }

        public static bool OutOfBorders(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 lineDirection = lineEnd - lineStart;
            float lineLength = lineDirection.magnitude;
            lineDirection.Normalize();
            float dot = Vector3.Dot(point - lineStart, lineDirection);
            return dot < 0f || dot > lineLength;
        }
        
        public static Vector3 GetClosestPointOnInfiniteLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            return lineStart + Vector3.Project(point - lineStart, lineEnd - lineStart);
        }
    }
}