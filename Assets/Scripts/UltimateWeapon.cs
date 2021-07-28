using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UltimateWeapon : MonoBehaviour
{
    public float nextFire;

    public bool IsReadyToFire(){
        if(Time.time > nextFire){
            return true;
        }
        return false;
    }

    public abstract void OnFire();

    public abstract void FireAt(Transform target);
    public abstract void LookAt(Vector3 target);
    public abstract float GetFireRate();
}
