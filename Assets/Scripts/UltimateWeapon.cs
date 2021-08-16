using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class UltimateWeapon : MonoBehaviour
{
    public float nextFire;
    public float ultiTicks;
    public float ultiTicksNeeded;

    public bool IsReadyToFire(){
        if(Time.time > nextFire){
            return true;
        }
        return false;
    }

    public abstract void FireAt(Transform target);
    public abstract void LookAt(Vector3 target);
    public abstract float GetFireRate();
    public abstract Events.UltimateWeapon GetUltimateWeaponType();
    public IEnumerator ultimateTick()
    {
        while (ultiTicks < ultiTicksNeeded)
        {
            ultiTicks++;
            this.GetComponentInParent<PlayerEvents>().tickedUltimateCooldown.Invoke((int) ultiTicks, (int) ultiTicksNeeded);
            yield return new WaitForSeconds(1);
        }
    }
}
