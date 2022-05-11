using System;
using UnityEngine;

namespace Code.Enemies
{
    [Serializable]
    public class EnemySpawnParams
    {
        public Vector3 LeftMoveBorder;
        public Vector3 RightMoveBorder;
        public Vector3 SpawnPosition;
        public Quaternion SpawnRotation;
        public float Speed;
        public float ShootingRate;

        public EnemySpawnParams(Vector3 leftMoveBorder, Vector3 rightMoveBorder, Vector3 spawnPosition, Quaternion spawnRotation, float speed, float shootingRate)
        {
            LeftMoveBorder = leftMoveBorder;
            RightMoveBorder = rightMoveBorder;
            SpawnPosition = spawnPosition;
            SpawnRotation = spawnRotation;
            Speed = speed;
            ShootingRate = shootingRate;
        }
    }
}