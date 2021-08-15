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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.GetComponent<IDebuffable>() != null){
            col.gameObject.GetComponent<IDebuffable>().AddDebuff<float>(DebuffableProperties.MOVEMENT_SPEED, new SlowWaterDebuff());
            Debug.Log("Debuffffeeeeeeeeeeeeeeeed");
        }
    }
}
