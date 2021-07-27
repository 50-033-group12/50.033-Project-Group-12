using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClippedPrimaryWeapon : PrimaryWeapon
{
    int clipRemaining;
    public int GetClipRemaining(){
        return clipRemaining;
    }
    public abstract int GetClipSize();

    public void Reload(){
        // ammoSource.Consume(GetClipSize());
        clipRemaining = GetClipSize();
    }

    public void FireAt(Transform target){
        clipRemaining -= 1;
    }
}
