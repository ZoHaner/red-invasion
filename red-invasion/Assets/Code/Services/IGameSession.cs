using System.Threading.Tasks;

namespace Code.Services
{
    public interface IGameSession
    {
        Task WarmUp();
        void SpawnPlayer();
        void SpawnEnemies();
    }
}