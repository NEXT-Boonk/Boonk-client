using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform rockPrefab;
    [SerializeField] private Transform arrowPrefab;
    [SerializeField] private Transform startPosition;

    [Space]
    [SerializeField] private float rockSpeed;
    [SerializeField] private float arrowSpeedMax, arrowSpeedMin, arrowChargeSpeed;
    private float arrowSpeed;

    public static List<Transform> spawnedObjects = new();

    private NetworkManager networkManager;
    public GameObject playerCamera;

    private TeamHandler teamHandler;
    public Team team;

    private void Start()
    {
        // Don't despawn camera if we are the owner.
        if (!IsOwner) return;
        playerCamera.SetActive(false);
        arrowSpeed = arrowSpeedMin;
    }
   
    // This will send the struct defined above when one of it's values changes.
    public override void OnNetworkSpawn()
    {
        networkManager = FindObjectOfType<NetworkManager>();

        if(networkManager != null)
	    {
        	teamHandler = networkManager.GetComponent<TeamHandler>();
        } else {
            Debug.LogError("Missing NetworkManager");
		}

        // Checks if the server is the one trigger "OnNetworkSpawn".
        if (IsServer)
	    {
            teamHandler.AddPlayer(this); // Runs the AddPlayer method form TeamHandler.
        }
    }

    public override void OnNetworkDespawn()
    {
        teamHandler.RemovePlayer(this);
    }
    
    private void Update()
    {
		// This checks if the code is not run by the owner, if so it does nothing.
        if(!IsOwner) return; 

        playerCamera.SetActive(true);

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


    private void ServerSpawnTool(Transform prefab, Transform Position, float Speed)
    {
        Transform spawnedObject = Instantiate(
			prefab, 
			Position.position,
			Quaternion.LookRotation(startPosition.forward)
		);

        spawnedObject.GetComponent<Rigidbody>().velocity = startPosition.forward * Speed;
        spawnedObject.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(
			spawnedObject.GetComponent<Rigidbody>().velocity
		);
        
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
        spawnedObjects.Add(spawnedObject);

        // Despawn objects if too many. Should be refactored to disapear over time.
        if(spawnedObjects.Count > 100)
	    {
            for (int i = 0; i < spawnedObjects.Count; i++)
	        {
                Destroy(spawnedObjects[i].gameObject);
            }

            spawnedObjects.Clear();
        }
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
