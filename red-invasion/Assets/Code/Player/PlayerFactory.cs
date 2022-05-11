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
            var player = GameObject.Instantiate(_playerPrefab, Vector3.up, Quaternion.identity);

            var playerCamera = player.GetComponent<CameraRotationView>();
            playerCamera.Construct(_inputService);
            playerCamera.Initialize();

            var playerBody = player.GetComponent<BodyRotationView>();
            playerBody.Construct(_inputService);
            playerBody.Initialize();

            var playerMovement = player.GetComponent<PlayerMovementView>();
            playerMovement.Construct(_inputService);
            playerMovement.Initialize();

            _gunFactory.ConfigurePlayerGun(player);
            
            _updateProvider.EnqueueRegister(playerCamera);
            _updateProvider.EnqueueRegister(playerBody);
            _updateProvider.EnqueueRegister(playerMovement);

            _playerCreated?.Invoke(player);
            return player;
        }
    }
}