using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff<T>
{
    private bool _isCancelled = false;

    /// <summary>
    /// Function to transmute the value of the DebuffableProperty.
    /// </summary>
    /// <param name="input">The initial value to affect</param>
    /// <returns>T - the transmuted value, bool - whether this debuff should stop the evaluation of all other debuffs on the same property.</returns>
    public abstract (T, bool) DebuffFunction(T input);
    
    /// <summary>
    /// Returns the name of this debuff type: e.g. DDOS Stun. Multiple instances of the same type should share the same name.
    /// </summary>
    /// <returns>debuff name</returns>
    public abstract string GetName();

    /// <summary>
    /// Returns whether multiple instances of this same debuff (identified by GetName()) will apply to the same DebuffableProperty.
    /// </summary>
    /// <returns>is stackable</returns>
    public abstract bool IsStackable();

    /// <summary>
    /// Checks if this debuff is cancelled. DebuffableProperties should NOT apply the effects of this debuff when it is cancelled.
    /// </summary>
    /// <returns>is cancelled</returns>
    public bool IsCancelled()
    {
        return _isCancelled;
    }

    /// <summary>
    /// Cancels this debuff. It will be removed from the DebuffableProperty on the next update.
    /// </summary>
    public void Cancel()
    {
        _isCancelled = true;
    }
}