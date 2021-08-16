using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class ClippedPrimaryWeapon : PrimaryWeapon
{
    int clipRemaining;
    public int reloadTick;
    public int reloadTicksNeeded = 90;
    public int GetClipRemaining(){
        return clipRemaining;
    }
    public abstract int GetClipSize();
    public override void Reload(){
        // if ammosource has enough to replenish the used up bullets, replenish
        if(ammoSource.GetCount() >= (GetClipSize() - clipRemaining))
        {
            reloadTick = 0;
        }

        //else do nothing
        else{
            Debug.Log("Ammo source is not enough!");
        }
        
        if(reloadTick < reloadTicksNeeded){
            StartCoroutine(ReloadTick());
        }
    }

    public override void FireAt(Transform target){
        clipRemaining -= 1;
    }

    IEnumerator ReloadTick(){
        while(reloadTick < reloadTicksNeeded){
            reloadTick++;
            this.GetComponentInParent<PlayerEvents>().tickedPrimaryReload.Invoke(reloadTick, reloadTicksNeeded);
            yield return null;
        }

        if(reloadTick == reloadTicksNeeded){
            ammoSource.Consume(GetClipSize() - clipRemaining);
            clipRemaining = GetClipSize();
            this.GetComponentInParent<PlayerEvents>().primaryAmmoChanged.Invoke(clipRemaining, ammoSource.GetCount());
        }
        
    }
}
