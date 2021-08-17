using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerManagement
{
    public class PlayerSpawner : MonoBehaviour
    {
        private GameObject _playerContainerPrefab;
        private Dictionary<Events.PrimaryWeapon, GameObject> _primaryWeaponPrefabs;
        private Dictionary<Events.SecondaryWeapon, GameObject> _secondaryWeaponPrefabs;
        private Dictionary<Events.UltimateWeapon, GameObject> _ultimateWeaponPrefabs;
        [SerializeField] private Transform[] spawnPositions;
    
        // editor stuff
        [SerializeField] private InputDevice device;

        // Start is called before the first frame update
        void Start()
        {
            ReloadPrefabs();
            int i = 0;
            foreach (int playerId in LoadoutManager.GetPlayerIds())
            {
                Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> playerLoadout = LoadoutManager.GetPlayerLoadout(playerId);
                var player = SpawnPlayer(playerId, playerLoadout);
                player.transform.position = spawnPositions[i % spawnPositions.Length].transform.position;
                player.transform.rotation = spawnPositions[i % spawnPositions.Length].transform.rotation;
                i++;
            }
        }

        public void ReloadPrefabs()
        {
            _playerContainerPrefab = (GameObject) Resources.Load("Models/PlayerContainer");
            _primaryWeaponPrefabs = new Dictionary<Events.PrimaryWeapon, GameObject>();
            _secondaryWeaponPrefabs = new Dictionary<Events.SecondaryWeapon, GameObject>();
            _ultimateWeaponPrefabs = new Dictionary<Events.UltimateWeapon, GameObject>();
            var primaryWeaponNames = Enum.GetNames(typeof(Events.PrimaryWeapon));
            foreach (var primaryWeaponName in primaryWeaponNames)
            {
                var primaryWeapon = (Events.PrimaryWeapon)Enum.Parse(typeof(Events.PrimaryWeapon), primaryWeaponName);
                var prefab = Resources.Load($"Models/Primary/{primaryWeaponName}");
                _primaryWeaponPrefabs[primaryWeapon] = (GameObject)prefab;
            }

            var secondaryWeaponNames = Enum.GetNames(typeof(Events.SecondaryWeapon));
            foreach (var secondaryWeaponName in secondaryWeaponNames)
            {
                var secondaryWeapon =
                    (Events.SecondaryWeapon)Enum.Parse(typeof(Events.SecondaryWeapon), secondaryWeaponName);
                var prefab = Resources.Load($"Models/Secondary/{secondaryWeaponName}");
                _secondaryWeaponPrefabs[secondaryWeapon] = (GameObject)prefab;
            }

            var ultimateWeaponNames = Enum.GetNames(typeof(Events.UltimateWeapon));
            foreach (var ultimateWeaponName in ultimateWeaponNames)
            {
                var ultimateWeapon =
                    (Events.UltimateWeapon) Enum.Parse(typeof(Events.UltimateWeapon), ultimateWeaponName);
                var prefab = Resources.Load($"Models/Ultimate/{ultimateWeaponName}");
                _ultimateWeaponPrefabs[ultimateWeapon] = (GameObject) prefab;
            }
        }

        public GameObject SpawnPlayer(int playerId, Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> loadout)
        {
            // create thymio body
            var player = PlayerInput.Instantiate(_playerContainerPrefab, controlScheme: "Gamepad", pairWithDevice: LoadoutManager.GetPlayerDevice(playerId));

            // create primary weapon
            Instantiate(_primaryWeaponPrefabs[loadout.Item1], player.transform);

            // create secondary weapon
            Instantiate(_secondaryWeaponPrefabs[loadout.Item2], player.transform);

            // create ultimate
            Instantiate(_ultimateWeaponPrefabs[loadout.Item3], player.transform);
            player.name = $"Player {playerId}";
            var tank = player.GetComponent<Tank>();
            tank.playerId = playerId;
            var playerEventBus = player.GetComponent<PlayerEvents>();
            UIManagerManager manager = UIManagerManager.GetInstance();
            if (manager != null && manager.GetPlayerUI(playerId) != null)
            {
                Debug.Log(playerId);
                UIManager ui = manager.GetPlayerUI(playerId);
                playerEventBus.equippedPrimary.AddListener(ui.ChangePrimaryWeapon);
                playerEventBus.equippedSecondary.AddListener(ui.ChangeSecondaryWeapon);
                playerEventBus.equippedUltimate.AddListener(ui.ChangeUltimateWeapon);
                playerEventBus.fuelChanged.AddListener(ui.ChangeFuel);
                playerEventBus.hpChanged.AddListener(ui.ChangeHealth);
                playerEventBus.primaryAmmoChanged.AddListener(ui.ChangeAmmo);
                playerEventBus.tickedPrimaryReload.AddListener(ui.TickedPrimaryReload);
                playerEventBus.tickedSecondaryCooldown.AddListener(ui.TickedSecondaryCooldown);
                playerEventBus.tickedUltimateCooldown.AddListener(ui.TickedUltimateCooldown);
            }
            return player.gameObject;
        }
    }
}