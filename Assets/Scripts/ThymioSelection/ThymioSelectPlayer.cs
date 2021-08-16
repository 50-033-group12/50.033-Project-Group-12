using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ThymioSelection
{
    public class ThymioSelectPlayer : MonoBehaviour
    {
        [SerializeField] private int playerId = 1;
        [SerializeField] private Material teamColor;

        [SerializeField] private GameObject _doorAbove;

        [SerializeField] private GameObject _doorBelow;

        [SerializeField] private GameObject _pressStartText;

        [SerializeField] private PrimaryWeaponDescription[] _primaryWeaponsAvailable;

        [SerializeField] private SecondaryWeaponDescription[] _secondaryWeaponsAvailable;

        [SerializeField] private UltimateWeaponDescription[] _ultimateWeaponsAvailable;

        private bool _active = false;
        private bool _isAnimating = false;
        private int _activeColumn = 0;
        private int _chosenPrimary = 0;
        private int _chosenSecondary = 0;
        private int _chosenUltimate = 0;
        private bool _playerReady = false;

        [SerializeField] private GameObject[] _equipmentBackgrounds;
        [SerializeField] private Image primaryIconContainer;
        [SerializeField] private Image secondaryIconContainer;
        [SerializeField] private Image ultimateIconContainer;
        [SerializeField] private Text playerIdText;
        [SerializeField] private Text readyStatusText;

        [SerializeField] private Text _itemName;
        [SerializeField] private Text _itemType;
        [SerializeField] private Text _itemDescription;

        [SerializeField] private Image colorPreview;

        // Start is called before the first frame update
        void Start()
        {
            playerIdText.text = $"PLAYER {playerId}";
            colorPreview.color = teamColor.color;
        }

        void OnStart()
        {
            if (_isAnimating) return;
            _active = !_active;
            if (_active)
            {
                OpenDoors();
                RedrawGUI();
            }
            else
            {
                CloseDoors();
            }
        }


        void OpenDoors()
        {
            _isAnimating = true;
            float movement = ((RectTransform)_doorBelow.transform).rect.width;
            LeanTween.alphaText((RectTransform)_pressStartText.transform, 0, 0.25f);
            LeanTween.moveX(_doorBelow, _doorBelow.transform.position.x - movement, 1f).setEaseOutCubic();
            LeanTween.moveX(_doorAbove, _doorAbove.transform.position.x + movement, 1f).setEaseOutCubic()
                .setOnComplete(() => this._isAnimating = false);
        }

        void CloseDoors()
        {
            _isAnimating = true;
            float movement = ((RectTransform)_doorBelow.transform).rect.width;
            LeanTween.alphaText((RectTransform)_pressStartText.transform, 1, 0.25f);
            LeanTween.moveX(_doorBelow, _doorBelow.transform.position.x + movement, 0.25f).setEaseInCubic();
            LeanTween.moveX(_doorAbove, _doorAbove.transform.position.x - movement, 0.25f).setEaseInCubic()
                .setOnComplete(() => this._isAnimating = false);
            ;
        }

        void OnDeviceLost()
        {
            if (_active)
            {
                _active = false;
                CloseDoors();
            }
        }

        void OnDeviceRegained()
        {
            Debug.Log("Device regained");
        }

        void OnReady(InputValue inputValue)
        {
            if (!_active) return;
            if (inputValue.Get<float>() != 0) return;
            _playerReady = !_playerReady;
            readyStatusText.text = _playerReady ? "READY!" : "PRESS RT";
        }

        public bool IsReady()
        {
            return _playerReady;
        }

        void OnNavigate(InputValue inputValue)
        {
            if (!_active) return;
            Vector2 value = inputValue.Get<Vector2>();
            if (value == Vector2.zero) return;
            _activeColumn = (int)Mathf.Clamp(_activeColumn + value.x, 0, 2);
            if (_activeColumn == 0)
            {
                _chosenPrimary = (int)Mathf.Clamp(_chosenPrimary + value.y, 0, _primaryWeaponsAvailable.Length - 1);
            }
            else if (_activeColumn == 1)
            {
                _chosenSecondary = (int)Mathf.Clamp(_chosenSecondary + value.y, 0, _secondaryWeaponsAvailable.Length - 1);
            }
            else if (_activeColumn == 2)
            {
                _chosenUltimate = (int)Mathf.Clamp(_chosenUltimate + value.y, 0, _ultimateWeaponsAvailable.Length - 1);
            }

            RedrawGUI();
        }

        void RedrawGUI()
        {
            for (int i = 0; i < 3; i++)
            {
                var currentImage = _equipmentBackgrounds[i].transform.Find("IconBackground").gameObject;
                LeanTween.cancel(currentImage);
                currentImage.transform.localScale = new Vector3(1, 1, 1);
            }

            if (_activeColumn < 3)
            {
                var currentImage = _equipmentBackgrounds[_activeColumn].transform.Find("IconBackground").gameObject;
                LeanTween.scaleX(currentImage, 1.5f, 0.5f).setLoopPingPong();
                LeanTween.scaleY(currentImage, 1.5f, 0.5f).setLoopPingPong();
            }

            // set item type text
            if (_activeColumn == 0)
            {
                _itemType.text = "Primary Weapon";
                _itemName.text = _primaryWeaponsAvailable[_chosenPrimary].name;
                _itemDescription.text = _primaryWeaponsAvailable[_chosenPrimary].description;
            }
            else if (_activeColumn == 1)
            {
                _itemType.text = "Secondary Weapon";
                _itemName.text = _secondaryWeaponsAvailable[_chosenSecondary].name;
                _itemDescription.text = _secondaryWeaponsAvailable[_chosenSecondary].description;
            }
            else if (_activeColumn == 2)
            {
                _itemType.text = "Ultimate Weapon";
                _itemName.text = _ultimateWeaponsAvailable[_chosenUltimate].name;
                _itemDescription.text = _ultimateWeaponsAvailable[_chosenUltimate].description;
            }

            primaryIconContainer.sprite = _primaryWeaponsAvailable[_chosenPrimary].icon;
            secondaryIconContainer.sprite = _secondaryWeaponsAvailable[_chosenSecondary].icon;
            ultimateIconContainer.sprite = _ultimateWeaponsAvailable[_chosenUltimate].icon;
        }

        public Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> GetLoadout()
        {
            return new Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon>(
                _primaryWeaponsAvailable[_chosenPrimary].primaryWeaponEnum,
                _secondaryWeaponsAvailable[_chosenSecondary].secondaryWeaponEnum,
                _ultimateWeaponsAvailable[_chosenUltimate].ultimateWeaponEnum
            );
        }
    }
}