using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeScript : MonoBehaviour
{
    GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.instance;

        if (TryGetComponent(out SkinnedMeshRenderer sKRenderer)) sKRenderer.material.color = gManager.playerColor;
        if (TryGetComponent(out MeshRenderer mRenderer)) mRenderer.material.color = gManager.playerColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
