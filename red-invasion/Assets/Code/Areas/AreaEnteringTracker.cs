using System;
using System.Threading.Tasks;
using Code.Services;
using UnityEngine;

namespace Code.Areas
{
    public class AreaEnteringTracker : IUpdatable
    {
        public Action OnAreaEntered;

        private readonly string _areaBoundsSettingsAddress;
        private readonly IAssetProvider _assetProvider;

        private AreaBounds _areaBounds;
        private Transform _targetTransform;

        public AreaEnteringTracker(string areaBoundsSettingsAddress, IAssetProvider assetProvider)
        {
            _areaBoundsSettingsAddress = areaBoundsSettingsAddress;
            _assetProvider = assetProvider;
        }

        public async Task Warmup()
        {
            _areaBounds = await _assetProvider.Load<AreaBounds>(_areaBoundsSettingsAddress);
        }

        public void SetTargetTransform(Transform transform)
        {
            _targetTransform = transform;
        }

        public void Tick(float deltaTime)
        {
            if (_targetTransform == null)
                return;

            if (TargetInBounds()) 
                OnAreaEntered?.Invoke();
        }

        private bool TargetInBounds() => 
            _areaBounds.Bounds.Contains(_targetTransform.position);
    }
}