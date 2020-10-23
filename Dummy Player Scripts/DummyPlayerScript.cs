using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class DummyPlayerScript : MonoBehaviour
{
    #region INSTANTIATIONS REGION

    //Plug and Play Unity Components
    private GameManager gManager;

    //Components drawn from the GameManager instance
    private GameObject head;
    private GameObject core;
    private GameObject rArm; //Right Arms are always guns, laser / machine / anti-materiel
    private GameObject lArm; //Left Arms are always melee weapons or shields
    private GameObject legs;
    private bool mechIsAssembled;
    private bool ticketToAssemble;
    private bool ticketToDisassemble;

    private GameObject hipLocator;
    private GameObject neckLocator;
    private GameObject rArmLocator;
    private GameObject lArmLocator;
    private GameObject weapon2Locator;
    public Transform weapon1HardPoint; // RECTIFY THIS DIFFERENCE!

    private GameObject weapon1; //floating outside of the rArm, for now, eventually the rArm and weapon1 slot will be one in the same
    private GameObject weapon2; //shoulder mounted weapons

    //Facing  and moving vectors
    [HideInInspector] public Vector3 gimbalFacing;
    [HideInInspector] public Vector3 globalVelocityVector = Vector3.zero;
    [HideInInspector] public Vector3 localVelocityVector = Vector3.zero;

    //Movement stats to tweak
    [SerializeField] private float lookSpeed = 4;

    //Rendering Bug Fixers
    short waitOneFrame = 0;

    #endregion

    private void Awake()
    {
        gManager = GameManager.instance;
        gManager.FindPlayer(gameObject);
        Physics.IgnoreLayerCollision(8,8); //player colliders can self-intersect. If this doesnt work, turn off all part colliders and find another way to make a total-collider
        Physics.IgnoreLayerCollision(8,11); //playerbullets dont hit player
        Physics.IgnoreLayerCollision(11,11); //playerbullets dont hit other bullets
    }

    //On Start(), the Player model is assembled from GameObjects marked  in the GameManager's partsList.
    void Start()
    {
        BuildMech();
    }

    // Update is called once per frame.
    void Update()
    {
        //Changes the direction the Player is looking from mouse input AND TILT which is calculated based on velocity
        if (gManager.leftClicked)
        {
            Vector3 mouseChange = new Vector3(0, Input.GetAxisRaw("Mouse X"));
            gimbalFacing -= mouseChange * lookSpeed;
            transform.rotation = Quaternion.Euler(gimbalFacing);
        }

        if (mechIsAssembled && ticketToDisassemble)
        {
            RemoveInstantiatedParts(); //remove mech
            mechIsAssembled = false; //mech is gone now
            ticketToDisassemble = false; //take the disassembly ticket
            ticketToAssemble = true; //issue assembly ticket, which can't run this frame.
            waitOneFrame = 0;
        }

        if (!mechIsAssembled && ticketToAssemble && waitOneFrame > 0)
        {
            BuildMech(); //build mech
            mechIsAssembled = true; //mech is here now
            ticketToAssemble = false; //take the assembly ticket
        }

        waitOneFrame++;
    }

    public void BuildMech()
    {
        MechaPartScript changingScript;

        foreach (GameObject m in gManager.activePartsList)
        {
            changingScript = m.GetComponent<MechaPartScript>();

            if (changingScript.part.hardPoint == HardPoint.Head) { head = m; }
            if (changingScript.part.hardPoint == HardPoint.Core) { core = m; }
            if (changingScript.part.hardPoint == HardPoint.RArm) { rArm = m; }
            if (changingScript.part.hardPoint == HardPoint.LArm) { lArm = m; }
            if (changingScript.part.hardPoint == HardPoint.Legs) { legs = m; }
            if (changingScript.part.hardPoint == HardPoint.Weapon1) { weapon1 = m; }
            if (changingScript.part.hardPoint == HardPoint.Weapon2) { weapon2 = m; }
        }

        legs = Instantiate(legs, this.transform);

        hipLocator = GameObject.Find("hip_Handle"); //core originates at the hipLocator on the legs
        core = Instantiate(core, hipLocator.transform);

        neckLocator = GameObject.Find("neckLocator"); //head originates at the neckLocator on the Core
        head = Instantiate(head, neckLocator.transform);

        rArmLocator = GameObject.Find("rArmLocator");
        rArm = Instantiate(rArm, rArmLocator.transform);

        lArmLocator = GameObject.Find("lArmLocator");
        lArm = Instantiate(lArm, lArmLocator.transform);

        weapon1HardPoint.localPosition = new Vector3(1, 1, 0);
        if (weapon1 != null) Instantiate(weapon1, weapon1HardPoint);  //mechs dont necessarily need a weapon 1 or 2

        weapon2Locator = GameObject.Find("weapon2Locator");
        if (weapon2 != null) Instantiate(weapon2, weapon2Locator.transform);  //mechs dont necessarily need a weapon 1 or 2

        mechIsAssembled = true;
    }
    public void RemoveInstantiatedParts()
    {
        Destroy(head);
        Destroy(core);
        Destroy(rArm);
        Destroy(lArm);
        Destroy(legs);

        mechIsAssembled = false;
    }
    public void RebuildMech()
    {
        if (mechIsAssembled) ticketToDisassemble = true;
    }

    //Make sure the player is visible in the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}