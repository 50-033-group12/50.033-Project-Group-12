using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SlowWaterDebuff : Debuff<float>
{
    public override (float, bool) DebuffFunction(float input)
    {
        return (input * 0.3f, false);
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

public class SlowingWater : MonoBehaviour
{
    private Dictionary<IDebuffable, SlowWaterDebuff> _activeDebuffs;

    private void Start()
    {
        _activeDebuffs = new Dictionary<IDebuffable, SlowWaterDebuff>();
    }

    void OnTriggerEnter(Collider col)
    {
        IDebuffable target = col.gameObject.GetComponentInParent<IDebuffable>();
        if (target != null && !_activeDebuffs.ContainsKey(target))
        {
            var debuff = new SlowWaterDebuff();
            target.AddDebuff<float>(DebuffableProperties.MOVEMENT_SPEED, debuff);
            _activeDebuffs.Add(target, debuff);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        IDebuffable target = col.gameObject.GetComponentInParent<IDebuffable>();
        if (target != null && _activeDebuffs.ContainsKey(target))
        {
            var debuff = _activeDebuffs[target];
            debuff.Cancel();
            _activeDebuffs.Remove(target);
        }
    }
}