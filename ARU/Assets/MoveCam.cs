using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Debug.Log("touuuchhhhhhhhhhhhhhhhhhhhhhhh");
            //cam.transform.Rotate(0, 10, 0);
        }
    }
}
