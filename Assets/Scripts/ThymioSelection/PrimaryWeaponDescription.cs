using UnityEngine;

namespace ThymioSelection
{
    public enum DamageCategory
    {
        Low,
        Medium,
        High,
    }

    [CreateAssetMenu(fileName = "PrimaryWeapon", menuName = "Item Descriptions/Primary Weapon", order = 1)]
    public class PrimaryWeaponDescription : ItemDescription
    {
        public bool isClipped;
        public int clipSize;
        public int reserveSize;
        public int fireRate;
        public int reloadTime;
        public DamageCategory damage;
    }
}