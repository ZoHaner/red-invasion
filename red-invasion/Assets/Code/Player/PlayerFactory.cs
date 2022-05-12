using System;
using System.Threading.Tasks;
using Code.Input;
using Code.Services;
using UnityEngine;

namespace Code.Player
{
    public class PlayerFactory
    {
        private const string PlayerAddress = "Player";

        private GameObject _playerPrefab;

        private readonly IInputService _inputService;
        private readonly IUpdateProvider _updateProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly PlayerGunFactory _gunFactory;

        private Action<GameObject> _playerCreated;
        
        private GameObject _player;

        public PlayerFactory(IInputService inputService, IUpdateProvider updateProvider, IAssetProvider assetProvider, PlayerGunFactory gunFactory)
        {
            _inputService = inputService;
            _updateProvider = updateProvider;
            _assetProvider = assetProvider;
            _gunFactory = gunFactory;
        }
        
        public async Task WarmUp()
        {
            _playerPrefab = await _assetProvider.Load<GameObject>(PlayerAddress);
        }
        
        public GameObject SpawnPlayer()
        {
            if (_player != null)
            {
                _player.transform.position = Vector3.zero;
                _player.SetActive(true);
                _playerCreated?.Invoke(_player);
                return _player;
            }
            
            _player = GameObject.Instantiate(_playerPrefab, Vector3.up, Quaternion.identity);

            var playerCamera = _player.GetComponent<CameraRotationView>();
            playerCamera.Construct(_inputService);
            playerCamera.Initialize();

            var playerBody = _player.GetComponent<BodyRotationView>();
            playerBody.Construct(_inputService);
            playerBody.Initialize();

            var playerMovement = _player.GetComponent<PlayerMovementView>();
            playerMovement.Construct(_inputService);
            playerMovement.Initialize();

            _gunFactory.ConfigurePlayerGun(_player);
            
            _updateProvider.EnqueueRegister(playerCamera);
            _updateProvider.EnqueueRegister(playerBody);
            _updateProvider.EnqueueRegister(playerMovement);

            _playerCreated?.Invoke(_player);
            return _player;
        }

        public void ReleasePlayer()
        {
            _player.SetActive(false);
        }
    }
}