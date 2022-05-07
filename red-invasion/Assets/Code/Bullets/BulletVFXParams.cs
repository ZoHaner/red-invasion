using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Bullets
{
    [CreateAssetMenu(menuName = "Settings/Create BulletVFX Params", fileName = "Bullet VFX Params", order = 0)]
    public class BulletVFXParams : ScriptableObject
    {
        public AssetReferenceGameObject PrefabReference;
    }
}