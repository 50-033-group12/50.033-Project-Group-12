using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G2PenGun : UltimateWeapon
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time + GetFireRate();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnFire(){
        FireAt(this.transform);
    }

    public override void FireAt(Transform target){
        
    }

    public override void LookAt(Vector3 target){
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetTurnRate() * Time.deltaTime);
    }
    
    public override float GetFireRate(){
        return 30f;
    }
}
