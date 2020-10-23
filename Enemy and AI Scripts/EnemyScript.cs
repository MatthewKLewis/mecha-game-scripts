using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private RaycastHit hit;
    private GameObject player;
    public Transform laserMuzzle;
    private AudioSource aSource;
    private AudioClip aClip;
    private LineRenderer lRenderer;
    [SerializeField] private int health;
    [SerializeField] private int laserDistance = 10;
    [SerializeField] private int laserDamage = 3;
    [SerializeField] private float lastFiredTime = 10;
    [SerializeField] private float FireRate = 2;
    [SerializeField] private float laserRenderTime = .1f;
    [SerializeField] private Gradient laserGradient;
    [SerializeField] private Gradient clearGradient;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        lRenderer = GetComponent<LineRenderer>();
        aSource = GetComponent<AudioSource>();
        aClip = aSource.clip;
        player = GameManager.instance.player;
        health = 10;
    }

    void FixedUpdate()
    {
        navAgent.SetDestination(player.transform.position);

        if (Physics.Raycast(laserMuzzle.position, laserMuzzle.forward, out hit, laserDistance))
        {
            if (Time.time > lastFiredTime + FireRate)
            {
                Debug.DrawRay(laserMuzzle.position, laserMuzzle.forward * hit.distance, Color.cyan, 0.1f);

                if (hit.collider.gameObject.layer == 8)
                {
                    lRenderer.colorGradient = laserGradient;
                    lRenderer.SetPosition(0, laserMuzzle.position);
                    lRenderer.SetPosition(1, hit.point);
                    aSource.PlayOneShot(aClip);
                    var _script = player.GetComponent<PlayerScript>();
                    _script.LoseHealth(laserDamage);
                }
                lastFiredTime = Time.time;
            }
        }

        if (Time.time > lastFiredTime + laserRenderTime) lRenderer.colorGradient = clearGradient;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        Debug.Log("Enemy current health is " + health);
    }
}
