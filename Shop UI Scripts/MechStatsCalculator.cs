using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MechStatsCalculator : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "hello world"; //32 lines
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
