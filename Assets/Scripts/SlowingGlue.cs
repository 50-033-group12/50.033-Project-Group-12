using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SlowGlueDebuff : Debuff<float>
{
    public override (float, bool) DebuffFunction(float input)
    {
        return (input * 0.3f, false);
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
            col.gameObject.GetComponent<IDebuffable>().AddDebuff<float>(DebuffableProperties.MOVEMENT_SPEED, new SlowGlueDebuff());
            Debug.Log("Debuffffeeeeed");
        }
    }
}
