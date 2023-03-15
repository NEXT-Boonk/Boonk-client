using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Transform enemy;

    [Space]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraSlack;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float lockOnDistance = 50f;

    [Space]
    [Range(0, 5)] [SerializeField] private float maxHeight;
    [Range(-0.01f, -1)] [SerializeField] private float minHeight;

    [Space]
    [SerializeField] private CinemachineBrain cinemachineBrain;
     private bool lockOn;

    private Vector3 pivotPoint;

    void Start()
    {
        pivotPoint = transform.position;
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        // changes between freecamera and lockon camera
        if (Input.GetKeyDown(KeyCode.Q))
	    {
            if (lockOn == true)
	        {
                DisableLockOn();
            }
            else if (lockOn == false)
	        {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("LockOn");

                enemy = SecondClosestTransform(player.transform.position, enemies, lockOnDistance);

                if (enemy != null)
		        {
                    lockOn = true;
                    cinemachineBrain.enabled = false;
                }
            }
        }

        if (lockOn == true)
        {
            // flytter cameraet så det er bag ved karakteren og kigger på punkt
            Vector3 current = pivotPoint;
            Vector3 target = player.transform.position + Vector3.up;
            float distanceToTarget = Vector3.Distance(player.position, enemy.position);

            if (distanceToTarget > lockOnDistance)
            {
                DisableLockOn();
                return;
            }

            float extraHeight = Mathf.Lerp(minHeight, maxHeight, Mathf.InverseLerp(0, lockOnDistance, distanceToTarget));

            pivotPoint = Vector3.MoveTowards(current, target + Vector3.up * extraHeight, Vector3.Distance(current, target) * cameraSlack);
            playerCamera.transform.position = pivotPoint;
            playerCamera.transform.LookAt((enemy.position + player.position) / 2);
            playerCamera.transform.position -= transform.forward * cameraDistance;
        }
    }

    void DisableLockOn()
    {
        lockOn = false;
        cinemachineBrain.enabled = true;
        enemy = null;
    }


    // Returns the SecondClosest tranfrom from an array of transforms
    Transform SecondClosestTransform(Vector3 position, GameObject[] gameObjects, float maxDistance)
    {
        Transform closestTransform = null;
        Transform secondClosestTransform = null;

        float closestDistance = Mathf.Infinity;
        float secondClosestDistance = Mathf.Infinity;

        foreach (GameObject gm in gameObjects)
	    {
            float distance = Vector3.Distance(position, gm.transform.position);

            if (distance < closestDistance)
	        {
                secondClosestDistance = closestDistance;
                secondClosestTransform = closestTransform;

                closestDistance = distance;
                closestTransform = gm.transform;
            }
	        else if (distance < secondClosestDistance)
	        {
                secondClosestDistance = distance;
                secondClosestTransform = gm.transform;
            }
        }

        if (secondClosestDistance >= maxDistance)
	    {
            secondClosestTransform = null;
        }

        return secondClosestTransform;
    }
}