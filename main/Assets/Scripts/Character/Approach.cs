using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enumeration class for each of the possible approaches an enemy may take when attacking the player or a building.
/// </summary>
public enum Approach
{
    /// <summary>
    /// <para>Enemy will move towards its target. Once within range, it will start firing. If the target leaves the enemy's
    /// range, the enemy will keep firing as it tries to move closer to the target.</para>
    /// In terms of enemy states, this is "move->attack->both".
    /// </summary>
    DEFAULT,

    /// <summary>
    /// <para>Enemy will move towards its target. Once within range, it will start firing. If the target leaves the enemy's
    /// range, it will stop firing and won't fire again until it is within range of the target. </para>
    /// In terms of enemy states, this is "move->attack->move".
    /// </summary>
    PASSIVE,

    /// <summary>
    /// <para>Enemy will constantly fire at target, regardless of whether or not the target is within its range.</para>
    /// In terms of enemy states, this is "both". 
    /// </summary>
    AGGRO
}
