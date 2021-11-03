using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCartesianCoor : MonoBehaviour
{
    public GameObject cartesianSystem;

    public void toggleOnOff(bool b)
    {
        Debug.Log("toogle: "+ b);
        cartesianSystem.SetActive(b);
    }
}
