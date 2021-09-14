using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapMove : MonoBehaviour
{
    public Text h;
    public static int yRotAR;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float childX = transform.position.x;
        float childZ = transform.position.z;
        float parentX = transform.parent.position.x;
        float parentZ = transform.parent.position.z;
        yRotAR = ((int)(Mathf.Atan2(childX - parentX, childZ - parentZ)/Mathf.PI*180)+360)% 360;
        h.text = "angle: " +yRotAR;
        //Math.atan2();
    }
}
