using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SetOriginFromFile : MonoBehaviour
{
    public GameObject btnSetPoi;
    public GameObject dialog;
    public void sendOrigFromFile()
    {

        string[] lines = File.ReadAllLines(Application.persistentDataPath + "/origin.txt");
        string[] parts = lines[0].Split(' ');

        btnSetPoi.GetComponent<SetPoi>().setLat(Convert.ToDouble(parts[0]));
        btnSetPoi.GetComponent<SetPoi>().setLon(Convert.ToDouble(parts[1]));
        btnSetPoi.GetComponent<SetPoi>().setPois();
        dialog.SetActive(false);
    }
}
