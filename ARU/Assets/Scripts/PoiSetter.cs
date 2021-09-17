using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class PoiSetter : MonoBehaviour
{
    public void setPoi()
    {
        string latStr = File.ReadAllText(Application.persistentDataPath + "/poiLat.txt");
        string lonStr = File.ReadAllText(Application.persistentDataPath + "/poiLon.txt");
        double lat = Convert.ToDouble(latStr);
        double lon = Convert.ToDouble(lonStr);
        SpinCity.poiLat = lat;
        SpinCity.poiLon = lon;
    }
}
