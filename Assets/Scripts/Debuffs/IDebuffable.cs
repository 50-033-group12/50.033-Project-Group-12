using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebuffable
{
    void AddDebuff<T>(DebuffableProperties property, Debuff<T> debuff);
}
