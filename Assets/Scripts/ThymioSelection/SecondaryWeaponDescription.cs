using UnityEngine;

namespace ThymioSelection
{
    
    [CreateAssetMenu(fileName = "SecondaryWeapon", menuName = "Item Descriptions/Secondary Weapon", order = 2)]
    public class SecondaryWeaponDescription : ItemDescription
    {
        public int cooldown;
    }
}