using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Use this class to wrap around a property you want to be affected by debuffs, such as movement speed.
/// </summary>
/// <typeparam name="T">The type of the property you are wrapping around</typeparam>
public class DebuffableProperty<T>
{
    private T _initialValue;
    private List<Debuff<T>> _debuffs;

    public DebuffableProperty(T initialValue)
    {
        _initialValue = initialValue;
        _debuffs = new List<Debuff<T>>();
    }

    /// <summary>
    /// Sets the new initial value. This is recommended to use debuffs instead of changing the initial value.
    /// </summary>
    /// <param name="newValue">The new initial value</param>
    public void SetInitialValue(T newValue)
    {
        _initialValue = newValue;
    }

    /// <summary>
    /// Adds a new debuff to this property. If the debuff is not stackable and it already being applied to this property, it will be ignored.
    /// </summary>
    /// <param name="debuff">the debuff to add</param>
    public void AddDebuff(Debuff<T> debuff)
    {
        if (!debuff.IsStackable())
        {
            if (_debuffs.Exists(d => d.GetName() == debuff.GetName()))
            {
                return;
            }
        }

        _debuffs.Add(debuff);
    }

    /// <summary>
    /// Gets the final value of this property after transmutting through all the debuffs.
    /// Each invocation checks for cancelled debuffs and removes them before they can modify the value.
    /// </summary>
    /// <returns>the final transmuted value</returns>
    public T GetFinalValue()
    {
        // remove cancelled debuffs
        _debuffs.RemoveAll(d => d.IsCancelled());
        var value = _initialValue;
        foreach(var debuff in _debuffs)
        {
            
            (T newValue, bool stop) = debuff.DebuffFunction(value);
            value = newValue;
            if (stop)
            {
                break;
            }
        }
        return value;
    }

    public void RemoveAllDebuffs()
    {
        _debuffs.Clear();
    }
}