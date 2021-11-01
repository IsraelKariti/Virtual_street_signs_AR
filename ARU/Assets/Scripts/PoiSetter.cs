using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class PoiSetter : MonoBehaviour
{
    public GameObject city;
    public Text SetPoiLog;
    public GameObject myPrefab;
    public GameObject parent;
    private Queue<GameObject> cubeQueue;
    private const double oneLatAngleInMeters = 111319.4444;
    string[] poisLines;
    public void setPoi()
    {
        // first step: get all pois coordinate in a loop


        poisLines = File.ReadAllLines(Application.persistentDataPath + "/allPoints.txt");
        cubeQueue = new Queue<GameObject>();
        // iterate on all pois
        foreach(string line in poisLines)
        {
            // split line to components
            string[] parts = line.Split(' ');
            // 0 lat
            // 1 lon
            // 2 height
            // 3 description
            // change format 12.34 to 12,34 
            //parts[0] = parts[0].Replace('.', ',');
            //parts[1] = parts[1].Replace('.', ',');
            //parts[2] = parts[2].Replace('.', ',');
            Debug.Log(" part 0: " + parts[0] + " part 1: " + parts[1]);

            // convert to double
            double partLat = Convert.ToDouble(parts[0]);
            double partLon = Convert.ToDouble(parts[1]);
            double partHeight = Convert.ToDouble(parts[2]);
            Debug.Log("lat part: " + partLat + " lon part: " + partLon);

            // 
            double latDiff = partLat - SpinCity.avgGPSOrigin.Item1;
            double lonDiff = partLon - SpinCity.avgGPSOrigin.Item2;
            SetPoiLog.text = "origLat: " + SpinCity.avgGPSOrigin.Item1 + "\norigLon: " + SpinCity.avgGPSOrigin.Item2;
            SetPoiLog.text += "\npoiLat: " + partLat + "\npoiLon: " + partLon;

            Debug.Log("orig lat: " + SpinCity.avgGPSOrigin.Item1 + " orig lon: " + SpinCity.avgGPSOrigin.Item2);

            double latDiffMeters = latDiff * oneLatAngleInMeters;
            double lonDiffMeters = lonDiff * Math.Cos(SpinCity.avgGPSOrigin.Item1.ToRadians()) * oneLatAngleInMeters;
            SetPoiLog.text += "\nlatMeters: " + latDiffMeters + "\nlonMeters: " + lonDiffMeters;

            Debug.Log("lat diff: " + latDiffMeters + " lon diff: " + lonDiffMeters);
            GameObject clone = Instantiate(myPrefab, new Vector3((float)lonDiffMeters, (float)partHeight, (float)latDiffMeters), Quaternion.identity, parent.transform);
            SetPoiLog.text += "\nz rotate: ";// + city.transform.rotation.eulerAngles.z;
            SetPoiLog.text += " " + city.transform.rotation.eulerAngles.y;

            clone.transform.RotateAround(parent.transform.position, new Vector3(0, 1, 0), city.transform.rotation.eulerAngles.y);// this line is QUESTIONABLE
            clone.GetComponent<TextMesh>().text = parts[3];
            //GameObject clone = Instantiate(myPrefab, new Vector3((float)0, (float)partHeight, (float)0), Quaternion.identity, parent.transform);
            cubeQueue.Enqueue(clone);
        }
        
        string latStr = File.ReadAllText(Application.persistentDataPath + "/poiLat.txt");
        string lonStr = File.ReadAllText(Application.persistentDataPath + "/poiLon.txt");
        double lat = Convert.ToDouble(latStr);
        double lon = Convert.ToDouble(lonStr);
        // instantiate poi from prefab (using orig coordiante from SpinCity
        SpinCity.poiLat = lat;
        SpinCity.poiLon = lon;
    }

    private void Update()
    {
        
    }
}
