using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEquipButtonScript : MonoBehaviour
{
    GameManager gManager;
    private ShopUIScript suiScript;
    private DummyPlayerScript dpScript;
    private ActiveInventorySlotScript isScript;

    GameObject mechaPart; //the mechaPart represented by its Inventory Slot Parent
    GameObject partToReplace;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        suiScript = GameObject.Find("Canvas").GetComponent<ShopUIScript>();
        dpScript = GameObject.Find("DummyPlayer").GetComponent<DummyPlayerScript>();
        isScript = gameObject.GetComponentInParent<ActiveInventorySlotScript>();
        mechaPart = isScript.mechaPart;
        Debug.Log("The Unequip Button can see that it is a button on an activeinventory slot representing..." + mechaPart.name);
    }

    public void Click()
    {
        gManager.activePartsList.Remove(mechaPart);
        gManager.partsList.Add(mechaPart);
        dpScript.RebuildMech();
    }
}
