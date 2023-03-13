using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/PlayerStat", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float attackSpeed;
    public float range;
    public int stagger;
    public int damage;
    public int defense;
    public int health;
    public float recoil;
}
