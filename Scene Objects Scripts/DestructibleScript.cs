using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleScript : MonoBehaviour
{
    public int health = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Instantiate Fractured Rigidbody
            Destroy(gameObject);
        }
    }

    public void LoseHealth(int loss)
    {
        health -= loss;
        Debug.Log("Crate lost " + loss + "HP. Current HP is: " + health);

    }
}
