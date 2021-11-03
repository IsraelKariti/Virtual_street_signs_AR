using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleARCartesian : MonoBehaviour
{
    public GameObject arCoorSys;

    public void ToggleARCartesianCoordianteSystem(bool b)
    {
        Debug.Log("arshow: " + b);
        arCoorSys.SetActive(b);
    }
}
