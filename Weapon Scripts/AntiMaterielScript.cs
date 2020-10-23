using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AntiMaterielScript : MonoBehaviour
{
    private GameManager gManager; //the Game Manager will track the camera's hit from now on.
    public GameObject antiMaterielPrefab;
    private AntiMaterielRound rocketScript;
    public Transform muzzle;

    public RaycastHit camerasHit;
    
    public bool isActive = true;
    private float fireRate;
    private float timeLastFired;
    public int ammo = 1000;

    //Audio
    private AudioSource aSource;
    private AudioClip aClip;

    void Start()
    {
        gManager = GameManager.instance;
        aSource = GetComponent<AudioSource>();
        aClip = aSource.clip;
        camerasHit = gManager.playerCameraHit;
        fireRate = gameObject.GetComponentInParent<MechaPartScript>().part.fireRate; //RAIL GUN SCRIPT MUST INHERIT FIRE RATE FROM THE MECHAPART
    }

    private void Update()
    {
        if (gManager.activeWeapon == 1) isActive = true; else isActive = false;
    }

    void FixedUpdate()
    {
        camerasHit = gManager.playerCameraHit;
        if (camerasHit.point != Vector3.zero) transform.LookAt(camerasHit.point);
        if (camerasHit.point == Vector3.zero) return;
        if (gManager.leftClicked && Time.time > (timeLastFired + fireRate) && ammo > 0 && isActive)
        {
            Debug.DrawLine(muzzle.position, camerasHit.point, Color.red, 1);
            var go = Instantiate(antiMaterielPrefab, muzzle.position, muzzle.rotation);
            rocketScript = go.GetComponent<AntiMaterielRound>();
            rocketScript.Fire();
            timeLastFired = Time.time;
            ammo--;
            aSource.PlayOneShot(aClip);
        }
    }
}

