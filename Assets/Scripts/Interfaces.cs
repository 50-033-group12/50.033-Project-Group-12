using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Fire();
    void Reload();
    void GetCooldown();
    void GetAmmo();
}

public interface IDamageable
{
    void AfflictDamage(DamageRequest req);
}

public interface AmmoSource
{
    int GetCount();
    void Consume(int amount);
}