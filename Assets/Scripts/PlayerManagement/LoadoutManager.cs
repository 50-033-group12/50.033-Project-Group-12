using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadoutManager : MonoBehaviour
{
    private static LoadoutManager _instance;

    private Dictionary<int, InputDevice> _playerDevices;

    private Dictionary<int, Events.PrimaryWeapon> _playerPrimaryWeapons;
    
    private Dictionary<int, Events.SecondaryWeapon> _playerSecondaryWeapons;

    private Dictionary<int, Events.UltimateWeapon> _playerUltimateWeapons;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Debug.LogError("There is already an instance of LoadoutManager!");
            return;
        }
        _instance = this;
        _playerDevices = new Dictionary<int, InputDevice>();
        _playerPrimaryWeapons = new Dictionary<int, Events.PrimaryWeapon>();
        _playerSecondaryWeapons = new Dictionary<int, Events.SecondaryWeapon>();
        _playerUltimateWeapons = new Dictionary<int, Events.UltimateWeapon>();
    }

    public void JoinPlayer(int playerId, InputDevice device)
    {
        _playerDevices.Add(playerId, device);
    }

    public void ChoosePrimary(int playerId, Events.PrimaryWeapon primaryWeapon)
    {
        _playerPrimaryWeapons.Add(playerId, primaryWeapon);
    }

    public void ChooseSecondary(int playerId, Events.SecondaryWeapon secondaryWeapon)
    {
        _playerSecondaryWeapons.Add(playerId, secondaryWeapon);
    }

    public void ChooseUltimate(int playerId, Events.UltimateWeapon ultimateWeapon)
    {
        _playerUltimateWeapons.Add(playerId, ultimateWeapon);
    }

    public (Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon) GetPlayerLoadout(int playerId)
    {
        return (_playerPrimaryWeapons[playerId], _playerSecondaryWeapons[playerId], _playerUltimateWeapons[playerId]);
    }
}
