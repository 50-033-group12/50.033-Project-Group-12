using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTarget : MonoBehaviour
{
    private Vector3 pivot;
    public float maxRadius = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate () 
	{
        pivot = this.transform.parent.transform.position;
    	Plane playerPlane = new Plane(Vector3.up, transform.position);
 
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

    	float hitdist = 0.0f;
    	// If the ray is parallel to the plane, Raycast will return false.
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	Vector3 targetPoint = ray.GetPoint(hitdist);
            float distance = Vector3.Distance(pivot, targetPoint);
            if(distance<=maxRadius){
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPoint, 5f);
            }
		}
    }
}
