using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ROCKETS ARE CALLED IN CLUSTERS AND DO NOT TRACK THE CAMERA'S RAYCAST HITS.
 * THEY FIRE VERTICALLY THEN HEAD TO A STATIC TARGET WITHIN A QUARTER SECOND.
 * 
 */

public class RocketScript : MonoBehaviour
{
    private Rigidbody rBody;
    private Camera playerCamera;
    private CameraScript playerCameraScript;
    private RaycastHit camerasHit;

    [SerializeField] private float missileSpeed = 2.5f;
    [SerializeField] private float life = 12f;
    [SerializeField] private int damage = 6;
    [SerializeField] private float rocketSpacing = 2;
    [SerializeField] private float rocketAngleJitter = 10;

    private float timeSpawned;

    [SerializeField] private float timeToTrack = .25f;
    [SerializeField] private UnityEngine.Object mediumExplosion;
    private float randomXOffset;
    private float randomYOffset;
    private float randomYAngleOffset;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = FindObjectOfType<Camera>(); //hopefully that works
        playerCameraScript = playerCamera.GetComponent<CameraScript>();
        camerasHit = playerCameraScript.hit;

        randomXOffset = UnityEngine.Random.Range(-rocketSpacing, rocketSpacing);
        randomYOffset = UnityEngine.Random.Range(-rocketSpacing, rocketSpacing);
        randomYAngleOffset = UnityEngine.Random.Range(-rocketAngleJitter, rocketAngleJitter);

        rBody = GetComponent<Rigidbody>(); //THE MISSILE RIGIDBODY DOES NOT USE GRAVITY
        transform.Rotate(-90 + randomYAngleOffset, 0, 0);
        rBody.AddForce(Vector3.up * missileSpeed, ForceMode.VelocityChange);
        timeSpawned = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeSpawned + timeToTrack)
        {
            if (camerasHit.point != Vector3.zero) //if there is no info in camerasHit, the camerasHit point is 0,0,0. KLUDGE?
            {
                transform.LookAt(new Vector3(camerasHit.point.x + randomXOffset, camerasHit.point.y, camerasHit.point.z + randomYOffset));
                rBody.AddForce(transform.forward * missileSpeed, ForceMode.VelocityChange);
            }
        }
        if (Time.time > timeSpawned + life) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(mediumExplosion, transform.position, transform.rotation);

        if (collision.collider.gameObject.layer == 12) //12 is the destructible layer
        {
            collision.collider.gameObject.GetComponent<DestructibleScript>().LoseHealth(damage);
        }

        if (collision.collider.gameObject.layer == 13)
        {
            collision.collider.gameObject.GetComponent<EnemyHealthScript>().LoseHealth(damage);
        }
        Destroy(gameObject);
    }
}
