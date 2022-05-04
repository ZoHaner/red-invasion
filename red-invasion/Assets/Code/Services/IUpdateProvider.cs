namespace Code.Services
{
    public interface IUpdateProvider
    {
        void EnqueueRegister(IUpdatable updatable);
        void EnqueueUnregister(IUpdatable updatable);
    }
}