using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform rockPrefab;
    [SerializeField] private Transform bowPrefab;
    [SerializeField] private Transform startPosition;

    [SerializeField] private float rockSpeed;
    [SerializeField] private float bowSpeedMax,bowSpeedMin,bowChargeSpeed;
    [SerializeField] private float bowSpeed;

    private Transform spawnedObjectTransform;
    public static List<Transform> spawnedObjectsList = new();

    private NetworkManager networkManager;

    private TeamHandler teamHandler;
    public Team team;

	// This is a variable that is sent over the network.
	// From here: https://www.youtube.com/watch?v=3yuBOB3VrCk&t=1487s&ab_channel=CodeMonkey
    private NetworkVariable<int> randomNumber = new(
	    1,
	    NetworkVariableReadPermission.Everyone,
    	NetworkVariableWritePermission.Owner
	);
    public GameObject playerCamera;

    // This is a struct, a refrence variable, not definable using the method above
    public struct MyCustomData: INetworkSerializable {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }


	/*
    This method can be used to define refrence variables, 
    refrence variables are variables like "class", "Object", "array" and "string" 
	among others. To refrence one of these, replace MyCustomData with the name of the refrence type one has already defined above.
	*/
	private NetworkVariable<MyCustomData> customNumber = new NetworkVariable<MyCustomData>(
	new MyCustomData {
	    _int = 51,
	    _bool = true,
	}, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /*
    This kode will send a random number when the value changes, not at all times, given the "OnValueChanged" part of the code
    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + "number: " + randomNumber.Value);
        };
    }
    */

    // This will send the struct defined above when one of it's values changes
    public override void OnNetworkSpawn() {
        networkManager = FindObjectOfType<NetworkManager>();

        if(networkManager != null){
        	teamHandler = networkManager.GetComponent<TeamHandler>();
        } else {
            Debug.LogError("Missing NetworkManager");
		}

        if (IsServer) // Checks if the server is the one trigger "OnNetworkSpawn".
        {
            teamHandler.AddPlayer(this); // Runs the AddPlayer method form TeamHandler.
        }

        
        customNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue._int + " and it's " + newValue._bool);
        };
    }

    private void Start() {
        // Don't despawn camera if we are the owner.
        if (IsOwner) return;
 
        playerCamera.SetActive(false);
        bowSpeed = bowSpeedMin;
    }
   

    public override void OnNetworkDespawn() {
        teamHandler.RemovePlayer(this);
    }

    
    private void Update() {
		// This checks if the code is not run by the owner, if so it does nothing.
        if(!IsOwner) return; 

        // Debug.Log(OwnerClientId + "number: " + randomNumber.Value); //this code sends the command of the random number, which is sent at all times
        if(Input.GetKeyDown(KeyCode.C)) {
            StoneServerRpc(new ServerRpcParams());
        }

        if(Input.GetKey(KeyCode.V)) {
            if(bowSpeed<bowSpeedMax)
                bowSpeed = bowSpeed + bowChargeSpeed* Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.V)){
            BowServerRpc(new ServerRpcParams());
            bowSpeed = bowSpeedMin;
        }

        playerCamera.SetActive(true);

        if(Input.GetKeyDown(KeyCode.T)) {
            randomNumber.Value = Random.Range(0,100); //changes the random number
        }

        if(Input.GetKeyDown(KeyCode.Y)) {
            if(customNumber.Value._int == 51){
				customNumber.Value = new MyCustomData{
					_int = 10,
					_bool = false,
				}; //sets a new struct
            } else {
				customNumber.Value = new MyCustomData{
					_int = 51,
					_bool = true,
				};
            }
        }

        // This code is connected to the code under the line "[ServerRpc]" further down
        if(Input.GetKeyDown(KeyCode.U)){
            TestServerRpc(new ServerRpcParams());
        }

        // This is connected to the ClientRpc further down
        if(Input.GetKeyDown(KeyCode.O)){
            // Thanks to the parameter, we only run the function on the client with the id of 1
            TestClientRpc(new ClientRpcParams {Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>{1}}});
        }

        // The code below creates a simple movement system
        /*
	    Vector3 moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed *Time.deltaTime;
	    */
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
        spawnedObjectsList.Add(spawnedObject);

        // Despawn objects if too many. Should be refactored to disapear over time.
        if(spawnedObjectsList.Count > 100){
            for (int i = 0; i < spawnedObjectsList.Count; i++) {
                DestroyImmediate(spawnedObjectsList[i].gameObject);
            }

            spawnedObjectsList.Clear();
        }
    }

    /*
	This is how to create a funktion that is run on the server, a serverRPC
    NOTE: that it won't be run on the local client, but instead be run on the server
    If you wish to add parameters you will need to have them as value types, not refrence types

    You can track which client sent the code to the server, by putting a parameter of "serverRpcParams parameter name", and calling 
    Receive.SenderClientId, this gives you the id of the player sending the funktion, which could be used to identify where the effect should occur
    Note that one has to put "[ServerRpc]" right above the code
    */
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams rpc){
        Debug.Log("Server rpc working: " + rpc.Receive.SenderClientId);
    }

    [ServerRpc]
    private void StoneServerRpc(ServerRpcParams _rpc){
        ServerSpawnTool(rockPrefab, startPosition, rockSpeed);
    }

    [ServerRpc]
    private void BowServerRpc(ServerRpcParams _rpc){
        ServerSpawnTool(bowPrefab, startPosition, bowSpeed);
    }

    /*
    A ClientRpc is a function that the server activates that is then run on the clients instead of the server, opposite of a serverRpc.
    The parameter ClientRpcParams can be used to specifi a specific client to run the function on.
    This would f.eks. allow the server to tell a player that they have died and run the death command on it.
    */
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams _clientRpcParams) {
        Debug.Log("ClientRPC");
    }
}
