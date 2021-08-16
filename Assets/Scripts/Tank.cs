using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Events;

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
    // HP, Battery
    private float health;
    private float maxHealth = 150f;
    private float fuel;
    private float maxFuel = 45f;

    // secondary and ultimate ticks
    private int secondaryTicks;
    private int secondaryTicksNeeded;
    private int ultiTicks;
    private int ultiTicksNeeded;

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

    // Events
    private PlayerEvents playerEvents;

    AmmoSource ammoSource;
    PrimaryWeapon weapon;
    SecondaryWeapon secondaryWeapon;
    UltimateWeapon ultimateWeapon;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        ammoSource = new ReplenishableAmmoSource(100);
        weapon = this.transform.GetComponentInChildren<PrimaryWeapon>();
        secondaryWeapon = this.transform.GetComponentInChildren<SecondaryWeapon>();
        ultimateWeapon = this.transform.GetComponentInChildren<UltimateWeapon>();
        weapon.SetAmmoSource(ammoSource);

        playerEvents = GetComponent<PlayerEvents>();

        playerEvents.equippedPrimary.Invoke(weapon.GetPrimaryWeaponType());
        playerEvents.equippedSecondary.Invoke(secondaryWeapon.GetSecondaryWeaponType());
        playerEvents.equippedUltimate.Invoke(ultimateWeapon.GetUltimateWeaponType());

        fuel = maxFuel;
        health = maxHealth;
        secondaryTicksNeeded = (int)(60 * secondaryWeapon.GetFireRate());
        secondaryTicks = secondaryTicksNeeded;

        ultiTicksNeeded = (int)(60 * ultimateWeapon.GetFireRate());
        ultiTicks = ultiTicksNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        // put back after rotation
        Vector3 oldPos = crosshair.transform.position;
        var isStationary = !(isMoving || isRotating);
        var emptyFuelPenalty = fuel >= 0f ? 1f : 0.5f;
        var movementAmount = moveVal * maxSpeed.GetFinalValue() * emptyFuelPenalty * Time.deltaTime;
        var rotationAmount = rotateVal * turnSpeed.GetFinalValue() * emptyFuelPenalty * Time.deltaTime;
        if (isMoving) transform.Translate(0, 0, movementAmount);
        if (isRotating) transform.Rotate(0, rotationAmount, 0);
        crosshair.transform.position = oldPos;
        if (isStationary)
        {
            var regenFuel = Time.deltaTime *
                            (secondaryWeapon.GetSecondaryWeaponType() == Events.SecondaryWeapon.SolarPanel ? 3f : 1f);
            fuel = Mathf.Clamp(fuel + regenFuel, 0, maxFuel);
        }
        else
        {
            fuel = Mathf.Clamp(fuel - Time.deltaTime, 0, maxFuel);
        }

        playerEvents.fuelChanged.Invoke(fuel, maxFuel);
        if (moveCrosshair)
        {
            crosshair.transform.position += moveCrossVal;
            if (Vector3.Distance(this.transform.position, crosshair.transform.position + moveCrossVal) > maxRange)
            {
                Vector3 direction = (this.transform.position - crosshair.transform.position).normalized;
                crosshair.transform.position += direction *
                                                (Vector3.Distance(this.transform.position,
                                                    crosshair.transform.position) - maxRange);
            }
        }

        weapon.LookAt(crosshair.transform.position);
        ultimateWeapon.LookAt(crosshair.transform.position);
    }

    public void ThrottleTank(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            isMoving = true;
            moveVal = value.ReadValue<Vector2>().y;
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            isMoving = false;
        }
    }

    public void RotateTank(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            isRotating = true;
            rotateVal = value.ReadValue<Vector2>().x;
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            isRotating = false;
        }
    }

    public void MoveCrosshair(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            moveCrosshair = true;
            float x = value.ReadValue<Vector2>().x;
            float y = value.ReadValue<Vector2>().y;
            moveCrossVal = new Vector3(x, 0, y);
        }
        else if (value.phase == InputActionPhase.Canceled)
        {
            moveCrosshair = false;
        }
    }

    public void FirePrimary(InputAction.CallbackContext value)
    {
        weapon.FireAt(crosshair.transform);
        playerEvents.firedPrimary.Invoke();
    }

    public void FireUltimate(InputAction.CallbackContext value)
    {
        ultimateWeapon.FireAt(crosshair.transform);
        playerEvents.firedUltimate.Invoke();
        ultiTicks = 0;
        StartCoroutine(ultimateTick());
    }

    public void ReloadPrimary(InputAction.CallbackContext value)
    {
        // Debug.Log("reload");
        weapon.Reload();
    }

    public void UseConsumable1(InputAction.CallbackContext value)
    {
        Debug.Log("use consumable 1");
        secondaryWeapon.FireAt(crosshair.transform);
        playerEvents.firedSecondary.Invoke();
        secondaryTicks = 0;
        StartCoroutine(secondaryTick());
    }

    public void UseConsumable2(InputAction.CallbackContext value)
    {
        Debug.Log("use consumable 2");
        secondaryWeapon.FireAt(crosshair.transform);
        playerEvents.firedSecondary.Invoke();
        secondaryTicks = 0;
        StartCoroutine(secondaryTick());
    }

    public void AfflictDamage(DamageRequest req)
    {
        if (health >= req.damage)
        {
            health -= req.damage;
            playerEvents.hpChanged.Invoke(health, maxHealth);
        }
        else
        {
            health = 0f;
            playerEvents.hpChanged.Invoke(health, maxHealth);
        }
    }

    IEnumerator secondaryTick()
    {
        while (secondaryTicks < secondaryTicksNeeded)
        {
            secondaryTicks++;
            this.GetComponentInParent<PlayerEvents>().tickedSecondaryCooldown
                .Invoke(secondaryTicks, secondaryTicksNeeded);
            yield return null;
        }
    }

    IEnumerator ultimateTick()
    {
        while (ultiTicks < ultiTicksNeeded)
        {
            ultiTicks++;
            this.GetComponentInParent<PlayerEvents>().tickedPrimaryReload.Invoke(ultiTicks, ultiTicksNeeded);
            yield return null;
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
            }
            else if (property == DebuffableProperties.ROTATION_SPEED)
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