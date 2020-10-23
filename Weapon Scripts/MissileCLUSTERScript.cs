using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MissileCLUSTERScript : MonoBehaviour
{
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
        if (gManager.leftClicked && Time.time > (timeLastFired + fireRate) && ammo > 4 && isActive)
        {
            var go1 = Instantiate(smartMissile, muzzle.position, muzzle.rotation);
            var go2 = Instantiate(smartMissile, new Vector3(muzzle.position.x + 1, muzzle.position.y, muzzle.position.z), muzzle.rotation);
            var go3 = Instantiate(smartMissile, new Vector3(muzzle.position.x - 1, muzzle.position.y, muzzle.position.z), muzzle.rotation);
            var go4 = Instantiate(smartMissile, new Vector3(muzzle.position.x, muzzle.position.y, muzzle.position.z + 1), muzzle.rotation);
            timeLastFired = Time.time;
            ammo -= 4;
            aSource.PlayOneShot(aClip);
        }
    }
}

