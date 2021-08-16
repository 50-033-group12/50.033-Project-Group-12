using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class SecondaryWeapon : MonoBehaviour
{
    public float nextFire;
    public float secondaryTicks;
    public float secondaryTicksNeeded;

    public bool IsReadyToFire(){
        if(Time.time >= nextFire){
            return true;
        }
        return false;
    }
    
    public abstract void FireAt(Transform target);
    public abstract void LookAt(Vector3 target);
    public abstract float GetFireRate();
    public abstract float GetTurnRate();
    public abstract Events.SecondaryWeapon GetSecondaryWeaponType();

    public IEnumerator secondaryTick()
    {
        while (secondaryTicks < secondaryTicksNeeded)
        {
            secondaryTicks+=1f;
            this.GetComponentInParent<PlayerEvents>().tickedSecondaryCooldown.Invoke((int) secondaryTicks, (int) secondaryTicksNeeded);
            yield return new WaitForSeconds(1);
        }
    }
}
