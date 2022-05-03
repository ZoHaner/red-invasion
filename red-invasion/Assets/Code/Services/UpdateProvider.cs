using System.Collections.Generic;
using Code.Enemies;
using UnityEngine;

namespace Code.Services
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();

        private void Update()
        {
            foreach (var updatable in _updatables) 
                updatable.Tick(Time.deltaTime);
        }

        public void Register(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
            {
                Debug.Log("This elements has already been registered");
                return;
            }

            _updatables.Add(updatable);
        }

        public void Unregister(IUpdatable updatable)
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