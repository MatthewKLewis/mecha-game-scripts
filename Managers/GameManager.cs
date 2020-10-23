using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// THE GAME MANAGER SERVES THREE FUNCTIONS:
// 1. Moving from Scene to Scene.
// 2. Tracking the players inventory and money
// 3. Watching for mouse and keyboard inputs (EXCEPT IN THE PLAYER and CAMERA MOVEMENT)

public class GameManager : MonoBehaviour
{
    #region Singleton!

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Extraneous GameManager Deleted");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
   
    }

    #endregion

    #region Instantiations!
    [HideInInspector] public GameObject player; //the player finds itself automatically
    [HideInInspector] public GameObject cameraGameObject;
    [HideInInspector] public Camera playerCam;
    [HideInInspector] public RaycastHit playerCameraHit;

    //INPUTS

    [HideInInspector] public bool forwardPressed;
    [HideInInspector] public bool backwardPressed;
    [HideInInspector] public bool leftPressed;
    [HideInInspector] public bool rightPressed;
    [HideInInspector] public bool spacePressed;
    [HideInInspector] public bool leftClicked;
    [HideInInspector] public bool rightClicked;

    [SerializeField] private GameObject defaultHeadPart;
    [SerializeField] private GameObject defaultCorePart;
    [SerializeField] private GameObject defaultRightArmPart;
    [SerializeField] private GameObject defaultLeftArmPart;
    [SerializeField] private GameObject defaultLegsPart;
    [SerializeField] private GameObject defaultWeapon1Part;
    [SerializeField] private GameObject defaultWeapon2Part;

    //CAREER STATS
    public int dollars;
    public int activeWeapon = 1;
    public bool mechIsInShop;
    public Color playerColor;
    public List<GameObject> partsList = new List<GameObject>();
    public List<GameObject> activePartsList = new List<GameObject>();
    public List<GameObject> purchasablePartsList = new List<GameObject>(); //add items to this as career advances

    #endregion

    private void Start()
    {
        //On Wake(), the Game manager ensures it is a Singleton, on Start, it initializes the "Career" MechaParts list with the defaults. Start happens once, not once per scene.
        activePartsList.Add(defaultHeadPart);
        activePartsList.Add(defaultCorePart);
        activePartsList.Add(defaultLegsPart);
        activePartsList.Add(defaultRightArmPart);
        activePartsList.Add(defaultLeftArmPart);
        if (defaultWeapon1Part != null) activePartsList.Add(defaultWeapon1Part); //optional additional parts
        if (defaultWeapon2Part != null) activePartsList.Add(defaultWeapon2Part); //optional additional parts

        Cursor.visible = true;

        /*
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            player = GameObject.Find("Player"); 
        }
        */

        cameraGameObject = GameObject.Find("Camera");
        playerCam = cameraGameObject.GetComponent<Camera>();
        activeWeapon = 1;
    }

    private void Update()
    {
        //Input listeners
        if (Input.GetKeyDown(KeyCode.W)) forwardPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) forwardPressed = false;
        if (Input.GetKeyDown(KeyCode.S)) backwardPressed = true;
        if (Input.GetKeyUp(KeyCode.S)) backwardPressed = false;
        if (Input.GetKeyDown(KeyCode.A)) leftPressed = true;
        if (Input.GetKeyUp(KeyCode.A)) leftPressed = false;
        if (Input.GetKeyDown(KeyCode.D)) rightPressed = true;
        if (Input.GetKeyUp(KeyCode.D)) rightPressed = false;
        if (Input.GetKeyDown(KeyCode.Space)) spacePressed = true;
        if (Input.GetKeyUp(KeyCode.Space)) spacePressed = false;

        if (Input.GetButton("Fire1")) leftClicked = true; else leftClicked = false;
        if (Input.GetButton("Fire2")) rightClicked = true; else rightClicked = false;

        if (!mechIsInShop) //if mech is not in shop, allow weapon switching
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) activeWeapon = 1; //Right Arm
            if (Input.GetKeyDown(KeyCode.Alpha2)) activeWeapon = 2; //Left Arm
            if (Input.GetKeyDown(KeyCode.Alpha3)) activeWeapon = 3; //Hardpoint 1
            if (Input.GetKeyDown(KeyCode.Alpha4)) activeWeapon = 4; //Hardpoint 2
        }
    }

    private void FixedUpdate()
    {
        // The Game Manager keeps track of the player camera raycast hit to send it down to any object in the scene
        if (SceneManager.GetActiveScene().buildIndex == 1) playerCameraHit = playerCam.GetComponent<CameraScript>().hit; //Only look for player in Scene 1 - Sample Scene
    }

    #region Input Methods

    public void HaltAllInputs()
    {
        forwardPressed = false;
        backwardPressed = false;
        leftPressed = false;
        rightPressed = false;
        spacePressed = false;
        leftClicked = false;
        rightClicked = false;
    }

    #endregion

    #region Level Changing Methods  //double check your spelling!
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
        activeWeapon = 1; //default weapon
        mechIsInShop = false;
        SuppressMousePointer();
    }

    public void LoadMechaShop()
    {
        SceneManager.LoadScene("AdvancedShopScene");
        activeWeapon = 5; //no weapon
        mechIsInShop = true;
        EnableMousePointer();
    }

    public void SuppressMousePointer()
    {
        Cursor.visible = false;
    }

    public void EnableMousePointer()
    {
        Cursor.visible = true;
    }
    #endregion

    #region Object Linkup Methods
    public void FindPlayer(GameObject p)
    {
        player = p;
    }

    public void FindCamera(Camera c)
    {
        playerCam = c;
    }
    #endregion

    #region Mecha Part List Methods
    //Mecha Part List Methods
    public void AddMechaPart(GameObject part)
    {
        partsList.Add(part);
        Debug.Log("The Player collected: " + part.name);
    }

    public void RemoveMechaPart(GameObject part)
    {
        for (int i = 0; i < partsList.Count; i++)
        {
            //if the part at partsList[i] is the same as the part sent to the method, erase the part from the partsList
            if (partsList.ElementAt(i) == part)
            {
                Debug.Log("Found the Part!");
                partsList.RemoveAt(i);
            }
        }
        
        Debug.Log("The Player lost / sold: " + part.name);
    }
    #endregion

    #region Mecha Assembly Methods
    public void MarkForAssembly(GameObject part)
    {
        for (int i = 0; i < partsList.Count; i++)
        {
            //if the part at partsList[i] is the same as the part sent to the method, erase the part from the partsList
            if (partsList.ElementAt(i) == part)
            {
                Debug.Log("Found the Part!");
                activePartsList.Add(part);
                partsList.RemoveAt(i);
            }
        }
        Debug.Log("The Player wants to assemble the mech with: " + part.name);
    }

    public void MarkForStorage(GameObject part)
    {
        for (int i = 0; i < activePartsList.Count; i++)
        {
            //if the part at activePartsList[i] is the same as the part sent to the method, move from the activePartsList
            if (activePartsList.ElementAt(i) == part)
            {
                Debug.Log("Found the Part!");
                partsList.Add(part);
                activePartsList.RemoveAt(i);
            }
        }
        Debug.Log("The Player DOES NOT WANT to assemble the mech with: " + part.name);
    }

    public bool MechaHasNecessaryParts()
    {
        bool hasHead = true;

        if (hasHead)
        {
            return true;
        }

        return false;
    }
    #endregion

    #region Player Finance Methods
    public void AddDollars(int d)
    {
        dollars += d;
    }

    public void RemoveDollars(int d)
    {
        dollars -= d;
    }
    #endregion
}

