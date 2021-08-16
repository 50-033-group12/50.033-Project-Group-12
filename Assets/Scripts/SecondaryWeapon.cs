using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class SecondaryWeapon : MonoBehaviour
{
    public float nextFire;

    public bool IsReadyToFire(){
        if(Time.time > nextFire){
            return true;
        }
        return false;
    }
    
    public abstract void FireAt(Transform target);
    public abstract void LookAt(Vector3 target);
    public abstract float GetFireRate();
    public abstract float GetTurnRate();
    public abstract Events.SecondaryWeapon GetSecondaryWeaponType();
}
