using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIScript : MonoBehaviour
{
    GameManager gManager;
    public Transform slotParent;
    public GameObject inventorySlot;

    void Start()
    {
        gManager = GameManager.instance;
    }

    private void Update()
    {
    }

    #region Display Player Inventory Methods
    public void RemoveAllParts()
    {
        Debug.Log("Clearing Inventory List...");
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Destroy(slotParent.GetChild(i).gameObject);
        }

    }
    public void DisplayAllInventory()
    {
        RemoveAllParts(); //clear the inventory first

        Debug.Log("Displaying Player Inventory:");

        foreach (GameObject m in gManager.partsList)
        {
            var go = Instantiate(inventorySlot, slotParent);
            go.GetComponent<InventorySlotScript>().mechaPart = m;
        }
    }
    public void DisplayHeadsInInventory()
    {
        RemoveAllParts(); //clear the inventory first

        //adds parts from inventory
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Head)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayCoresInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Core)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayRarmsInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.RArm)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayLarmsInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.LArm)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayLegsInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Legs)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayWeapon1sInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Weapon1)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    public void DisplayWeapon2sInInventory()
    {
        RemoveAllParts(); //clear the inventory first
        foreach (GameObject m in gManager.partsList)
        {
            if (m.GetComponent<MechaPartScript>().part.hardPoint == HardPoint.Weapon2)
            {
                var go = Instantiate(inventorySlot, slotParent);
                go.GetComponent<InventorySlotScript>().mechaPart = m;
            }
        }
    }
    #endregion

    #region Shop Inventory Display and List Alteration Methods
    public void DisplayShopInventory()
    {
        RemoveAllParts(); //clear the inventory first

        foreach (GameObject m in gManager.purchasablePartsList)
        {
            Debug.Log(m.name);
        }
    }

    public void DisplayReadyBuild()
    {
        //Adds all parts that are marked for assembly
        foreach (GameObject m in gManager.activePartsList)
        {
            Debug.Log(m.name);
        }
    }

    public void AddPartToShopInventory(GameObject part)
    {
        gManager.purchasablePartsList.Add(part);
    }

    public void RemovePartFromShopInventory(GameObject part)
    {
        for (int i = 0; i < gManager.purchasablePartsList.Count; i++)
        {
            //if the part at partsList[i] is the same as the part sent to the method, erase the part from the partsList
            if (gManager.purchasablePartsList.ElementAt(i) == part)
            {
                Debug.Log("Found the Part!");
                gManager.purchasablePartsList.RemoveAt(i);
            }
        }
    }
    #endregion
}
