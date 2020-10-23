using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CanvasScript : MonoBehaviour
{
    //the class's own variables
    public Vector3 playerLooking;

    //objs and components to get variables from
    public PlayerScript playerScript;
    public GameManager gManager;
    public HealthBarScript hbScript;
    public EnergyBarScript ebScript;
    public Text debugText;

    [SerializeField] private UnityEngine.UI.Image weaponIconOne;
    [SerializeField] private UnityEngine.UI.Image weaponIconTwo;
    [SerializeField] private UnityEngine.UI.Image weaponIconThree;
    [SerializeField] private UnityEngine.UI.Image weaponIconFour;

    // Start is called before the first frame update
    void Start()
    {
        gManager = gManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        playerLooking = gManager.cameraGameObject.GetComponent<CameraScript>().cameraFacing;

        //display player info
        debugText.text = ("Health: " + playerScript.health + "\n" + "Energy: " + (int)Math.Max(playerScript.energyReserves, 0) + "\n" + "Vector: " + playerScript.localVelocityVector + 
            "\n" + "World Coordinates: " + playerScript.transform.position + "\n" + "Fuel Pressure: " + playerScript.fuelPressure + "\n" + "CameraFacing" + playerLooking + "\n" + "Active Weapon: "
            + gManager.activeWeapon);
    }
}
