using System;
using UnityEngine;

namespace Code.Bullets.VFX
{
    public class BulletVFXView : MonoBehaviour
    {
        public Action<BulletVFXView> ParticlesStopped;
        
        [SerializeField] private ParticleSystem _particleSystem;
        
        public void Initialize()
        {
            var main = _particleSystem.GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        
        public void OnParticleSystemStopped()
        {
            Debug.Log("System has stopped!");
            ParticlesStopped?.Invoke(this);
        }
    }
}