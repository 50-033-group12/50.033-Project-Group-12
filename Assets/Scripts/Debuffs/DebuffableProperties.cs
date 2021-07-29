using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of properties that can be debuffed.
/// </summary>
public enum DebuffableProperties
{
    /// <summary>
    /// Affects the movement speed of the unit
    /// </summary>
    MOVEMENT_SPEED,
    /// <summary>
    /// Affects the rotation speed of the nuit
    /// </summary>
    ROTATION_SPEED,
    /// <summary>
    /// Affects the activeness of the unit (if false, the unit is stunned and should not act)
    /// </summary>
    ACTIVE
}
