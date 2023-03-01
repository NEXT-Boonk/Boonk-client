using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/PlayerStat", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float attackSpeed;
    public int Range;
    public int Stagger;
    public int Damage;
    public int Defense;
    public int HP;

}
