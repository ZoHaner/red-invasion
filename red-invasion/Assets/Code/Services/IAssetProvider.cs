using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Services
{
    public interface IAssetProvider
    {
        Task<T> Load<T>(string address) where T : class;
        Task<T> Load<T>(AssetReferenceGameObject reference) where T : class;
    }
}