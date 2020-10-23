using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButtonScript : MonoBehaviour
{
    GameManager gManager;

    void Start()
    {
        gManager = GameManager.instance;
    }

    public void LeaveShop()
    {
        Debug.Log("Player leaving the shop with: " + gManager.activePartsList.Count + " parts assembled and " + gManager.partsList.Count + " parts stored.");
        gManager.PlayGame();
    }
}
