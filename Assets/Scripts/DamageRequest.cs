using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRequest
{
    public float damage;
    public bool penetrating;
    public GameObject source;

    public DamageRequest(float d, bool p, GameObject source){
        this.damage = d;
        this.penetrating = p;
        this.source = source;
    }
}
