using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueAmmo : MonoBehaviour
{
    public AudioClip sfx;
    public GameObject glue;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        Debug.Log("Collision behaviour here");
        if(col.tag == "Ground" || col.tag == "Obstacles"){
            Vector3 newPos = new Vector3(this.transform.position.x, 0.03f, this.transform.position.z);
            Instantiate(glue, newPos, this.transform.rotation);
            this.GetComponent<AudioSource>().PlayOneShot(sfx);
            Destroy(gameObject, sfx.length);
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }

}
