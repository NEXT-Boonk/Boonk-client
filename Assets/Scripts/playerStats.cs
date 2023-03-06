using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/playerStat", order = 1)]

public class playerStats : ScriptableObject
{
    public float attackSpeed;
    public float Range;
    public int Stagger;
    public int Damage;
    public int Defense;
    public int HP;
    public float Recoil;

}
