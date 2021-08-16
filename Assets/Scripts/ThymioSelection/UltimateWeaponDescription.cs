using UnityEngine;

namespace ThymioSelection
{
    [CreateAssetMenu(fileName = "UltimateWeapon", menuName = "Item Descriptions/Ultimate Weapon", order = 3)]
    public class UltimateWeaponDescription : ItemDescription
    {
        public int cooldown;
        public Events.UltimateWeapon ultimateWeaponEnum;
    }
}