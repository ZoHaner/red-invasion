using System.Collections.Generic;
using UnityEngine;

namespace Code.Services
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private readonly HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
        private readonly Queue<IUpdatable> _updatablesToAdd = new Queue<IUpdatable>();
        private readonly Queue<IUpdatable> _updatablesToRemove = new Queue<IUpdatable>();

        private void Update()
        {
            foreach (var updatable in _updatables)
                updatable.Tick(Time.deltaTime);

            RemoveElements();
            AddElements();
        }

        private void RemoveElements()
        {
            while (_updatablesToRemove.Count > 0)
                Unregister(_updatablesToRemove.Dequeue());
        }

        private void AddElements()
        {
            while (_updatablesToAdd.Count > 0)
                Register(_updatablesToAdd.Dequeue());
        }

        public void EnqueueRegister(IUpdatable updatable) =>
            _updatablesToAdd.Enqueue(updatable);

        public void EnqueueUnregister(IUpdatable updatable) =>
            _updatablesToRemove.Enqueue(updatable);

        private void Register(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
            {
                Debug.Log("This elements has already been registered");
                return;
            }

            _updatables.Add(updatable);
        }

        private void Unregister(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
            {
                _updatables.Remove(updatable);
                return;
            }

            Debug.Log("This elements wasn't registered");
        }
    }
}