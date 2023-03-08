using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamHandler : MonoBehaviour
{
    public static TeamHandler instance;

    [SerializeField] private List<PlayerNetwork> teamA = new List<PlayerNetwork>(); //Serialized for testing
    [SerializeField] private List<PlayerNetwork> teamB = new List<PlayerNetwork>(); //Serialized for testing

    /*
    makes the team handler a singleton, making sure it isnt destroyed on scenetransitions
    */
    private void Awake()
    {
        
    }
    //[Server]
    /*
    adds players to 1 of 2 teams depending on how many players are in each team, 
    always adding a new player to the team with the lowest amount of player
    */
    public void AddPlayer(PlayerNetwork player)
    {
        //Set team
        if (teamA.Count <= teamB.Count)
        {
            player.SetTeam(0);
            //player.SetColor(Color.green);
            teamA.Add(player);
        }
        else
        {
            player.SetTeam(1);
            //player.SetColor(Color.red);
            teamB.Add(player);
        }
    }

    //[Server]
    
    //Removes a player from their current team
    
    public void RemovePlayer(PlayerNetwork player)
    {
        if (player.GetTeam() == 0)
        {
            teamA.Remove(player);
        }
        else if (player.GetTeam() == 1)
        {
            teamB.Remove(player);
        }
        else
        {
            Debug.LogError("Player was removed with a team other than 0 or 1");
        }
    }
    //Changes the team of the player to the oppisite of their current team 
    public void ChangeTeam(PlayerNetwork player)
    {
        if (player.GetTeam() == 0)
        {
            player.SetTeam(1);
            //player.SetColor(Color.red);
            teamA.Remove(player);
            teamB.Add(player);
        }
        else if (player.GetTeam() == 1)
        {
            player.SetTeam(0);
            //player.SetColor(Color.green);
            teamB.Remove(player);
            teamA.Add(player);
        }
        else
        {
            Debug.Log("Player with team other than 0 or 1 tried to change team");
        }
    }
}