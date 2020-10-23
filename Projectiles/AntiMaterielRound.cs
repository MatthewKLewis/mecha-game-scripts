using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System;

/*
 * Every time a new round is instantiated, the calling game object (a player, NPC, or enemy) must also immediately call the FIRE METHOD!
 * Otherwise the rocket will originate at World Zero (0,0,0) and fly along the World Z axis.
 * 
 * Each frame, the round will send out a raycast to check if a collision will occur between its current position and the next frame's projected position
 * If the IF (Physics.Raycast()) detects a collision, the round is moved to the location of the projected collision and despawned (Destroy())
 * A reflection Vector3 is calculated between the round's forward vector and the collision object's surface normal
 * 
 * 
 */

public class AntiMaterielRound : MonoBehaviour
{
    [SerializeField] private float velocity = 100f;
    [SerializeField] private float life = 100f;
    [SerializeField] private int damage = 4;
    private float lifeTimer;

    //explosions created by rockets
    public UnityEngine.Object smallExplosion;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        //the rocket goes forward every frame
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);

        //Destroy the rocket if it strikes other geometry
        if (Physics.Raycast(transform.position, transform.forward, out hit, velocity * Time.deltaTime))
        {
            transform.position = hit.point;
            Vector3 reflected = Vector3.Reflect(transform.forward, hit.normal);
            RocketSFX(hit, reflected);

            if (hit.transform.gameObject.layer == 12) //12 is the destructible layer
            {
                hit.transform.GetComponent<DestructibleScript>().LoseHealth(damage);
            }
            if(hit.transform.gameObject.layer == 13)
            {
                hit.transform.GetComponent<EnemyHealthScript>().LoseHealth(damage);
            }
            Destroy(gameObject);
        }
        
        //Destroy the rocket if it reaches the end of its life
        if (Time.time > lifeTimer + life)
        {
            Destroy(gameObject);
        }
    }

    private void RocketSFX(RaycastHit hit, Vector3 reflected)
    {
        RaycastHit h = hit;
        Vector3 RRR = new Vector3(reflected.x * 90, reflected.y * 90, reflected.z * 90);

        Instantiate(smallExplosion, h.point, (Quaternion.Euler(RRR)));
        //Instantiate(Smoke or other Particle System)

    }

    public void Fire()
    {
        lifeTimer = Time.time;
    }
}
