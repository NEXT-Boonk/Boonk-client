using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum Team
{ 
    Forrest = 0,
    Snow = 1
}

public class TeamHandler : MonoBehaviour
{
    // Serialized for testing
    [SerializeField] public List<PlayerNetwork> forrestTeam = new List<PlayerNetwork>();
    [SerializeField] public List<PlayerNetwork> snowTeam = new List<PlayerNetwork>(); 

    public void AddPlayer(PlayerNetwork player)
    {
        // Adds a team to the player.
        if (forrestTeam.Count <= snowTeam.Count)
        {
            player.team = Team.Forrest;
            forrestTeam.Add(player);
            // player.SetColor(Color.green);
        }
        else
        {
            player.team = Team.Snow;
            snowTeam.Add(player);
            // player.SetColor(Color.red);
        }
    }

    public void RemovePlayer(PlayerNetwork player)
    {
        // Removes a player from their current team
        if (player.team == Team.Forrest)
        {
            forrestTeam.Remove(player);
        }
        else if (player.team == Team.Snow)
        {
            snowTeam.Remove(player);
        }
        else
        {
            Debug.LogError("Player was not assigned at team.");
        }
    }

    public void ChangeTeam(PlayerNetwork player)
    {
        // Changes the players current team. 
        if (player.team == Team.Forrest)
        {
            player.team = Team.Snow;
            forrestTeam.Remove(player);
            snowTeam.Add(player);
            // player.SetColor(Color.red);
        }
        else if (player.team == Team.Snow)
        {
            player.team = Team.Forrest;
            snowTeam.Remove(player);
            forrestTeam.Add(player);
            // player.SetColor(Color.green);
        }
        else
        {
            Debug.Log("Player was not assigned a team.");
        }
    }
}