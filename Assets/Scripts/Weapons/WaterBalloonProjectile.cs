using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonProjectile : MonoBehaviour
{
    public AudioClip sfx;
    public GameObject water;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.tag == "Ground" || col.tag == "Obstacles"){
            Vector3 newPos = new Vector3(this.transform.position.x, 0.44f, this.transform.position.z);
            Instantiate(water, newPos, Quaternion.identity);
            this.GetComponent<AudioSource>().PlayOneShot(sfx);
            Destroy(gameObject, sfx.length);
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
