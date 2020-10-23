using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrenadeLauncherScript : MonoBehaviour
{
    private GameManager gManager; //the Game Manager will track the camera's hit from now on.
    public GameObject grenadePrefab;
    public Transform muzzle;

    public RaycastHit camerasHit;
    
    public bool isActive = false;
    private float fireRate;
    private float timeLastFired;
    public int ammo = 100;

    //Audio
    //[SerializeField] private AudioSource aSource;
    //[SerializeField] private AudioClip aClip;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        camerasHit = gManager.playerCameraHit;
        fireRate = gameObject.GetComponentInParent<MechaPartScript>().part.fireRate; //RAIL GUN SCRIPT MUST INHERIT FIRE RATE FROM THE MECHAPART
    }

    private void Update()
    {
        if (gManager.activeWeapon == 3) isActive = true; else isActive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        camerasHit = gManager.playerCameraHit;
        if (camerasHit.point != Vector3.zero) transform.LookAt(camerasHit.point);
        if (camerasHit.point == Vector3.zero) return;

        if (gManager.leftClicked && Time.time > (timeLastFired + fireRate) && ammo > 0 && isActive)
        {
            Debug.DrawLine(muzzle.position, camerasHit.point, Color.red, 1);
            var go = Instantiate(grenadePrefab, muzzle.position, muzzle.rotation);
            timeLastFired = Time.time;
            ammo--;
            //aSource.PlayOneShot(aClip);
        }
    }
}

