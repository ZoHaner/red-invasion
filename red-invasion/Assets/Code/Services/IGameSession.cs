using System;
using System.Threading.Tasks;

namespace Code.Services
{
    public interface IGameSession
    {
        Task WarmUp();
        void SpawnPlayer();
        void SpawnEnemies();
        void Initialize();
        Action WinGame { get; set; }
        Action LooseGame { get; set; }
        void Cleanup();
    }
}