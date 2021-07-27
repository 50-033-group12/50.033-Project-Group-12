using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    public PrimaryWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        // // Finding weapon by name, can be changed later
        // weapon = this.transform.Find("Weapon").gameObject.GetComponent<IWeapon>();
    
        weapon = GetComponentInChildren<PrimaryWeapon>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movement & Rotate
        float translation = Input.GetAxis("Vertical") * movementSpeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(-translation, 0, 0);
        transform.Rotate(0, rotation, 0);

        // Shoot button
        if(Input.GetKeyDown("mouse 0")){
            weapon.OnFire();
        }
    }
}
