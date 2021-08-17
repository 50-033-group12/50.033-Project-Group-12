using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class ClippedPrimaryWeapon : PrimaryWeapon
{
    protected int clipRemaining;
    protected int reloadTick = 0;
    protected int reloadTicksNeeded = 20;

    private bool IsReloading
    {
        get
        {
            return (reloadTick > 0 && reloadTick < reloadTicksNeeded);
        }
    }

    protected virtual void Start()
    {
        nextFire = Time.time;
        SwapMagazine();
    }

    public int GetClipRemaining()
    {
        return clipRemaining;
    }

    public abstract int GetClipSize();

    public override bool IsReadyToFire()
    {
        return !IsReloading && base.IsReadyToFire();
    }

    public override void Reload()
    {
        if (ammoSource.GetCount() <= (GetClipSize() - clipRemaining))
        {
            return;
        }

        if (reloadTick == 0)
        {
            StartCoroutine(ReloadTick());
        }
    }

    public override void FireAt(Transform target)
    {
        clipRemaining -= 1;
        nextFire = Time.time + GetFireRate();
    }

    IEnumerator ReloadTick()
    {
        while (reloadTick < reloadTicksNeeded)
        {
            reloadTick++;
            this.GetComponentInParent<PlayerEvents>().tickedPrimaryReload.Invoke(reloadTick, reloadTicksNeeded);
            yield return new WaitForSeconds(0.1f);
        }

        reloadTick = 0;
        SwapMagazine();
    }

    protected void SwapMagazine()
    {
        ammoSource.Consume(GetClipSize() - clipRemaining);
        clipRemaining = GetClipSize();
        this.GetComponentInParent<PlayerEvents>().primaryAmmoChanged.Invoke(clipRemaining, ammoSource.GetCount());
    }
}