using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * MISSILES ARE CALLED ONE AT A TIME AND FUNCTION AS WIRE-GUIDED PROJECTILES,
 * TRACKING TOWARDS THE CROSSHAIR'S INTERSECTION WITH SCENE GEOMETRY.
 * 
 */

public class SmartMissleScript : MonoBehaviour
{
    private Rigidbody rBody;
    private Camera playerCamera;
    private CameraScript playerCameraScript;
    private RaycastHit camerasHit;
    
    [SerializeField] private float missileSpeed = 2f;
    [SerializeField] private float life = 12f;
    [SerializeField] private int damage = 10;
    private float timeSpawned;

    [SerializeField] private float timeToTrack = 4;
    [SerializeField] private UnityEngine.Object mediumExplosion;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = FindObjectOfType<Camera>(); //hopefully that works
        playerCameraScript = playerCamera.GetComponent<CameraScript>();
        camerasHit = playerCameraScript.hit;

        rBody = GetComponent<Rigidbody>(); //THE MISSILE RIGIDBODY DOES NOT USE GRAVITY
        transform.Rotate(-90,0,0);
        rBody.AddForce(Vector3.up * missileSpeed, ForceMode.VelocityChange);
        timeSpawned = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        camerasHit = playerCameraScript.hit;

        if (Time.time > timeSpawned + timeToTrack)
        {
            if (camerasHit.point != Vector3.zero) //if there is no info in camerasHit, the camerasHit point is 0,0,0. KLUDGE?
            {
                transform.LookAt(camerasHit.point);
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
