using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserGun : MonoBehaviour, IWeapon
{
    public GameObject bullet;
    public float bulletSpeed;
    private int ammo = 10;
    private int clip;
    public float fireRate = 0.5f;
    private float nextShot;

    // Start is called before the first frame update
    void Start()
    {
        nextShot = Time.time;
        clip = ammo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Required method for IWeapon interface
    public void Fire(){
        Debug.Log(Time.time > nextShot);
        if(Time.time > nextShot && clip > 0){
            // Instantiate bullet
            GameObject bulletShot = Instantiate(bullet, this.transform.position, Quaternion.identity);

            // Add impulse to propel it forward
            Rigidbody m_Rigidbody = bulletShot.GetComponent<Rigidbody>();
            m_Rigidbody.AddForce(-this.transform.right * bulletSpeed, ForceMode.Impulse);

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
