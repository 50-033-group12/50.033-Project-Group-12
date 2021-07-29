using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloonProjectile : MonoBehaviour
{
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
        collission();
        if(col.tag == "Ground" || col.tag == "Obstacles"){
            Vector3 newPos = new Vector3(this.transform.position.x, 0.03f, this.transform.position.z);
            Instantiate(water, newPos, this.transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible(){
        Destroy(gameObject);
    }

    public void collission(){
        Debug.Log("Collision behaviour here");
    }
}
