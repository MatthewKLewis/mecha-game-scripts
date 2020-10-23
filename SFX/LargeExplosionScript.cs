using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeExplosionScript : MonoBehaviour
{
    [SerializeField] private int lifeTotal = 100;
    [SerializeField] private SphereCollider sCollider;
    private int lifeIndex;
    private int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        lifeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifeIndex++;
        if (lifeIndex == lifeTotal) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 12) //12 is the destructible layer
        {
            collision.collider.gameObject.GetComponent<DestructibleScript>().LoseHealth(damage);
        }
        if (collision.collider.gameObject.layer == 13) //13 is the enemies layer
        {
            collision.collider.gameObject.GetComponent<EnemyHealthScript>().LoseHealth(damage);
        }
    }
}
