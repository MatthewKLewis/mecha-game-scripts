using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float pitchSpeed = 1;
    [SerializeField] private int viewLimits = 65;


    public Vector3 cameraFacing = Vector3.zero;

    public Camera thisCamera;
    public RaycastHit hit;
    public LayerMask layer;
    public Transform gimbalTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.FindCamera(thisCamera);
        GameManager.instance.cameraGameObject = thisCamera.gameObject;
    }

    // Fixed is called at 100hz
    void FixedUpdate()
    {
        //Raycast Debug
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
        }
    }

    void Update()
    {
        //Takes X and Y mouse input...
        Vector3 mouseChange = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));

        //...and rotates the gimbal that the camera is attached to, clamping to a reasonable field of view.
        cameraFacing += mouseChange * pitchSpeed;
        cameraFacing = new Vector3(Mathf.Clamp(cameraFacing.x, -viewLimits, viewLimits),0,0);
        gimbalTransform.localRotation = Quaternion.Euler(cameraFacing);
    }
}
