using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MissilePodScript : MonoBehaviour
{
    /*
     * - All weapons must be set to inactive when the Mech is in the shop!
     * -
     */

    private GameManager gManager; //the Game Manager will track the camera's hit from now on.
    public GameObject smartMissile;
    public Transform muzzle;
    
    public bool isActive = false; //ONLY WEAPON1S HAVE isActive MARKED TO TRUE. OTHERWISE ALL WEAPONS WILL FIRE.
    private float fireRate;
    private float timeLastFired;
    public int ammo = 100;

    //Audio
    private AudioSource aSource;
    private AudioClip aClip;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        aSource = GetComponent<AudioSource>();
        aClip = aSource.clip;
        fireRate = gameObject.GetComponentInParent<MechaPartScript>().part.fireRate; //RAIL GUN SCRIPT MUST INHERIT FIRE RATE FROM THE MECHAPART
    }

    private void Update()
    {
        if (gManager.activeWeapon == 2) isActive = true; else isActive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gManager.leftClicked && Time.time > (timeLastFired + fireRate) && ammo > 0 && isActive)
        {
            var go = Instantiate(smartMissile, muzzle.position, muzzle.rotation);
            timeLastFired = Time.time;
            ammo--;
            aSource.PlayOneShot(aClip);
        }
    }
}

