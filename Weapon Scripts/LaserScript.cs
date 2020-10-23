using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LaserScript : MonoBehaviour
{
    private GameManager gManager;   //the Game Manager tracks the camera's hit
    //Laser Weapons don't instantiate a projectile
    public LineRenderer lRenderer;  //Instead, they render a laser line
    public Transform laserMuzzle;
    public Gradient laserGradient;
    public Gradient clearGradient;

    public RaycastHit camerasHit;
    
    public bool isActive = false;
    private float fireRate;
    private float timeLastFired;
    [SerializeField] private float laserRenderTime = .02f;
    [SerializeField] public int ammo = 10000;
    [SerializeField] public int damage = 1;          //Laser weapons track their own damage since they do not instantiate projectiles

    //Audio
    //[SerializeField] private AudioSource aSource;
    //[SerializeField] private AudioClip aClip;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        camerasHit = gManager.playerCameraHit;
        fireRate = gameObject.GetComponentInParent<MechaPartScript>().part.fireRate; //WEAPON SCRIPTS MUST INHERIT FIRE RATE FROM THE MECHAPART
    }

    private void Update()
    {
        if (gManager.activeWeapon == 3) isActive = true; else isActive = false;
    }

    void FixedUpdate()
    {
        camerasHit = gManager.playerCameraHit;
        if (camerasHit.point != Vector3.zero) transform.LookAt(camerasHit.point);
        if (camerasHit.point == Vector3.zero) return; //Don't look at World Origin

        //Shoot, but not a projectile
        if (gManager.leftClicked && Time.time > (timeLastFired + fireRate) && ammo > 0 && isActive)
        {
            Debug.DrawLine(laserMuzzle.position, camerasHit.point, Color.red, 1);
            lRenderer.colorGradient = laserGradient;
            lRenderer.SetPosition(0, laserMuzzle.position);
            lRenderer.SetPosition(1, camerasHit.point);
            var _damageScript = camerasHit.collider.gameObject.GetComponent<EnemyHealthScript>();
            if (camerasHit.collider.gameObject.layer == 13) _damageScript.LoseHealth(1);

            timeLastFired = Time.time;
            ammo--;
            //aSource.PlayOneShot(aClip);
        }

        if (Time.time > timeLastFired + laserRenderTime) lRenderer.colorGradient = clearGradient;
    }
}

