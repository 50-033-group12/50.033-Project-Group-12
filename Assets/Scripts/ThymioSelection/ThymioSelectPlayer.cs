using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ThymioSelection
{
    public class ThymioSelectPlayer : MonoBehaviour
    {
        [SerializeField] private int playerId = 1;
        [SerializeField] private ColorPalette teamColorPalette;

        [SerializeField] private GameObject doorAbove;

        [SerializeField] private GameObject doorBelow;

        [SerializeField] private GameObject pressStartText;

        [SerializeField] private PrimaryWeaponDescription[] primaryWeaponsAvailable;

        [SerializeField] private SecondaryWeaponDescription[] secondaryWeaponsAvailable;

        [SerializeField] private UltimateWeaponDescription[] ultimateWeaponsAvailable;

        private bool _active = false;
        private bool _isAnimating = false;
        private int _activeColumn = 0;
        private int _chosenPrimaryIndex = 0;
        private int _chosenSecondaryIndex = 0;
        private int _chosenUltimateIndex = 0;
        private int _chosenColorIndex = 0;
        private bool _playerReady = false;
        private GameObject _playerPreview;

        public PrimaryWeaponDescription ChosenPrimary
        {
            get
            {
                int x = _chosenPrimaryIndex;
                int m = primaryWeaponsAvailable.Length;
                return primaryWeaponsAvailable[(x % m + m) % m];
            }
        }

        public SecondaryWeaponDescription ChosenSecondary
        {
            get
            {
                int x = _chosenSecondaryIndex;
                int m = secondaryWeaponsAvailable.Length;
                return secondaryWeaponsAvailable[(x % m + m) % m];
            }
        }

        public UltimateWeaponDescription ChosenUltimate
        {
            get
            {
                int x = _chosenUltimateIndex;
                int m = ultimateWeaponsAvailable.Length;
                return ultimateWeaponsAvailable[(x % m + m) % m];
            }
        }

        public Color ChosenColor
        {
            get
            {
                int x = _chosenColorIndex;
                int m = teamColorPalette.colors.Length;
                return teamColorPalette.colors[(x % m + m) % m];
            }
        }

        [SerializeField] private GameObject[] equipmentBackgrounds;
        [SerializeField] private Image primaryIconContainer;
        [SerializeField] private Image secondaryIconContainer;
        [SerializeField] private Image ultimateIconContainer;
        [SerializeField] private Text playerIdText;
        [SerializeField] private Text readyStatusText;

        [SerializeField] private Text itemName;
        [SerializeField] private Text itemType;
        [SerializeField] private Text itemDescription;

        [SerializeField] private Image colorPreview;

        // Start is called before the first frame update
        void Start()
        {
            playerIdText.text = $"PLAYER {playerId}";
            colorPreview.color = teamColorPalette.colors[0];
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
            float movement = ((RectTransform)doorBelow.transform).rect.width;
            LeanTween.alphaText((RectTransform)pressStartText.transform, 0, 0.25f);
            LeanTween.moveX(doorBelow, doorBelow.transform.position.x - movement, 1f).setEaseOutCubic();
            LeanTween.moveX(doorAbove, doorAbove.transform.position.x + movement, 1f).setEaseOutCubic()
                .setOnComplete(() => this._isAnimating = false);
        }

        void CloseDoors()
        {
            _isAnimating = true;
            float movement = ((RectTransform)doorBelow.transform).rect.width;
            LeanTween.alphaText((RectTransform)pressStartText.transform, 1, 0.25f);
            LeanTween.moveX(doorBelow, doorBelow.transform.position.x + movement, 0.25f).setEaseInCubic();
            LeanTween.moveX(doorAbove, doorAbove.transform.position.x - movement, 0.25f).setEaseInCubic()
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
            _activeColumn = (int)Mathf.Clamp(_activeColumn + value.x, 0, 3);
            if (_activeColumn == 0)
            {
                _chosenPrimaryIndex = _chosenPrimaryIndex + (int)value.y;
            }
            else if (_activeColumn == 1)
            {
                _chosenSecondaryIndex = _chosenSecondaryIndex + (int)value.y;
            }
            else if (_activeColumn == 2)
            {
                _chosenUltimateIndex = _chosenUltimateIndex + (int)value.y;
            }
            else if (_activeColumn == 3)
            {
                _chosenColorIndex = _chosenColorIndex + (int)value.y;
            }

            RedrawGUI();
        }

        void RedrawGUI()
        {
            for (int i = 0; i < 3; i++)
            {
                var currentImage = equipmentBackgrounds[i];
                LeanTween.cancel(currentImage);
                currentImage.transform.localScale = Vector3.one;
            }

            if (_playerPreview != null)
            {
                Destroy(_playerPreview);
            }

            colorPreview.color = ChosenColor;
            LeanTween.cancel(colorPreview.gameObject);
            colorPreview.transform.localScale = Vector3.one;

            if (_activeColumn < 3)
            {
                var currentImage = equipmentBackgrounds[_activeColumn];
                LeanTween.scaleX(currentImage, 1.5f, 0.5f).setLoopPingPong();
                LeanTween.scaleY(currentImage, 1.5f, 0.5f).setLoopPingPong();
            }
            else if (_activeColumn == 3)
            {
                var currentImage = colorPreview.gameObject;
                LeanTween.scaleX(currentImage, 1.5f, 0.5f).setLoopPingPong();
                LeanTween.scaleY(currentImage, 1.5f, 0.5f).setLoopPingPong();
            }

            // set item type text
            if (_activeColumn == 0)
            {
                itemType.text = "Primary Weapon";
                itemName.text = ChosenPrimary.name;
                itemDescription.text = ChosenPrimary.description;
            }
            else if (_activeColumn == 1)
            {
                itemType.text = "Secondary Weapon";
                itemName.text = ChosenSecondary.name;
                itemDescription.text = ChosenSecondary.description;
            }
            else if (_activeColumn == 2)
            {
                itemType.text = "Ultimate Weapon";
                itemName.text = ChosenUltimate.name;
                itemDescription.text = ChosenUltimate.description;
            }

            primaryIconContainer.sprite = ChosenPrimary.icon;
            secondaryIconContainer.sprite = ChosenSecondary.icon;
            ultimateIconContainer.sprite = ChosenUltimate.icon;

            _playerPreview = GetComponent<PlayerPreviewSpawner>().SpawnPlayerPreview(playerId,
                new Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon>(
                    ChosenPrimary.primaryWeaponEnum,
                    ChosenSecondary.secondaryWeaponEnum,
                    ChosenUltimate.ultimateWeaponEnum
                ));
            var painter = _playerPreview.GetComponent<TankPainter>();
            painter.CloneMaterials();
            painter.PaintTeamColors(ChosenColor);
        }

        public Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon> GetLoadout()
        {
            return new Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon>(
                ChosenPrimary.primaryWeaponEnum,
                ChosenSecondary.secondaryWeaponEnum,
                ChosenUltimate.ultimateWeaponEnum
            );
        }
    }
}