using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class LaunchProjectile : NetworkBehaviour
{
    public Transform launchPoint;
    public GameObject projectilie;
    public float launchVelocity = 10f;

    public float coolDownTime = 2f;

    private float nextFireTime = 0;

    void Update()
    {
        if(!IsOwner) return; //This checks if the code is not run by the player, if so it does nothing.
        if (Time.time > nextFireTime)
        {


            if (Input.GetButtonDown("Fire1"))
            {
                //Debug.Log(Time.time);
                nextFireTime = Time.time + coolDownTime;
                var _projectile = Instantiate(projectilie, launchPoint.position, launchPoint.rotation);
                _projectile.GetComponent<Rigidbody>().velocity = launchPoint.up * launchVelocity;

                // Debug.Log(nextFireTime);
            }
        }
    }









}