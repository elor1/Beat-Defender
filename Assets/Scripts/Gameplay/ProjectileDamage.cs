using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage
{
    /// <summary>
    /// Reduces health of player/enemy by a given amount
    /// </summary>
    /// <param name="health">Heath to be reduced</param>
    /// <param name="damage">Amount to reduce health by</param>
    public static void DecreaseHealth(ref int health, int damage)
    {
        health -= damage;
    }
}
