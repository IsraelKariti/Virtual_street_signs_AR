using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapMove : MonoBehaviour
{
    public Text h;
    public static int yRotAR;
    public static float dist;
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
        float diffX = childX - parentX;
        float diffZ = childZ - parentZ;
        dist = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffZ, 2));
        yRotAR = ((int)(Mathf.Atan2(diffX,diffZ)/Mathf.PI*180)+360)% 360;
        h.text = "angle: " +yRotAR;
        //Math.atan2();
    }
}
