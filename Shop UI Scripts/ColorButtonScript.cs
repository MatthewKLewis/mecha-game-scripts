using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonScript : MonoBehaviour
{
    public Color hue;
    GameManager gManager;
    [SerializeField] private DummyPlayerScript dScript;

    // Start is called before the first frame update
    void Start()
    {
        hue = this.gameObject.GetComponent<Image>().color;
        gManager = GameManager.instance;
    }

    public void OnClick()
    {
        gManager.playerColor = hue;
        dScript.RebuildMech();
    }
}
