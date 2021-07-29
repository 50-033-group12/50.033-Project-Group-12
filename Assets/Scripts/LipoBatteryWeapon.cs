using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipoBatteryWeapon : UltimateWeapon
{
    public GameObject bomb;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time + GetFireRate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FireAt(Transform target){
        if(IsReadyToFire()){
            // Instantiate bomb
            Vector3 newPos = this.transform.position + (this.transform.parent.transform.right * 2f);
            GameObject bulletShot = Instantiate(bomb, newPos, this.transform.rotation);
            
            nextFire = Time.time + GetFireRate();
        }
    }

    public override void LookAt(Vector3 target){
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetTurnRate() * Time.deltaTime);
    }
    
    public override float GetFireRate(){
        return 15f;
    }

    // Would you REALLY rotate a battery????
    public float GetTurnRate(){
        return 5f;
    }
}
