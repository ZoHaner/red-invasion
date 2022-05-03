using Code.Enemies;

namespace Code.Services
{
    public interface IUpdateProvider
    {
        void Register(IUpdatable updatable);
        void Unregister(IUpdatable updatable);
    }
}