using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserGun : PrimaryWeapon
{
    public GameObject bullet;
    
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 calcBallisticVelocityVector(Transform source, Transform target, float angle)
    {
        Vector3 direction = target.position - source.position;                     // get target direction
        float h = direction.y;                                            // get height difference
        direction.y = 0;                                                  // remove height
        float distance = direction.magnitude;                             // get horizontal distance
        float a = angle * Mathf.Deg2Rad;                                  // Convert angle to radians
        direction.y = distance * Mathf.Tan(a);                            // Set direction to elevation angle
        distance += h/Mathf.Tan(a);                                       // Correction for small height differences
        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
        return velocity * direction.normalized;
    }

    public override void FireAt(Transform target){
        if(IsReadyToFire()){
            // Instantiate bullet
            GameObject bulletShot = Instantiate(bullet, this.transform.position, this.transform.rotation);
            
            Rigidbody m_Rigidbody = bulletShot.GetComponent<Rigidbody>();
            Vector3 vel = calcBallisticVelocityVector(this.transform, target, 60f);
            // Debug.Log(vel);
            m_Rigidbody.velocity = vel;

            nextFire = Time.time + GetFireRate();
        }
    }

    public override void LookAt(Vector3 target){
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetTurnRate() * Time.deltaTime);
    }

    public override float GetFireRate()
    {
        return 1.5f;
    }

    public override float GetTurnRate()
    {
        return 2f;
    }
}
