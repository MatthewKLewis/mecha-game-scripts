using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechaPart
{
    public string name;
    public HardPoint hardPoint;

    public int weight;
    public float powerConsumption; //can be negative for engine parts

    public int health;
    public float fireRate;
    public int value;

    public Transform muzzle;
}

public enum HardPoint { Head, Core, RArm, LArm, Legs, Weapon1, Weapon2 }
