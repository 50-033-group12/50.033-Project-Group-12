using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Events
{
    public class PlayerEventDummySource : MonoBehaviour
    {
        private PlayerEvents _playerEvents;

        private float _fuel;
        private readonly float _maxFuel = 30.0f;
        private int _reloadTick;
        private readonly int _reloadTicksNeeded = 90;
        private int _ultimateCooldownTick;
        private readonly int _ultimateCooldownTicksNeeded = 300;
        private int _secondaryCooldownTick;
        private readonly int _secondaryCooldownTicksNeeded = 150;

        private int _clipAmmo;
        private int _totalAmmo;
        private readonly int _clipSize = 10;

        // choose in inspector to test
        [SerializeField] private PrimaryWeapon _primaryWeapon;
        [SerializeField] private SecondaryWeapon _secondaryWeapon;
        [SerializeField] private UltimateWeapon _ultimateWeapon;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(_primaryWeapon);
            _playerEvents = GetComponent<PlayerEvents>();
            _fuel = _maxFuel;
            _reloadTick = _reloadTicksNeeded;
            _ultimateCooldownTick = 0;
            _secondaryCooldownTick = 0;
            _clipAmmo = _clipSize;
            _totalAmmo = _clipSize * 5;

            _playerEvents.equippedPrimary.Invoke(_primaryWeapon);
            _playerEvents.equippedSecondary.Invoke(_secondaryWeapon);
            _playerEvents.equippedUltimate.Invoke(_ultimateWeapon);
        }

        // Update is called once per frame
        void Update()
        {
            if (_reloadTick < _reloadTicksNeeded)
            {
                _reloadTick++;
                _playerEvents.tickedPrimaryReload.Invoke(_reloadTick, _reloadTicksNeeded);
                if (_reloadTick == _reloadTicksNeeded)
                {
                    _totalAmmo = Math.Max(0, _totalAmmo - _clipSize);
                    _clipAmmo = Math.Min(_totalAmmo, _clipSize);
                    _playerEvents.primaryAmmoChanged.Invoke(_clipAmmo, _totalAmmo);
                }
            }

            if (_secondaryCooldownTick < _secondaryCooldownTicksNeeded)
            {
                _secondaryCooldownTick++;
                _playerEvents.tickedSecondaryCooldown.Invoke(_secondaryCooldownTick, _secondaryCooldownTicksNeeded);
            }

            if (_ultimateCooldownTick < _ultimateCooldownTicksNeeded)
            {
                _ultimateCooldownTick++;
                _playerEvents.tickedUltimateCooldown.Invoke(_ultimateCooldownTick, _ultimateCooldownTicksNeeded);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (_fuel <= 0)
            {
                Debug.Log("No more fuel!");
                return;
            }

            _fuel -= 0.1f;
            _playerEvents.fuelChanged.Invoke(_fuel, _maxFuel);
        }

        public void FirePrimary(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (_clipAmmo <= 0)
            {
                Debug.Log("Pls reload!");
                return;
            }

            _clipAmmo--;
            _playerEvents.firedPrimary.Invoke();
            _playerEvents.primaryAmmoChanged.Invoke(_clipAmmo, _totalAmmo);
        }

        public void FireSecondary(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (_secondaryCooldownTick < _secondaryCooldownTicksNeeded)
            {
                Debug.Log("Secondary is not ready!");
                return;
            }

            _secondaryCooldownTick = 0;
            _playerEvents.firedSecondary.Invoke();
        }

        public void FireUltimate(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (_ultimateCooldownTick < _ultimateCooldownTicksNeeded)
            {
                Debug.Log("Ultimate is not ready!");
                return;
            }

            _ultimateCooldownTick = 0;
            _playerEvents.firedUltimate.Invoke();
        }

        public void Reload(InputAction.CallbackContext context)
        {
            if (_totalAmmo <= 0)
            {
                Debug.Log("Not enough ammo to reload");
                return;
            }

            if (_clipAmmo == _clipSize)
            {
                Debug.Log("Clip is full");
                return;
            }

            Debug.Log("Starting reload...");
            _reloadTick = 0;
        }
    }
}