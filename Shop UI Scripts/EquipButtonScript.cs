using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipButtonScript : MonoBehaviour
{
    GameManager gManager;
    private ShopUIScript suiScript;
    private DummyPlayerScript dpScript;
    private InventorySlotScript isScript;

    GameObject mechaPart; //the mechaPart represented by its Inventory Slot Parent
    GameObject partToReplace;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        suiScript = transform.root.GetComponent<ShopUIScript>();

        dpScript = GameObject.Find("DummyPlayer").GetComponent<DummyPlayerScript>();
        isScript = gameObject.GetComponentInParent<InventorySlotScript>();
        mechaPart = isScript.mechaPart;
        Debug.Log("The Equip Button can see that it is a button on an inventory slot representing..." + mechaPart.name);
    }

    public void Click()
    {
        //when a player equips a part, it should first remove any part of the same hardpoint from the unactive list.
        foreach (GameObject m in gManager.activePartsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == mechaPart.GetComponent<MechaPartScript>().part.hardPoint)
            {
                partToReplace = m;
            }

        }
        gManager.activePartsList.Remove(partToReplace);
        gManager.partsList.Add(partToReplace);
        gManager.activePartsList.Add(mechaPart);
        gManager.partsList.Remove(mechaPart);
        dpScript.RebuildMech();

        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Head) suiScript.DisplayHeadsInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Core) suiScript.DisplayCoresInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Legs) suiScript.DisplayLegsInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.RArm) suiScript.DisplayRarmsInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.LArm) suiScript.DisplayLarmsInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Weapon1) suiScript.DisplayWeapon1sInInventory();
        if (mechaPart.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Weapon2) suiScript.DisplayWeapon2sInInventory();
    }
}
