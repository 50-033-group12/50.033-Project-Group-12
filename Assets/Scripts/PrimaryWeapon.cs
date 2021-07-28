using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimaryWeapon : MonoBehaviour
{
    public AmmoSource ammoSource;
    public float nextFire;
    
    public void SetAmmoSource(AmmoSource source){
        ammoSource = source;
    }

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
}
