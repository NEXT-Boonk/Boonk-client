using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/PlayerStat", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int damage;
    public int defense;
    public int health;
}
