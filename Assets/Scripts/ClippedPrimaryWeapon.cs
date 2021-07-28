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
        // if ammosource has enough to replenish the used up bullets, replenish
        if(ammoSource.GetCount() >= (GetClipSize() - clipRemaining))
        {
            ammoSource.Consume(GetClipSize() - clipRemaining);
            clipRemaining = GetClipSize();
        }

        //else do nothing
        else{
            Debug.Log("Ammo source is not enough!");
        }
        
    }

    public void FireAt(Transform target){
        clipRemaining -= 1;
    }
}
