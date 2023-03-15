using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform rockPrefab;
    [SerializeField] private Transform arrowPrefab;
    [SerializeField] private Transform startPosition;

    [SerializeField] private float rockSpeed;
    [SerializeField] private float arrowSpeedMax, arrowSpeedMin, arrowChargeSpeed;
    private float arrowSpeed;

    private Transform spawnedObjectTransform;
    public static List<Transform> spawnedObjects = new();

    private NetworkManager networkManager;

    private TeamHandler teamHandler;
    public Team team;
    public GameObject playerCamera;

    // This is a struct, a refrence variable, not definable using the method above
    public struct MyCustomData: INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	    {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
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

    private void Start()
    {
        // Don't despawn camera if we are the owner.
        if (IsOwner) return;
 
        playerCamera.SetActive(false);
        arrowSpeed = arrowSpeedMin;
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
                arrowSpeed = arrowSpeed + arrowChargeSpeed * Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.V))
	    {
            ArrowServerRpc(new ServerRpcParams());
            arrowSpeed = arrowSpeedMin;
        }

        // This is connected to the ClientRpc further down
        if(Input.GetKeyDown(KeyCode.O))
	    {
            // Thanks to the parameter, we only run the function on the client with the id of 1
            TestClientRpc(new ClientRpcParams {Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>{1}}});
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

    /*
    A ClientRpc is a function that the server activates that is then run on the clients instead of the server, opposite of a serverRpc.
    The parameter ClientRpcParams can be used to specifi a specific client to run the function on.
    This would f.eks. allow the server to tell a player that they have died and run the death command on it.
    */
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams _clientRpcParams)
    {
        Debug.Log("ClientRPC");
    }
}
