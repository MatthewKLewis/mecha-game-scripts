using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDollyScrollScript : MonoBehaviour
{
    
    [SerializeField] private float scaleCoefficient = .25f;
    private float distanceChange;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-1, 4.5f, -6);
        distanceChange = -6;
    }

    // Update is called once per frame
    void Update()
    {
        distanceChange -= (Input.mouseScrollDelta.y * scaleCoefficient);
        distanceChange = Mathf.Clamp(distanceChange, -9, -6);
        transform.position  = new Vector3(transform.position.x, transform.position.y, distanceChange);
    }
}
