using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

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

    public override void LookAt(Vector3 target){
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, GetTurnRate() * Time.deltaTime);
    }

    public override void FireAt(Transform target){
        if(IsReadyToFire() && GetClipRemaining() > 0){
            // Instantiate bullet
            GameObject bulletShot = Instantiate(bullet, this.transform.position, this.transform.rotation);
            bulletShot.GetComponent<RubberBandProjectile>().shotFrom = this.transform.parent.Find("Thymio 1/Body").gameObject;

            Rigidbody m_Rigidbody = bulletShot.GetComponent<Rigidbody>();
            m_Rigidbody.AddForce(this.transform.forward * bulletSpeed, ForceMode.Impulse);
            
            nextFire = Time.time + GetFireRate();
            base.FireAt(target);
            this.GetComponentInParent<PlayerEvents>().firedPrimary.Invoke();
            this.GetComponentInParent<PlayerEvents>().primaryAmmoChanged.Invoke(GetClipRemaining(), ammoSource.GetCount());
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

    public override Events.PrimaryWeapon GetPrimaryWeaponType(){
        return Events.PrimaryWeapon.RubberBandGun;
    }
}
