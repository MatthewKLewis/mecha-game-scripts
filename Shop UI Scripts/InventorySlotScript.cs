using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlotScript : MonoBehaviour
{
    public GameObject mechaPart;
    private MechaPartScript mechaPartScript;

    private GameManager gManager;

    public GameObject equipButton;

    public TextMeshProUGUI partName;
    public TextMeshProUGUI partWeight;
    public TextMeshProUGUI partArmor;
    public TextMeshProUGUI partEnergy;
    public TextMeshProUGUI partProjectile;
    public TextMeshProUGUI partFireRate;
    public TextMeshProUGUI partValue;
    public TextMeshProUGUI partTier;

    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;
        mechaPartScript = mechaPart.GetComponent<MechaPartScript>();

        Instantiate(equipButton, transform);

        partName.text = mechaPartScript.part.name;
        partWeight.text = "weight: " + mechaPartScript.part.weight.ToString();
        partArmor.text = "Armor: " + mechaPartScript.part.health.ToString();
        partEnergy.text = "energy: " + mechaPartScript.part.powerConsumption.ToString();
        partProjectile.text = "projectile: ";
        partFireRate.text = "fire rate: " + mechaPartScript.part.fireRate.ToString();
        partValue.text = "value: " + mechaPartScript.part.value.ToString();
        partTier.text = "tier: ";

    }
}
