using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    // Movement
    public bool isMoving = false;
    public float moveVal;
    private float maxSpeed = 10f;

    // Rotation
    public bool isRotating = false;
    public float rotateVal;
    private float turnSpeed = 40f;

    // Crosshair
    public GameObject crosshair;
    public bool moveCrosshair = false;
    public Vector3 moveCrossVal;
    public float maxRange = 15f;

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
        if(isMoving){
            transform.Translate(-moveVal * 6f * Time.deltaTime, 0 ,0);
        }

        if(isRotating){
            Vector3 oldPos = crosshair.transform.position;
            transform.Rotate(0, rotateVal * turnSpeed * Time.deltaTime, 0);
            crosshair.transform.position = oldPos;
        }

        if(moveCrosshair){
            crosshair.transform.position += moveCrossVal;
            if(Vector3.Distance(this.transform.position, crosshair.transform.position + moveCrossVal) > maxRange){
                Vector3 direction = (this.transform.position - crosshair.transform.position).normalized;
                crosshair.transform.position += direction * (Vector3.Distance(this.transform.position, crosshair.transform.position) - maxRange);
            }
        }
        weapon.LookAt(crosshair.transform.position);
    }

    public void ThrottleTank(InputAction.CallbackContext value)
    {
        if(value.phase == InputActionPhase.Performed){
            isMoving = true;
            moveVal = value.ReadValue<Vector2>().y;
        }
        else if(value.phase == InputActionPhase.Canceled){
            isMoving = false;
        }
    }

    public void RotateTank(InputAction.CallbackContext value)
    {
        if(value.phase == InputActionPhase.Performed){
            isRotating = true;
            rotateVal = value.ReadValue<Vector2>().x;
        }
        else if(value.phase == InputActionPhase.Canceled){
            isRotating = false;
        }
    }

    public void MoveCrosshair(InputAction.CallbackContext value)
    {
        if(value.phase == InputActionPhase.Performed){
            moveCrosshair = true;
            float x = value.ReadValue<Vector2>().x;
            float y = value.ReadValue<Vector2>().y;
            moveCrossVal = new Vector3(x,0,y);
        }
        else if(value.phase == InputActionPhase.Canceled){
            moveCrosshair = false;
        }
    }

    public void FirePrimary(InputAction.CallbackContext value)
    {
        weapon.FireAt(crosshair.transform);
    }

    public void FireUltimate(InputAction.CallbackContext value)
    {
        Debug.Log("ultimate");
        // todo
    }

    public void ReloadPrimary(InputAction.CallbackContext value)
    {
        Debug.Log("reload");
        // todo
    }

    public void UseConsumable1(InputAction.CallbackContext value)
    {
        Debug.Log("use consumable 1");
        // todo
    }

    public void UseConsumable2(InputAction.CallbackContext value)
    {
        Debug.Log("use consumable 2");
        // todo
    }
}
