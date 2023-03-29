using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private int snowTeamTicket;
    [SerializeField] private int forestTeamTicket;
    [Space]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform startPosition;

    [Space]
    [SerializeField] private float rockSpeed;
    [SerializeField] private float arrowSpeedMax, arrowSpeedMin, arrowChargeSpeed;
    private float arrowSpeed;
    public static List<GameObject> spawnedObjects = new();

    private NetworkManager networkManager;
    public Camera playerCamera;

    private TeamHandler teamHandler;
    public Team team;

    private Vector3 forestSpawn = new Vector3(-301f, 10.0f, 297.88f);
    private Vector3 winterSpawn = new Vector3(285.36f, 10.0f, 297.88f);

    


    private void Start()
    {
        //needs to be removed
        if(IsServer || IsHost){
            snowTeamTicket = 3;
            forestTeamTicket = 3;
        }
        arrowSpeed = arrowSpeedMin;
        // Don't despawn camera if we are the owner.
        if (!IsOwner) return;
        playerCamera.gameObject.SetActive(false);
    }
   
    // This will send the struct defined above when one of it's values changes.
    public override void OnNetworkSpawn()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        if(networkManager != null)
	    {
        	teamHandler = networkManager.GetComponent<TeamHandler>();
        } 
	    else
	    {
            Debug.LogError("Missing NetworkManager");
		}

        // Checks if the server is the one to trigger "OnNetworkSpawn".
        if (IsServer) {
            teamHandler.AddPlayer(this);
        }

        if(!IsOwner){
            return;
        }
        playerSpawn();

        Debug.Log(snowTeamTicket+" first");
        
    }

    public override void OnNetworkDespawn()
    {
        teamHandler.RemovePlayer(this);
    }
    
    private void Update()
    {
        Debug.Log(snowTeamTicket);
		// This checks if the code is NOT run by the owner, if so it does nothing.
        if(!IsOwner) return; 
        playerCamera.gameObject.SetActive(true);

        // v  replace with death function
        if(Input.GetKeyDown(KeyCode.Y))
	    {
            PlayerDeathServerRpc(new ServerRpcParams());
        }
      
        // Debug.Log(OwnerClientId + "number: " + randomNumber.Value); //this code sends the command of the random number, which is sent at all times
        if(Input.GetKeyDown(KeyCode.C))
	    {
            StoneServerRpc(new ServerRpcParams());
        }

        if(Input.GetKey(KeyCode.V))
	    {
            if(arrowSpeed < arrowSpeedMax)
                arrowSpeed += arrowChargeSpeed * Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.V))
	    {
            ArrowServerRpc(new ServerRpcParams());
            arrowSpeed = arrowSpeedMin;
        }
    }


    private void ServerSpawnTool(GameObject prefab, Transform Position, float Speed)
    {
        GameObject newObject = Instantiate(
			prefab, 
			Position.position,
			Quaternion.LookRotation(startPosition.forward)
		);

        newObject.GetComponent<Rigidbody>().velocity = startPosition.forward * Speed;
        newObject.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(
			newObject.GetComponent<Rigidbody>().velocity
		);
        
        newObject.GetComponent<NetworkObject>().Spawn(true);
        spawnedObjects.Add(newObject);
    }

    private void KillFunction(bool killAll){
        if(killAll){
        TeamHandler teHa = NetworkManager.GetComponent<TeamHandler>();
        for(int i = 0; i < teHa.forrestTeam.Count; i++){
            GameObject t = teHa.forrestTeam[i].gameObject;
            t.GetComponent<PlayerHealth>().currentHealth = 0;
        }

        for(int i = 0; i < teHa.snowTeam.Count; i++){
            GameObject t = teHa.snowTeam[i].gameObject;
            t.GetComponent<PlayerHealth>().currentHealth = 0;
        }
        }else{
            this.GetComponent<PlayerHealth>().currentHealth = 0;
        }
        
    }

    private void Deathbox(){
        if(this.GetComponent<Transform>().position.y < -10){
            KillFunction(false);
        } 
    } 

    private void PlayerTicketRemove(Team teamRemovedTicket)
    {
        if(snowTeamTicket <= 0){
            snowTeamTicket = 48;
            forestTeamTicket = 48;
            KillFunction(true);
            Debug.Log("Forest team wins");
        }else if(forestTeamTicket <= 0){
            snowTeamTicket = 48;
            forestTeamTicket = 48;
            KillFunction(true);
            Debug.Log("Snow team wins");
        }
        else if(teamRemovedTicket == Team.Forrest)
        {
            forestTeamTicket--;
            Debug.Log(teamRemovedTicket+" "+forestTeamTicket);
        }
        else
        {
            snowTeamTicket--;
            Debug.Log(teamRemovedTicket+" "+snowTeamTicket);
        }
    }

    public void playerSpawn(){

        TeamHandler teHa = NetworkManager.GetComponent<TeamHandler>();
        CharacterController con = this.GetComponent<CharacterController>();
        con.enabled = false;
        if (teHa.forrestTeam.Contains(this))
        {
            transform.position = forestSpawn;
            PlayerDeathServerRpc(new ServerRpcParams());

        } else
        {
            transform.position = winterSpawn;
            PlayerDeathServerRpc(new ServerRpcParams());

        }
        con.enabled = true;

    }



    [ServerRpc(RequireOwnership = false)]
    private void PlayerDeathServerRpc(ServerRpcParams _rpc)
    {
        PlayerTicketRemove(team);
    }
    [ServerRpc]
    private void StoneServerRpc(ServerRpcParams _rpc)
    {
        ServerSpawnTool(rockPrefab, startPosition, rockSpeed);
    }

    [ServerRpc]
    private void ArrowServerRpc(ServerRpcParams _rpc)
    {
        ServerSpawnTool(arrowPrefab, startPosition, arrowSpeed);
    }

    
}
