using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SendCoorToFile : MonoBehaviour
{
    public InputField lat;
    public InputField lon;
    public InputField height;
    public InputField desc;
    public GameObject dialog;

    public void sendPoiToFile()
    {
        File.AppendAllText(Application.persistentDataPath + "/"+dialog.GetComponent<InsertDialogWrapper>().fileName+".txt", lat.text + " " + lon.text + " " + height.text + " " + desc.text + "\n");
        lat.text = "";
        lon.text = "";
        height.text = "";
        desc.text = "";
        dialog.GetComponent<InsertDialogWrapper>().fileName = "";
        dialog.SetActive(false);
    }
}
