using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawNorth : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.rotation.y - Input.compass.trueHeading, transform.eulerAngles.z);
    }
}
