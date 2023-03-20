using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
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

    private void Start()
    {
        // Don't despawn camera if we are the owner.
        if (!IsOwner) return;
        playerCamera.gameObject.SetActive(false);
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

        // Checks if the server is the one to trigger "OnNetworkSpawn".
        if (IsServer) {
            teamHandler.AddPlayer(this);
        }
    }

    public override void OnNetworkDespawn()
    {
        teamHandler.RemovePlayer(this);
    }
    
    private void Update()
    {
		// This checks if the code is NOT run by the owner, if so it does nothing.
        if(!IsOwner) return; 
        playerCamera.gameObject.SetActive(true);

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

        // Despawn objects if too many. Should be refactored to disapear over time.
        if(spawnedObjects.Count > 100)
	    {
            for (int i = 0; i < spawnedObjects.Count; i++)
	        {
                Destroy(spawnedObjects[i]);
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
