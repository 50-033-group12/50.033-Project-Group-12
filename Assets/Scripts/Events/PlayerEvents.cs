using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public enum PrimaryWeapon
    {
        StaplerGun,
        RubberBandGun,
        EraserGun,
        Penknife
    }

    public enum SecondaryWeapon
    {
        Glue,
        PaperShield,
        TrackingDart,
        WirelessShark,
        Solenoid,
        UsbFan,
        BottleRocket,
        SolarPanel,
        JumperWires,
        BrushlessMotors,
        GladiatorArmor
    }

    public enum UltimateWeapon
    {
        Ddos,
        G2PenGun,
        LiPoBattery,
        WaterBalloon,
        StaplerEraserBeyblade,
        ExtendedPenknife
    }

    public class PlayerEvents : MonoBehaviour
    {
        // initialisation
        public UnityEvent<PrimaryWeapon> equippedPrimary;
        public UnityEvent<SecondaryWeapon> equippedSecondary;
        public UnityEvent<UltimateWeapon> equippedUltimate;

        // things happening to the robot
        public UnityEvent<float, float> fuelChanged;
        public UnityEvent<float, float> hpChanged;
        public UnityEvent<int, int> primaryAmmoChanged;

        // user initiated events
        public UnityEvent firedPrimary;
        public UnityEvent firedSecondary;
        public UnityEvent firedUltimate;

        // cooldowns and reloads
        public UnityEvent<int, int> tickedPrimaryReload;
        public UnityEvent<int, int> tickedSecondaryCooldown;
        public UnityEvent<int, int> tickedUltimateCooldown;
    }
}