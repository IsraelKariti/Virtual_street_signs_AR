using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SendManualPoiToFile : MonoBehaviour
{
    public InputField lat;
    public InputField lon;
    public InputField height;
    public InputField desc;
    public GameObject dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sendPoiToFile()
    {
        File.AppendAllText(Application.persistentDataPath + "/allPoints.txt", lat.text + " " + lon.text + " " + height.text + " " + desc.text + "\n");
        lat.text = "";
        lon.text = "";
        height.text = "";
        desc.text = "";
        dialog.SetActive(false);
    }
}
