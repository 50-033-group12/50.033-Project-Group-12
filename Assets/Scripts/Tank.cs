using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

class SlowDebuff : Debuff<float>
{
    public override (float, bool) DebuffFunction(float input)
    {
        return (input * 0.5f, false);
    }

    public override string GetName()
    {
        return "test";
    }

    public override bool IsStackable()
    {
        return false;
    }
}
public class Tank : MonoBehaviour, IDebuffable, IDamageable
{
    // HP
    private float health = 150f;

    // Movement
    public bool isMoving = false;
    public float moveVal;
    private DebuffableProperty<float> maxSpeed = new DebuffableProperty<float>(10f);

    // Rotation
    public bool isRotating = false;
    public float rotateVal;
    private DebuffableProperty<float> turnSpeed = new DebuffableProperty<float>(40f);

    // Crosshair
    public GameObject crosshair;
    public bool moveCrosshair = false;
    public Vector3 moveCrossVal;
    public float maxRange = 50f;
    
    // stunned
    private DebuffableProperty<bool> active = new DebuffableProperty<bool>(true);
    
    AmmoSource ammoSource;
    PrimaryWeapon weapon;
    UltimateWeapon ultimateWeapon;
    Rigidbody rigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        ammoSource = new ReplenishableAmmoSource(100);
        weapon = this.transform.GetComponentInChildren<PrimaryWeapon>();
        ultimateWeapon = this.transform.GetComponentInChildren<UltimateWeapon>();
        weapon.SetAmmoSource(ammoSource);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving){
            transform.Translate(0, 0 ,moveVal * maxSpeed.GetFinalValue() * Time.deltaTime);
        }

        if(isRotating){
            Vector3 oldPos = crosshair.transform.position;
            transform.Rotate(0, rotateVal * turnSpeed.GetFinalValue() * Time.deltaTime, 0);
            crosshair.transform.position = oldPos;
            // Debug.Log(turnSpeed.GetFinalValue());
        }

        if(moveCrosshair){
            crosshair.transform.position += moveCrossVal;
            if(Vector3.Distance(this.transform.position, crosshair.transform.position + moveCrossVal) > maxRange){
                Vector3 direction = (this.transform.position - crosshair.transform.position).normalized;
                crosshair.transform.position += direction * (Vector3.Distance(this.transform.position, crosshair.transform.position) - maxRange);
            }
        }
        weapon.LookAt(crosshair.transform.position);
        ultimateWeapon.LookAt(crosshair.transform.position);
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
        ultimateWeapon.FireAt(crosshair.transform);
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

    public void AfflictDamage(DamageRequest req){
        if(health >= req.damage){
            health -= req.damage;
            // fire event here
        }
        else{
            health = 0f;
        }
    }

    // /// <summary>
    // /// Adding debuffs for float properties
    // /// </summary>
    // /// <param name="property"></param>
    // /// <param name="debuff"></param>
    // /// <exception cref="NotImplementedException"></exception>
    // public void AddDebuff(DebuffableProperties property, Debuff<float> debuff)
    // {
    //     
    //     if (property == DebuffableProperties.MOVEMENT_SPEED)
    //     {
    //         maxSpeed.AddDebuff(debuff);
    //     }else if (property == DebuffableProperties.ROTATION_SPEED)
    //     {
    //         turnSpeed.AddDebuff(debuff);
    //     }
    //     else
    //     {
    //         throw new System.NotImplementedException();
    //     }
    // }
    //
    // /// <summary>
    // /// Adding debuffs for bool properties
    // /// </summary>
    // /// <param name="property"></param>
    // /// <param name="debuff"></param>
    // /// <exception cref="NotImplementedException"></exception>
    // public void AddDebuff(DebuffableProperties property, Debuff<bool> debuff)
    // {
    //     if (property == DebuffableProperties.ACTIVE)
    //     {
    //         active.AddDebuff(debuff);
    //         return;
    //     }
    //     throw new System.NotImplementedException();
    // }

    public void AddDebuff<T>(DebuffableProperties property, Debuff<T> debuff)
    {
        if (typeof(T) == typeof(float))
        {
            if (property == DebuffableProperties.MOVEMENT_SPEED)
            {
                maxSpeed.AddDebuff(debuff as Debuff<float>);
            }else if (property == DebuffableProperties.ROTATION_SPEED)
            {
                turnSpeed.AddDebuff(debuff as Debuff<float>);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        if (typeof(T) == typeof(bool))
        {
            if (property == DebuffableProperties.ACTIVE)
            {
                active.AddDebuff(debuff as Debuff<bool>);
            }
        }
    }
}
