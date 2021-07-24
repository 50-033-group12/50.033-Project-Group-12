using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaplerGun : MonoBehaviour, IWeapon
{
    public GameObject bullet;
    public float bulletSpeed;
    private int ammo = 10;
    private int clip;
    public float fireRate = 0.5f;
    private float nextShot;
    private float rotationSpeed = 10f;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        nextShot = Time.time;
        clip = ammo;
        // MANUAL FINDING BY NAME
        target = this.transform.Find("Target").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate () 
	{
    	// Generate a plane that intersects the transform's position with an upwards normal.
    	Plane playerPlane = new Plane(Vector3.up, transform.position);
 
    	// Generate a ray from the cursor position
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
 
    	// Determine the point where the cursor ray intersects the plane.
    	// This will be the point that the object must look towards to be looking at the mouse.
    	// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
    	//   then find the point along that ray that meets that distance.  This will be the point
    	//   to look at.
    	float hitdist = 0.0f;
    	// If the ray is parallel to the plane, Raycast will return false.
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	// Get the point along the ray that hits the calculated distance.
        	Vector3 targetPoint = ray.GetPoint(hitdist);
 
        	// Determine the target rotation.  This is the rotation if the transform looks at the target point.
        	Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
 
        	// Smoothly rotate towards the target point.
        	transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
    }

    Vector3 calcBallisticVelocityVector(Transform source, Transform target, float angle)
    {
        Vector3 direction = target.position - source.position;            // get target direction
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

    // Required method for IWeapon interface
    public void Fire(){
        if(Time.time > nextShot && clip > 0){
            // Instantiate bullet
            GameObject bulletShot = Instantiate(bullet, this.transform.position, Quaternion.identity);

            // Add impulse to propel it forward
            // transform.right is bcs of where the model is facing, adjust later
            
            Rigidbody m_Rigidbody = bulletShot.GetComponent<Rigidbody>();
            Vector3 vel = calcBallisticVelocityVector(this.transform, target.transform, 15f);
            // Debug.Log(vel);
            m_Rigidbody.velocity = vel;

            clip--;
            nextShot = Time.time + fireRate;
        }
    }

    public void Reload(){
        clip = ammo;
    }

    public void GetCooldown(){
        Debug.Log("getcooldown called");
    }

    public void GetAmmo(){
        Debug.Log("getammo called");
    }
}
