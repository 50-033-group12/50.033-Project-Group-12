using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishableAmmoSource : AmmoSource
{
    public int count;

    // Start is called before the first frame update
    public ReplenishableAmmoSource(int initialAmount){
        count = initialAmount;
    }

    public int GetCount(){
        return count;
    }

    public void Consume(int amount){
        if(count >= amount){
            count -= amount;
        }
    }

    public void Replenish(int amount){
        count += amount;
    }
}
