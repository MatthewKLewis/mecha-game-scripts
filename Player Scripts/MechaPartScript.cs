using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  TOP LEVEL SCRIPT FOR INSTANTIATED MECHA PARTS, IT REFERENCES A MECHA PART OBJECT AND ADDS UNITY COMPONENTS
 *  THIS SCRIPT IS REQUIRED BECAUSE ALL GAME OBJECT COMPONENTS MUST DERIVE FROM MONOBEHAVIOR
 *  SOON THIS SCRIPT WILL BE NEEDED TO ADJUST COLLIDER BOXES AND TRANSFORMS, IT CAN CHANGE MATERIALS
 *  EVENTUALLY, THIS SCRIPT CAN TRACK THE DURABILITY OF THE MECHAPART AND RUN DETERIORATE AND REPAIR METHODS ON IT.
 */


public class MechaPartScript : MonoBehaviour
{
    public MechaPart part;
    public int health;
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = part.health;
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    public void DeterioratePart(int damage)
    {
        health -= damage;
    }

}
