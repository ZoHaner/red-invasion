using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Bullets
{
    [CreateAssetMenu(menuName = "Settings/Create Bullet Parameters", fileName = "Bullet Parameters")]
    public class BulletParams : ScriptableObject
    {
        public AssetReferenceGameObject PrefabReference;
        public LayerMask CollisionLayerMask;
        public float Speed = 15f;
        public float Radius = 0.075f;
    }
}