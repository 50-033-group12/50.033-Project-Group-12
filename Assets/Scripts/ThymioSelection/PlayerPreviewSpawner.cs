using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreviewSpawner : MonoBehaviour
{
    private GameObject _thymioBodyPrefab;
    private Dictionary<Events.PrimaryWeapon, GameObject> _primaryWeaponPrefabs;
    private Dictionary<Events.SecondaryWeapon, GameObject> _secondaryWeaponPrefabs;
    private Dictionary<Events.UltimateWeapon, GameObject> _ultimateWeaponPrefabs;
    [SerializeField] private Transform spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        ReloadPrefabs();
    }

    public void ReloadPrefabs()
    {
        _thymioBodyPrefab = (GameObject)Resources.Load("Models/ThymioBody");
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
                (Events.UltimateWeapon)Enum.Parse(typeof(Events.UltimateWeapon), ultimateWeaponName);
            var prefab = Resources.Load($"Models/Ultimate/{ultimateWeaponName}");
            _ultimateWeaponPrefabs[ultimateWeapon] = (GameObject)prefab;
        }
    }

    public GameObject SpawnPlayerPreview(int playerId,
        Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> loadout)
    {
        //create empty parent
        var preview = new GameObject();
        // create thymio body
        var body = Instantiate(_thymioBodyPrefab, preview.transform);

        // create primary weapon
        var primary = Instantiate(_primaryWeaponPrefabs[loadout.Item1], preview.transform);
        foreach (var mb in primary.GetComponentsInChildren<MonoBehaviour>())
        {
            mb.enabled = false;
        }

        // create secondary weapon
        var secondary = Instantiate(_secondaryWeaponPrefabs[loadout.Item2], preview.transform);
        foreach (var mb in secondary.GetComponentsInChildren<MonoBehaviour>())
        {
            mb.enabled = false;
        }

        // create ultimate
        var ultimate = Instantiate(_ultimateWeaponPrefabs[loadout.Item3], preview.transform);
        foreach (var mb in ultimate.GetComponentsInChildren<MonoBehaviour>())
        {
            mb.enabled = false;
        }

        preview.transform.position = spawnPosition.position;
        preview.transform.rotation = spawnPosition.rotation;
        preview.name = $"Player {playerId} Preview";

        preview.AddComponent(typeof(TankPainter));
        return preview;
    }
}