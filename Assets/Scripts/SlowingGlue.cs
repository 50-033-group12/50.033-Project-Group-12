using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

class SlowGlueDebuff : Debuff<float>
{
    public override (float, bool) DebuffFunction(float input)
    {
        return (input * 0.5f, false);
    }

    public override string GetName()
    {
        return "SLow GLue";
    }

    public override bool IsStackable()
    {
        return false;
    }
}

public class SlowingGlue : MonoBehaviour
{

    private Dictionary<IDebuffable, SlowGlueDebuff> _activeDebuffs;

    private void Start()
    {
        _activeDebuffs = new Dictionary<IDebuffable, SlowGlueDebuff>();
    }

    void OnTriggerEnter(Collider col)
    {
        IDebuffable target = col.gameObject.GetComponentInParent<IDebuffable>();
        if (target != null && !_activeDebuffs.ContainsKey(target))
        {
            var debuff = new SlowGlueDebuff();
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