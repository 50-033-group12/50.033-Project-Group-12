using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject _playerContainerPrefab;
    private Dictionary<Events.PrimaryWeapon, GameObject> _primaryWeaponPrefabs;
    private Dictionary<Events.SecondaryWeapon, GameObject> _secondaryWeaponPrefabs;
    private Dictionary<Events.UltimateWeapon, GameObject> _ultimateWeaponPrefabs;
    
    // editor stuff
    [SerializeField] private InputDevice device;

    // Start is called before the first frame update
    void Start()
    {
        ReloadPrefabs();
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

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnPlayer(int playerId, Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> loadout)
    {

        // create thymio body
        var player = Instantiate(_playerContainerPrefab);

        // create primary weapon
        Instantiate(_primaryWeaponPrefabs[loadout.Item1], player.transform);

        // create secondary weapon
        Instantiate(_secondaryWeaponPrefabs[loadout.Item2], player.transform);

        // create ultimate
        Instantiate(_ultimateWeaponPrefabs[loadout.Item3], player.transform);
        player.name = "Player 1";
        var inputActions = player.GetComponent<PlayerInput>();
    }
}