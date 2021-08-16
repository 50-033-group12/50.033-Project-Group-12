using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserGunProjectile : MonoBehaviour
{
    public GameObject shotFrom;
    public float damage = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject != shotFrom){
            if (col.tag == "Player"){
                DamageRequest req = new DamageRequest(damage, false, this.gameObject);
                col.GetComponentInParent<IDamageable>().AfflictDamage(req);
                Destroy(gameObject);
            }
            else if(col.tag == "Ground" || col.tag == "Obstacles"){
                Destroy(gameObject);
            }
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
