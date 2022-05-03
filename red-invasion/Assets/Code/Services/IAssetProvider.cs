using System.Threading.Tasks;

namespace Code.Services
{
    public interface IAssetProvider
    {
        Task<T> Load<T>(string address) where T : class;
    }
}