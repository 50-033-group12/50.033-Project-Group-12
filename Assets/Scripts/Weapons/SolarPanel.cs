using System;
using UnityEngine;

namespace Weapons
{
    public class SolarPanel: SecondaryWeapon
    {
        private void Start()
        {
            nextFire = Time.time + GetFireRate();
            StartCoroutine(secondaryTick());
        }

        public override void FireAt(Transform target)
        {
            return;
        }

        public override void LookAt(Vector3 target)
        {
            return;
        }

        public override float GetFireRate()
        {
            return 1f;
        }

        public override float GetTurnRate()
        {
            return 0;
        }

        public override Events.SecondaryWeapon GetSecondaryWeaponType()
        {
            return Events.SecondaryWeapon.SolarPanel;
        }
    }
}