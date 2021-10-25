using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class CloseSave : MonoBehaviour
{
    public GameObject dialogPopup;
    public Text collectBtnTxt;
    public InputField heightInput;
    public InputField descInput;
    public double lat;
    public double lon;

    public void closeAndSave()
    {
        string height = heightInput.text;
        string desc = descInput.text;

        File.AppendAllText(Application.persistentDataPath + "/allPoints.txt", "lat: " + lat+" lon: "+lon+" height: "+height+" desc: "+desc+"\n");
        dialogPopup.SetActive(false);
        CollectorHandler.qLat.Clear();
        CollectorHandler.qLon.Clear();
        heightInput.text = "";
        descInput.text = "";
        collectBtnTxt.text = "Mark";
    }
}
