using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Buffs")]
    [Range(1f,10f)]public float maxHealthScaler = 1; // Done
    public bool monsterKillRegeneration; // Done
    public bool staticRegeneration; // Done

    [Header("Dash Buffs")] 
    public bool iceDash;
    public bool secondWind; // Done
    public bool cantFall; // Done

    [Header("Attack Buffs")] 
    public bool burnDamage; // Done
    [Range(1f,3f)]public float attackRange = 1; // Done
    public bool swordRangedAttack; // Done
}
