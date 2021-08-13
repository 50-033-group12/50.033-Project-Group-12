using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserGunProjectile : MonoBehaviour
{
    public int damage = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if (col.tag == "player"){
            // call damage here
            Destroy(gameObject);
        }
        else if(col.tag == "Ground" || col.tag == "Obstacles"){
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
