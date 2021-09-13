using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCompassMover : MonoBehaviour
{
    public Camera cam;

    AndroidJavaObject compassProvider;
    long time;
    // Start is called before the first frame update
    void Start()
    {
        // create the current UNITY activity
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

        //instantiate the android plugin
        compassProvider = new AndroidJavaObject("com.example.compasslib.CompassLib", unityActivity);
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (compassProvider.Get<long>("time") > time)
        {
            int azimuth = compassProvider.Get<int>("azimuth");
            int camY = (int)cam.transform.eulerAngles.y;
            // add the new compass reading to the queue
            int toRotate = (azimuth - camY + 360) % 360;
            // rotate the virtual city to align with the readings
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -toRotate, transform.eulerAngles.z);
        }
    }
}
