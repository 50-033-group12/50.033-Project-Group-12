using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    private float moveSpeed = 6f;
    private float maxSpeed = 10f;
    private float turnSpeed = 40f;
    AmmoSource ammoSource;
    PrimaryWeapon weapon;
    Rigidbody rigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        ammoSource = new ReplenishableAmmoSource(100);
        weapon = this.transform.GetComponentInChildren<PrimaryWeapon>();
        weapon.SetAmmoSource(ammoSource);
    }

    // Update is called once per frame
    void Update()
    {
        // For testing of functions
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        ThrottleTank(translation);
        RotateTank(rotation);
    }

    public void ThrottleTank(float input){
        Vector3 i = -this.transform.right * input;
        rigidBody.drag = (moveSpeed / maxSpeed);
        rigidBody.AddForce(i * moveSpeed);
    }

    public void RotateTank(float input){
        transform.Rotate(0, input*turnSpeed*Time.deltaTime, 0);
    }

    public void MoveCrosshair(Vector3 diff){
        Debug.Log("move crosshair");
    }

    public void FirePrimary(){
        weapon.OnFire();
    }

    
}
