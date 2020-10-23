using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    #region INSTANTIATIONS REGION

    //Plug and Play Unity Components
    private Rigidbody rigidbodyComponent;
    public Camera playerCamera;
    private GameManager gManager;
    //public Animator legAnimator;

    public Transform weapon1HardPoint;

    //Components drawn from the GameManager instance
    private GameObject head;
    private GameObject core;
    private GameObject rArm; //Right Arms are always guns, laser / machine / anti-materiel
    private GameObject lArm; //Left Arms are always melee weapons or shields
    private GameObject legs;

    private GameObject hipLocator;
    private GameObject neckLocator;
    private GameObject rArmLocator;
    private GameObject lArmLocator;
    private GameObject weapon2Locator;

    //private GameObject headGameObject;

    private GameObject weapon1; //floating outside of the rArm, for now, eventually the rArm and weapon1 slot will be one in the same
    private GameObject weapon2; //shoulder mounted weapons

    //Facing  and moving vectors
    [HideInInspector] public Vector3 playerFacing = Vector3.zero;
    [HideInInspector] public Vector3 globalVelocityVector = Vector3.zero;
    [HideInInspector] public Vector3 localVelocityVector = Vector3.zero;

    //Movement stats to tweak
    [SerializeField] private float lookSpeed = 4;
    [SerializeField] private float moveForce = 100;
    [SerializeField] private int jumpForce = 600;
    [SerializeField] private int energyRechargeRate = 1;
    [SerializeField] private int energyConsumptionRate = 10;
    public float energyReserves;
    public float energyReserveMaximum = 1000;
    public float fuelPressure;

    //Health
    public int health = 100;
    public int maxHealth = 100;

    #endregion

    private void Awake()
    {
        gManager = GameManager.instance;
        gManager.FindPlayer(this.gameObject);
        Physics.IgnoreLayerCollision(8,8); //player colliders can self-intersect. If this doesnt work, turn off all part colliders and find another way to make a total-collider
        Physics.IgnoreLayerCollision(8,11); //playerbullets dont hit player
        Physics.IgnoreLayerCollision(11,11); //playerbullets dont hit other bullets
    }

    //On Start(), the Player model is assembled from GameObjects marked  in the GameManager's partsList.
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();

        MechaPartScript changingScript;

        foreach (GameObject m in gManager.activePartsList)
        {
            changingScript = m.GetComponent<MechaPartScript>();

            if (changingScript.part.hardPoint == HardPoint.Head) {head = m;}
            if (changingScript.part.hardPoint == HardPoint.Core) {core = m;}
            if (changingScript.part.hardPoint == HardPoint.RArm) { rArm = m; } //not yet initialized
            if (changingScript.part.hardPoint == HardPoint.LArm) { lArm = m; } //not yet initialized
            if (changingScript.part.hardPoint == HardPoint.Legs) {legs = m;}


            if (changingScript.part.hardPoint == HardPoint.Weapon1) {weapon1 = m;}
            if (changingScript.part.hardPoint == HardPoint.Weapon2) {weapon2 = m;}
        }

        //legsHardPoint.localPosition = new Vector3(0, 0, 0); //legs originate at 0,0,0
        Instantiate(legs, this.transform); //instantiate as a variable object to set the Animator component in next line.
        //legAnimator = gO.GetComponentInChildren<Animator>(); //Better

        hipLocator = GameObject.Find("hip_Handle"); //core originates at the hipLocator on the legs
        Debug.Log("hip is located at " + hipLocator.transform.position);
        Instantiate(core, hipLocator.transform);

        neckLocator = GameObject.Find("neckLocator"); //head originates at the neckLocator on the Core
        Debug.Log("neck is located at " + neckLocator.transform.position);
        Instantiate(head, neckLocator.transform);

        //CAMERA
        Debug.Log("camera is located at " + neckLocator.transform.position);
        //CAMERA

        rArmLocator = GameObject.Find("rArmLocator");
        Instantiate(rArm, rArmLocator.transform);

        lArmLocator = GameObject.Find("lArmLocator");
        Instantiate(lArm, lArmLocator.transform);

        weapon1HardPoint.localPosition = new Vector3(1, 1, 0); //mechs dont necessarily need a weapon 1 or 2
        if (weapon1 != null) Instantiate(weapon1, weapon1HardPoint);

        weapon2Locator = GameObject.Find("weapon2Locator");
        if (weapon2 != null) Instantiate(weapon2, weapon2Locator.transform);

        //Game Stats
        health = maxHealth;
        energyReserves = energyReserveMaximum;
    }

    // Update is called once per frame.
    void Update()
    {
        //Changes the direction the Player is looking from mouse input AND TILT which is calculated based on velocity
        //get mouse input
        Vector3 mouseChange = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));
        playerFacing += mouseChange * lookSpeed;

        //rotate Player model based on the addition of velocity tilt and mouse input
        this.transform.localRotation = Quaternion.Euler(0, playerFacing.y, playerFacing.z);

        //Zoom-in function
        if (gManager.rightClicked) playerCamera.focalLength = 40; else playerCamera.fieldOfView = 75;

        //localizing magnitude of movement velocity.
        globalVelocityVector = new Vector3(rigidbodyComponent.velocity.x, rigidbodyComponent.velocity.y, rigidbodyComponent.velocity.z);
        localVelocityVector = transform.InverseTransformDirection(globalVelocityVector);

        //recharge energy to a cap of 100
        energyReserves += energyRechargeRate;
        energyReserves = Math.Min(energyReserves, energyReserveMaximum);

        //jump force falloff
        fuelPressure = energyReserves / energyReserveMaximum;
        health = Math.Min(health, maxHealth);

        /*Animations:
        if (localVelocityVector.x > 3) //moving right
        {
            legAnimator.SetInteger("condition", 1);

        }
        if (localVelocityVector.x <= 3 && localVelocityVector.x >= -3) //mostly still
        {
            legAnimator.SetInteger("condition", 0);
        }
        if (localVelocityVector.x < -3) //moving left
        {
            legAnimator.SetInteger("condition", 2);
        }
        */
    }

    //Update is called at 100hz.
    private void FixedUpdate()
    {
        //Rigid Body Force Based Movement
        if (gManager.forwardPressed) { rigidbodyComponent.AddForce(transform.forward * moveForce, ForceMode.Force); }
        if (gManager.backwardPressed) { rigidbodyComponent.AddForce(-transform.forward * moveForce, ForceMode.Force); }
        if (gManager.rightPressed) { rigidbodyComponent.AddForce(transform.right * moveForce, ForceMode.Force); }
        if (gManager.leftPressed) { rigidbodyComponent.AddForce(-transform.right * moveForce, ForceMode.Force); }

        //Jump Force
        if (gManager.spacePressed && energyReserves > 10)
        {
            energyReserves -= energyConsumptionRate;
            rigidbodyComponent.AddForce(transform.up * jumpForce * fuelPressure, ForceMode.Acceleration);
        }
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
    }

    //Manager method for collisions with all LAYERS of objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) //Health and Ammo, 100 of everything for now.
        {
            health += 100;
            gManager.dollars += 100;
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 15) //Portals
        {
            gManager.HaltAllInputs();
            gManager.LoadMechaShop();
        }

        if (other.gameObject.layer == 16) //Collectible Mecha Parts
        {
            
            gManager.AddMechaPart(other.GetComponent<CollectiblePartScript>().prize); //the Game Object drag dropped into the CollectibleMP
            Destroy(other.gameObject);
        }
    }

    //Make sure the player is visible in the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}