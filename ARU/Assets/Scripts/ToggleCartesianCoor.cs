using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCartesianCoor : MonoBehaviour
{
    public GameObject cartesianSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void toggleOnOff(bool b)
    {
        Debug.Log("toogle: "+ b);
        cartesianSystem.SetActive(b);
    }
}
