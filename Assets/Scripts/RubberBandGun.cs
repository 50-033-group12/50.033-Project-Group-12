using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBandGun : ClippedPrimaryWeapon
{
    public GameObject bullet;
    public float bulletSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnFire()
    {
        Fire(this.transform);
    }

    void FixedUpdate () 
	{
    	Plane playerPlane = new Plane(Vector3.up, transform.position);
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    	float hitdist = 0.0f;
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	// Get the point along the ray that hits the calculated distance.
        	Vector3 targetPoint = ray.GetPoint(hitdist);
        	LookAt(targetPoint);
		}
    }

    public override void LookAt(Vector3 target){
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetTurnRate() * Time.deltaTime);
    }

    public override void Fire(Transform target){
        if(IsReadyToFire() && GetClipRemaining() > 0){
            // Instantiate bullet
            GameObject bulletShot = Instantiate(bullet, target.position, target.rotation);
            
            Rigidbody m_Rigidbody = bulletShot.GetComponent<Rigidbody>();
            m_Rigidbody.AddForce(this.transform.forward * bulletSpeed, ForceMode.Impulse);
            
            nextFire = Time.time + GetFireRate();
            base.FireAt(target);
        }
    }

    public override float GetFireRate()
    {
        return 1f;
    }

    public override float GetTurnRate()
    {
        return 3f;
    }

    public override int GetClipSize()
    {
        return 5;
    }
}
