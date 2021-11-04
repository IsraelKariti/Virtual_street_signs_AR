using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCalcOrigLon : MonoBehaviour
{
    public Text t;

    // Update is called once per frame
    void Update()
    {
        t.text = "" + SpinCity.avgGPSOrigin.Item2;
    }
}
