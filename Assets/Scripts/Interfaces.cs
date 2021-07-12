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