using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplosionScript : MonoBehaviour
{
    [SerializeField] private int lifeTotal = 5;
    private int lifeIndex;

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
}
