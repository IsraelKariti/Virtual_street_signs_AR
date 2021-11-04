using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SetPoi : MonoBehaviour
{
    public GameObject dialog;
    public Camera cam;
    public GameObject city;
    public Text SetPoiLog;
    public GameObject myPrefab;

    private double origLat;
    private double origLon;


    private Queue<GameObject> cubeQueue;
    private const double oneLatAngleInMeters = 111319.4444;
    string[] poisLines;
    public void setLat(double la)
    {
        origLat = la;
    } 
    public void setLon(double lo)
    {
        origLon = lo;
    }
    public void openDialog()
    {
        dialog.SetActive(true);
    }

    public void setPois()
    {
        // first step: get all pois coordinate in a loop


        poisLines = File.ReadAllLines(Application.persistentDataPath + "/allPoints.txt");
        cubeQueue = new Queue<GameObject>();
        // iterate on all pois
        foreach (string line in poisLines)
        {
            // split line to components
            string[] parts = line.Split(' ');

            // convert to double
            double partLat = Convert.ToDouble(parts[0]);
            double partLon = Convert.ToDouble(parts[1]);
            double partHeight = Convert.ToDouble(parts[2]);

            // 
            double latDiff = partLat - origLat;
            double lonDiff = partLon - origLon;
            SetPoiLog.text = "origLat: " + origLat + "\norigLon: " + origLon;
            SetPoiLog.text += "\npoiLat: " + partLat + "\npoiLon: " + partLon;


            double latDiffMeters = latDiff * oneLatAngleInMeters;
            double lonDiffMeters = lonDiff * Math.Cos(origLat.ToRadians()) * oneLatAngleInMeters;
            SetPoiLog.text += "\nlatMeters: " + latDiffMeters + "\nlonMeters: " + lonDiffMeters;

            GameObject clone = Instantiate(myPrefab, new Vector3((float)lonDiffMeters, (float)partHeight, (float)latDiffMeters), Quaternion.identity, city.transform);
            SetPoiLog.text += "\ny rotate: " + city.transform.rotation.eulerAngles.y;

            clone.transform.RotateAround(city.transform.position, new Vector3(0, 1, 0), city.transform.rotation.eulerAngles.y);// this line is QUESTIONABLE

            clone.GetComponent<TextMesh>().text = parts[3];
            clone.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
            //GameObject clone = Instantiate(myPrefab, new ((float)0, (float)partHeight, (float)0), Quaternion.identity, parent.transform);
            cubeQueue.Enqueue(clone);
        }
    }
}
