using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCompassMover : MonoBehaviour
{
    public Camera cam;
    public static int toRotate;
    AndroidJavaObject androidCompassProvider;
    long androidCompassTime;
    public static Queue<int> qAndroidCompass;
    // Start is called before the first frame update
    void Start()
    {
        qAndroidCompass = new Queue<int>();
        // create the current UNITY activity
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

        //instantiate the android plugin
        androidCompassProvider = new AndroidJavaObject("com.example.compasslib.CompassLib", unityActivity);
        androidCompassTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (androidCompassProvider.Get<long>("time") > androidCompassTime)
        {
            int azimuth = androidCompassProvider.Get<int>("azimuth");
            int camY = (int)cam.transform.eulerAngles.y;
            // add the new compass reading to the queue
            toRotate = (azimuth - camY + 360) % 360;
            // rotate the virtual city to align with the readings
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -toRotate, transform.eulerAngles.z);
            qAndroidCompass.Enqueue(toRotate);
        }
    }
}
