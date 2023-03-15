using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraScript : MonoBehaviour
{

    [SerializeField] private Transform enemy;
    [SerializeField] Transform player;
    [Space]
    [SerializeField] private Camera camera;
    [SerializeField] float cameraSlack;
    [SerializeField] float cameraDistance;
    [SerializeField] float lockOnDistance = 50f;

    [Space]
    [Range(0, 5)] [SerializeField] float maxHeight;
    [Range(-0.01f, -1)] [SerializeField] float minHeight;

    [Space]
    [SerializeField] CinemachineBrain cinemachineBrain;

    private Vector3 pivotPoint;

    // sætter 
    void Start()
    {
        pivotPoint = transform.position;
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    [SerializeField] private bool lockOn;

    void Update()
    {
        // changes between freecamera and lockon camera
        if (Input.GetKeyDown("q"))
        {
            if (lockOn == true)
            {
                slukLockOn();

            }
            else if (lockOn == false)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("lockOnObjectTag");
                GameObject[] enemies = players;
                enemy = secondClosestTransform(player.transform.position, enemies, lockOnDistance);

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
                slukLockOn();
                return;
            }

            float extraHeight = Mathf.Lerp(minHeight, maxHeight, Mathf.InverseLerp(0, lockOnDistance, distanceToTarget));

            pivotPoint = Vector3.MoveTowards(current, target + Vector3.up * extraHeight, Vector3.Distance(current, target) * cameraSlack);
            camera.transform.position = pivotPoint;
            camera.transform.LookAt((enemy.position + player.position) / 2);
            camera.transform.position -= transform.forward * cameraDistance;
        }
    }

    void slukLockOn()
    {
        lockOn = false;
        cinemachineBrain.enabled = true;
        enemy = null;
    }


    // returns the SecondClosest tranfrom from an array of transforms
    Transform secondClosestTransform(Vector3 position, GameObject[] gameObjects, float maxDistance)
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