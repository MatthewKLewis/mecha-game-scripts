using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairScript : MonoBehaviour
{
    public Image crossHairImage;
    private Color crossHairColor;

    // Start is called before the first frame update
    void Start()
    {
        crossHairImage = GetComponent<Image>();
        crossHairColor = crossHairImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(Color newColor)
    {
        crossHairColor = newColor;
    }

    public void ChangeImage(Image newImage)
    {
        crossHairImage = newImage;
    }
}
